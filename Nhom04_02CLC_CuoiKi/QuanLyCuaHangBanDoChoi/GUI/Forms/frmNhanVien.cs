using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
namespace GUI.Forms
{
    public partial class frmNhanVien : Form
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }
        int manv = 0;
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            LoadDataGridViewTheoBoLoc();
            cboGioiTinh.SelectedIndex = 0;
        }
        private void LoadDataGridViewTheoBoLoc()
        {
            dgvNhanVien.DataSource = NHANVIENBL.GetInstance.GetDanhSachNhanVienTheoBoLoc();
            dgvNhanVien.ClearSelection();
        }
        private void ResetColorControls()
        {
            foreach (Control ctrl in pnlThongTinNhanVien.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.BackColor == Color.OrangeRed)
                    {
                        ctrl.BackColor = Color.White;
                    }
                }
            }
            if (picHinhAnh.BackColor == Color.OrangeRed)
            {
                picHinhAnh.BackColor = Color.White;
            }
        }
        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVien.SelectedRows.Count == 1)
                {
                    ResetColorControls();
                    DataGridViewRow dr = dgvNhanVien.SelectedRows[0];
                    manv = int.Parse(dr.Cells["Mã NV"].Value.ToString().Trim());
                    txtTen.Text = dr.Cells["Tên NV"].Value.ToString().Trim();
                    dateNgaySinh.Value = Convert.ToDateTime(dr.Cells["Ngày Sinh"].Value);
                    txtSoDienThoai.Text = dr.Cells["SĐT"].Value.ToString().Trim();
                    MemoryStream ms = new MemoryStream((byte[])dgvNhanVien.CurrentRow.Cells["Hình Ảnh"].Value‌​);
                    picHinhAnh.Image = Image.FromStream(ms);
                    cboGioiTinh.SelectedItem = dr.Cells["Giới Tính"].Value.ToString();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        private void LamMoi()
        {
            txtTen.Clear();
            txtSoDienThoai.Clear();
            cboGioiTinh.SelectedIndex = 0;
            dateNgaySinh.Value = DateTime.Now;
            picHinhAnh.Image = null;
            ResetColorControls();
        }

        private void btnLamMoiThongTin_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnNgungKinhDoanh_Click(object sender, EventArgs e)
        {
            NHANVIENBL.GetInstance.ThoiViecNhanVien(manv);
            LoadDataGridViewTheoBoLoc();
        }
        private bool CheckControls()
        {
            int r = 0;
            foreach (Control ctrl in pnlThongTinNhanVien.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Text == "")
                    {
                        ctrl.BackColor = Color.OrangeRed;
                        r = 1;
                    }
                }
            }
            if (picHinhAnh.Image == null)
            {
                r = 1;
                picHinhAnh.BackColor = Color.OrangeRed;
            }
            if (r == 0)
                return true;
            return false;
        }
        private bool CheckDate()
        {
            if (dateNgaySinh.Value >= DateTime.Now)
            {
                return false;
            }
            return true;
        }
        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (CheckControls())
            {
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length <= 12 && txtSoDienThoai.Text.Length >= 10)
                    {
                        if (CheckDate())
                        {
                            NHANVIEN nv = new NHANVIEN();
                            nv.HOTEN = txtTen.Text;
                            if (cboGioiTinh.Text == "Nam")
                                nv.GIOITINH = true;
                            else
                                nv.GIOITINH = false;
                            nv.NGAYSINH = dateNgaySinh.Value.ToString();
                            nv.SDT = txtSoDienThoai.Text;
                            Image img = picHinhAnh.Image;
                            nv.HINHANH = ImageToByteArray(img);
                            if (NHANVIENBL.GetInstance.ThemNhanVien(nv))
                            {
                                LoadDataGridViewTheoBoLoc();
                                //LamMoi();
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Mã nhân viên đã tồn tại";
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải từ 10 đến 12 số!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Họ tên chỉ được tối đa 50 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin nhân viên";
                frm.ShowDialog();
            }
        }

        private void picHinhAnh_Click(object sender, EventArgs e)
        {
            if (picHinhAnh.BackColor == Color.OrangeRed)
            {
                picHinhAnh.BackColor = Color.White;
            }
            frmLoadImage frm = new frmLoadImage();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                picHinhAnh.Image = frm.img;
            }
        }

        private void btnCapNhatSP_Click(object sender, EventArgs e)
        {
            if (CheckControls())
            {
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length >= 10 && txtSoDienThoai.Text.Length <= 12)
                    {
                        if (CheckDate())
                        {
                            NHANVIEN nvDTO = new NHANVIEN();
                            nvDTO.MANV = manv;
                            nvDTO.HOTEN = txtTen.Text;
                            if (cboGioiTinh.Text == "Nam")
                                nvDTO.GIOITINH = true;
                            else
                                nvDTO.GIOITINH = false;
                            nvDTO.NGAYSINH = dateNgaySinh.Value.ToString();
                            nvDTO.SDT = txtSoDienThoai.Text;
                            Image img = picHinhAnh.Image;
                            nvDTO.HINHANH = ImageToByteArray(img);
                            if (NHANVIENBL.GetInstance.SuaThongTinNhanVien(nvDTO))
                            {
                                LoadDataGridViewTheoBoLoc();
                                LamMoi();
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Ngày sinh không hợp lệ";
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải từ 10 đến 12 số";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Họ tên chỉ được tối đa 50 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin nhân viên";
                frm.ShowDialog();
            }
        }

        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            if (txtTenNV.Text != "")
            {
                dgvNhanVien.DataSource = NHANVIENBL.GetInstance.GetDanhSachNhanVienTimKiem(txtTenNV.Text);
                dgvNhanVien.ClearSelection();
            }
            else
            {
                LoadDataGridViewTheoBoLoc();
            }
        }
    }
}
