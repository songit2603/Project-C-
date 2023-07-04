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
    public class HOADONBL
    {
        private static HOADONBL Instance;
        public static HOADONBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new HOADONBL();
                }
                return Instance;
            }
        }
        private HOADONBL() { }
        private int  sohd;
        public void CapNhatTenKhachHang(int makh, int sohd)
        {
            Context context = new Context();
            var qr = context.HOADONs;
            if (qr.Any())
            {
                var hoadon = qr.First(o => o.SOHD == sohd);
                hoadon.MAKH = makh;
            }
            context.SaveChanges();
        }
        #region Số lượng sản phẩm đã bán 
        public  int getSoLuongSanPhamDaBan()
        {
            try
            {
                Context db = new Context();
                var qr = db.V_HOADONs.Select(o => o.SOLUONG).Sum();
                return (int)qr;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public  string getTongDoanhThu()
        {
            try
            {
                Context db = new Context();
                var qr = db.HOADONs.Select(o => o.THANHTIEN).Sum();
                return convertgia((double)qr);
            }
            catch (Exception)
            {

                return "0";
            }

        }
        public void XoaHD(int SOHD)
        {
            Context context = new Context();
             var hd =context.HOADONs.First(o => o.SOHD == SOHD);
            context.HOADONs.Remove(hd);
        }
        #endregion
        #region Cập Nhật Tiền Khách Hàng Trong Hóa Đơn
        public bool CapNhatSoLuongTienKhachHang(int SOHD, decimal TienKhachHangTra, decimal TienThua)
        {
            Context context=new Context();
            //string sql = "UPDATE HOADON SET TIENKHACHHANGTRA = @TIENKHACHHANGTRA,TIENTHUA = @TIENTHUA WHERE SOHD = @SOHD";
            var HoaDonsSave = context.HOADONs;
            if (HoaDonsSave.Any())
            {
                var hoadon = HoaDonsSave.First(o => o.SOHD == SOHD);
                hoadon.TIENKHACHHANGTRA = TienKhachHangTra;
                hoadon.TIENTHUA = TienThua;
                context.SaveChanges();
            }
            return true;
        }
        #endregion
        #region Thêm Hóa Đơn
        public void ThemHoaDon(HOADON hdDTO)
        {
            var context = new Context();
            var hoadon = new HOADON
            {
                NGAYLAP = hdDTO.NGAYLAP,
                MAKH = hdDTO.MAKH,
                MANV = hdDTO.MANV,
                THANHTIEN = hdDTO.THANHTIEN,
                DATHANHTOAN = hdDTO.DATHANHTOAN,
                TIENKHACHHANGTRA = hdDTO.TIENKHACHHANGTRA,
                TIENTHUA = hdDTO.TIENTHUA,
            };
            context.HOADONs.Add(hoadon);
            context.SaveChanges();
        }
        #endregion
        #region Lấy SoHD Max
        public int GetSOHDMax()
        {     
            Context db = new Context();
            var qr = from p in db.HOADONs
                     orderby p.SOHD descending
                     select p.SOHD;
            foreach (var n in qr.Take(1))
            {
                sohd = n;
            }
            return sohd;
        }
        #endregion
        #region View Hóa đơn
        public void InsertViewHoaDon(int SOHD)
        {
            int sohd = GetSOHDMax();
            Context context = new Context();
            var qr = from hd in context.HOADONs
                     join cthd in context.CTHDs on hd.SOHD equals cthd.SOHD
                     join nv in context.NHANVIENs on hd.MANV equals nv.MANV
                     join kh in context.KHACHHANGs on hd.MAKH equals kh.MAKH
                     join sp in context.SANPHAMs on cthd.MASP equals sp.MASP
                     select new
                     {
                         sohoadon = hd.SOHD,
                         tensp = sp.TENSP,
                         dongiaban = sp.DONGIABAN,
                         soluong = cthd.SOLUONG,
                         thanhtien = sp.DONGIABAN-sp.DONGIABAN*sp.KHUYENMAI/100,
                         ngaylap = hd.NGAYLAP,
                         tienkhachtra = hd.TIENKHACHHANGTRA,
                         tienthua = hd.TIENTHUA,
                         tonghoadon = (double)hd.THANHTIEN,
                         tennv = nv.HOTEN,
                         khuyenmai = sp.KHUYENMAI,
                         tenkhachhang = kh.HOTEN
                     };
            foreach (var n in qr)
            {
                V_HOADON a = new V_HOADON();
                a.SOHD = n.sohoadon;
                a.TENSP = n.tensp;
                a.DONGIABAN = convertgia(n.dongiaban);
                a.SOLUONG = n.soluong;
                a.THANHTIEN = convertgia((double)n.thanhtien);
                a.NGAYLAP = n.ngaylap;
                a.TIENKHACHHANGTRA = convertgia((double)n.tienkhachtra);
                a.TIENTHUA = convertgia((double)n.tienthua);
                a.TONGHOADON = convertgia((double)n.tonghoadon);
                a.TENNV = n.tennv;
                a.KHUYENMAI = n.khuyenmai;
                a.TENKH = n.tenkhachhang;
                context.V_HOADONs.Add(a);
            }
                context.SaveChanges();
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
        #endregion
        #region In Hóa Đơn
        public DataSet InHoaDon(int SOHD)
        {
            Context context = new Context();
            try
            {
                string sql = "SELECT * FROM V_HOADON WHERE SOHD=" + SOHD + "";
                InsertViewHoaDon(SOHD);
                DataSet ds = context.DataSet("SELECT * FROM V_HOADON WHERE SOHD=" + SOHD + "");
                ds.Tables[0].Columns.Add("DOCSOTIEN");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double TongTien = Convert.ToDouble(ds.Tables[0].Rows[i]["TONGHOADON"]);
                    string soTienChu = So_chu(TongTien);
                    ds.Tables[0].Rows[i]["DOCSOTIEN"] = "(" + soTienChu + ")";
                }
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;

                case "1":
                    result = "một";
                    break;

                case "2":
                    result = "hai";
                    break;

                case "3":
                    result = "ba";
                    break;

                case "4":
                    result = "bốn";
                    break;

                case "5":
                    result = "năm";
                    break;

                case "6":
                    result = "sáu";
                    break;

                case "7":
                    result = "bảy";
                    break;

                case "8":
                    result = "tám";
                    break;

                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }


        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }


        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
            }

            return Ktach;
        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();
        }
        #endregion
    }
}
