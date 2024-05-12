using System;
using System.Windows.Forms;

namespace QLBH
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Database.DatabaseLayer.Connect();
        }

        private void mnuChatLieu_Click(object sender, EventArgs e)
        {
            Forms.frmChatLieu frmCL = new Forms.frmChatLieu();
            frmCL.StartPosition = FormStartPosition.CenterScreen;
            frmCL.Show();
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            Forms.frmNhanVien frmNV = new Forms.frmNhanVien();
            frmNV.StartPosition = FormStartPosition.CenterScreen;
            frmNV.Show();
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            Forms.frmKhachHang frmKH = new Forms.frmKhachHang();
            frmKH.StartPosition = FormStartPosition.CenterScreen;
            frmKH.Show();
        }

        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            Forms.frmHang frmHang = new Forms.frmHang();
            frmHang.StartPosition = FormStartPosition.CenterScreen;
            frmHang.Show();
        }

        private void mnuHoaDon_Click(object sender, EventArgs e)
        {
            Forms.frmHDBan frmHDBan = new Forms.frmHDBan();
            frmHDBan.StartPosition = FormStartPosition.CenterScreen;
            frmHDBan.Show();
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Database.DatabaseLayer.Disconnect();
            Application.Exit();
        }
    }
}
