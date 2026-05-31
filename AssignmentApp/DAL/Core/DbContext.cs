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
                // Connection string do người dùng cung cấp
                connstring = @"Data Source=ADMIN-PC;Initial Catalog=CKNet;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
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

        // Check whether a given column exists in a table in the current database.
        public static bool ColumnExists(string tableName, string columnName)
        {
            if (Conn == null)
                throw new InvalidOperationException("Database connection is not initialized.");

            const string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column";
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return false;
                return Convert.ToInt32(result) > 0;
            }
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
