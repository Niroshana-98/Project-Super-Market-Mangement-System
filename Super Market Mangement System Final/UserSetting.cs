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
    public partial class UserSetting : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        Main main;
        public string username;
        string name;
        string role;
        string accstatus;
        public UserSetting(Main mn)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadUser();
        }
        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            command = new SqlCommand("SELECT * FROM tbUser", connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i, dr[0].ToString(), dr[3].ToString(), dr[4].ToString(), dr[2].ToString());
            }
            dr.Close();
            connection.Close();
        }
        public void Clear()
        {
            txtFullName.Clear();
            txtPassword.Clear();
            txtRePassword.Clear();
            txtUserName.Clear();
            cmbRole.Text = "";
            txtUserName.Focus();
        }

        private void btnAccSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != txtRePassword.Text)
                {
                    MessageBox.Show("Password did not Match !","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }

                connection.Open();
                command=new SqlCommand("INSERT INTO tbUser(Username,Password,Role,Name)VALUES(@Username,@Password,@Role,@Name)",connection);
                command.Parameters.AddWithValue("@Username", txtUserName.Text);
                command.Parameters.AddWithValue("@Password",txtPassword.Text);
                command.Parameters.AddWithValue("@Role",cmbRole.Text);
                command.Parameters.AddWithValue("@Name",txtFullName.Text);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("New Account has been Successfully Saved!", "Saved Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                LoadUser();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Warning");
            }
        }

        private void btnAccCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnPassSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtCuPass.Text != main._pass)
                {
                    MessageBox.Show("Current Password did not Match!","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                if (txtNewPass.Text != txtRePass2.Text)
                {
                    MessageBox.Show("Confirm New Password did not Match!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dbcon.ExecuteQuery("UPDATE tbUser SET Password='"+txtNewPass.Text+"'WHERE Username='"+lblUsername.Text+"'");
                MessageBox.Show("Password has been Successfully Changed!","Changed Password",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UserSetting_Load(object sender, EventArgs e)
        {
            lblUsername.Text = main.lblUserName.Text;
        }

        private void btnPassCancel_Click(object sender, EventArgs e)
        {
            ClearCP();
        }
        public void ClearCP()
        {
            txtCuPass.Clear();
            txtNewPass.Clear();
            txtRePass2.Clear();
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            int i = dgvUser.CurrentRow.Index;
            username = dgvUser[1,i].Value.ToString();
            name=dgvUser[2,i].Value.ToString();
            role=dgvUser[4,i].Value.ToString();
            accstatus=dgvUser[3,i].Value.ToString();
            if (lblUsername.Text == username)
            {
                btnRemove.Enabled = false;
                btnResetPass.Enabled = false;
                lblAccNote.Text = "To Change your Password, go to change Password tag.";
            }
            else
            {
                btnRemove.Enabled = true;
                btnResetPass.Enabled = true;
                lblAccNote.Text = "To Change the Password For " + username + ",Click Reset Password.";
            }
            gbUser.Text = "Password For " + username;
            lblAccNote.Text = "To Change the Password For " + username + ",Click Reset Password.";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("You choose to remove this account from this point of Sales system's user list.\n\n Are you sure you want to remove'"+username+"'\\'"+role+"'","User Account",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbcon.ExecuteQuery("DELETE FROM tbUser WHERE Username ='" + username + "'");
                MessageBox.Show("Account has been Successfully deleted");
                LoadUser();
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            ResetPassword reset = new ResetPassword(this);
            reset.ShowDialog();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            Propertiess propertiess = new Propertiess(this);
            propertiess.Text = name + "\\" + username + "Properties";
            propertiess.txtFName.Text=name;
            propertiess.cmbRole.Text = role;
            propertiess.cmbActivate.Text = accstatus;
            propertiess.username = username;
            propertiess.ShowDialog();
        }
    }
}
