using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;
namespace BLL
{
    public class NHANVIENBL
    {
        private static NHANVIENBL Instance;
        public static NHANVIENBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NHANVIENBL();
                }
                return Instance;
            }
        }
        private NHANVIENBL() { }
        Context db = new Context();
        public DataTable GetDanhSachNhanVienTimKiem(string tennv)
        {
            return db.DataTable("SELECT MANV as N'Mã NV',HOTEN as N'Tên NV',SDT as N'SĐT',NGAYSINH as N'Ngày Sinh',HINHANH as N'Hình Ảnh' from NHANVIEN where HOTEN LIKE N'%" + tennv + "%' ");
        }
        public DataTable GetDanhSachNhanVienTheoBoLoc()
        {
            return db.DataTable("SELECT MANV as N'Mã NV',HOTEN as N'Tên NV',SDT as N'SĐT',NGAYSINH as N'Ngày Sinh',HINHANH as N'Hình Ảnh' from NHANVIEN");
        }
        public void ThoiViecNhanVien(int MANV)
        {
            var nhanvien = db.NHANVIENs.Find(MANV);
            if (nhanvien != null)
            {
                db.NHANVIENs.Remove(nhanvien);
                db.SaveChanges();
            }
        }
        public bool ThemNhanVien(NHANVIEN c)
        {
            using (var db = new Context())
            {
                var nhanvien = new NHANVIEN { HOTEN = c.HOTEN, SDT = c.SDT, GIOITINH = c.GIOITINH, NGAYSINH = c.NGAYSINH, HINHANH = c.HINHANH };
                db.NHANVIENs.Add(nhanvien);
                db.SaveChanges();
                return true;

            }
        }
        public bool SuaThongTinNhanVien(NHANVIEN c)
        {
            Context context = new Context();
            var qr = context.NHANVIENs;
            if(qr.Any())
            {
                var nhanvien = qr.First(o => o.MANV == c.MANV);
                nhanvien.HOTEN = c.HOTEN;
                nhanvien.SDT = c.SDT;
                nhanvien.NGAYSINH=c.NGAYSINH;
                context.SaveChanges();
            }
            return true;
        }

    }
}
