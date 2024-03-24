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
    public partial class LookUpProduct : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        Cashier cashier;
        public LookUpProduct(Cashier cash)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            cashier=cash;
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
            command = new SqlCommand("SELECT p.Pcode,p.Barcode,p.Pdesc,b.Brand,c.Category,p.SellingPrice,p.Qty FROM tbProduct AS p INNER JOIN tbBrand AS b ON b.Id =p.Bid INNER JOIN tbCategory AS c on c.ID=p.Cid WHERE CONCAT(p.Pdesc, b.Brand, c.Category) LIKE '%" + txtSearch.Text + "%'", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                Qty qty = new Qty(cashier);
                qty.ProductDetails(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString(), double.Parse(dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString()),cashier.lblTransno.Text, int.Parse(dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString()));
                qty.ShowDialog();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void LookUpProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.Dispose();
            }
        }
    }
}
