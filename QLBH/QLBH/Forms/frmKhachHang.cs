using System;
using System.Data;
using System.Windows.Forms;
using QLBH.DAL;

namespace QLBH.Forms
{
    public partial class frmKhachHang : Form
    {
        private DataTable tblKhach;
        
        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            txtMaKH.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                tblKhach = KhachHangDAL.GetAllKhachHang();
                dgvKhachHang.DataSource = tblKhach;

                dgvKhachHang.Columns["MaKhach"].HeaderText = "Mã khách hàng";
                dgvKhachHang.Columns["TenKhach"].HeaderText = "Tên khách hàng";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dgvKhachHang.Columns["DienThoai"].HeaderText = "Điện thoại";

                dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvKhachHang.AllowUserToAddRows = false;
                dgvKhachHang.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới", "Thông báo",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKH.Focus();
                return;
            }

            if (tblKhach.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaKH.Text = dgvKhachHang.CurrentRow.Cells["MaKhach"].Value.ToString();
            txtTenKH.Text = dgvKhachHang.CurrentRow.Cells["TenKhach"].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
            mskDienThoai.Text = dgvKhachHang.CurrentRow.Cells["DienThoai"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;

            ResetValues();
            txtMaKH.Enabled = true;
            txtMaKH.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maKH = txtMaKH.Text.Trim();
                string tenKH = txtTenKH.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string dienThoai = mskDienThoai.Text.Trim();

                try
                {
                    KhachHangDAL.InsertKhachHang(maKH, tenKH, diaChi, dienThoai);
                    MessageBox.Show("Lưu thông tin khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnXoa.Enabled = true;
                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnBoQua.Enabled = false;
                    btnLuu.Enabled = false;
                    txtMaKH.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu thông tin khách hàng: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maKH = txtMaKH.Text.Trim();
                string tenKH = txtTenKH.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string dienThoai = mskDienThoai.Text.Trim(); 
                
                try
                {
                    KhachHangDAL.UpdateKhachHang(maKH, tenKH, diaChi, dienThoai);
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnBoQua.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin khách hàng: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text.Trim();

            if (!string.IsNullOrEmpty(maKH))
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        KhachHangDAL.DeleteKhachHang(maKH);
                        MessageBox.Show("Xóa khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ResetValues();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtMaKH.Enabled = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtMaKH.Text))
            {
                MessageBox.Show("Bạn phải nhập mã khách hàng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaKH.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenKH.Text))
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenKH.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mskDienThoai.Text))
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mskDienThoai.Focus();
                return false;
            }

            return true;
        }

        private void ResetValues()
        {
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiaChi.Text = "";
            mskDienThoai.Text = "";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaKH_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}
