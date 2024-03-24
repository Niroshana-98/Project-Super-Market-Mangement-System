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
    
    public partial class Qty : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Point Of Sales";
        private string Pcode;
        private double Price;
        private string Transno;
        private int qty;
        Cashier cashier;
        public Qty(Cashier cash)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            cashier = cash;
        }
        public void ProductDetails(string Pcode,double Price,string Transno,int qty)
        {
            this.Pcode=Pcode;
            this.Price=Price;
            this.Transno=Transno;
            this.qty=qty;
            
        }

        private void Qty_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar == 13)&&(txtQty.Text != string.Empty))
            {
                try
                {
                    string id = "";
                    int cart_qty = 0;
                    bool found = false;
                    connection.Open();
                    command = new SqlCommand("SELECT * FROM tbCart WHERE Transno=@Transno and Pcode=@Pcode", connection);
                    command.Parameters.AddWithValue("@Transno", Transno);
                    command.Parameters.AddWithValue("@Pcode", Pcode);
                    dr = command.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        id = dr["Id"].ToString();
                        cart_qty = int.Parse(dr["Qty"].ToString());
                        found = true;
                    }
                    else found = false;
                    dr.Close();
                    connection.Close();

                    if (found)
                    {
                        if (qty < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Unable to procced. Remaining qty on hand is" + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        connection.Open();
                        command = new SqlCommand("UPDATE tbCart SET Qty= (Qty +" + int.Parse(txtQty.Text) + ")WHERE Id= '" + id + "'", connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        cashier.txtBarcode.Clear();
                        cashier.txtBarcode.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }
                    else
                    {
                        if (qty < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Unable to procced. Remaining qty on hand is" + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        connection.Open();
                        command = new SqlCommand("INSERT INTO tbCart(Transno,Pcode,Price,Qty,Sdate,Cashier)VALUES(@Transno,@Pcode,@Price,@Qty,@Sdate,@Cashier)", connection);
                        command.Parameters.AddWithValue("@Transno", Transno);
                        command.Parameters.AddWithValue("@Pcode", Pcode);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@Qty", int.Parse(txtQty.Text));
                        command.Parameters.AddWithValue("@Sdate", DateTime.Now);
                        command.Parameters.AddWithValue("@Cashier", cashier.lblUsername.Text);
                        command.ExecuteNonQuery();
                        connection.Close();
                        cashier.txtBarcode.Clear();
                        cashier.txtBarcode.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, stitle);
                }

            }
        }

        private void Qty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
