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
    public partial class Category : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Category()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            LoadCategory();
        }
        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            connection.Open();
            command = new SqlCommand("SELECT * FROM tbCategory ORDER BY Category", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr["Id"].ToString(), dr["Category"].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryModule module = new CategoryModule(this);
            module.ShowDialog();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("DELETE FROM tbCategory WHERE Id LIKE'" + dgvCategory[1, e.RowIndex].Value.ToString() + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Category has been successfully Deleted", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Edit")
            {
                CategoryModule module = new CategoryModule(this);
                module.lblId.Text = dgvCategory[1, e.RowIndex].Value.ToString();
                module.txtCategory.Text = dgvCategory[2, e.RowIndex].Value.ToString();
                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();

            }
            LoadCategory();
        }
    }
}
