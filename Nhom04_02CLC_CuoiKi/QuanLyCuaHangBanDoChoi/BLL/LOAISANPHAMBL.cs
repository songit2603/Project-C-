using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data;
namespace BLL
{
    public class LOAISANPHAMBL
    {
        private static LOAISANPHAMBL Instance;
        public static LOAISANPHAMBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new LOAISANPHAMBL();
                }
                return Instance;
            }
        }
        private LOAISANPHAMBL() { }
        Context db = new Context();
        public object getDsLoaiSanPham()
        {
            return db.LOAISANPHAMs.Select(o => o).ToList();
        }
        public DataTable GetDanhSachLoaiSanPham()
        {
            return db.DataTable("SELECT MALOAISP AS N'Mã Loại SP', TENLOAISP AS N'Tên Loại SP' FROM LOAISANPHAM");
        }
        public bool ThemLoaiSanPham(LOAISANPHAM c)
        {
            try
            {
                var context = new Context();
                var loaisanpham = new LOAISANPHAM { MALOAISP = c.MALOAISP, TENLOAISP = c.TENLOAISP };
                context.LOAISANPHAMs.Add(loaisanpham);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool CapNhatLoaiSanPham(LOAISANPHAM c)
        {
            var context = new Context();
            var qr = context.LOAISANPHAMs;
            if (qr.Any())
            {
                var sanpham = qr.First(o => o.MALOAISP == c.MALOAISP);
                sanpham.TENLOAISP = c.TENLOAISP;
            }
            context.SaveChanges();
            return true;

        }
        public bool NgungKinhDoanh(string maloaisp)
        {
            var context = new Context();
            var loaisp = context.LOAISANPHAMs.Find(maloaisp);
            if (loaisp != null)
            {
                context.LOAISANPHAMs.Remove(loaisp);
                context.SaveChanges();
            }
            return true;

        }
    }
}
