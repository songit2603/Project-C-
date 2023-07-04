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
    public partial class frmBanSanPham : Form
    {
        public frmBanSanPham()
        {
            InitializeComponent();
        }
        int sumpage = 0;
        bool hd = false;
        public static int SOHD = 0;
        public static double ThanhTien = 0;
        public static int SOHD_Report = 0;
        private void frmBanSanPham_Load(object sender, EventArgs e)
        {
            LoadDanhSachSanPhamTheoBoLoc(1);
        }
        private string convertgia(double? gia)
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
        private void LoadDanhSachSanPhamTheoBoLoc(int numpage)
        {
            flowLayoutPanelSanPham.Controls.Clear();
            DataTable dt = SANPHAMBL.GetInstance.GetDanhSachSanPhamTheoBoLoc();
            //dgvTest.DataSource = dt;
            int soDongTrenTrang = 8;
            int soTrang = dt.Rows.Count / soDongTrenTrang;
            if (dt.Rows.Count % soDongTrenTrang != 0)
            {
                soTrang += 1;
            }
            if (soTrang == 0)
            {
                sumpage = 0;
                lblPageNumber.Text = "0/" + sumpage;
            }
            else
            {
                sumpage = soTrang;
                lblPageNumber.Text = "1/" + sumpage;
            }
            int k = 0;
            int j = 0;
            //dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                if (k >= (numpage * 8 - 8))
                {
                    j++;
                    if (j > 8)
                    {
                        row.Delete();
                        continue;
                    }
                    continue;
                }
                row.Delete();
                k++;
            }
            dt.AcceptChanges();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ucSanPham sp = new ucSanPham();
                sp.spDTO.MASP = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                sp.spDTO.TENSP = dt.Rows[i].ItemArray[1].ToString();
                sp.spDTO.MALOAISP = dt.Rows[i].ItemArray[2].ToString();
                sp.spDTO.SOLUONG = int.Parse(dt.Rows[i].ItemArray[3].ToString());
                sp.spDTO.DONGIABAN = double.Parse(dt.Rows[i].ItemArray[7].ToString());
                sp.spDTO.KHUYENMAI = double.Parse(dt.Rows[i].ItemArray[4].ToString());
                sp.spDTO.HINHANH = (byte[])dt.Rows[i].ItemArray[8];
                MemoryStream ms = new MemoryStream(sp.spDTO.HINHANH);
                sp.picSP.Image = Image.FromStream(ms);
                sp.lblTenSP.Text = sp.spDTO.TENSP;
                if (sp.spDTO.KHUYENMAI == 0)
                {
                    sp.lblGiaKM.Text = "Giá: " + convertgia(sp.spDTO.DONGIABAN) + " ₫";
                    sp.lblGiaGoc.Visible = false;
                    sp.pictureBox2.Visible = false;
                    sp.panel1.Visible = false;
                }
                else
                {
                    sp.lblKM.Text = "-" + convertgia(sp.spDTO.KHUYENMAI) + "%";
                    sp.lblGiaGoc.Text = convertgia(sp.spDTO.DONGIABAN) + " ₫";
                    //sp.spDTO.DONGIABAN = sp.spDTO.DONGIABAN - ((sp.spDTO.DONGIABAN * sp.spDTO.KHUYENMAI) / 100);
                    sp.lblGiaKM.Text = "Giá: " + convertgia(sp.spDTO.DONGIABAN - (sp.spDTO.DONGIABAN * sp.spDTO.KHUYENMAI) / 100) + " ₫";
                    //sp.pictureBox2.Click += PictureBox2_Click;
                }
                if (sp.spDTO.SOLUONG <= 0)
                {
                    sp.lblSanCo.Text = "Hết hàng!";
                }
                else
                    sp.lblSanCo.Text = "Sẵn có: " + sp.spDTO.SOLUONG.ToString();
                sp.lblGiaGoc.Font = new Font("UTM Avo", 10, FontStyle.Strikeout);
                sp.Click += Sp_Click;
                sp.lblGiaGoc.Click += LblGiaGoc_Click;
                sp.lblGiaKM.Click += LblGiaKM_Click;
                sp.lblKM.Click += LblKM_Click;
                sp.lblTenSP.Click += LblTENSP_Click;
                sp.panel1.Click += Panel1_Click;
                sp.picSP.Click += PicSP_Click;
                sp.pictureBox2.Click += PictureBox2_Click;

                sp.KeyDown += Sp_KeyDown;
                sp.lblGiaGoc.KeyDown += Sp_KeyDown;
                sp.lblGiaKM.KeyDown += Sp_KeyDown;
                sp.lblKM.KeyDown += Sp_KeyDown;
                sp.lblTenSP.KeyDown += Sp_KeyDown;
                sp.panel1.KeyDown += Sp_KeyDown;
                sp.picSP.KeyDown += Sp_KeyDown;
                sp.pictureBox2.KeyDown += Sp_KeyDown; 
                flowLayoutPanelSanPham.Controls.Add(sp);
            }
        }
        private void Sp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
                btnThanhToan.PerformClick();
            if (e.KeyCode == Keys.F12)
                btnHuy.PerformClick();
        }
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            double tong = 0;
            PictureBox p = (PictureBox)sender;
            ucSanPham u = (ucSanPham)p.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void PicSP_Click(object sender, EventArgs e)
        {
            double tong = 0;
            PictureBox p = (PictureBox)sender;
            ucSanPham u = (ucSanPham)p.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                   //txtSDT.Enabled = false;
                }
            }
        }
        private void Panel1_Click(object sender, EventArgs e)
        {
            double tong = 0;
            Panel pn = (Panel)sender;
            ucSanPham u = (ucSanPham)pn.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    //txtSDT.Enabled = false;
                    hd = true;
                }
            }
        }
        private void LblTENSP_Click(object sender, EventArgs e)
        {
            double tong = 0;
            Label p = (Label)sender;
            ucSanPham u = (ucSanPham)p.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void LblKM_Click(object sender, EventArgs e)
        {
            double tong = 0;
            Label p = (Label)sender;
            Panel pn = (Panel)p.Parent;
            ucSanPham u = (ucSanPham)pn.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void LblGiaKM_Click(object sender, EventArgs e)
        {
            double tong = 0;
            Label p = (Label)sender;
            ucSanPham u = (ucSanPham)p.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //xtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void Sp_Click(object sender, EventArgs e)
        {
            double tong = 0;
            ucSanPham u = (ucSanPham)sender;
            //ucSanPham u = new ucSanPham();
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void LblGiaGoc_Click(object sender, EventArgs e)
        {
            double tong = 0;
            Label p = (Label)sender;
            ucSanPham u = (ucSanPham)p.Parent;
            u.Select();
            if (u.spDTO.SOLUONG > 0)
            {
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    DataGridViewRow r = dgvCTHD.Rows[i];
                    if (int.Parse(r.Cells[0].Value.ToString()) == u.spDTO.MASP)
                    {
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        dgvCTHD.Rows[i].Selected = true;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        tong = giakm * int.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        if (hd == true)
                        {

                        }
                        else
                        {
                            ThemHoaDon();
                            hd = true;
                            //txtSDT.Enabled = false;
                        }
                        return;
                    }
                }
                tong = (double)(u.spDTO.DONGIABAN - ((u.spDTO.DONGIABAN * u.spDTO.KHUYENMAI) / 100));
                dgvCTHD.Rows.Insert(dgvCTHD.Rows.Count, u.spDTO.MASP, u.spDTO.TENSP, u.spDTO.DONGIABAN, u.spDTO.KHUYENMAI, 1, tong, "-", "+");
                dgvCTHD.Rows[dgvCTHD.Rows.Count - 1].Selected = true;
                LoadTongHoaDon();
                if (hd == true)
                {

                }
                else
                {
                    ThemHoaDon();
                    hd = true;
                    //txtSDT.Enabled = false;
                }
            }
        }
        private void ThemHoaDon()
        {
            HOADON hddTO = new HOADON();
            if (txtSDT.Text == "")
                hddTO.MAKH = 1;
            else
                hddTO.MAKH = KHACHHANGBL.GetInstance.GetMaKhSDT(txtSDT.Text);
            hddTO.MANV = frmDangNhap.TenDangNhap;
            hddTO.NGAYLAP = DateTime.Now;
            hddTO.THANHTIEN = 0;
            hddTO.DATHANHTOAN = false;
            HOADONBL.GetInstance.ThemHoaDon(hddTO);
            SOHD = HOADONBL.GetInstance.GetSOHDMax();
            btnHuy.BackColor = Color.OrangeRed;
            frmThongBao frm = new frmThongBao();
            frm.lblThongBao.Text = "Thêm thành công";
            frm.ShowDialog();
            //txtSDT.Enabled = false;
        }
        private void LoadTongHoaDon()
        {
            double tong = 0;
            for (int i = 0; i < dgvCTHD.Rows.Count; i++)
            {
                tong += double.Parse(dgvCTHD.Rows[i].Cells[5].Value.ToString());
            }
            ThanhTien = tong;
            lblTongTien.Text = convertgia(tong) + " ₫";
            if (txtTienKHTra.Text == "")
                return;
            if (tong < double.Parse(txtTienKHTra.Text))
            {
                txtTienThua.Text = ((Math.Abs(ThanhTien - double.Parse(txtTienKHTra.Text)))) + "";
                btnThanhToan.BackColor = Color.Green;
            }
            else
            {
                txtTienThua.Text = "Không đủ";
                btnThanhToan.BackColor = Color.Gray;
            }

        }
        private void btnPre_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            string pre = "";
            string str = lblPageNumber.Text;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '/')
                {
                    break;
                }
                else
                {
                    pre += str[i];
                }
            }
            if (pre == "1")
                return;

            string n = "";
            string num = lblPageNumber.Text;
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] == '/')
                {
                    break;
                }
                else
                {
                    n += num[i];
                }
            }
            LoadDanhSachSanPhamTheoBoLoc(int.Parse(n) - 1);
            lblPageNumber.Text = int.Parse(n) - 1 + "/" + sumpage;
            Cursor = Cursors.Default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            string pre = "";
            string next = "";
            string str = lblPageNumber.Text;
            bool b = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '/' && b == false)
                {
                    pre += str[i];
                    continue;
                }
                if (str[i] != '/' && b == true)
                {
                    next += str[i];
                    continue;
                }
                if (str[i] == '/')
                {
                    b = true;
                }
            }
            if (pre == next)
                return;

            string n = "";
            string num = lblPageNumber.Text;
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] == '/')
                {
                    break;
                }
                else
                {
                    n += num[i];
                }
            }
            LoadDanhSachSanPhamTheoBoLoc(int.Parse(n) + 1);
            lblPageNumber.Text = int.Parse(n) + 1 + "/" + sumpage;
            Cursor = Cursors.Default;
        }

        private void dgvCTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                DataGridViewRow r = dgvCTHD.SelectedRows[0];
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    if (r.Cells[0].Value.ToString() == dgvCTHD.Rows[i].Cells[0].Value.ToString())
                    {
                        if (int.Parse(r.Cells[4].Value.ToString()) == 1)
                        {
                            dgvCTHD.Rows.Remove(r);
                            LoadTongHoaDon();
                            return;
                        }
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) - 1;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        double tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();

                        if (txtTienKHTra.Text != "")
                        {
                            if (ThanhTien < double.Parse(txtTienKHTra.Text))
                                txtTienThua.Text = ((Math.Abs(ThanhTien - double.Parse(txtTienKHTra.Text)))) + "";
                            else
                            {
                                txtTienThua.Text = "Không đủ";
                                btnThanhToan.BackColor = Color.Gray;
                            }
                        }

                        return;
                    }
                }
            }
            if (e.ColumnIndex == 7)
            {
                DataGridViewRow r = dgvCTHD.SelectedRows[0];
                for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                {
                    if (r.Cells[0].Value.ToString() == dgvCTHD.Rows[i].Cells[0].Value.ToString())
                    {
                        if (int.Parse(r.Cells[4].Value.ToString())+1 > SANPHAMBL.GetInstance.GetSoLuongMatHang(int.Parse(r.Cells[0].Value.ToString())))
                        {
                            MessageBox.Show("Nhập ít thôi, quá số lượng rầu !!");
                        }
                        else
                        dgvCTHD.Rows[i].Cells[4].Value = int.Parse(r.Cells[4].Value.ToString()) + 1;
                        double giagoc = double.Parse(r.Cells[2].Value.ToString());
                        int km = int.Parse(r.Cells[3].Value.ToString());
                        double giakm = giagoc - ((giagoc * km) / 100);
                        double tong = giakm * double.Parse(r.Cells[4].Value.ToString());
                        dgvCTHD.Rows[i].Cells[5].Value = tong;
                        LoadTongHoaDon();
                        try
                        {
                            if (ThanhTien <= double.Parse(txtTienKHTra.Text))
                            {
                                txtTienThua.Text = ((Math.Abs(ThanhTien - double.Parse(txtTienKHTra.Text)))).ToString() + "";

                            }
                            else
                            {
                                txtTienThua.Text = "Không đủ";
                                btnThanhToan.BackColor = Color.Gray;
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }

                        return;
                    }
                }
            }
        }

        private void dgvCTHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvCTHD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
                ThanhToanF9();
            if (e.KeyCode == Keys.F12)
                HuyF12();
        }
        private void ThanhToanF9()
        {
            btnThanhToan.PerformClick();
        }
        private void HuyF12()
        {
            btnHuy.PerformClick();
        }
        private void txtTienKHTra_TextChanged(object sender, EventArgs e)
        {
            if (txtTienKHTra.Text != "")
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                double value = double.Parse(txtTienKHTra.Text, System.Globalization.NumberStyles.AllowThousands);
                txtTienKHTra.Text = String.Format(culture, "{0:N0}", value);
                txtTienKHTra.Select(txtTienKHTra.Text.Length, 0);

                try
                {
                    if (ThanhTien <= double.Parse(txtTienKHTra.Text))
                    {
                        txtTienThua.Text = Math.Abs((ThanhTien - double.Parse(txtTienKHTra.Text))) + "";
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("en-US");
                        double v = double.Parse(txtTienThua.Text, System.Globalization.NumberStyles.AllowThousands);
                        txtTienThua.Text = String.Format(c, "{0:N0}", v);
                        txtTienThua.Select(txtTienThua.Text.Length, 0);
                        btnThanhToan.BackColor = Color.Green;
                    }
                    else
                    {
                        txtTienThua.Text = "Không đủ";
                        btnThanhToan.BackColor = Color.Gray;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        private void ThemCTDH()
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgvCTHD.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dgvCTHD.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            if (CTHDBL.GetInstance.ThemCTHD(dt, SOHD, decimal.Parse(ThanhTien.ToString())))
            {
                CapNhatSoLuong();
                SOHD_Report = SOHD;
                CapNhatTienKhachHang();
                ClearThongTinHD();
                SOHD = 0;
            }
        }
        private void ClearThongTinHD()
        {
            dgvCTHD.Rows.Clear();
            lblTongTien.Text = "0";
            txtTienKHTra.Text = "";
            txtTienThua.Text = "";
            txtSDT.Text = "";
            btnThanhToan.BackColor = Color.Gray;
            btnHuy.BackColor = Color.Gray;
        }
        private void CapNhatTienKhachHang()
        {
            HOADONBL.GetInstance.CapNhatSoLuongTienKhachHang(SOHD_Report, decimal.Parse(txtTienKHTra.Text), decimal.Parse(txtTienThua.Text));
        }
        private void CapNhatSoLuong()
        {
            for (int i = 0; i < dgvCTHD.Rows.Count; i++)
            {
                DataGridViewRow r = dgvCTHD.Rows[i];
                SANPHAMBL.GetInstance.CapNhatSoLuongKhiBanHang(int.Parse(r.Cells[0].Value.ToString()), int.Parse(r.Cells[4].Value.ToString()));
            }
        }


        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (btnThanhToan.BackColor == Color.Gray)
                return;
            frmThongBao frm = new frmThongBao();
            frm.lblThongBao.Text = "Đã thanh toán đơn hàng";
            frm.ShowDialog();
            this.Cursor = Cursors.AppStarting;
            if (txtSDT.Text != "")
            {
                KHACHHANGBL.GetInstance.CapNhatDoanhSoKhachHang(txtSDT.Text, ThanhTien);
                HOADONBL.GetInstance.CapNhatTenKhachHang(KHACHHANGBL.GetInstance.GetMaKhSDT(txtSDT.Text.Trim()),SOHD);
            }
            ThemCTDH();
            //frmTest frmtet = new frmTest();
/*            frmtet.dgvTest.DataSource = HOADONBL.GetInstance.InsertViewHoaDon(SOHD);
            frmtet.Show();*/
            InHoaDon();
            LoadDanhSachSanPhamTheoBoLoc(1);
            hd = false;
            txtSDT.Enabled = true;
            this.Cursor = Cursors.Default;
        }
        private void InHoaDon()
        {
            frmInHoaDon frm = new frmInHoaDon();
            frm.Show();
            frm.TopMost = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            HOADONBL.GetInstance.XoaHD(SOHD);
            btnHuy.BackColor = Color.Gray;
            dgvCTHD.Rows.Clear();
            hd = false;
            SOHD = 0;
            txtSDT.Text = "";
            lblTongTien.Text = "0";
            txtTienThua.Text = "";
            txtTienKHTra.Text = "";
            btnThanhToan.BackColor = Color.Gray;
            txtSDT.Enabled = true;
        }
    }
}
