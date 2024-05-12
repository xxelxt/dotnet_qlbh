using QLBH.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLBH.Database
{
    internal static class DatabaseLayer
    {
        private static readonly string connectionString = "Data Source=XXELXT\\XXELXT;Initial Catalog=QLBH;Integrated Security=True;Encrypt=False";

        public static SqlConnection conn { get; private set; }

        public static void Connect()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Kết nối thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kết nối không thành công: " + ex.Message);
            }
        }

        public static void Disconnect()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
                Console.WriteLine("Đã đóng kết nối");
            }
        }

        public static DataTable GetDataToTable(string sql)
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu từ bảng: " + ex.Message);
            }

            return table;
        }

        public static DataTable GetDataToTable(string sql, SqlParameter[] parameters)
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu từ bảng: " + ex.Message);
            }

            return table;
        }

        public static DataTable GetDataToTable(string sql, string[] parameters, object[] values)
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    if (parameters != null && values != null && parameters.Length == values.Length)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            command.Parameters.AddWithValue(parameters[i], values[i]);
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu từ bảng: " + ex.Message);
            }

            return table;
        }

        public static bool CheckKey(string sql, SqlParameter[] parameters)
        {
            bool recordExists = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            recordExists = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra khóa: " + ex.Message);
                recordExists = false;
            }

            return recordExists;
        }

        public static void RunSql(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RunSqlDel(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                RunSql(sql, parameters);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("Dữ liệu đang được dùng, không thể xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void FillCombo(string sql, ComboBox cbo, string valueMember, string displayMember)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        cbo.DataSource = dataTable;
                        cbo.ValueMember = valueMember;
                        cbo.DisplayMember = displayMember;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu vào ComboBox: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string GetFieldValues(string sql)
        {
            string result = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        object value = cmd.ExecuteScalar();
                        if (value != null && value != DBNull.Value)
                        {
                            result = value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy giá trị từ cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        public static object GetFieldValues(string sql, SqlParameter[] parameters = null)
        {
            object result = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy giá trị từ cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }


        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;

            partsDay = DateTime.Now.ToShortDateString().Split('/');
            
            // Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;

            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            
            // Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = Functions.ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];

            // Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);

            string t;
            t = String.Format("{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key += t;

            return key;
        }
    }
}
