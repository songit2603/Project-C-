using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DAL;
namespace BLL
{
    public class DataRun
    {
        private static DataRun Instance;
        public static DataRun GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new DataRun();
                }
                return Instance;
            }
        }
        private DataRun() { }
         public void themdulieu()
        {
            VerifyDatabaseExists();
            if (InsertDataLoaiSanPham() == true)
            {
                InsertDataNhanVien();
                InsertDataKhachHang();
                InsertDataSanPham();
                InsertDataTaiKhoan();
            }
        }

        private static void VerifyDatabaseExists()
        {
            using (var context = new Context())
            {
                context.Database.CreateIfNotExists();
            }
        }
        static void InsertDataTaiKhoan()
        {
            using (var context = new Context())
            {
                var taikhoan1 = new TAIKHOAN{ MANV=1,MATKHAU=Encoding.ASCII.GetBytes("admin")};
                var taikhoan2 = new TAIKHOAN { MANV = 2, MATKHAU = Encoding.ASCII.GetBytes("user") };
                var taikhoan3 = new TAIKHOAN { MANV = 3, MATKHAU = Encoding.ASCII.GetBytes("user") };
                context.TAIKHOANs.Add(taikhoan1);
                context.TAIKHOANs.Add(taikhoan2);
                context.TAIKHOANs.Add(taikhoan3);
                context.SaveChanges();
            }
        }
        static void InsertDataNhanVien()
        {
            using (var context = new Context())
            {
                var nhanvien1 = new NHANVIEN {HOTEN = "Phan Hoàng Khải", SDT = "0367151727", GIOITINH = true, NGAYSINH = "01/03/2002", HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image11) };
                var nhanvien2 = new NHANVIEN {HOTEN = "Nguyễn Hồng Sơn", SDT = "0367151729", GIOITINH = true, NGAYSINH = "03/05/2002", HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image12) };
                var nhanvien3 = new NHANVIEN {HOTEN = "Nguyễn Hoàng Thiên Bảo", SDT = "0367151728", GIOITINH = true, NGAYSINH = "02/04/2002",HINHANH=HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image13) };
                context.NHANVIENs.Add(nhanvien1);
                context.NHANVIENs.Add(nhanvien2);
                context.NHANVIENs.Add(nhanvien3);
                context.SaveChanges();
            }
        }
        static void InsertDataKhachHang()
        {
            using (var context = new Context())
            {
                var khachhang1 = new KHACHHANG { HOTEN = "Khách hàng vãng lai",DIACHI=null, SDT = null,GIOITINH=null,NGAYDANGKY=null, DOANHSO = 0 };
                var khachhang2 = new KHACHHANG { HOTEN = "Vương Đình Huệ", DIACHI = "Nghệ An", SDT = "0999112911", GIOITINH = true, NGAYDANGKY = "10/6/2022", DOANHSO = 999000 };
                var khachhang3 = new KHACHHANG { HOTEN = "Nguyễn Phú Trọng", DIACHI = "Bắc Ninh", SDT = "0999112912", GIOITINH = true, NGAYDANGKY = "10/6/2022", DOANHSO = 8880000 };
                var khachhang4 = new KHACHHANG { HOTEN = "Vũ Đức Đam", DIACHI = "Hải Dương", SDT = "0999112913", GIOITINH = true, NGAYDANGKY = "10/6/2022", DOANHSO = 123456000 };
                context.KHACHHANGs.Add(khachhang1);
                context.KHACHHANGs.Add(khachhang2);
                context.KHACHHANGs.Add(khachhang3);
                context.KHACHHANGs.Add(khachhang4);
                context.SaveChanges();
            }
        }
        static bool InsertDataLoaiSanPham()
        {
            try
            {
                using (var context = new Context())
                {
                    var loaisanpham1 = new LOAISANPHAM { MALOAISP = "LOAI01", TENLOAISP = "Xe Đồ Chơi" };
                    var loaisanpham2 = new LOAISANPHAM { MALOAISP = "LOAI02", TENLOAISP = "Búp bê" };
                    var loaisanpham3 = new LOAISANPHAM { MALOAISP = "LOAI03", TENLOAISP = "Mô hình" };
                    var loaisanpham4 = new LOAISANPHAM { MALOAISP = "LOAI04", TENLOAISP = "Robot" };
                    context.LOAISANPHAMs.Add(loaisanpham1);
                    context.LOAISANPHAMs.Add(loaisanpham2);
                    context.LOAISANPHAMs.Add(loaisanpham3);
                    context.LOAISANPHAMs.Add(loaisanpham4);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        static void InsertDataSanPham()
        {
            using (var context = new Context())
            {
                var sanpham1 = new SANPHAM { MALOAISP = "LOAI01", TENSP = "Xe tăng điều khiển từ xa", SOLUONG = 20, DONGIANHAP = 200000, DONGIABAN = 300000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image1),LOINHUAN=100, KHUYENMAI=5 };
                var sanpham2 = new SANPHAM { MALOAISP = "LOAI01", TENSP = "Xe đua biến hình", SOLUONG = 50, DONGIANHAP = 500000, DONGIABAN = 700000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image2), LOINHUAN = 100, KHUYENMAI = 10 };
                var sanpham3 = new SANPHAM { MALOAISP = "LOAI02", TENSP = "Búp bê Barbie", SOLUONG = 100, DONGIANHAP = 100000, DONGIABAN = 250000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image3), LOINHUAN = 100, KHUYENMAI = 15 };
                var sanpham4 = new SANPHAM { MALOAISP = "LOAI02", TENSP = "Búp bê Annabelle", SOLUONG = 10, DONGIANHAP = 1000000, DONGIABAN = 1500000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image4), LOINHUAN = 100, KHUYENMAI = 20 };
                var sanpham5 = new SANPHAM { MALOAISP = "LOAI03", TENSP = "Mô hình lego ", SOLUONG = 150, DONGIANHAP = 500000, DONGIABAN = 750000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image5), LOINHUAN = 100, KHUYENMAI = 5 };
                var sanpham6 = new SANPHAM { MALOAISP = "LOAI03", TENSP = "Mô hình Gundam Limited Edition", SOLUONG = 20, DONGIANHAP = 2000000, DONGIABAN = 2500000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image6), LOINHUAN = 100, KHUYENMAI = 5 };
                var sanpham7 = new SANPHAM { MALOAISP = "LOAI03", TENSP = "Mô hình Yasuo Siêu Phẩm", SOLUONG = 30, DONGIANHAP = 1000000, DONGIABAN = 2000000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image7), LOINHUAN = 100, KHUYENMAI = 5 };
                var sanpham8 = new SANPHAM { MALOAISP = "LOAI04", TENSP = "Robot Pacific Rim", SOLUONG = 5, DONGIANHAP = 3000000, DONGIABAN = 3500000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image8), LOINHUAN = 100, KHUYENMAI = 5 };
                var sanpham9 = new SANPHAM { MALOAISP = "LOAI04", TENSP = "Rbot Siêu Nhân Gao", SOLUONG = 40, DONGIANHAP = 250000, DONGIABAN = 400000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image9), LOINHUAN = 100, KHUYENMAI = 5 };
                var sanpham10 = new SANPHAM { MALOAISP = "LOAI04", TENSP = "Robot Trái cây", SOLUONG = 700, DONGIANHAP = 400000, DONGIABAN = 500000, HINHANH = HinhAnhSP.GetInstance.ImageToByteArray(HinhAnhSP.GetInstance.image10), LOINHUAN = 100, KHUYENMAI = 5 };
                context.SANPHAMs.Add(sanpham1);
                context.SANPHAMs.Add(sanpham2);
                context.SANPHAMs.Add(sanpham3);
                context.SANPHAMs.Add(sanpham4);
                context.SANPHAMs.Add(sanpham5);
                context.SANPHAMs.Add(sanpham6);
                context.SANPHAMs.Add(sanpham7);
                context.SANPHAMs.Add(sanpham8);
                context.SANPHAMs.Add(sanpham9);
                context.SANPHAMs.Add(sanpham10);
                context.SaveChanges();
            }
        }

    }
}
