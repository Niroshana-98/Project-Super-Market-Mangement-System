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
    public partial class DailySale : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        Main main;
        public string solduser;
        public DailySale(Main mn)
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadCashier();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadCashier()
        {
            cmbCashier.Items.Clear();
            cmbCashier.Items.Add("All Cashier");
            connection.Open();
            command = new SqlCommand("SELECT * FROM tbUser WHERE Role LIKE 'Cashier'", connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                cmbCashier.Items.Add(dr["Username"].ToString());
            }
            dr.Close();
            connection.Close();
        }
        public void LoadSold()
        {
            int i = 0;
            double total = 0;
            dgvSold.Rows.Clear();
            connection.Open();
            if (cmbCashier.Text == "All Cashier")
            {
                command = new SqlCommand("SELECT c.Id,c.Transno,c.Pcode,p.Pdesc,c.Price,c.Qty,c.Disc,c.Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate between '" + dtFrom.Value + "' AND '" + dtTo.Value + "'", connection);
            }
            else
            {
                command = new SqlCommand("SELECT c.Id,c.Transno,c.Pcode,p.Pdesc,c.Price,c.Qty,c.Disc,c.Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate between '" + dtFrom.Value + "' AND '" + dtTo.Value + "' AND Cashier LIKE '" + cmbCashier.Text + "'", connection);
            }
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                total += double.Parse(dr["Total"].ToString());
                dgvSold.Rows.Add(i, dr["Id"].ToString(), dr["Transno"].ToString(), dr["Pcode"].ToString(), dr["Pdesc"].ToString(), dr["Price"].ToString(), dr["Qty"].ToString(), dr["Disc"].ToString(), dr["Total"].ToString());
            }
            dr.Close();
            connection.Close();
            lblToatal.Text = total.ToString("#,##0.00");
        }

        private void cmbCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void DailySale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if (colName == "Cancel")
            {
                CancelOrder cancelorder = new CancelOrder(this);
                cancelorder.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancelorder.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancelorder.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancelorder.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancelorder.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancelorder.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancelorder.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancelorder.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                cancelorder.txtCancelBy.Text = solduser;
                cancelorder.ShowDialog();

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SMMSReport report = new SMMSReport();
            string param = "Date From: " + dtFrom.Value.ToShortDateString() + "To: " + dtTo.Value.ToShortDateString();
            if (cmbCashier.Text == "All Cashier")
            {
                report.LoadDailyReport("SELECT c.Id,c.Transno,c.Pcode,p.Pdesc,c.Price,c.Qty,c.Disc AS Discount,c.Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate between '" + dtFrom.Value + "' AND '" + dtTo.Value + "'", param, cmbCashier.Text);
            }
            else
            {
                report.LoadDailyReport("SELECT c.Id,c.Transno,c.Pcode,p.Pdesc,c.Price,c.Qty,c.Disc AS Discount,c.Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE Status LIKE 'Sold' AND Sdate between '" + dtFrom.Value + "' AND '" + dtTo.Value + "' AND Cashier LIKE '" + cmbCashier.Text + "'", param, cmbCashier.Text);
            }
            report.ShowDialog();
        }
        private void dgvSold_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if (colName == "Cancel")
            {
                CancelOrder cancel = new CancelOrder(this);
                cancel.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancel.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancel.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancel.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancel.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancel.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancel.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancel.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                if (lblTitle.Visible == false)
                    cancel.txtCancelBy.Text = main.lblUserName.Text;
                else
                    cancel.txtCancelBy.Text = solduser;
                cancel.ShowDialog();
            }
        }
    }
}
