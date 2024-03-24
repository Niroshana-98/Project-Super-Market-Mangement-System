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
    public partial class Discount : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();       
        string stitle = "Point Of Sales";
        Cashier cashier;
        public Discount(Cashier cash)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            cashier = cash;
            txtDiscount.Focus();
            this.KeyPreview = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Discount_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Escape) this.Dispose();
            else if (e.KeyCode == Keys.Enter) btnSave.PerformClick();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double disc=double.Parse(txtTotalPrice.Text)*double.Parse(txtDiscount.Text) * 0.01;
                txtDiscAmount.Text = disc.ToString("#,##0.00");
            }
            catch(Exception)
            {
                txtDiscAmount.Text = "0.00";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Add discount? Click Yes to Confirm", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    command=new SqlCommand("UPDATE tbCart SET Disc_persent =@Disc_persent WHERE Id =@Id",connection);                   
                    command.Parameters.AddWithValue("@Disc_persent", double.Parse(txtDiscount.Text));
                    command.Parameters.AddWithValue("@Id", int.Parse(lblId.Text));
                    command.ExecuteNonQuery();
                    connection.Close();
                    cashier.LoadCart();
                    this.Dispose();

                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message, stitle);
            }
        }
    }
}
