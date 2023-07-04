using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using DAL;

namespace BLL
{
    public class KHACHHANGBL
    {
        private static KHACHHANGBL Instance;
        public static KHACHHANGBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new KHACHHANGBL();
                }
                return Instance;
            }
        }
        private KHACHHANGBL() { }
        Context db = new Context();
        public DataTable GetDanhSachKhachHangTimKiem(string tenkh)
        {
            return db.DataTable("SELECT MAKH as N'Mã KH',HOTEN as N'Tên KH',DIACHI as N'Địa Chỉ',SDT as N'SĐT',NGAYDANGKY as N'Ngày Đăng Ký',DOANHSO as N'Doanh Số' , CASE GIOITINH WHEN 0 THEN N'Nữ' WHEN 1 THEN N'Nam' END AS N'Giới Tính' FROM KHACHHANG WHERE MAKH != 1 AND HOTEN LIKE N'%" + tenkh + "%' ");
        }
        public bool CapNhatDoanhSoKhachHang(string sdt, double DOANHSO)
        {
            //string sql = "UPDATE KHACHHANG SET DOANHSO=DOANHSO+@DOANHSO WHERE MAKH = @MAKH";
            try
            {
                using(Context context = new Context())
                {
                    var saveKhachHangs = context.KHACHHANGs;
                    if(saveKhachHangs.Any())
                    {
                        var khachhang=saveKhachHangs.First(o=>o.SDT==sdt);
                        DOANHSO += DOANHSO;
                    }
                    context.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
                return false;

        }
        public DataTable GetDanhSachKhachHang()
        {
            Context db = new Context();
            return db.DataTable("SELECT MAKH as N'Mã KH',HOTEN as N'Tên KH',DIACHI as N'Địa Chỉ',SDT as N'SĐT',NGAYDANGKY as N'Ngày Đăng Ký',DOANHSO as N'Doanh Số' , CASE GIOITINH WHEN 0 THEN N'Nữ' WHEN 1 THEN N'Nam' END AS N'Giới Tính' FROM KHACHHANG");
        }
        public int GetMaKhSDT(string sdt)
        {
            try
            {
                var context = new Context();
                return context.KHACHHANGs.First(o => o.SDT == sdt).MAKH;
            }
            catch (Exception)
            {
                return 1;
 
            }
        }
        public bool XoaKhachHang(int MAKH)
        {
            Context context = new Context();
            var khachhang = context.KHACHHANGs.Find(MAKH);
            if(khachhang!=null)
            {
                context.KHACHHANGs.Remove(khachhang);
                context.SaveChanges();
            }
            return true;
        }
        public bool SuaThongTinKhachHang(KHACHHANG c)
        {
            Context context = new Context();
            var qr = context.KHACHHANGs;
            if(qr.Any())
            {
                var khachhang = qr.First(o => o.MAKH == c.MAKH);
                khachhang.HOTEN = c.HOTEN;
                khachhang.SDT = c.SDT;
                khachhang.DIACHI = c.DIACHI;
                khachhang.DOANHSO = c.DOANHSO;
                context.SaveChanges();
            }
            return true;
        }
        public bool ThemKhachHang(KHACHHANG c)
        {
            var context = new Context();
            var khachhang = new KHACHHANG {HOTEN=c.HOTEN,DIACHI=c.DIACHI,NGAYDANGKY=c.NGAYDANGKY,SDT=c.SDT,DOANHSO=0};
            context.KHACHHANGs.Add(khachhang);
            context.SaveChanges();
            return true;
        }
    }
}
