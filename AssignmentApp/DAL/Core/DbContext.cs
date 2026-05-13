using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace AssignmentApp.DAL.Core
{
    public static class DbContext
    {
        public static SqlConnection? Conn;
        public static string connstring = "";

        public static bool Ketnoi()
        {
            try
            {
                connstring = @"Data Source=LAPTOP-TEEPQA0B\SQLEXPRESS;Initial Catalog=TestChamnetCK;Integrated Security=True;TrustServerCertificate=True";
                Conn = new SqlConnection(connstring);
                
                if (Conn.State != ConnectionState.Open)
                {
                    Conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Ngatketnoi()
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
                Conn = null;
            }
        }

        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            return table;
        }

        public static bool CheckKey(string sql)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            return table.Rows.Count > 0;
        }

        public static void RunSql(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public static string GetFieldValues(string sql)
        {
            string ma = "";
            
            if (Conn == null || Conn.State == ConnectionState.Closed)
            {
                Ketnoi();
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, Conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ma = reader.GetValue(0)?.ToString() ?? "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ma;
        }
    }
}
