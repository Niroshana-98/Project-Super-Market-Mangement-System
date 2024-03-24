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
    public partial class Propertiess : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();

        UserSetting account;
        public string username;
        public Propertiess(UserSetting user)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            account = user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want to Change this account Properties?","Change Properties", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    connection.Open();
                    command = new SqlCommand("UPDATE tbUser SET Name=@Name, Role=@Role, Isactivate=@Isactivate WHERE Username='" + username + "'",connection);
                    command.Parameters.AddWithValue("@Name", txtFName.Text);
                    command.Parameters.AddWithValue("@Role", cmbRole.Text);
                    command.Parameters.AddWithValue("@Isactivate", cmbActivate.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Account Properties has been successfully Changed!", "Update Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    account.LoadUser();
                    this.Dispose();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Propertiess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
