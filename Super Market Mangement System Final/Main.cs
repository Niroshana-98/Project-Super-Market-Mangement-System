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
    public partial class Main : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public string _pass;
        public Main()
        {
            InitializeComponent();
            customizeDesign();
            connection = new SqlConnection(dbcon.myConnection());
            //connection.Open();

        }
        #region panelSlide
        private void customizeDesign()
        {
            panelSubProduct.Visible = false;
            panelSubRecord.Visible = false;
            panelSubStock.Visible = false;
            panelSubSetting.Visible = false;
        }
        private void hideSubmenu()
        {
            if (panelSubProduct.Visible == true)
                panelSubProduct.Visible = false;
            if (panelSubRecord.Visible == true)
                panelSubRecord.Visible = false;
            if (panelSubSetting.Visible == true)
                panelSubSetting.Visible = false;
            if (panelSubStock.Visible == true)
                panelSubStock.Visible = false;
        }
        private void showSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }
        #endregion panelSlide
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
            hideSubmenu();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubProduct);
        }

        private void btnProductList_Click(object sender, EventArgs e)
        {
            openChildForm(new Product());
            hideSubmenu();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            openChildForm(new Category());
            hideSubmenu();
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            openChildForm(new Brand());
            hideSubmenu();
        }

        private void btnInStock_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubStock);
        }

        private void btnStockEntry_Click(object sender, EventArgs e)
        {
            openChildForm(new StockIn(this));
            hideSubmenu();
        }

        private void btnStockAdjustment_Click(object sender, EventArgs e)
        {
            openChildForm(new Adjustment(this));
            hideSubmenu();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            openChildForm(new Supplier());
            hideSubmenu();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubRecord);
        }

        private void btnSaleHistory_Click(object sender, EventArgs e)
        {
            openChildForm(new DailySale(this));
            hideSubmenu();
        }

        private void btnPOSRecord_Click(object sender, EventArgs e)
        {
            openChildForm(new Record());
            hideSubmenu();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubSetting);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserSetting(this));
            hideSubmenu();
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            hideSubmenu();
            Store store = new Store();
            store.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            hideSubmenu();
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            btnDashboard.PerformClick();
            Noti();
        }
        public void Noti()
        {
            int i = 0;
            connection.Open();
            command = new SqlCommand("SELECT * FROM vwCriticalItems", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                Alert alert = new Alert(this);
                alert.lblPcode.Text = dr["Pcode"].ToString();
                alert.btnReorder.Enabled = true;
                alert.showAlert(i + ". " + dr["Pdesc"].ToString() + " - " + dr["Qty"].ToString());
            }
            dr.Close();
            connection.Close();
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            openChildForm(new Barcode());
            hideSubmenu();
        }
    }
}
