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
    public partial class BrandModule : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        Brand brand;
        public BrandModule(Brand br)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            brand = br;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this brand?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("INSERT INTO tbBrand(Brand)VALUES(@Brand)", connection);
                    command.Parameters.AddWithValue("@Brand", txtBrand.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been succeseful saved", "MarketNet");
                    clear();
                    brand.LoadBrand();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBrand.Clear();
        }
        public void clear()
        {
            txtBrand.Clear();
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            txtBrand.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update this brand?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                connection.Open();
                command = new SqlCommand("UPDATE tbBrand SET Brand=@Brand WHERE Id LIKE'" + lblId.Text + "'", connection);
                command.Parameters.AddWithValue("@Brand", txtBrand.Text);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Brand has been successfully update.", "MarketNet");
                clear();
                this.Dispose();
            }
        }
    }
}
