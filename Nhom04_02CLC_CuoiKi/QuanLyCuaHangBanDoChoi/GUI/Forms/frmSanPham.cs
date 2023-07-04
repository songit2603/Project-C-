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
    public partial class frmSanPham : Form
    {
        public frmSanPham()
        {
            InitializeComponent();
        }
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadCboLoaiSP();
            LoadDataGridViewTheoBoLoc();
        }
        private void LoadCboLoaiSP()
        {
            cboLoai.DataSource = LOAISANPHAMBL.GetInstance.getDsLoaiSanPham();
            cboLoai.DisplayMember = "TENLOAISP";
            cboLoai.ValueMember = "MALOAISP";
        }

        private void LoadDataGridViewTheoBoLoc()
        {
            dgvSanPham.DataSource = SANPHAMBL.GetInstance.GetDanhSachSanPhamTheoBoLoc();
            dgvSanPham.ClearSelection();
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
        private bool CheckControls()
        {
            int r = 0;
            foreach (Control ctrl in pnlThongTinSanPham.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Text == "" && ctrl.Name != "txtGiaBan")
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
        private void LamMoi()
        {
            txtTen.Clear();
            txtSoLuong.Clear();
            txtKhuyenMai.Clear();
            txtGiaBan.Clear();
            txtSoLuong.Clear();
            txtGiaNhap.Clear();
            txtLoiNhuan.Clear();
            picHinhAnh.Image = null;
            ResetColorControls();

        }

        public byte[] ImageToByteArray(Image imageIn)
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
                if (txtTen.Text.Length < 200)
                {
                    if (int.Parse(txtKhuyenMai.Text) < 100)
                    {
                        if (int.Parse(txtLoiNhuan.Text) > 0)
                        {
                            SANPHAM sp = new SANPHAM();
                            sp.TENSP = txtTen.Text;
                            sp.MALOAISP = cboLoai.SelectedValue.ToString().Trim();
                            sp.SOLUONG= int.Parse(txtSoLuong.Text);
                            sp.KHUYENMAI = int.Parse(txtKhuyenMai.Text);
                            sp.DONGIANHAP = double.Parse(txtGiaNhap.Text);
                            sp.LOINHUAN = int.Parse(txtLoiNhuan.Text);
                            sp.DONGIABAN = double.Parse(txtGiaBan.Text);
                            Image img = picHinhAnh.Image;
                            sp.HINHANH = ImageToByteArray(img);
                            frmThongBao frm = new frmThongBao();
                            try
                            {
                                SANPHAMBL.GetInstance.themSanPham(sp);
                                frm.lblThongBao.Text = "Thêm thành công";
                                frm.ShowDialog();
                                LoadDataGridViewTheoBoLoc();
                                LamMoi();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                frm.lblThongBao.Text = "Lợi nhuận phải lớn hơn 0%!";
                                frm.ShowDialog();
                            }
                        }

                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Khuyến mãi phải nhỏ hơn 100%!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Tên sản phẩm tối đa 200 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin sản phẩm";
                frm.ShowDialog();
            }
        }

      

        private void txtGiaBan_TextChanged(object sender, EventArgs e)
        {
            if (txtGiaBan.Text != "")
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                double value = double.Parse(txtGiaBan.Text, System.Globalization.NumberStyles.AllowThousands);
                txtGiaBan.Text = String.Format(culture, "{0:N0}", value);
                txtGiaBan.Select(txtGiaBan.Text.Length, 0);
            }
        }
        private string ConvertTien(double gia)
        {
            string giaban = gia.ToString();
            string result = "";
            int d = 0;
            for (int i = giaban.Length - 1; i >= 0; i--)
            {
                d++;
                result += giaban[i];
                if (d == 3 && i != 0)
                {
                    result += ',';
                    d = 0;
                }
            }
            char[] charArray = result.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        private void txtLoiNhuan_TextChanged(object sender, EventArgs e)
        {

        }
        int masp = 0;

        private void btnCapNhatSP_Click(object sender, EventArgs e)
        {
            if (CheckControls())
            {
                if (txtTen.Text.Length < 200)
                {
                    if (int.Parse(txtKhuyenMai.Text) < 100)
                    {
                        if (int.Parse(txtLoiNhuan.Text) > 0)
                        {
                            SANPHAM sp = new SANPHAM();
                            sp.MASP = masp;
                            sp.TENSP = txtTen.Text;
                            sp.MALOAISP = cboLoai.SelectedValue.ToString().Trim();
                            sp.SOLUONG = int.Parse(txtSoLuong.Text);
                            sp.KHUYENMAI = int.Parse(txtKhuyenMai.Text);
                            sp.DONGIANHAP = double.Parse(txtGiaNhap.Text);
                            sp.LOINHUAN = int.Parse(txtLoiNhuan.Text);
                            sp.DONGIABAN = double.Parse(txtGiaBan.Text);
                            Image img = picHinhAnh.Image;
                            sp.HINHANH = ImageToByteArray(img);
                            frmThongBao frm = new frmThongBao();
                            try
                            {
                                SANPHAMBL.GetInstance.SuaSanPham(sp);
                                LoadDataGridViewTheoBoLoc();
                                LamMoi();
                                frm.lblThongBao.Text = "Cập nhật thành công";
                                frm.ShowDialog();
                            }
                            catch (Exception ex)
                            {
                                frm.lblThongBao.Text = "Cập nhật thất bại";
                                frm.ShowDialog();
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Khuyến mãi phải nhỏ hơn 100%!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Tên sản phẩm tối đa 200 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin sản phẩm";
                frm.ShowDialog();
            }
        }

        private void btnNgungKinhDoanh_Click(object sender, EventArgs e)
        {
            if (CheckControls())
            {
                frmThongBao frm = new frmThongBao();
                try
                {
                    SANPHAMBL.GetInstance.NgungKinhDoanhSanPham(masp);
                    LoadDataGridViewTheoBoLoc();
                    LamMoi();
                    frm.lblThongBao.Text = "xóa thành công";
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    frm.lblThongBao.Text = "xóa thất bại";
                    frm.ShowDialog();
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa chọn sản phẩm cần ngừng kinh doanh!";
                frm.ShowDialog();
            }
        }
        private void ResetColorControls()
        {
            foreach (Control ctrl in pnlThongTinSanPham.Controls)
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
        private void btnLamMoiThongTin_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

        }

        private void btnApDung_Click(object sender, EventArgs e)
        {
            dgvSanPham.DataSource = SANPHAMBL.GetInstance.GetDanhSachSanPhamTheoBoLoc();
            dgvSanPham.ClearSelection();
        }


        private void txtGiaNhap_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGiaNhap.Text != "")
                {
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                    double value = double.Parse(txtGiaNhap.Text, System.Globalization.NumberStyles.AllowThousands);
                    txtGiaNhap.Text = String.Format(culture, "{0:N0}", value);
                    txtGiaNhap.Select(txtGiaNhap.Text.Length, 0);
                }
            }
            catch (Exception)
            {

                
            }
        }

        private void txtLoiNhuan_TextChanged_1(object sender, EventArgs e)
        {

            try
            {
                if (txtLoiNhuan.Text == "")
                {
                    txtGiaBan.Clear();
                    return;
                }
                if (int.Parse(txtLoiNhuan.Text) > 0)
                {
                    int n = int.Parse(txtGiaNhap.Text.Replace(",", "")) + ((int.Parse(txtGiaNhap.Text.Replace(",", "")) * int.Parse(txtLoiNhuan.Text.Replace(",", "")) / 100));
                    txtGiaBan.Text = ConvertTien((double)n);
                }
                else
                {
                    txtGiaBan.Clear();
                }
            }
            catch (Exception)
            {

               
            }
        }

        private void txtTen_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void txtKhuyenMai_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void txtGiaNhap_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void txtLoiNhuan_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void txtSoLuong_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void btn_TatCa_Click(object sender, EventArgs e)
        {
            LoadDataGridViewTheoBoLoc();
        }

        private void dgvSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPham.SelectedRows.Count == 1)
                {
                    ResetColorControls();
                    DataGridViewRow dr = dgvSanPham.SelectedRows[0];
                    masp = int.Parse(dr.Cells["Mã SP"].Value.ToString().Trim());
                    txtTen.Text = dr.Cells["Tên SP"].Value.ToString().Trim();
                    cboLoai.SelectedValue = dr.Cells["Mã Loại SP"].Value.ToString();
                    txtGiaNhap.Text = dr.Cells["Đơn Giá Nhập"].Value.ToString().Trim();
                    txtLoiNhuan.Text = dr.Cells["Lợi Nhuận"].Value.ToString().Trim();
                    txtGiaBan.Text = dr.Cells["Đơn Giá Bán"].Value.ToString().Trim();
                    txtSoLuong.Text = dr.Cells["Số Lượng"].Value.ToString().Trim();
                    txtKhuyenMai.Text = dr.Cells["Khuyến Mãi"].Value.ToString().Trim();
                    MemoryStream ms = new MemoryStream((byte[])dgvSanPham.CurrentRow.Cells["Hình Ảnh"].Value‌​);
                    picHinhAnh.Image = Image.FromStream(ms);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        private void pnlLoaiSP_Click(object sender, EventArgs e)
        {
            frmLoaiSanPham frm = new frmLoaiSanPham();
            frm.ShowDialog();
            if (frm.b)
            {
                LoadCboLoaiSP();
                LoadDataGridViewTheoBoLoc();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmLoaiSanPham frm = new frmLoaiSanPham();
            frm.ShowDialog();
            if (frm.b)
            {
                LoadCboLoaiSP();
                LoadDataGridViewTheoBoLoc();
            }
        }
    }
}
