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
using Microsoft.Reporting.WinForms;

namespace Super_Market_Mangement_System_Final
{
    public partial class Recept : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string store;
        string address;
        Cashier cashier;
        public Recept(Cashier cash)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            cashier = cash;
            LoadStore();
        }
        public void LoadStore()
        {
            connection.Open();
            command = new SqlCommand("SELECT * FROM tbStore", connection);
            dr = command.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                store = dr["Store"].ToString();
                address = dr["Address"].ToString();
            }
            dr.Close();
            connection.Close();
        }

        private void Recept_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
        public void LoadRecept(string pcash, string pchange)
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptRecept.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                connection.Open();
                da.SelectCommand = new SqlCommand("SELECT c.Id, c.Transno, c.Pcode, c.Price, c.Qty, c.Disc, c.Total, c.Sdate, c.Status, p.Pdesc FROM tbCart AS c INNER JOIN tbProduct AS p ON p.Pcode=c.Pcode WHERE c.Transno LIKE '"+cashier.lblTransno.Text+"'",connection);
                da.Fill(ds.Tables["dtRecept"]);
                connection.Close();

                ReportParameter pVatable = new ReportParameter("pVatable",cashier.lblVatable.Text);
                ReportParameter pVat = new ReportParameter("pVat", cashier.lblVat.Text);
                ReportParameter pDiscount = new ReportParameter("pDiscount", cashier.lblDiscount.Text);
                ReportParameter pTotal = new ReportParameter("pTotal", cashier.lblDisplayTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pChange = new ReportParameter("pChange", pchange);
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Invoice #: " + cashier.lblTransno.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", cashier.lblUsername.Text);

                reportViewer1.LocalReport.SetParameters(pVatable);
                reportViewer1.LocalReport.SetParameters(pVat);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);

                rptDataSource = new ReportDataSource("DataSet1", ds.Tables["dtRecept"]);
                reportViewer1.LocalReport.DataSources.Add(rptDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 30;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void Recept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
