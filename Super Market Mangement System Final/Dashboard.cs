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
    public partial class Dashboard : Form
    {
        SqlConnection connection = new SqlConnection();
        DBConnect dbcon = new DBConnect();
        public Dashboard()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            string Sdate = DateTime.Now.ToShortDateString();
            lblDailySale.Text = dbcon.ExtractData("SELECT ISNULL(SUM(Total),0) AS Total FROM tbCart WHERE Status LIKE 'Sold' AND Sdate BETWEEN '" + Sdate + "' AND '" + Sdate + "'").ToString("#,##0.00");
            lblTotalProduct.Text = dbcon.ExtractData("SELECT COUNT(*) FROM tbProduct").ToString("#,##0");
            lblStockOnHand.Text = dbcon.ExtractData("SELECT ISNULL(SUM(Qty), 0) AS Qty FROM tbProduct").ToString("#,##0");
            lblCriticalItem.Text = dbcon.ExtractData("SELECT COUNT(*) FROM vwCriticalItems").ToString("#,##0");
        }
    }
}
