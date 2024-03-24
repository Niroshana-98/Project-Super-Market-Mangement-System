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
using System.Xml.Linq;

namespace Super_Market_Mangement_System_Final
{
    public partial class Void : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        CancelOrder cancelOrder;
        public Void(CancelOrder cancel)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            txtUsername.Select();
            cancelOrder = cancel;
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text.ToLower() == cancelOrder.txtCancelBy.Text.ToLower())
                {
                    MessageBox.Show("Void by name and cancelled by name are same!. Please void by another person.","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                string user;
                connection.Open();
                command = new SqlCommand("SELECT*FROM tbUser WHERE Username=@Username AND Password=@Password", connection);
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPass.Text);
                dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    user = dr["Username"].ToString();
                    dr.Close();
                    connection.Close();
                    SaveCancelOrder(user);
                    if (cancelOrder.cmbInventory.Text == "Yes")
                    {
                        dbcon.ExecuteQuery("UPDATE tbProduct SET Qty=Qty + " + cancelOrder.udCancelQty.Value + "WHERE Pcode='" + cancelOrder.txtPcode.Text + "'");
                    }
                    dbcon.ExecuteQuery("UPDATE tbCart SET Qty=Qty - " + cancelOrder.udCancelQty.Value + "WHERE Id LIKE '" + cancelOrder.txtId.Text + "'");
                    MessageBox.Show("Order transaction successfully cancelled!", "Cancel Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                    cancelOrder.ReloadSoldList();
                    cancelOrder.Dispose();
                }
                dr.Close();
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SaveCancelOrder(string user)
        {
            try
            {
                connection.Open();
                command=new SqlCommand("INSERT INTO tbCancel (Transno,Pcode,Price,Qty,Total,Sdate,Voidby,Cancelledby,Reason,Action) VALUES (@Transno,@Pcode,@Price,@Qty,@Total,@Sdate,@Voidby,@Cancelledby,@Reason,@Action)",connection);
                command.Parameters.AddWithValue("@Transno", cancelOrder.txtTransno.Text);
                command.Parameters.AddWithValue("@Pcode", cancelOrder.txtPcode.Text);
                command.Parameters.AddWithValue("@Price", double.Parse(cancelOrder.txtPrice.Text));
                command.Parameters.AddWithValue("@Qty", int.Parse(cancelOrder.txtQty.Text));
                command.Parameters.AddWithValue("@Total", double.Parse(cancelOrder.txtTotal.Text));
                command.Parameters.AddWithValue("Sdate", DateTime.Now);
                command.Parameters.AddWithValue("@Voidby", user);
                command.Parameters.AddWithValue("@Cancelledby", cancelOrder.txtCancelBy.Text);
                command.Parameters.AddWithValue("@Reason", cancelOrder.txtReason.Text);
                command.Parameters.AddWithValue("@Action", cancelOrder.cmbInventory.Text);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Void_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnVoid.PerformClick();
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtPass.Focus();
            }
        }
    }
}
