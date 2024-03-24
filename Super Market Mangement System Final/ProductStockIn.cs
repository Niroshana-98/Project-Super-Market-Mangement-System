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
    public partial class ProductStockIn : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Point Of Sales";
        StockIn stockIn;
        public ProductStockIn(StockIn stk)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            stockIn = stk;
            LoadProduct();
            
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            command = new SqlCommand("SELECT Pcode,Pdesc,Qty FROM tbProduct WHERE Pdesc LIKE '%" + txtSearch.Text + "%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                if (stockIn.txtStockInBy.Text == string.Empty)
                {
                    MessageBox.Show("Please enter stock in by name",stitle,MessageBoxButtons.OK,MessageBoxIcon.Warning);                   
                    stockIn.txtStockInBy.Focus();
                    this.Dispose();
                    
                }
                if(MessageBox.Show("Add this items ?",stitle,MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    addStockIn(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
                    MessageBox.Show("Successfully added", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
            }
        }
        public void addStockIn(string pcode)
        {
            try
            {
                connection.Open();
                command = new SqlCommand("INSERT INTO tbStockIn (Refno, Pcode, Sdate, Stockinby, Supplierid)VALUES (@Refno, @Pcode, @Sdate, @Stockinby, @Supplierid)", connection);
                command.Parameters.AddWithValue("@Refno", stockIn.txtRefNo.Text);
                command.Parameters.AddWithValue("@Pcode", pcode);
                command.Parameters.AddWithValue("@Sdate", stockIn.dtStockIn.Value);
                command.Parameters.AddWithValue("@Stockinby", stockIn.txtStockInBy.Text);
                command.Parameters.AddWithValue("@Supplierid", stockIn.lblId.Text);
                command.ExecuteNonQuery();
                connection.Close();
                stockIn.LoadStockIn();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void ProductStockIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
