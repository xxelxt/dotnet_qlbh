using System;
using System.Data;
using System.Windows.Forms;
using QLBH.Database;
using QLBH.DAL;
using QLBH.Class;
using System.Drawing;

namespace QLBH.Forms
{
    public partial class frmHDBan : Form
    {
        private DataTable tblCTHDB;

        public frmHDBan()
        {
            InitializeComponent();
        }

        private void frmHDBan_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnIn.Enabled = false;

            txtMaHDB.ReadOnly = true;
            txtTenNV.ReadOnly = true;
            txtTenKH.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtDienThoai.ReadOnly = true;

            txtTenHang.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongTien.ReadOnly = true;

            txtGiamGia.Text = "0";
            txtTongTien.Text = "0";

            string sqlCBKhach = "SELECT MaKhach, TenKhach FROM tblKhach";
            string sqlCBNV = "SELECT MaNV, TenNV FROM tblNhanVien";
            string sqlCBHang = "SELECT MaHang, TenHang FROM tblHang";

            DatabaseLayer.FillCombo(sqlCBKhach, cboMaKH, "MaKhach", "MaKhach");
            DatabaseLayer.FillCombo(sqlCBNV, cboMaNV, "MaNV", "MaNV");
            DatabaseLayer.FillCombo(sqlCBHang, cboMaHang, "MaHang", "MaHang");

            cboMaKH.SelectedIndex = -1;
            cboMaNV.SelectedIndex = -1;
            cboMaHang.SelectedIndex = -1;

            if (txtMaHDB.Text != "")
            {
                LoadDataHD();
                btnXoa.Enabled = true;
                btnIn.Enabled = true;
            }

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string maHDB = txtMaHDB.Text;

                tblCTHDB = HDBanDAL.GetAllHDB(maHDB);
                dgvHDB.DataSource = tblCTHDB;

                dgvHDB.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvHDB.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvHDB.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvHDB.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvHDB.Columns["GiamGia"].HeaderText = "Giảm giá %";
                dgvHDB.Columns["ThanhTien"].HeaderText = "Thành tiền";

                dgvHDB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvHDB.AllowUserToAddRows = false;
                dgvHDB.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataHD()
        {
            string maHDB = txtMaHDB.Text;

            txtNgayBan.Text = Functions.ConvertDateTime(HDBanDAL.GetNgayBanByMaHDB(maHDB));
            cboMaNV.Text = HDBanDAL.GetMaNVByMaHDB(maHDB);
            cboMaKH.Text = HDBanDAL.GetMaKhachByMaHDB(maHDB);
            txtTongTien.Text = HDBanDAL.GetTongTienByMaHDB(maHDB).ToString();

            lblTongTien.Text = "Tổng tiền bằng chữ: " + Functions.ChuyenSoSangChu(txtTongTien.Text);
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnIn.Enabled = false;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHDB.Text = DatabaseLayer.CreateKey("HDB");
            txtNgayBan.Text = DateTime.Now.ToString("dd/MM/yyyy");
            LoadData();
        }

