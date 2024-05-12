using System;
using System.Data;
using System.Data.SqlClient;
using QLBH.Database;

namespace QLBH.DAL
{
    public static class NhanVienDAL
    {
        private static readonly string TableName = "tblNhanVien";

        public static DataTable GetAllNhanVien()
        {
            string sql = "SELECT * FROM " + TableName;
            return DatabaseLayer.GetDataToTable(sql);
        }

        public static void InsertNhanVien(string maNV, string tenNV, string gioiTinh, string diaChi, string dienThoai, string ngaySinh)
        {
            string sqlCheckKey = "SELECT MaNV FROM " + TableName + " WHERE MaNV = @MaNV";
            SqlParameter[] checkKeyParams = { new SqlParameter("@MaNV", maNV) };

            if (DatabaseLayer.CheckKey(sqlCheckKey, checkKeyParams))
            {
                throw new Exception("Mã nhân viên đã tồn tại");
            }

            string sqlInsert = "INSERT INTO " + TableName + " (MaNV, TenNV, GioiTinh, DiaChi, DienThoai, NgaySinh) " +
                               "VALUES (@MaNV, @TenNV, @GioiTinh, @DiaChi, @DienThoai, @NgaySinh)";

            SqlParameter[] insertParams = {
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@TenNV", tenNV),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai),
                new SqlParameter("@NgaySinh", ngaySinh)
            };

            DatabaseLayer.RunSql(sqlInsert, insertParams);
        }


        public static void UpdateNhanVien(string maNV, string tenNV, string gioiTinh, string diaChi, string dienThoai, string ngaySinh)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET TenNV = @TenNV, GioiTinh = @GioiTinh, DiaChi = @DiaChi, DienThoai = @DienThoai, NgaySinh = @NgaySinh WHERE MaNV = @MaNV";
            SqlParameter[] updateParams = {
                new SqlParameter("@TenNV", tenNV),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@MaNV", maNV)
            };

            DatabaseLayer.RunSql(sqlUpdate, updateParams);
        }


        public static void DeleteNhanVien(string maNV)
        {
            string sqlDelete = "DELETE FROM " + TableName + " WHERE MaNV = @MaNV";
            SqlParameter[] deleteParams = { new SqlParameter("@MaNV", maNV) };

            DatabaseLayer.RunSqlDel(sqlDelete, deleteParams);
        }
    }
}
