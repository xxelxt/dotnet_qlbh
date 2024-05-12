using System;
using System.Data;
using System.Data.SqlClient;
using QLBH.Class;
using QLBH.Database;

namespace QLBH.DAL
{
    internal class HDBanDAL
    {
        public static DataTable GetAllHDB(string maHDB)
        {
            string sql = "SELECT A.MaHang, B.TenHang, A.SoLuong, A.DonGia, A.GiamGia, A.ThanhTien " +
                "FROM tblCTHDB AS A INNER JOIN tblHang AS B ON A.MaHang = B.MaHang WHERE A.MaHDB = @MaHDB";
            SqlParameter[] sqlParams = { new SqlParameter("@MaHDB", maHDB) };
            
            return DatabaseLayer.GetDataToTable(sql, sqlParams);
        }

        public static string GetNgayBanByMaHDB(string maHDB)
        {
            string sql = "SELECT NgayBan FROM tblHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = { new SqlParameter("@MaHDB", maHDB) };

            object result = DatabaseLayer.GetFieldValues(sql, sqlParams);

            if (result != null && result != DBNull.Value && result is DateTime)
            {
                DateTime ngayBan = (DateTime)result;

                return ngayBan.ToString("dd/MM/yyyy");
            }
            else if (result != null && result != DBNull.Value && result is string)
            {
                return (string)result;
            }

            return string.Empty;
        }

        public static string GetMaNVByMaHDB(string maHDB)
        {
            string sql = "SELECT MaNV FROM tblHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = { new SqlParameter("@MaHDB", maHDB) };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetMaKhachByMaHDB(string maHDB)
        {
            string sql = "SELECT MaKhach FROM tblHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = { new SqlParameter("@MaHDB", maHDB) };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static double GetTongTienByMaHDB(string maHDB)
        {
            string sql = "SELECT TongTien FROM tblHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = { new SqlParameter("@MaHDB", maHDB) };

            object result = DatabaseLayer.GetFieldValues(sql, sqlParams);
            return Convert.ToDouble(result);
        }

        public static double GetSoLuongFromCTHDB(string maHDB, string maHang)
        {
            string sql = "SELECT SoLuong FROM tblCTHDB WHERE MaHDB = @MaHDB AND MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB),
                new SqlParameter("@MaHang", maHang)
            };

            object result = DatabaseLayer.GetFieldValues(sql, sqlParams);
            return Convert.ToDouble(result);
        }

        public static void DeleteCTHDB(string maHDB, string maHang)
        {
            string sql = "DELETE FROM tblCTHDB WHERE MaHDB = @MaHDB AND MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB),
                new SqlParameter("@MaHang", maHang)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static double GetSoLuongFromHang(string maHang)
        {
            string sql = "SELECT SoLuong FROM tblHang WHERE MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHang", maHang)
            };

            object result = DatabaseLayer.GetFieldValues(sql, sqlParams);
            return Convert.ToDouble(result);
        }

