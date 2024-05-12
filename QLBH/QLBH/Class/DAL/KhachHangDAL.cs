using System;
using System.Data;
using System.Data.SqlClient;
using QLBH.Database;

namespace QLBH.DAL
{
    public static class KhachHangDAL
    {
        private static readonly string TableName = "tblKhach";

        public static DataTable GetAllKhachHang()
        {
            string sql = "SELECT * FROM " + TableName;
            return DatabaseLayer.GetDataToTable(sql);
        }

        public static void InsertKhachHang(string maKhach, string tenKhach, string diaChi, string dienThoai)
        {
            string sqlCheckKey = "SELECT MaKhach FROM " + TableName + " WHERE MaKhach = @MaKhach";
            SqlParameter[] checkKeyParams = { new SqlParameter("@MaKhach", maKhach) };

            if (DatabaseLayer.CheckKey(sqlCheckKey, checkKeyParams))
            {
                throw new Exception("Mã khách hàng đã tồn tại");
            }

            string sqlInsert = "INSERT INTO " + TableName + " (MaKhach, TenKhach, DiaChi, DienThoai) " +
                               "VALUES (@MaKhach, @TenKhach, @DiaChi, @DienThoai)";

            SqlParameter[] insertParams = {
                new SqlParameter("@MaKhach", maKhach),
                new SqlParameter("@TenKhach", tenKhach),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai)
            };

            DatabaseLayer.RunSql(sqlInsert, insertParams);
        }


        public static void UpdateKhachHang(string maKhach, string tenKhach, string diaChi, string dienThoai)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET TenKhach = @TenKhach, DiaChi = @DiaChi, DienThoai = @DienThoai WHERE MaKhach = @MaKhach";
            SqlParameter[] updateParams = {
                new SqlParameter("@TenKhach", tenKhach),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai),
                new SqlParameter("@MaKhach", maKhach)
            };

            DatabaseLayer.RunSql(sqlUpdate, updateParams);
        }


        public static void DeleteKhachHang(string maKhach)
        {
            string sqlDelete = "DELETE FROM " + TableName + " WHERE MaKhach = @MaKhach";
            SqlParameter[] deleteParams = { new SqlParameter("@MaKhach", maKhach) };

            DatabaseLayer.RunSqlDel(sqlDelete, deleteParams);
        }
    }
}
