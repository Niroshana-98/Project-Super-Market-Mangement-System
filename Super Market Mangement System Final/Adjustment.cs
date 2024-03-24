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
    public partial class Adjustment : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        Main main;
        int _qty;
        public Adjustment(Main mn)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            main = mn;
            ReferenceNo();
            LoadStock();
            lblUsername.Text=main.lblUserName.Text;
        }
        public void ReferenceNo()
        {
            Random rnd=new Random();
            lblRefNo.Text=rnd.Next().ToString();
        }
        public void LoadStock()
        {
            int i = 0;
            dgvAdjustment.Rows.Clear();
            command = new SqlCommand("SELECT p.Pcode,p.Barcode,p.Pdesc,b.Brand,c.Category,p.Price,p.Qty FROM tbProduct AS p INNER JOIN tbBrand AS b ON b.Id =p.Bid INNER JOIN tbCategory AS c on c.ID=p.Cid WHERE CONCAT(p.Pdesc, b.Brand, c.Category) LIKE '%" + txtSearch.Text + "%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvAdjustment.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void dgvAdjustment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvAdjustment.Columns[e.ColumnIndex].Name;
            if(colName == "Select")
            {
                lblPcode.Text = dgvAdjustment.Rows[e.RowIndex].Cells[1].Value.ToString();
                lblDesc.Text = dgvAdjustment.Rows[e.RowIndex].Cells[3].Value.ToString() + " " + dgvAdjustment.Rows[e.RowIndex].Cells[4].Value.ToString() + " " + dgvAdjustment.Rows[e.RowIndex].Cells[5].Value.ToString();
                _qty = int.Parse(dgvAdjustment.Rows[e.RowIndex].Cells[7].Value.ToString());
                btnSave.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStock();
        }
        public void Clear()
        {
            lblDesc.Text = "";
            lblPcode.Text = "";
            txtQty.Clear();
            txtRemark.Clear();
            cmbAction.Text = "";
            ReferenceNo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAction.Text == "")
                {
                    MessageBox.Show("Please Select Action for add or Reduce.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbAction.Focus();
                    return;
                }
                if (txtQty.Text == "")
                {
                    MessageBox.Show("Please input Quantity for add or Reduce.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQty.Focus();
                    return;
                }
                if (txtRemark.Text == "")
                {
                    MessageBox.Show("Need Reason for stock Adjustment.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRemark.Focus();
                    return;
                }

                if(int.Parse(txtQty.Text) > _qty)
                {
                    MessageBox.Show("Stock On Hand Quantity should be greater than adjustment Quantity","Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(cmbAction.Text == "Remove From Inventory")
                {
                    dbcon.ExecuteQuery("UPDATE tbProduct SET Qty = (Qty - " + int.Parse(txtQty.Text) + ") WHERE Pcode LIKE '" + lblPcode.Text + "'");
                }
                else if(cmbAction.Text == "Add To Inventory")
                {
                    dbcon.ExecuteQuery("UPDATE tbProduct SET Qty = (Qty + " + int.Parse(txtQty.Text) + ") WHERE Pcode LIKE '" + lblPcode.Text + "'");
                }
                dbcon.ExecuteQuery("INSERT INTO tbAdjustment(Referenceno, Pcode, Qty, Action, Remarks, Sdate, [User]) VALUES ('"+lblRefNo.Text+"','"+lblPcode.Text+"','"+int.Parse(txtQty.Text)+"','"+cmbAction.Text+"','"+txtRemark.Text+"','"+DateTime.Now.ToShortDateString()+"','"+lblUsername.Text+"')");
                MessageBox.Show("Stock has been successfully adjustment.", "Process completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                Clear();
                btnSave.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