        public static void UpdateTongTien(string maHDB, double newTongTien)
        {
            string sql = "UPDATE tblHDB SET TongTien = @TongTien WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = {
                new SqlParameter("@TongTien", newTongTien),
                new SqlParameter("@MaHDB", maHDB)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static void DeleteCTHDBByMaHDB(string maHDB)
        {
            string sql = "DELETE FROM tblCTHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static void DeleteHDBByMaHDB(string maHDB)
        {
            string sql = "DELETE FROM tblHDB WHERE MaHDB = @MaHDB";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static string GetTenNVByMaNV(string maNV)
        {
            string sql = "SELECT TenNV FROM tblNhanVien WHERE MaNV = @MaNV";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaNV", maNV)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetTenKHByMaKH(string maKH)
        {
            string sql = "SELECT TenKhach FROM tblKhach WHERE MaKhach = @MaKH";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaKH", maKH)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetDiaChiByMaKH(string maKH)
        {
            string sql = "SELECT DiaChi FROM tblKhach WHERE MaKhach = @MaKH";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaKH", maKH)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetDienThoaiByMaKH(string maKH)
        {
            string sql = "SELECT DienThoai FROM tblKhach WHERE MaKhach = @MaKH";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaKH", maKH)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetTenHangByMaHang(string maHang)
        {
            string sql = "SELECT TenHang FROM tblHang WHERE MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHang", maHang)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static string GetDonGiaByMaHang(string maHang)
        {
            string sql = "SELECT DonGiaBan FROM tblHang WHERE MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHang", maHang)
            };

            return DatabaseLayer.GetFieldValues(sql, sqlParams).ToString();
        }

        public static DataTable GetThongTinHDBan(string maHDB)
        {
            string sql = @"SELECT a.MaHDB, a.NgayBan, a.TongTien, b.TenKhach, b.DiaChi, b.DienThoai, c.TenNV
                   FROM tblHDB AS a
                   JOIN tblKhach AS b ON a.MaKhach = b.MaKhach
                   JOIN tblNhanVien AS c ON a.MaNV = c.MaNV
                   WHERE a.MaHDB = @MaHDB";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            return DatabaseLayer.GetDataToTable(sql, sqlParams);
        }

        public static DataTable GetThongTinCTHDB(string maHDB)
        {
            string sql = @"SELECT b.TenHang, a.SoLuong, a.DonGia, a.GiamGia, a.ThanhTien
                   FROM tblCTHDB AS a
                   JOIN tblHang AS b ON a.MaHang = b.MaHang
                   WHERE a.MaHDB = @MaHDB";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            return DatabaseLayer.GetDataToTable(sql, sqlParams);
        }

        public static bool CheckMaHDBExists(string maHDB)
        {
            string sql = "SELECT MaHDB FROM tblHDB WHERE MaHDB = @MaHDB";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            return DatabaseLayer.CheckKey(sql, sqlParams);
        }

        public static bool CheckMaHangInHDB(string maHang, string maHDB)
        {
            string sql = "SELECT MaHang FROM tblCTHDB WHERE MaHang = @MaHang AND MaHDB = @MaHDB";
            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHang", maHang),
                new SqlParameter("@MaHDB", maHDB)
            };

            return DatabaseLayer.CheckKey(sql, sqlParams);
        }

        public static double GetSoLuongHang(string maHang)
        {
            string sql = "SELECT Soluong FROM tblHang WHERE MaHang = @MaHang";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHang", maHang)
            };

            return Convert.ToDouble(DatabaseLayer.GetFieldValues(sql, sqlParams));
        }

        public static void UpdateSoLuongHang(string maHang, double soLuong)
        {
            string sql = "UPDATE tblHang SET SoLuong = @SoLuong WHERE MaHang = @MaHang";
            SqlParameter[] sqlParams = {
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@MaHang", maHang)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static void InsertHDB(string maHDB, DateTime ngayBan, string maNV, string maKhach, double tongTien)
        {
            string sql = @"INSERT INTO tblHDB (MaHDB, NgayBan, MaNV, MaKhach, TongTien) 
                   VALUES (@MaHDB, @NgayBan, @MaNV, @MaKhach, @TongTien)";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB),
                new SqlParameter("@NgayBan", ngayBan),
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@MaKhach", maKhach),
                new SqlParameter("@TongTien", tongTien)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }


        public static void InsertCTHDB(string maHDB, string maHang, double soLuong, double donGia, double giamGia, double thanhTien)
        {
            string sql = @"INSERT INTO tblCTHDB (MaHDB, MaHang, SoLuong, DonGia, GiamGia, ThanhTien) 
                       VALUES (@MaHDB, @MaHang, @SoLuong, @DonGia, @GiamGia, @ThanhTien)";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB),
                new SqlParameter("@MaHang", maHang),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@DonGia", donGia),
                new SqlParameter("@GiamGia", giamGia),
                new SqlParameter("@ThanhTien", thanhTien)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }

        public static double GetTongTienHDB(string maHDB)
        {
            string sql = "SELECT TongTien FROM tblHDB WHERE MaHDB = @MaHDB";

            SqlParameter[] sqlParams = {
                new SqlParameter("@MaHDB", maHDB)
            };

            return Convert.ToDouble(DatabaseLayer.GetFieldValues(sql, sqlParams));
        }

        public static void UpdateTongTienHDB(string maHDB, double tongTienMoi)
        {
            string sql = "UPDATE tblHDB SET TongTien = @TongTienMoi WHERE MaHDB = @MaHDB";

            SqlParameter[] sqlParams = {
                new SqlParameter("@TongTienMoi", tongTienMoi),
                new SqlParameter("@MaHDB", maHDB)
            };

            DatabaseLayer.RunSql(sql, sqlParams);
        }
    }
}
