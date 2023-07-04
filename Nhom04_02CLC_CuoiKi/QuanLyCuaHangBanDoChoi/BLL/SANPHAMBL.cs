using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data;

namespace BLL
{
    public class SANPHAMBL
    {
        private static SANPHAMBL Instance;
        public static SANPHAMBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new SANPHAMBL();
                }
                return Instance;
            }
        }
        private SANPHAMBL() { }
        private int soluong;
        #region Lấy SoHD Max
        public int GetSoLuongMatHang(int masp)
        {
            Context db = new Context();
            var qr = from p in db.SANPHAMs
                     where p.MASP == masp
                     select p.SOLUONG;
            foreach (var n in qr)
            {
                soluong = (int)n;
            }
            return soluong;
        }
        #endregion
        #region Cập Nhật Số Lượng Khi Bán Hàng

        public bool CapNhatSoLuongKhiBanHang(int MaSP, int SoLuong)
        {
            //string sql = "UPDATE SANPHAM SET SoLuong = SoLuong-@SoLuong WHERE MASP = @MASP";
            var context = new Context();
            var saveSoLuongSanPHam = context.SANPHAMs;
            if (saveSoLuongSanPHam.Any())
            {
                var sanpham = saveSoLuongSanPHam.First(o => o.MASP == MaSP);
                sanpham.SOLUONG = sanpham.SOLUONG - SoLuong;
                context.SaveChanges();
            }
            return true;
        }
        #endregion
        public DataTable GetDanhSachSanPhamTheoBoLoc()
        {
            var context = new Context();
            DataTable allSP = context.DataTable("SELECT MASP as N'Mã SP',TENSP as N'Tên SP',MALOAISP as N'Mã Loại SP',SOLUONG as N'Số Lượng',KHUYENMAI as N'Khuyến Mãi', DONGIANHAP as N'Đơn Giá Nhập', LOINHUAN as N'Lợi Nhuận', DONGIABAN as N'Đơn Giá Bán',HINHANH as N'Hình Ảnh' FROM SANPHAM");
            return allSP;
        }
        public void themSanPham(SANPHAM c)
        {
            var context = new Context();
            var sanpham = new SANPHAM { TENSP = c.TENSP, MALOAISP = c.MALOAISP, SOLUONG = c.SOLUONG, KHUYENMAI = c.KHUYENMAI, DONGIANHAP = c.DONGIANHAP, LOINHUAN = c.LOINHUAN, DONGIABAN = c.DONGIABAN, HINHANH = c.HINHANH };
            context.SANPHAMs.Add(sanpham);
            context.SaveChanges();

        }
        public void SuaSanPham(SANPHAM c)
        {
            var context = new Context();
            var qr = context.SANPHAMs;
            if (qr.Any())
            {
                var sanpham = qr.First(o => o.MASP == c.MASP);
                sanpham.TENSP = c.TENSP;
                sanpham.KHUYENMAI = c.KHUYENMAI;
                sanpham.MALOAISP = c.MALOAISP;
                sanpham.SOLUONG = c.SOLUONG;
                sanpham.DONGIANHAP = c.DONGIANHAP;
                sanpham.LOINHUAN = c.LOINHUAN;
                sanpham.DONGIABAN = c.DONGIABAN;
                sanpham.HINHANH = c.HINHANH;
            }
            context.SaveChanges();

        }
        public void NgungKinhDoanhSanPham(int masp)
        {
            var context = new Context();
            var person = context.SANPHAMs.Find(masp);
            if (person != null)
            {
                context.SANPHAMs.Remove(person);
                context.SaveChanges();
            }

        }

    }
}
