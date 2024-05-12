using System;
using System.Data;
using System.Data.SqlClient;
using QLBH.Database;

namespace QLBH.DAL
{
    public static class ChatLieuDAL
    {
        private static readonly string TableName = "tblChatLieu";

        public static DataTable GetAllChatLieu()
        {
            string sql = "SELECT MaCL, TenCL FROM " + TableName;
            return DatabaseLayer.GetDataToTable(sql);
        }

        public static void InsertChatLieu(string maCL, string tenCL)
        {
            string sqlCheckKey = "SELECT MaCL FROM " + TableName + " WHERE MaCL = @MaCL";
            SqlParameter[] checkKeyParams = { new SqlParameter("@MaCL", maCL) };

            if (DatabaseLayer.CheckKey(sqlCheckKey, checkKeyParams))
            {
                throw new Exception("Mã chất liệu đã tồn tại");
            }

            string sqlInsert = "INSERT INTO " + TableName + " (MaCL, TenCL) VALUES (@MaCL, @TenCL)";
            SqlParameter[] insertParams = {
                new SqlParameter("@MaCL", maCL),
                new SqlParameter("@TenCL", tenCL)
            };

            DatabaseLayer.RunSql(sqlInsert, insertParams);
        }

        public static void UpdateChatLieu(string maCL, string tenCL)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET TenCL = @TenCL WHERE MaCL = @MaCL";
            SqlParameter[] updateParams = {
                new SqlParameter("@TenCL", tenCL),
                new SqlParameter("@MaCL", maCL)
            };

            DatabaseLayer.RunSql(sqlUpdate, updateParams);
        }

        public static void DeleteChatLieu(string maCL)
        {
            string sqlDelete = "DELETE FROM " + TableName + " WHERE MaCL = @MaCL";
            SqlParameter[] deleteParams = { new SqlParameter("@MaCL", maCL) };

            DatabaseLayer.RunSqlDel(sqlDelete, deleteParams);
        }
    }
}
