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
    public partial class CategoryModule : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        Category category;
        public CategoryModule(Category ct)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            category = ct;
        }
        public void Clear()
        {
            txtCategory.Clear();
            txtCategory.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Category?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("INSERT INTO tbCategory(Category)VALUES(@Category)", connection);
                    command.Parameters.AddWithValue("@Category", txtCategory.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been succeseful saved", "Point Of Sales");
                    Clear();
                }
                category.LoadCategory();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update this Category?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                connection.Open();
                command = new SqlCommand("UPDATE tbCategory SET Category=@Category WHERE Id LIKE'" + lblId + "'", connection);
                command.Parameters.AddWithValue("@Category", txtCategory.Text);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Brand has been successfully update.", "Point Of Sales");
                Clear();
                this.Dispose();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void CategoryModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