        private void ResetValues()
        {
            txtMaHDB.Text = "";
            txtNgayBan.Text = "";
            cboMaNV.Text = "";
            cboMaKH.Text = "";
            txtTongTien.Text = "0";
            lblTongTien.Text = "Tổng tiền bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maHDB = txtMaHDB.Text.Trim();

            // Kiểm tra mã hóa đơn đã tồn tại hay chưa
            if (!HDBanDAL.CheckMaHDBExists(maHDB))
            {
                // Kiểm tra các thông tin chung của hóa đơn
                if (txtNgayBan.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNgayBan.Focus();
                    return;
                }

                if (cboMaNV.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboMaNV.Focus();
                    return;
                }

                if (cboMaKH.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboMaKH.Focus();
                    return;
                }

                // Lưu thông tin chung vào bảng tblHDB  
                DateTime ngayBan;
                if (!DateTime.TryParse(txtNgayBan.Text.Trim(), out ngayBan))
                {
                    MessageBox.Show("Ngày bán không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNgayBan.Focus();
                    return;
                }

                string maNV = cboMaNV.SelectedValue.ToString();
                string maKH = cboMaKH.SelectedValue.ToString();
                double tongTien = Convert.ToDouble(txtTongTien.Text.Trim());

                HDBanDAL.InsertHDB(maHDB, ngayBan, maNV, maKH, tongTien);
            }

            // Kiểm tra thông tin của các mặt hàng
            string maHang = cboMaHang.SelectedValue.ToString();
            double soLuong;
            if (!double.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng mặt hàng không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }

            double donGia = Convert.ToDouble(txtDonGia.Text);
            double giamGia = Convert.ToDouble(txtGiamGia.Text);
            double thanhTien = donGia * soLuong * (1 - giamGia / 100); // Tính toán thành tiền

            // Kiểm tra xem mã hàng đã có trong chi tiết hóa đơn chưa
            if (HDBanDAL.CheckMaHangInHDB(maHang, maHDB))
            {
                MessageBox.Show("Mã hàng này đã có trong hóa đơn, vui lòng chọn mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetValuesHang();
                cboMaHang.Focus();
                return;
            }

            // Kiểm tra số lượng hàng trong kho còn đủ để cung cấp không
            double soLuongCon = HDBanDAL.GetSoLuongHang(maHang);
            if (soLuong > soLuongCon)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + soLuongCon, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }

            // Chèn chi tiết hóa đơn vào bảng tblCTHDB
            HDBanDAL.InsertCTHDB(maHDB, maHang, soLuong, donGia, giamGia, thanhTien);

            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            double soLuongMoi = soLuongCon - soLuong;
            HDBanDAL.UpdateSoLuongHang(maHang, soLuongMoi);

            // Cập nhật lại tổng tiền cho hóa đơn bán
            double tongTienHDB = HDBanDAL.GetTongTienHDB(maHDB);
            double tongMoi = tongTienHDB + thanhTien;
            HDBanDAL.UpdateTongTienHDB(maHDB, tongMoi);

            // Cập nhật giao diện người dùng
            txtTongTien.Text = tongMoi.ToString();
            lblTongTien.Text = "Tổng tiền bằng chữ: " + Functions.ChuyenSoSangChu(tongMoi.ToString());
            LoadData();
            ResetValuesHang();

            // Bật các nút chức năng sau khi lưu thành công
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnIn.Enabled = true;
        }

        private void ResetValuesHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void dgvHDB_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tblCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maHang = dgvHDB.CurrentRow.Cells["MaHang"].Value.ToString();
                string maHDB = txtMaHDB.Text;

                // Xóa hàng trong chi tiết hóa đơn
                double soLuong = HDBanDAL.GetSoLuongFromCTHDB(maHDB, maHang);
                HDBanDAL.DeleteCTHDB(maHDB, maHang);

                // Cập nhật lại số lượng hàng
                double soLuongHang = HDBanDAL.GetSoLuongFromHang(maHang);
                double newSoLuongHang = soLuongHang + soLuong;
                HDBanDAL.UpdateSoLuongHang(maHang, newSoLuongHang);

                // Cập nhật lại tổng tiền của hóa đơn
                double thanhTien = Convert.ToDouble(dgvHDB.CurrentRow.Cells["ThanhTien"].Value);
                double tongTienHDB = HDBanDAL.GetTongTienByMaHDB(maHDB);
                double newTongTienHDB = tongTienHDB - thanhTien;
                HDBanDAL.UpdateTongTien(maHDB, newTongTienHDB);

                txtTongTien.Text = newTongTienHDB.ToString();
                lblTongTien.Text = "Tổng tiền bằng chữ: " + Functions.ChuyenSoSangChu(newTongTienHDB.ToString());

                LoadData();
            }
        }

        private void DelHang(string maHDB, string maHang)
        {
            // Chuyển sang DAL
        }

