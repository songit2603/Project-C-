using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    public class TAIKHOANBL
    {
        private static TAIKHOANBL Instance;
        public static TAIKHOANBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new TAIKHOANBL();
                }
                return Instance;
            }
        }
        private TAIKHOANBL() { }
        string tennv;
        public bool KiemTraDangNhap(int manv, string mk)
        {
            try
            {
                Context context = new Context();
                var taikhoan = context.TAIKHOANs.First(o => o.MANV == manv);
                if (mk == Encoding.ASCII.GetString(taikhoan.MATKHAU))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public string getTenTaiKhoan(int manv)
        {
            Context db = new Context();

            var qr = from taikhoan in db.TAIKHOANs
                     join nhanvien in db.NHANVIENs on taikhoan.MANV equals nhanvien.MANV
                     where taikhoan.MANV == manv
                     select nhanvien.HOTEN;
            foreach(var n in qr)
            {
                tennv = n;
            }
            return tennv;
        }
    }
}
