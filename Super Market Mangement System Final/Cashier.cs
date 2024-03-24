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
using AForge.Video.DirectShow;
using AForge.Video;
using ZXing;
using DarrenLee.Media;

namespace Super_Market_Mangement_System_Final
{
    public partial class Cashier : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;

        int qty;
        string id;
        string price;

        //Camera captureDevice=new Camera();

        string stitle = "Point Of Sales";
        public Cashier()
        {
            InitializeComponent();
            connection = new SqlConnection(dbcon.myConnection());
            GetTranNo();
            lblDate.Text=DateTime.Now.ToShortDateString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Exit Application ? ","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        public void slide(Button button)
        {
            panelSlide.BackColor=Color.White;
            panelSlide.Height=button.Height;
            panelSlide.Top=button.Top;
        }
        #region button
        private void btnTransaction_Click(object sender, EventArgs e)
        {
            slide(btnTransaction);
            GetTranNo();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            slide(btnSearch);
            LookUpProduct lookUp = new LookUpProduct(this);
            lookUp.LoadProduct();
            lookUp.ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            slide(btnDiscount);
            Discount discount = new Discount(this);
            discount.lblId.Text = id;
            discount.txtTotalPrice.Text = price;
            discount.ShowDialog(); 
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            slide(btnPayment);
            Settle settle=new Settle(this);
            settle.txtSale.Text=lblDisplayTotal.Text;
            settle.ShowDialog();
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            slide(btnClearCart);
            if(MessageBox.Show("Remove all items from cart?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                connection.Open();
                command=new SqlCommand("DELETE FROM tbCart WHERE Transno LIKE'"+lblTransno.Text+"'",connection);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("All items has been successfully remove", "Remove Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }

        private void btnDailySales_Click(object sender, EventArgs e)
        {
            slide(btnDailySales);
            DailySale dailySale = new DailySale(new Main());
            dailySale.solduser=lblUsername.Text;
            dailySale.dtFrom.Enabled = false;
            dailySale.dtTo.Enabled= false;
            dailySale.cmbCashier.Enabled = false;
            dailySale.cmbCashier.Text = lblUsername.Text;
            dailySale.btnClose.Visible = true;
            dailySale.lblTitle.Visible = true;
            dailySale.ShowDialog();
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            slide(btnChangePass);
            ChangePassword change = new ChangePassword(this);
            change.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            if(dgvCash.Rows.Count > 0)
            {
                MessageBox.Show("Unable to Logout. Please Cancel the Transaction.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }
        #endregion button
        public void LoadCart()
        {
            try
            {
                Boolean hascart=false;
                int i = 0;
                double total = 0;
                double discount = 0;
                dgvCash.Rows.Clear();
                connection.Open();
                command = new SqlCommand("SELECT c.ID, c.Pcode, p.Pdesc, c.Price,c.Qty, c.Disc, c.Total FROM tbCart AS c INNER JOIN tbProduct AS p ON c.Pcode=p.Pcode WHERE c.Transno LIKE @Transno and c.Status LIKE 'Pending'", connection);
                command.Parameters.AddWithValue("@Transno", lblTransno.Text);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Convert.ToDouble(dr["Total"].ToString());
                    discount += Convert.ToDouble(dr["Disc"].ToString());
                    dgvCash.Rows.Add(i, dr["ID"].ToString(), dr["Pcode"].ToString(), dr["Pdesc"].ToString(), dr["Price"].ToString(), dr["Qty"].ToString(), dr["Disc"].ToString(), double.Parse(dr["Total"].ToString()).ToString("#,##0.00"));
                    hascart = true;
                }
                dr.Close();
                connection.Close();
                lblSaleTotal.Text = total.ToString("#,##0,00");
                lblDiscount.Text = discount.ToString("#,##0,00");
                GetCartTotal();
                if (hascart) { btnClearCart.Enabled = true; btnPayment.Enabled = true; btnDiscount.Enabled = true;}
                else { btnClearCart.Enabled = false; btnPayment.Enabled = false; btnDiscount.Enabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
            
        }
        public void GetCartTotal()
        {
            double discount = double.Parse(lblDiscount.Text);
            double sales = double.Parse(lblSaleTotal.Text);// - discount;
            //double vat = sales * 1/100;
            //double vatable=sales-vat;

            //lblVat.Text = vat.ToString("#,##0,00");
            //lblVatable.Text = vatable.ToString("#,##0,00");
            lblDisplayTotal.Text = sales.ToString("#,##0,00");
            //lblDiscount.Text = discount.ToString("#,##0,00");
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        public void GetTranNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                //string transno = sdate + "1001";
                //lblTransno.Text = transno;
                string transno;
                connection.Open();
                command = new SqlCommand("SELECT TOP 1 Transno FROM tbCart WHERE transno LIKE '" + sdate + "%' ORDER BY Id Desc", connection);
                dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message,stitle);
            }
            
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text == string.Empty) return;
                else
                {
                    string _Pcode;
                    double _Price;
                    int _qty ;
                    connection.Open();
                    command=new SqlCommand("SELECT * FROM tbProduct WHERE Barcode LIKE '" + txtBarcode.Text + "'",connection);
                    dr = command.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["Qty"].ToString());
                        _Pcode = dr["Pcode"].ToString();
                        _Price = double.Parse(dr["Price"].ToString());
                        _qty=int.Parse(txtQty.Text);
                        
                        dr.Close();
                        connection.Close();
                        AddToCart(_Pcode, _Price, _qty);
                    }
                    dr.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message, stitle,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        public void AddToCart(string _Pcode,double _Price,int _qty)
        {
            try
            {
                string id = "";
                int cart_qty = 0;
                bool found=false;
                connection.Open();
                command=new SqlCommand("SELECT * FROM tbCart WHERE Transno=@Transno and Pcode=@Pcode",connection);
                command.Parameters.AddWithValue("@Transno", lblTransno.Text);
                command.Parameters.AddWithValue("@Pcode",_Pcode);
                dr=command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    id = dr["Id"].ToString();
                    cart_qty = int.Parse(dr["Qty"].ToString()) ;
                    found=true;
                }
                else found=false;
                dr.Close();
                connection.Close();

                if (found)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to procced. Remaining Quantity on hand is"+ qty,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    connection.Open();
                    command= new SqlCommand("UPDATE tbCart SET Qty= (Qty +" + _qty +")WHERE Id= '" + id +"'",connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    txtBarcode.SelectionStart = 0;
                    txtBarcode.SelectionLength = txtBarcode.Text.Length;
                    LoadCart();
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
                    command.Parameters.AddWithValue("@Transno", lblTransno.Text);
                    command.Parameters.AddWithValue("@Pcode", _Pcode);
                    command.Parameters.AddWithValue("@Price", _Price);
                    command.Parameters.AddWithValue("@Qty", _qty);
                    command.Parameters.AddWithValue("@Sdate", DateTime.Now);
                    command.Parameters.AddWithValue("@Cashier", lblUsername.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    LoadCart();
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void dgvCash_SelectionChanged(object sender, EventArgs e)
        {
            int i = dgvCash.CurrentRow.Index;
            id = dgvCash[1, i].Value.ToString();
            price = dgvCash[7, i].Value.ToString();
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            

            if (colName == "Delete")
            {
                
                //slide(btnClearCart);
                if (MessageBox.Show("Remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.ExecuteQuery("DELETE FROM tbCart WHERE Id LIKE'" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Items has been successfully remove", "Remove Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if(colName == "colAdd")
            {
                int i = 0;
                connection.Open();
                command = new SqlCommand("SELECT SUM(Qty) AS Qty FROM tbProduct WHERE Pcode LIKE'" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY Pcode", connection);
                i = int.Parse(command.ExecuteScalar().ToString());
                connection.Close();
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {
                    dbcon.ExecuteQuery("UPDATE tbCart SET Qty=Qty + " + int.Parse(txtQty.Text) + "WHERE Transno LIKE '" + lblTransno.Text+"' AND Pcode LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Remaining qty on hand is " + i + "!", "Out Of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "colReduce")
            {
                int i = 0;
                connection.Open();
                command = new SqlCommand("SELECT SUM(Qty) AS Qty FROM tbCart WHERE Pcode LIKE'" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY Pcode", connection);
                i = int.Parse(command.ExecuteScalar().ToString());
                connection.Close();
                if (i>1)
                {
                    dbcon.ExecuteQuery("UPDATE tbCart SET Qty = Qty - " + int.Parse(txtQty.Text) + "WHERE Transno LIKE '" + lblTransno.Text + "' AND Pcode LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Remaining Qty on cart is " + i + "!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
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
                Alert alert = new Alert(new Main());
                alert.lblPcode.Text = dr["Pcode"].ToString();
                alert.showAlert(i + ". " + dr["Pdesc"].ToString() + " - " + dr["Qty"].ToString());
            }
            dr.Close();
            connection.Close();
        }
        private void Cashier_Load(object sender, EventArgs e)
        {
           
        }

        private void Cashier_Load_1(object sender, EventArgs e)
        {
            Noti();
        }

        private void Cashier_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F8)
            //{
                //captureDevice.OnFrameArrived += captureDevice_OnframeArrived;
                //captureDevice.Start();
            //}
        }

        private void captureDevice_OnframeArrived(object source, FrameArrivedEventArgs e)
        {
            Bitmap bitmap = (Bitmap)e.GetFrame();
            BarcodeReader barcodeReader = new BarcodeReader();
            var result=barcodeReader.Decode(bitmap);
            if(result != null)
            {
                txtBarcode.Invoke(new MethodInvoker(delegate ()
                {txtBarcode.Text = result.ToString();}));
            }
        }

        private void Cashier_FormClosing(object sender, FormClosingEventArgs e)
        {
            //captureDevice.Stop();
        }
    }
}
