using System;
using System.Data;
using System.Windows.Forms;
using QLBH.DAL;

namespace QLBH.Forms
{
    public partial class frmChatLieu : Form
    {
        private DataTable tblChatLieu;

        public frmChatLieu()
        {
            InitializeComponent();
        }

        private void frmChatLieu_Load(object sender, EventArgs e)
        {
            txtMaCL.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                tblChatLieu = ChatLieuDAL.GetAllChatLieu();
                dgvChatLieu.DataSource = tblChatLieu;

                dgvChatLieu.Columns["MaCL"].HeaderText = "Mã chất liệu";
                dgvChatLieu.Columns["TenCL"].HeaderText = "Tên chất liệu";

                dgvChatLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // dgvChatLieu.Columns["MaCL"].Width = 100;
                // dgvChatLieu.Columns["TenCL"].Width = 300;

                dgvChatLieu.AllowUserToAddRows = false;
                dgvChatLieu.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvChatLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaCL.Focus();
                return;
            }

            if (tblChatLieu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvChatLieu.CurrentRow != null)
            {
                txtMaCL.Text = dgvChatLieu.CurrentRow.Cells["MaCL"].Value.ToString();
                txtTenCL.Text = dgvChatLieu.CurrentRow.Cells["TenCL"].Value.ToString();

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;
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
            txtMaCL.Enabled = true;
            txtMaCL.Focus();

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maCL = txtMaCL.Text.Trim();
                string tenCL = txtTenCL.Text.Trim();

                try
                {
                    ChatLieuDAL.InsertChatLieu(maCL, tenCL);
                    MessageBox.Show("Thêm chất liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnBoQua.Enabled = false;
                    btnLuu.Enabled = false;
                    txtMaCL.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm chất liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string maCL = txtMaCL.Text.Trim();
                string tenCL = txtTenCL.Text.Trim();

                try
                {
                    ChatLieuDAL.UpdateChatLieu(maCL, tenCL);
                    MessageBox.Show("Cập nhật chất liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetValues();

                    btnBoQua.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật chất liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maCL = txtMaCL.Text.Trim();

            if (!string.IsNullOrEmpty(maCL))
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        ChatLieuDAL.DeleteChatLieu(maCL);
                        MessageBox.Show("Xóa chất liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ResetValues();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa chất liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtMaCL.Enabled = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaCL.Text))
            {
                MessageBox.Show("Bạn phải nhập mã chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaCL.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenCL.Text))
            {
                MessageBox.Show("Bạn phải nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCL.Focus();
                return false;   
            }

            return true;
        }

        private void ResetValues()
        {
            txtMaCL.Text = "";
            txtTenCL.Text = "";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaCL_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}