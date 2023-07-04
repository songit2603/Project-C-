using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;
namespace BLL
{
    public class CTHDBL
    {
        private static CTHDBL Instance;
        public static CTHDBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new CTHDBL();
                }
                return Instance;
            }
        }
        private CTHDBL() { }
        Context db = new Context();
        public bool ThemCTHD(DataTable dt, int SOHD, decimal THANHTIEN)
        {
            //Thêm chi tiết hóa đơn
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var cthd = new CTHD
                {
                    SOHD = SOHD,
                    MASP = int.Parse(dt.Rows[i][0].ToString()),
                    SOLUONG = int.Parse(dt.Rows[i][4].ToString())
                };
                db.CTHDs.Add(cthd);
                db.SaveChanges();
            }
            //Cập nhật hóa đơn
            var hoadon = db.HOADONs;
            if (hoadon.Any())
            {
                var qr = hoadon.First(o => o.SOHD == SOHD);
                qr.DATHANHTOAN = true;
                qr.THANHTIEN = THANHTIEN;
                db.SaveChanges();
            }
            return true;
        }

    }
}
