using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using QLBH.Database;
using QLBH.DAL;

namespace QLBH.Forms
{
    public partial class frmHang : Form
    {
        private DataTable tblHang;

        public frmHang()
        {
            InitializeComponent();
        }

        private void frmHang_Load(object sender, EventArgs e)
        {
            txtMaHang.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;

            try
            {
                string sqlCBChatLieu = "SELECT MaCL, TenCL FROM tblChatlieu";
                DatabaseLayer.FillCombo(sqlCBChatLieu, cboChatLieu, "MaCL", "TenCL");

                cboChatLieu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu vào ComboBox Chatlieu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadData();
            ResetValues();
        }

        private void LoadData()
        {
            try
            {
                tblHang = HangDAL.GetAllHang();
                dgvHang.DataSource = tblHang;

                dgvHang.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvHang.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvHang.Columns["ChatLieu"].HeaderText = "Chất liệu";
                dgvHang.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvHang.Columns["DonGiaNhap"].HeaderText = "Đơn giá nhập";
                dgvHang.Columns["DonGiaBan"].HeaderText = "Đơn giá bán";

                dgvHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvHang.AllowUserToAddRows = false;
                dgvHang.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới", "Thông báo",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }

            if (tblHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvHang.CurrentRow != null)
            {
                txtMaHang.Text = dgvHang.CurrentRow.Cells["MaHang"].Value.ToString();
                txtTenHang.Text = dgvHang.CurrentRow.Cells["TenHang"].Value.ToString();
                cboChatLieu.Text = dgvHang.CurrentRow.Cells["ChatLieu"].Value.ToString();
                txtSoLuong.Text = dgvHang.CurrentRow.Cells["SoLuong"].Value.ToString();
                txtDonGiaNhap.Text = dgvHang.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
                txtDonGiaBan.Text = dgvHang.CurrentRow.Cells["DonGiaBan"].Value.ToString();

                txtAnh.Text = DatabaseLayer.GetFieldValues("SELECT Anh FROM tblHang WHERE MaHang = N'" + txtMaHang.Text + "'");
                if (!string.IsNullOrEmpty(txtAnh.Text))
                {
                    picAnh.Image = Image.FromFile(txtAnh.Text);
                }
                else
                {
                    picAnh.Image = null;
                }

                txtGhiChu.Text = DatabaseLayer.GetFieldValues("SELECT GhiChu FROM tblHang WHERE MaHang = N'" + txtMaHang.Text + "'");

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;

                EnableNumberField();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;

            btnLuu.Enabled = true;
            ResetValues();

            EnableNumberField();

            txtMaHang.Enabled = true;
            txtMaHang.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maHang = txtMaHang.Text.Trim();
                string tenHang = txtTenHang.Text.Trim();
                string maCL = cboChatLieu.SelectedValue.ToString();
                float soLuong = Convert.ToSingle(txtSoLuong.Text);
                float donGiaNhap = Convert.ToSingle(txtDonGiaNhap.Text);
                float donGiaBan = Convert.ToSingle(txtDonGiaBan.Text);

                string anh = txtAnh.Text;
                string ghiChu = txtGhiChu.Text;

                try
                {
                    HangDAL.InsertHang(maHang, tenHang, maCL, soLuong, donGiaNhap, donGiaBan, anh, ghiChu);
                    MessageBox.Show("Thêm hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnBoQua.Enabled = false;
                    btnLuu.Enabled = false;
                    txtMaHang.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm hàng: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                EnableNumberField();

                string maHang = txtMaHang.Text.Trim();
                string tenHang = txtTenHang.Text.Trim();
                string maCL = cboChatLieu.SelectedValue.ToString();
                float soLuong = Convert.ToSingle(txtSoLuong.Text);
                float donGiaNhap = Convert.ToSingle(txtDonGiaNhap.Text);
                float donGiaBan = Convert.ToSingle(txtDonGiaBan.Text);

                string anh = txtAnh.Text;
                string ghiChu = txtGhiChu.Text;

                try
                {
                    HangDAL.UpdateHang(maHang, tenHang, maCL, soLuong, donGiaNhap, donGiaBan, anh, ghiChu);
                    MessageBox.Show("Cập nhật hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnBoQua.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật hàng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maHang = txtMaHang.Text;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hàng " + maHang + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    HangDAL.DeleteHang(maHang);
                    MessageBox.Show("Xóa hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hàng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png|All Files (*.*)|*.*";
            dlgOpen.InitialDirectory = "E:\\";
            dlgOpen.FilterIndex = 1;
            dlgOpen.Title = "Chọn hình ảnh để hiển thị";

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                picAnh.Image = Image.FromFile(dlgOpen.FileName);
                txtAnh.Text = dlgOpen.FileName;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql = "SELECT H.MaHang, H.TenHang, CL.TenCL AS ChatLieu, H.SoLuong, H.DonGiaNhap, H.DonGiaBan " +
                "FROM tblHang H " +
                "INNER JOIN tblChatlieu CL ON H.MaCL = CL.MaCL " +
                "WHERE 1=1";
            List<string> parameters = new List<string>();
            List<object> values = new List<object>();

            if (!string.IsNullOrWhiteSpace(txtMaHang.Text))
            {
                sql += " AND MaHang LIKE @MaHang";
                parameters.Add("@MaHang");
                values.Add("%" + txtMaHang.Text + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                sql += " AND TenHang LIKE @TenHang";
                parameters.Add("@TenHang");
                values.Add("%" + txtTenHang.Text + "%");
            }

            if (!string.IsNullOrWhiteSpace(cboChatLieu.Text))
            {
                sql += " AND MaCL = @MaCL";
                parameters.Add("@MaCL");
                values.Add(cboChatLieu.SelectedValue);
            }

            try
            {
                tblHang = DatabaseLayer.GetDataToTable(sql, parameters.ToArray(), values.ToArray());

                if (tblHang.Rows.Count == 0)
                {
                    MessageBox.Show("Không có bản ghi thỏa mãn điều kiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Có " + tblHang.Rows.Count + " bản ghi thỏa mãn điều kiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dgvHang.DataSource = tblHang;
                ResetValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            tblHang = HangDAL.GetAllHang();
            dgvHang.DataSource = tblHang;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaHang.Text))
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHang.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenHang.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboChatLieu.Text))
            {
                MessageBox.Show("Bạn phải chọn chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboChatLieu.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDonGiaNhap.Text))
            {
                MessageBox.Show("Bạn phải nhập đơn giá nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDonGiaBan.Text))
            {
                MessageBox.Show("Bạn phải nhập đơn giá bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAnh.Text))
            {
                MessageBox.Show("Bạn phải chọn ảnh minh hoạ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ResetValues()
        {
            txtMaHang.Text = "";
            txtTenHang.Text = "";
            cboChatLieu.Text = "";
            txtSoLuong.Text = "0";
            txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = "0";

            txtSoLuong.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            txtDonGiaBan.Enabled = false;

            txtAnh.Text = "";
            picAnh.Image = null;
            txtGhiChu.Text = "";
        }

        private void EnableNumberField()
        {
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtDonGiaBan.Enabled = true;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaHang_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}
