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
    public partial class Record : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Record()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            LoadCriticalItems();
            LoadInventoryList();
        }
        public void LoadTopSelling()
        {
            int i = 0;
            dgvTopSelling.Rows.Clear();
            connection.Open();

            //Sort By Total Amount
            if (cmbTopSell.Text == "Sort By Qty")
            {
                command = new SqlCommand("SELECT TOP 10 Pcode, Pdesc, isnull(sum(Qty),0) AS Qty, ISNULL(SUM(Total),0) AS Total FROM vwTopSelling WHERE Sdate BETWEEN '" + dtFromTopSell.Value.ToString() + "' AND '" + dtToTopSell.Value.ToString() + "' AND Status LIKE 'Sold' GROUP BY Pcode, Pdesc ORDER BY Qty DESC", connection);
            }
            else if (cmbTopSell.Text == "Sort By Total Amount")
            {
                command = new SqlCommand("SELECT TOP 10 Pcode, Pdesc, isnull(sum(Qty),0) AS Qty, ISNULL(SUM(Total),0) AS Total FROM vwTopSelling WHERE Sdate BETWEEN '" + dtFromTopSell.Value.ToString() + "' AND '" + dtToTopSell.Value.ToString() + "' AND Status LIKE 'Sold' GROUP BY Pcode, Pdesc ORDER BY Total DESC", connection);
            }
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvTopSelling.Rows.Add(i, dr["Pcode"].ToString(), dr["Pdesc"].ToString(), dr["Qty"].ToString(), double.Parse(dr["Total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            connection.Close();
        }
        public void LoadSoldItems()
        {
            try
            {
                dgvSoldItems.Rows.Clear();
                int i = 0;
                connection.Open();
                command = new SqlCommand("SELECT c.Pcode, p.Pdesc, c.Price, sum(c.Qty) as Qty, SUM(c.Disc) AS Disc, SUM(c.Total) AS Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate BETWEEN '" + dtFromSoldItems.Value.ToString() + "' AND '" + dtToSoldItems.Value.ToString() + "' GROUP BY c.Pcode, p.Pdesc, c.Price", connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvSoldItems.Rows.Add(i, dr["Pcode"].ToString(), dr["Pdesc"].ToString(), double.Parse(dr["Price"].ToString()).ToString("#,##0.00"), dr["Qty"].ToString(), dr["Disc"].ToString(), double.Parse(dr["Total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                connection.Close();

                connection.Open();
                command = new SqlCommand("SELECT ISNULL(SUM(total),0) FROM tbCart WHERE Status LIKE 'Sold' AND Sdate BETWEEN '" + dtFromSoldItems.Value.ToString() + "' AND '" + dtToSoldItems.Value.ToString() + "'", connection);
                lblTotal.Text = double.Parse(command.ExecuteScalar().ToString()).ToString("#,##0.00");
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void LoadCriticalItems()
        {
            try
            {
                dgvCriticalItems.Rows.Clear();
                int i = 0;
                connection.Open();
                command = new SqlCommand("SELECT * FROM vwCriticalItems", connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCriticalItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void LoadInventoryList()
        {
            try
            {
                dgvInventoryList.Rows.Clear();
                int i = 0;
                connection.Open();
                command = new SqlCommand("SELECT * FROM vwInventoryList", connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvInventoryList.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void LoadCancelItems()
        {
            int i = 0;
            dgvCancel.Rows.Clear();
            connection.Open();
            command = new SqlCommand("SELECT * FROM vwCancelItems WHERE Sdate BETWEEN '" + dtFromCancel.Value.ToString() + "' AND '" + dtToCancel.Value.ToString() + "'", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCancel.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), DateTime.Parse(dr[6].ToString()).ToShortDateString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(), dr[10].ToString());
            }
            dr.Close();
            connection.Close();
        }
        public void LoadStockInHist()
        {
            int i = 0;
            dgvStockIn.Rows.Clear();
            connection.Open();
            command = new SqlCommand("SELECT * FROM vwStockIn WHERE cast(Sdate AS Date) BETWEEN '" + dtFromStockIn.Value.ToString() + "' AND '" + dtToStockIn.Value.ToString() + "' AND Status LIKE 'Done'", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
            }
            dr.Close();
            connection.Close();
        }
        private void btnLoadTopSell_Click(object sender, EventArgs e)
        {
            if (cmbTopSell.Text == "Select Sort Type")
            {
                MessageBox.Show("Please select sort type from the dropdown list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTopSell.Focus();
                return;
            }
            LoadTopSelling();
        }

        private void btnLoadSoldItems_Click(object sender, EventArgs e)
        {
            LoadSoldItems();
        }

        private void btnLoadCancel_Click(object sender, EventArgs e)
        {
            LoadCancelItems();
        }

        private void btnLoadSockIn_Click(object sender, EventArgs e)
        {
            LoadStockInHist();
        }

        private void btnPrintTopSell_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            string param = "From : " + dtFromTopSell.Value.ToString() + " To : " + dtToTopSell.Value.ToString();
            if (cmbTopSell.Text == "Sort By Qty")
            {
                report.LoadTopSelling("SELECT TOP 10 Pcode, Pdesc, isnull(sum(Qty),0) AS Qty, ISNULL(SUM(Total),0) AS Total FROM vwTopSelling WHERE Sdate BETWEEN '" + dtFromTopSell.Value.ToString() + "' AND '" + dtToTopSell.Value.ToString() + "' AND Status LIKE 'Sold' GROUP BY Pcode, Pdesc ORDER BY Qty DESC", param, "TOP SELLING ITEMS SORT BY QTY");
            }
            else if (cmbTopSell.Text == "Sort By Total Amount")
            {
                report.LoadTopSelling("SELECT TOP 10 Pcode, Pdesc, isnull(sum(Qty),0) AS Qty, ISNULL(SUM(Total),0) AS Total FROM vwTopSelling WHERE Sdate BETWEEN '" + dtFromTopSell.Value.ToString() + "' AND '" + dtToTopSell.Value.ToString() + "' AND Status LIKE 'Sold' GROUP BY Pcode, Pdesc ORDER BY Total DESC", param, "TOP SELLING ITEMS SORT BY Total Amount");
            }
            report.ShowDialog();
        }

        private void btnPrintSoldItems_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            string param = "From : " + dtFromSoldItems.Value.ToString() + " To : " + dtToSoldItems.Value.ToString();
            report.LoadSoldItems("SELECT c.Pcode, p.Pdesc, c.Price, sum(c.Qty) as Qty, SUM(c.Disc) AS Disc, SUM(c.Total) AS Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate BETWEEN '" + dtFromSoldItems.Value.ToString() + "' AND '" + dtToSoldItems.Value.ToString() + "' GROUP BY c.Pcode, p.Pdesc, c.Price", param);
            report.ShowDialog();
        }

        private void btnPrintInventoryList_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            report.LoadInventory("SELECT * FROM vwInventoryList");
            report.ShowDialog();
        }

        private void btnPrintCancelledOrder_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            string param = "From : " + dtFromCancel.Value.ToString() + " To : " + dtToCancel.Value.ToString();
            report.LoadCancelledOrder("SELECT * FROM vwCancelItems WHERE Sdate BETWEEN '" + dtFromCancel.Value.ToString() + "' AND '" + dtToCancel.Value.ToString() + "'", param);
            report.ShowDialog();
        }

        private void btnPrintStockIn_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            string param = "From : " + dtFromStockIn.Value.ToString() + " To : " + dtToStockIn.Value.ToString();
            report.LoadStockInHist("SELECT * FROM vwStockIn WHERE cast(Sdate AS Date) BETWEEN '" + dtFromStockIn.Value.ToString() + "' AND '" + dtToStockIn.Value.ToString() + "' AND Status LIKE 'Done'", param);
            report.ShowDialog();
        }
    }
}
