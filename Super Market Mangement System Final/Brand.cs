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
    public partial class Brand : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Brand()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            LoadBrand();
        }
        public void LoadBrand()
        {
            int i = 0;
            dgvBrand.Rows.Clear();
            connection.Open();
            command = new SqlCommand("SELECT * FROM tbBrand ORDER BY Brand", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvBrand.Rows.Add(i, dr["Id"].ToString(), dr["Brand"].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BrandModule moduleform = new BrandModule(this);
            moduleform.ShowDialog();
        }

        private void dgvBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //For update and delete brand by cell click from tdBrand
            String colName = dgvBrand.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbBrand WHERE Id LIKE'" + dgvBrand[1, e.RowIndex].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Brand has been successfully Deleted", "MarketNet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Edit")
            {
                BrandModule brandModule = new BrandModule(this);
                brandModule.lblId.Text = dgvBrand[1, e.RowIndex].Value.ToString();
                brandModule.txtBrand.Text = dgvBrand[2, e.RowIndex].Value.ToString();
                brandModule.btnSave.Enabled = false;
                brandModule.btnUpdate.Enabled = true;
                brandModule.ShowDialog();

            }
            LoadBrand();
        }
    }
}
