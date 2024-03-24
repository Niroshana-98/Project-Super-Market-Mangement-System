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
    public partial class ProductModule : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Point Of Sales";
        Product product;
        public ProductModule(Product pd)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            product = pd;
            LoadBrand();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cmbCategory.Items.Clear();
            cmbCategory.DataSource = dbcon.getTable("SELECT * FROM tbCategory");
            cmbCategory.DisplayMember = "Category";
            cmbCategory.ValueMember = "Id";
        }
        public void LoadBrand()
        {
            cmbBrand.Items.Clear();
            cmbBrand.DataSource = dbcon.getTable("SELECT * FROM tbBrand");
            cmbBrand.DisplayMember = "Brand";
            cmbBrand.ValueMember = "Id";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            txtSellPrice.Clear();
            cmbBrand.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            nudReOrder.Value = 1;

            txtPcode.Enabled = true;
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are sure want to save this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("INSERT INTO tbProduct(Pcode,Barcode,Pdesc,Bid,Cid,Price,SellingPrice,Recorder)VALUES (@Pcode,@Barcode,@Pdesc,@Bid,@Cid,@Price,@SellingPrice,@Recorder)", connection);
                    command.Parameters.AddWithValue("@Pcode", txtPcode.Text);
                    command.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                    command.Parameters.AddWithValue("@Pdesc", txtDescription.Text);
                    command.Parameters.AddWithValue("@Bid", cmbBrand.SelectedValue);
                    command.Parameters.AddWithValue("@Cid", cmbCategory.SelectedValue);
                    command.Parameters.AddWithValue("@Price", double.Parse(txtPrice.Text));
                    command.Parameters.AddWithValue("@SellingPrice",double.Parse(txtSellPrice.Text));
                    command.Parameters.AddWithValue("@Recorder", nudReOrder.Value);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Product has been successfully saved", stitle);
                    Clear();
                    product.LoadProduct();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                if (MessageBox.Show("Are sure want to Update this product?", "Update Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("UPDATE tbProduct SET Barcode=@Barcode,Pdesc=@Pdesc,Bid=@Bid,Cid=@Cid,Price=@Price,SellingPrice=@SellingPrice,Recorder=@Recorder WHERE Pcode LIKE @Pcode", connection);
                    command.Parameters.AddWithValue("@Pcode", txtPcode.Text);
                    command.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                    command.Parameters.AddWithValue("@Pdesc", txtDescription.Text);
                    command.Parameters.AddWithValue("@Bid", cmbBrand.SelectedValue);
                    command.Parameters.AddWithValue("@Cid", cmbCategory.SelectedValue);
                    command.Parameters.AddWithValue("@Price", double.Parse(txtPrice.Text));
                    command.Parameters.AddWithValue("@SellingPrice",double.Parse(txtSellPrice.Text));
                    command.Parameters.AddWithValue("@Recorder", nudReOrder.Value);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Product has been successfully saved", stitle);
                    Clear();
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProductModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void ProductModule_Load(object sender, EventArgs e)
        {

        }
    }
}
