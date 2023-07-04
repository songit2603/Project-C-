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
using DAL;
namespace GUI.Forms
{
    public partial class frmKhachHang : Form
    {
        public frmKhachHang()
        {
            InitializeComponent();
        }
        int makh = 0;
        private void LoadDgvKhachHang()
        {
            dgvKhachHang.DataSource = KHACHHANGBL.GetInstance.GetDanhSachKhachHang();
            dgvKhachHang.ClearSelection();
        }
        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (CheckControls())
            {
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length <= 12 && txtSoDienThoai.Text.Length >= 10)
                    {
                        if (txtDiaChi.Text.Length <= 200)
                        {
                            KHACHHANG khDTO = new KHACHHANG();
                            khDTO.MAKH = makh;
                            khDTO.HOTEN = txtTen.Text;
                            khDTO.DIACHI = txtDiaChi.Text;
                            if (cboGioiTinh.Text == "Nam")
                                khDTO.GIOITINH = true;
                            else
                                khDTO.GIOITINH = false;
                            khDTO.NGAYDANGKY = dateNgayDangKy.Value.ToString();
                            khDTO.SDT = txtSoDienThoai.Text;

                            if (KHACHHANGBL.GetInstance.ThemKhachHang(khDTO))
                            {
                                LoadDgvKhachHang();
                                LamMoi();
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Địa chỉ tối đa 200 ký tự!";
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
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin khách hàng";
                frm.ShowDialog();
            }
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            LoadDgvKhachHang();

        }

        private void btnCapNhatSP_Click(object sender, EventArgs e)
        {

            if (CheckControls())
            {
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length <= 12 && txtSoDienThoai.Text.Length >= 10)
                    {
                        if (txtDiaChi.Text.Length <= 200)
                        {
                            KHACHHANG khDTO = new KHACHHANG();
                            khDTO.MAKH = makh;
                            khDTO.HOTEN = txtTen.Text;
                            khDTO.DIACHI = txtDiaChi.Text;
                            if (cboGioiTinh.Text == "Nam")
                                khDTO.GIOITINH = true;
                            else
                                khDTO.GIOITINH = false;
                            khDTO.NGAYDANGKY = dateNgayDangKy.Value.ToString();
                            khDTO.SDT = txtSoDienThoai.Text;

                            if (KHACHHANGBL.GetInstance.SuaThongTinKhachHang(khDTO))
                            {
                                LoadDgvKhachHang();
                                LamMoi();
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Địa chỉ tối đa 200 ký tự!";
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
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin khách hàng";
                frm.ShowDialog();
            }
        }
        private void ResetColorControls()
        {
            foreach (Control ctrl in pnlThongTinKhachHang.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.BackColor == Color.OrangeRed)
                    {
                        ctrl.BackColor = Color.White;
                    }
                }
            }
        }


        private void dgvKhachHang_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvKhachHang.SelectedRows.Count == 1)
                {
                    ResetColorControls();
                    DataGridViewRow dr = dgvKhachHang.SelectedRows[0];
                    makh = int.Parse(dr.Cells["Mã KH"].Value.ToString().Trim());
                    txtTen.Text = dr.Cells["Tên KH"].Value.ToString().Trim();
                    cboGioiTinh.SelectedItem = dr.Cells["Giới Tính"].Value.ToString();
                    dateNgayDangKy.Value = Convert.ToDateTime(dr.Cells["Ngày Đăng Ký"].Value);
                    txtSoDienThoai.Text = dr.Cells["SĐT"].Value.ToString().Trim();
                    txtDiaChi.Text = dr.Cells["Địa Chỉ"].Value.ToString().Trim();
                    //txtDoanhSo.Text = ConvertTien(Convert.ToDouble(dr.Cells["Doanh Số"].Value.ToString().Trim()));
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        private bool CheckControls()
        {
            int r = 0;
            foreach (Control ctrl in pnlThongTinKhachHang.Controls)
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
            if (r == 0)
                return true;
            return false;
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

        private void btnLamMoiThongTin_Click(object sender, EventArgs e)
        {
            LamMoi();
        }
        private void LamMoi()
        {
            txtTen.Clear();
            txtDiaChi.Clear();
            //txtDoanhSo.Clear();
            txtSoDienThoai.Clear();
            cboGioiTinh.SelectedIndex = 0;
            dateNgayDangKy.Value = DateTime.Now;
            ResetColorControls();
        }

        private void btnNgungKinhDoanh_Click(object sender, EventArgs e)
        {
            if (KHACHHANGBL.GetInstance.XoaKhachHang(makh))
            {
                LamMoi();
                LoadDgvKhachHang();
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Xóa khách hàng thất bại!";
                frm.ShowDialog();
            }
        }

        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {
            if (txtTenKH.Text != "")
            {
                dgvKhachHang.DataSource = KHACHHANGBL.GetInstance.GetDanhSachKhachHangTimKiem(txtTenKH.Text);
                dgvKhachHang.ClearSelection();
            }
            else
            {
                LoadDgvKhachHang();
            }
        }
    }
}
