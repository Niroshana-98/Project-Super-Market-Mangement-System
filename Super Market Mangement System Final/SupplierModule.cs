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
    public partial class SupplierModule : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Point Of Sales";
        Supplier supplier;
        public SupplierModule(Supplier sp)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            supplier = sp;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void Clear()
        {
            txtAddress.Clear();
            txtConPerson.Clear();
            txtEmail.Clear();
            txtFaxNo.Clear();
            txtPhone.Clear();
            txtSupplier.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtSupplier.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Save this record? Click Yes to confirm.", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command=new SqlCommand("INSERT INTO tbSupplier (Supplier, Address, ContactPerson, Phone, Email, Fax) VALUES (@Supplier, @Address, @ContactPerson, @Phone, @Email, @Fax)",connection);
                    command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);
                    command.Parameters.AddWithValue("@ContactPerson", txtConPerson.Text);
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@Fax", txtFaxNo.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been Successfully saved!","Saved Record",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Clear();
                    supplier.LoadSuppiler();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Update this Record ? Click Yes to confirm","CONFIRM",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command =new SqlCommand("UPDATE tbSupplier SET Supplier=@Supplier,Address=@Address,ContactPerson=@ContactPerson,Phone=@Phone,Email=@Email,Fax=@Fax WHERE Id=@Id",connection);
                    command.Parameters.AddWithValue("@Id", lblId.Text);
                    command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);
                    command.Parameters.AddWithValue("@ContactPerson", txtConPerson.Text);
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@Fax", txtFaxNo.Text);
                    //connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record has been Successfully Updated!", "Updated Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    this.Dispose();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void SupplierModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
