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
    public partial class Product : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Product()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            LoadProduct();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            command = new SqlCommand("SELECT p.Pcode,p.Barcode,p.Pdesc,b.Brand,c.Category,p.Price,p.SellingPrice,p.Recorder FROM tbProduct AS p INNER JOIN tbBrand AS b ON b.Id =p.Bid INNER JOIN tbCategory AS c on c.ID=p.Cid WHERE CONCAT(p.Pdesc, b.Brand, c.Category,p.Pcode) LIKE '%" +txtSearch.Text +"%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule productModule = new ProductModule(this);
            productModule.ShowDialog();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModule product = new ProductModule(this);
                product.txtPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                product.txtBarcode.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                product.txtDescription.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                product.cmbBrand.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.cmbCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                product.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
                product.txtSellPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString();
                product.nudReOrder.Value = int.Parse(dgvProduct.Rows[e.RowIndex].Cells[8].Value.ToString());

                product.txtPcode.Enabled = false;
                product.btnSave.Enabled = false;
                product.btnUpdate.Enabled = true;
                product.ShowDialog();

            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbProduct WHERE Pcode LIKE'" + dgvProduct[1, e.RowIndex].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Product has been successfully Deleted", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProduct();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
