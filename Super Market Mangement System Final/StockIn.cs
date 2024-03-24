using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Super_Market_Mangement_System_Final
{
    public partial class StockIn : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Point Of Sales";
        Main main;
        public StockIn(Main mn)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadSupplier();
            GetRefNo();
            txtStockInBy.Text = main.lblUserName.Text;
        }
        public void GetRefNo()
        {
            Random rnd=new Random();
            txtRefNo.Clear();
            txtRefNo.Text+=rnd.Next();
        }
        public void LoadSupplier()
        {
            cmbSupplier.Items.Clear();
            cmbSupplier.DataSource = dbcon.getTable("SELECT * FROM tbSupplier");
            cmbSupplier.DisplayMember = "Supplier";
        }
        public void ProductForSupplier(string pcode)
        {
            string supplier = "";
            connection.Open();
            command = new SqlCommand("SELECT * FROM vwStockIn WHERE Pcode LIKE '" + pcode + "'", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                supplier = dr["Supplier"].ToString();
            }
            dr.Close();
            connection.Close();
            cmbSupplier.Text = supplier;
        }

        public void LoadStockIn()
        {
            int i = 0;
            dgvStockIn.Rows.Clear();
            connection.Open();
            command=new SqlCommand("SELECT * FROM vwStockIn WHERE Refno LIKE '" + txtRefNo.Text + "'AND Status LIKE 'Pending'",connection);
            dr=command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr["Supplier"].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection.Open();
            command=new SqlCommand("SELECT * FROM tbSupplier WHERE Supplier LIKE '" + cmbSupplier.Text + "'",connection);
            dr = command.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblId.Text = dr["Id"].ToString();
                txtConPerson.Text = dr["Contactperson"].ToString();
                txtAddress.Text = dr["Address"].ToString();
            }
            dr.Close();
            connection.Close();
        }

        private void cmbSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void liGenerate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GetRefNo();
        }

        private void linProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProductStockIn productStock = new ProductStockIn(this);
            productStock.ShowDialog();
        }

        private void btnEntry_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvStockIn.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure to save this record ?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for(int i=0; i < dgvStockIn.Rows.Count; i++)
                        {
                            connection.Open();
                            command=new SqlCommand("UPDATE tbProduct SET Qty = Qty + " + int.Parse(dgvStockIn.Rows[i].Cells[5].Value.ToString()) + "WHERE Pcode LIKE'" + dgvStockIn.Rows[i].Cells[3].Value.ToString() + "'",connection);
                            command.ExecuteNonQuery();
                            connection.Close();

                            connection.Open();
                            command = new SqlCommand("UPDATE tbStockIn SET Qty = Qty + " + int.Parse(dgvStockIn.Rows[i].Cells[5].Value.ToString()) + ",Status='Done'WHERE Id LIKE'" + dgvStockIn.Rows[i].Cells[1].Value.ToString() + "'", connection);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        Clear();
                        LoadStockIn();
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,stitle,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        public void Clear()
        {
            txtRefNo.Clear();
            txtStockInBy.Clear();
            dtStockIn.Value=DateTime.Now;
        }

        private void dgvStockIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvStockIn.Columns[e.ColumnIndex].Name;
            if(colName == "Delete")
            {
                if(MessageBox.Show("Remove this items ?",stitle,MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbStockIn WHERE Id='" + dgvStockIn.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Item has been successfully removed", stitle,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    LoadStockIn();
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                dgvInStockHistory.Rows.Clear();
                connection.Open();
                command = new SqlCommand("SELECT * FROM vwStockIn WHERE CAST(Sdate AS Date) BETWEEN '"+dtFrom.Value.ToShortDateString()+ "' AND '"+dtTo.Value.ToShortDateString()+"' AND Status LIKE 'Done'", connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvInStockHistory.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString(), dr["Supplier"].ToString());
                }
                dr.Close();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cmbSupplier_TextChanged(object sender, EventArgs e)
        {
            connection.Open();
            command = new SqlCommand("SELECT * FROM tbSupplier WHERE Supplier LIKE '" + cmbSupplier.Text + "'", connection);
            dr = command.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblId.Text = dr["Id"].ToString();
                txtConPerson.Text = dr["ContactPerson"].ToString();
                txtAddress.Text = dr["Address"].ToString();

            }
            dr.Close();
            connection.Close();
        }
    }
}
