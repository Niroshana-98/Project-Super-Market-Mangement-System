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
    public partial class Store : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        bool havestoreinfo=false;
        public Store()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            LoadStore();
        }
        public void LoadStore()
        {
            try
            {
                connection.Open();
                command=new SqlCommand("SELECT * FROM tbStore",connection);
                dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    havestoreinfo = true;
                    txtStName.Text = dr["Store"].ToString();
                    txtAddress.Text = dr["Address"].ToString() ;
                }
                else
                {
                    txtStName.Clear();
                    txtAddress.Clear();
                }
                dr.Close();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Save Store Details?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                    if (havestoreinfo)
                    {
                        dbcon.ExecuteQuery("UPDATE tbStore SET Store='"+txtStName.Text+"', Address='"+txtAddress.Text+"'");
                    }
                    else
                    {
                        dbcon.ExecuteQuery("INSERT INTO tbStore (Store,Address) VALUES ('" + txtStName.Text +"','" + txtAddress.Text + "')");
                    }
                MessageBox.Show("Store detail has been successfully saved!","Save Record",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Dispose();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Store_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
