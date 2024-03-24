using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Super_Market_Mangement_System_Final
{
    internal class DBConnect
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        private string con;
        public string myConnection()
        {
            con = @"Data Source=DESKTOP-E2RI9G8\SQLEXPRESS;Initial Catalog=""Management System_1"";Integrated Security=True;Encrypt=False";
            return con;
        }
        public DataTable getTable(String qury)
        {
            connection.ConnectionString = myConnection();
            command = new SqlCommand(qury, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public void ExecuteQuery(String sql)
        {
            try
            {
                connection.ConnectionString = myConnection();
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string getPassword(string username)
        {
            string password="";
            connection.ConnectionString = myConnection();
            connection.Open();
            command = new SqlCommand("SELECT Password FROM tbUser WHERE Username = '"+ username +"'", connection);
            dr=command.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                password = dr["Password"].ToString();
            }
            dr.Close();
            connection.Close();
            return password;
        }
        public double ExtractData(string sql)
        {

            connection = new SqlConnection();
            connection.ConnectionString = myConnection();
            connection.Open();
            command = new SqlCommand(sql, connection);
            double data = double.Parse(command.ExecuteScalar().ToString());
            connection.Close();
            return data;

        }
    }
}
