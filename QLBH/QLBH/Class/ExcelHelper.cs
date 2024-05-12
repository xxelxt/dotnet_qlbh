using System;
using System.Data;
using Microsoft.Office.Interop.Excel;

namespace QLBH.Class
{
    public static class ExcelHelper
    {
        public static void CreateHoaDonBan(string maHDB, System.Data.DataTable tblThongtinHD, System.Data.DataTable tblThongtinHang)
        {
            Application exApp = new Application();
            Workbook exBook;
            Worksheet exSheet;
            Range exRange;

            exBook = exApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            exSheet = (Worksheet)exBook.Worksheets[1];

            // Định dạng tiêu đề và thông tin chung của hóa đơn
            exRange = exSheet.Range["A1:F3"];
            exRange.Merge();
            exRange.Font.Size = 14;
            exRange.Font.Bold = true;
            exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            exRange.Value = "HÓA ĐƠN BÁN HÀNG";

            if (tblThongtinHD.Rows.Count > 0)
            {
                exRange = exSheet.Range["A4:F4"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Mã hóa đơn: {tblThongtinHD.Rows[0]["MaHDB"]}";

                exRange = exSheet.Range["A5:F5"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Ngày bán: {Convert.ToDateTime(tblThongtinHD.Rows[0]["NgayBan"]).ToString("dd/MM/yyyy")}";

                exRange = exSheet.Range["A6:F6"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Khách hàng: {tblThongtinHD.Rows[0]["TenKhach"]}";

                exRange = exSheet.Range["A7:F7"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Địa chỉ: {tblThongtinHD.Rows[0]["DiaChi"]}";

                exRange = exSheet.Range["A8:F8"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Điện thoại: {tblThongtinHD.Rows[0]["DienThoai"]}";
            }

            // Định dạng bảng thông tin chi tiết hàng hóa
            exRange = exSheet.Range["A10:F10"];
            exRange.Merge();
            exRange.Font.Size = 12;
            exRange.Font.Bold = true;
            exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            exRange.Value = "Thông tin chi tiết hàng hóa";

            exSheet.Cells[11, 1] = "STT";
            exSheet.Cells[11, 2] = "Tên hàng";
            exSheet.Cells[11, 3] = "Số lượng";
            exSheet.Cells[11, 4] = "Đơn giá";
            exSheet.Cells[11, 5] = "Giảm giá";
            exSheet.Cells[11, 6] = "Thành tiền";

            int rowIndex = 12;
            int stt = 1;
            foreach (DataRow row in tblThongtinHang.Rows)
            {
                exSheet.Cells[rowIndex, 1] = stt++;
                exSheet.Cells[rowIndex, 2] = row["TenHang"];
                exSheet.Cells[rowIndex, 3] = row["SoLuong"];
                exSheet.Cells[rowIndex, 4] = row["DonGia"];
                exSheet.Cells[rowIndex, 5] = row["GiamGia"];
                exSheet.Cells[rowIndex, 6] = row["ThanhTien"];
                rowIndex++;
            }

            // Tính toán và in tổng tiền
            if (tblThongtinHD.Rows.Count > 0)
            {
                exRange = exSheet.Range[$"D{rowIndex + 1}:F{rowIndex + 1}"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Bold = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignRight;
                exRange.Value = "Tổng tiền: " + tblThongtinHD.Rows[0]["TongTien"];

                exRange = exSheet.Range[$"A{rowIndex + 2}:F{rowIndex + 2}"];
                exRange.Merge();
                exRange.Font.Size = 12;
                exRange.Font.Italic = true;
                exRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                exRange.Value = $"Bằng chữ: {Functions.ChuyenSoSangChu(tblThongtinHD.Rows[0]["TongTien"].ToString())}";
            }

            exApp.Visible = true;

            // Lưu file Excel
            string filePath = $"E:\\rkive\\6\\.NET\\ON_CLASS\\Excel\\{tblThongtinHD.Rows[0]["MaHDB"]}.xlsx"; // Đường dẫn lưu file
            exBook.SaveAs(filePath); // Lưu file Excel
            exApp.Visible = true; // Hiển thị file Excel

            // Giải phóng tài nguyên COM
            ReleaseObject(exSheet);
            ReleaseObject(exBook);
            ReleaseObject(exApp);
        }

        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
