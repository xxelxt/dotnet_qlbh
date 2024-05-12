using System;
using System.Data;
using System.Data.SqlClient;
using QLBH.Database;

namespace QLBH.DAL
{
    internal class HangDAL
    {
        private static readonly string TableName = "tblHang";

        public static DataTable GetAllHang()
        {
            string sql = @"SELECT H.MaHang, H.TenHang, CL.TenCL AS ChatLieu, H.SoLuong, H.DonGiaNhap, H.DonGiaBan 
                       FROM tblHang H 
                       INNER JOIN tblChatlieu CL ON H.MaCL = CL.MaCL";

            return DatabaseLayer.GetDataToTable(sql);
        }

        public static void InsertHang(string maHang, string tenHang, string maCL, float soLuong, float donGiaNhap, float donGiaBan, string anh, string ghiChu)
        {
            string sqlCheckKey = "SELECT MaHang FROM " + TableName + " WHERE MaHang = @MaHang";
            SqlParameter[] checkKeyParams = { new SqlParameter("@MaHang", maHang) };

            if (DatabaseLayer.CheckKey(sqlCheckKey, checkKeyParams))
            {
                throw new Exception("Mã hàng đã tồn tại");
            }

            string sqlInsert = "INSERT INTO " + TableName + " (MaHang, TenHang, MaCL, SoLuong, DonGiaNhap, DonGiaBan, Anh, GhiChu) VALUES (@MaHang, @TenHang, @MaCL, @SoLuong, @DonGiaNhap, @DonGiaBan, @Anh, @GhiChu)";
            SqlParameter[] insertParams =
            {
                new SqlParameter("@MaHang", maHang),
                new SqlParameter("@TenHang", tenHang),
                new SqlParameter("@MaCL", maCL),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@DonGiaNhap", donGiaNhap),
                new SqlParameter("@DonGiaBan", donGiaBan),
                new SqlParameter("@Anh", anh),
                new SqlParameter("@GhiChu", ghiChu)
            };

            DatabaseLayer.RunSql(sqlInsert, insertParams);
        }

        public static void UpdateHang(string maHang, string tenHang, string maCL, float soLuong, float donGiaNhap, float donGiaBan, string anh, string ghiChu)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET TenHang = @TenHang, MaCL = @MaCL, SoLuong = @SoLuong, DonGiaNhap = @DonGiaNhap, DonGiaBan = @DonGiaBan, Anh = @Anh, GhiChu = @GhiChu WHERE MaHang = @MaHang";
            SqlParameter[] updateParams =
            {
                new SqlParameter("@TenHang", tenHang),
                new SqlParameter("@MaCL", maCL),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@DonGiaNhap", donGiaNhap),
                new SqlParameter("@DonGiaBan", donGiaBan),
                new SqlParameter("@Anh", anh),
                new SqlParameter("@GhiChu", ghiChu),
                new SqlParameter("@MaHang", maHang)
                };

            DatabaseLayer.RunSql(sqlUpdate, updateParams);
        }

        public static void DeleteHang(string maHang)
        {
            string sqlDelete = "DELETE FROM " + TableName + " WHERE MaHang = @MaHang";
            SqlParameter[] deleteParams = { new SqlParameter("@MaHang", maHang) };

            DatabaseLayer.RunSqlDel(sqlDelete, deleteParams);
        }
    }
}
