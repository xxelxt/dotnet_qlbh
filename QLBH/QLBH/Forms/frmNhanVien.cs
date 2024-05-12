using System;
using System.Data;
using System.Windows.Forms;
using QLBH.DAL;
using QLBH.Class;

namespace QLBH.Forms
{
    public partial class frmNhanVien : Form
    {
        private DataTable tblNhanVien;

        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            txtMaNV.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                tblNhanVien = NhanVienDAL.GetAllNhanVien();
                dgvNhanVien.DataSource = tblNhanVien;

                dgvNhanVien.Columns["MaNV"].HeaderText = "Mã nhân viên";
                dgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
                dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dgvNhanVien.Columns["DienThoai"].HeaderText = "Điện thoại";
                dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";

                dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvNhanVien.AllowUserToAddRows = false;
                dgvNhanVien.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới", "Thông báo",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNV.Focus();
                return;
            }

            if (tblNhanVien.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaNV.Text = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            txtTenNV.Text = dgvNhanVien.CurrentRow.Cells["TenNV"].Value.ToString();
            
            if (dgvNhanVien.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam")
                chkGioiTinhNam.Checked = true;
            else
                chkGioiTinhNam.Checked = false;
            
            txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells["DiaChi"].Value.ToString();
            mskDienThoai.Text = dgvNhanVien.CurrentRow.Cells["DienThoai"].Value.ToString();
            mskNgaySinh.Text = dgvNhanVien.CurrentRow.Cells["NgaySinh"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;

            ResetValues();
            txtMaNV.Enabled = true;
            txtMaNV.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maNV = txtMaNV.Text.Trim();
                string tenNV = txtTenNV.Text.Trim();
                string gioiTinh = chkGioiTinhNam.Checked ? "Nam" : "Nữ";
                string diaChi = txtDiaChi.Text.Trim();
                string dienThoai = mskDienThoai.Text.Trim();
                string ngaySinh = Functions.ConvertDateTime(mskNgaySinh.Text);

                try
                {
                    NhanVienDAL.InsertNhanVien(maNV, tenNV, gioiTinh, diaChi, dienThoai, ngaySinh);
                    MessageBox.Show("Lưu thông tin nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnBoQua.Enabled = false;
                    btnLuu.Enabled = false;
                    txtMaNV.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu thông tin nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maNV = txtMaNV.Text.Trim();
                string tenNV = txtTenNV.Text.Trim();
                string gioiTinh = chkGioiTinhNam.Checked ? "Nam" : "Nữ";
                string diaChi = txtDiaChi.Text.Trim();
                string dienThoai = mskDienThoai.Text.Trim();
                string ngaySinh = Functions.ConvertDateTime(mskNgaySinh.Text);

                try
                {
                    NhanVienDAL.UpdateNhanVien(maNV, tenNV, gioiTinh, diaChi, dienThoai, ngaySinh);
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnBoQua.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();

            if (!string.IsNullOrEmpty(maNV))
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        NhanVienDAL.DeleteNhanVien(maNV);
                        MessageBox.Show("Xóa nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ResetValues();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaNV.Enabled = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }

            if (mskDienThoai.Text == "(   )     ")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskDienThoai.Focus();
                return false;
            }

            if (mskNgaySinh.Text == "  /  /")
            {
                MessageBox.Show("Bạn phải nhập ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskNgaySinh.Focus();
                return false;
            }

            if (!Functions.IsDate(mskNgaySinh.Text))
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskNgaySinh.Text = "";
                mskNgaySinh.Focus();
                return false;
            }

            return true;
        }

        private void ResetValues()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            chkGioiTinhNam.Checked = true;
            txtDiaChi.Text = "";
            mskDienThoai.Text = "";
            mskNgaySinh.Text = "";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}
