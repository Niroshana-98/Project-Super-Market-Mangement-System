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
    public partial class ResetPassword : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        UserSetting user;
        public ResetPassword(UserSetting account)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            user = account;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtNePass.Text != txtRePass.Text)
            {
                MessageBox.Show("The Password you typed do not match. Type the Password for this Account in both text boxes.","Add User Wizard",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return; 
            }
            else
            {
                if(MessageBox.Show("Reset Password ?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dbcon.ExecuteQuery("UPDATE tbUser SET Password = '" +txtNePass.Text+"'WHERE Username ='"+user.username+"'");
                    MessageBox.Show("Password has been Successfully Reset", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ResetPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