        private void DelUpdateTongTien(string maHDB, double thanhTien)
        {
            // Chuyển sang DAL
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Xóa chi tiết hóa đơn
                    HDBanDAL.DeleteCTHDBByMaHDB(txtMaHDB.Text);

                    // Xóa hóa đơn chính
                    HDBanDAL.DeleteHDBByMaHDB(txtMaHDB.Text);

                    ResetValues();
                    LoadData();
                    btnXoa.Enabled = false;
                    btnIn.Enabled = false;

                    MessageBox.Show("Đã xóa hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnIn.Enabled = false;
            btnThem.Enabled = true;
            ResetValues();
            LoadData();
        }

        private void cboMaNV_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaNV.Text))
            {
                txtTenNV.Text = "";
                return;
            }

            txtTenNV.Text = HDBanDAL.GetTenNVByMaNV(cboMaNV.Text);
        }

        private void cboMaKH_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaKH.Text))
            {
                txtTenKH.Text = "";
                txtDiaChi.Text = "";
                txtDienThoai.Text = "";
                return;
            }

            txtTenKH.Text = HDBanDAL.GetTenKHByMaKH(cboMaKH.Text);
            txtDiaChi.Text = HDBanDAL.GetDiaChiByMaKH(cboMaKH.Text);
            txtDienThoai.Text = HDBanDAL.GetDienThoaiByMaKH(cboMaKH.Text);
        }

        private void cboMaHang_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaHang.Text))
            {
                txtTenHang.Text = "";
                txtDonGia.Text = "";
                return;
            }

            txtTenHang.Text = HDBanDAL.GetTenHangByMaHang(cboMaHang.Text);
            txtDonGia.Text = HDBanDAL.GetDonGiaByMaHang(cboMaHang.Text);
        }

        private void TinhThanhTien()
        {
            double sl, gg, dg;

            // Lấy giá trị số lượng (nếu không có thì mặc định là 0)
            sl = string.IsNullOrEmpty(txtSoLuong.Text) ? 0 : Convert.ToDouble(txtSoLuong.Text);

            // Lấy giá trị giảm giá (nếu không có thì mặc định là 0)
            gg = string.IsNullOrEmpty(txtGiamGia.Text) ? 0 : Convert.ToDouble(txtGiamGia.Text);

            // Lấy giá trị đơn giá (nếu không có thì mặc định là 0)
            dg = string.IsNullOrEmpty(txtDonGia.Text) ? 0 : Convert.ToDouble(txtDonGia.Text);

            // Tính toán thành tiền
            double tt = sl * dg - sl * dg * gg / 100;

            // Hiển thị kết quả vào txtThanhTien
            txtThanhTien.Text = tt.ToString();
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                string maHDB = txtMaHDB.Text;
                DataTable tblThongtinHDBan, tblThongtinCTHDB;

                // Lấy thông tin hóa đơn và các mặt hàng
                tblThongtinHDBan = HDBanDAL.GetThongTinHDBan(maHDB);
                tblThongtinCTHDB = HDBanDAL.GetThongTinCTHDB(maHDB);

                // Tạo và hiển thị hóa đơn trong Excel
                ExcelHelper.CreateHoaDonBan(maHDB, tblThongtinHDBan, tblThongtinCTHDB);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHDB.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaHDB.Focus();
                return;
            }
            txtMaHDB.Text = cboMaHDB.Text;
            LoadDataHD();
            LoadData();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            btnIn.Enabled = true;
            cboMaHDB.SelectedIndex = -1;
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtGiamGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboMaHDBan_DropDown(object sender, EventArgs e)
        {
            string sqlCBHDBan = "SELECT MaHDB FROM tblHDB";
            DatabaseLayer.FillCombo(sqlCBHDBan, cboMaHDB, "MaHDB", "MaHDB");
            cboMaHDB.SelectedIndex = -1;
        }

        private void btnNgayBan_Click(object sender, EventArgs e)
        {
            
        }

        private void frmHDBan_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetValues();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}