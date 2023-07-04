using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
namespace GUI.Forms
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        public static int Quyen;
        public static int TenDangNhap;

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            XuLyDangNhap();
        }
        private void XuLyDangNhap()
        {
            Cursor = Cursors.AppStarting;
            if (TAIKHOANBL.GetInstance.KiemTraDangNhap(int.Parse(txtTenDangNhap.Text), txtMatKhau.Text) == true)
            {
                frmMain frm = new frmMain();
                btnDangNhap.BackColor = Color.FromArgb(0, 100, 0);
                TenDangNhap = int.Parse(txtTenDangNhap.Text);
                if (txtTenDangNhap.Text != "1")
                    frm.btnNhanVien.Enabled  = false;
                frm.lbTenNV.Text = TAIKHOANBL.GetInstance.getTenTaiKhoan(int.Parse(txtTenDangNhap.Text))+" - Nguyễn Hồng Sơn - Nguyễn Hoàng Thiên Bảo";
                txtMatKhau.Text = "";
                txtTenDangNhap.Text = "";
                Cursor = Cursors.Default;
                frm.Show();
                this.Hide();
            }
            else
            {
                Cursor = Cursors.Default;
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Tên đăng nhập hoặc mật khẩu không đúng!\nVui lòng nhập lại...";
                frm.Show();
            }
        }
        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = '●';
            btnENTITY.PerformClick();
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            if (btnShowPass.ImageIndex == 0)
            {
                btnShowPass.ImageIndex = 1;
                txtMatKhau.Focus();
            }
            else
            {
                btnShowPass.ImageIndex = 0;
                txtMatKhau.Focus();
            }
            if (txtMatKhau.PasswordChar == '●')
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '●';
            }
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text != "" && txtMatKhau.Text != "")
            {
                btnDangNhap.BackColor = Color.FromArgb(0, 122, 204);
            }
            else
            {
                btnDangNhap.BackColor = Color.DimGray;
            }
        }

        private void txtTenDangNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XuLyDangNhap();
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XuLyDangNhap();
            }
        }

        private void btnENTITY_Click(object sender, EventArgs e)
        {
            try
            {
                lblDangKhoiTao.Visible = true;
                lblDangKhoiTao.ForeColor = Color.LightGreen;
                //lblDangKhoiTao.ForeColor = Color.Red;
                lblDangKhoiTao.Text = "Khởi tạo thành công!!!";
                DataRun.GetInstance.themdulieu();
            }
            catch (Exception)
            {

                
            }
        }
    }
}
