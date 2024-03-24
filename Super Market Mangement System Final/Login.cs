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
    public partial class Login : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;

        public string _pass = "";
        public bool _isactivate;
        public Login()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            txtName.Select();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string _username = "", _name = "", _role = "";
            try
            {
                bool found;
                connection.Open();
                command=new SqlCommand("SELECT*FROM tbUser WHERE Username=@Username AND Password=@Password",connection);
                command.Parameters.AddWithValue("@Username",txtName.Text);
                command.Parameters.AddWithValue("@Password",txtPass.Text);
                dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    _username = dr["Username"].ToString();
                    _name = dr["Name"].ToString();
                    _role = dr["Role"].ToString();
                    _pass = dr["Password"].ToString();
                    _isactivate = bool.Parse(dr["Isactivate"].ToString());

                }
                else
                {
                    found= false;
                }
                dr.Close();
                connection.Close();

                if (found)
                {
                    if (!_isactivate)
                    {
                        MessageBox.Show("Account is Deactivate. Unable to Login", "Deactive Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (_role == "Cashier")
                    {
                        //MessageBox.Show("Welcome " + _name + " ", "Login Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Clear();
                        txtPass.Clear();
                        this.Hide();
                        Cashier cashier = new Cashier();
                        cashier.lblUsername.Text = _username;
                        cashier.lblName.Text = _name +" | "+ _role;
                        cashier.ShowDialog();
                    }
                    else
                    {
                        //MessageBox.Show("Welcome " + _name + " ", "Login Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Clear();
                        txtPass.Clear();
                        this.Hide();
                        Main main = new Main();
                        main.lblUserName.Text = _username;
                        main.lblName.Text = _name;
                        main._pass = _pass;                       
                        main.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "Login Fail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtPass.Focus();
            }
        }
    }
}
