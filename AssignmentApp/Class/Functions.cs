using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AssignmentApp.Class
{
    public static class Functions
    {
        // Thêm dấu ? để báo rằng biến này có thể null (Hết lỗi CS8618)
        public static SqlConnection? Conn;
        public static string connstring = ""; // Gán giá trị mặc định (Hết lỗi CS8618)

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
                Conn = null; // Hết lỗi CS8625
            }
        }

        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            return table;
        }

        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma;
            cbo.DisplayMember = ten;
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

        public static void RunSqlDel(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Dữ liệu đang được dùng, không thể xóa...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        public static bool IsDate(string d)
        {
            string[] parts = d.Split('/');
            try
            {
                if ((Convert.ToInt32(parts[0]) >= 1) && (Convert.ToInt32(parts[0]) <= 31) && (Convert.ToInt32(parts[1]) >= 1) && (Convert.ToInt32(parts[1]) <= 12) && (Convert.ToInt32(parts[2]) >= 1900))
                    return true;
            }
            catch { }
            return false;
        }

        public static string ConvertDateTime(string d)
        {
            string[] parts = d.Split('/');
            return String.Format("{0}/{1}/{2}", parts[1], parts[0], parts[2]);
        }

        public static string GetFieldValues(string sql)
        {
            string ma = "";
            
            // Đảm bảo kết nối luôn mở trước khi truy vấn
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