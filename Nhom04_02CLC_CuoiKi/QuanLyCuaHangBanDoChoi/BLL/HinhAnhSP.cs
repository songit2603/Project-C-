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

namespace BLL
{
    public partial class HinhAnhSP : Form
    {
        public HinhAnhSP()
        {
            InitializeComponent();
        }
        private static HinhAnhSP Instance;
        public static HinhAnhSP GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new HinhAnhSP();
                }
                return Instance;
            }
        }
        public Image image1 = Properties.Resource._1xetang;
        public Image image2 = Properties.Resource._2xeduabienhinh;
        public Image image3 = Properties.Resource._3bupbebarbie;
        public Image image4 = Properties.Resource._4bupbeannabelle;
        public Image image5 = Properties.Resource._5lego;
        public Image image6 = Properties.Resource._6gundam;
        public Image image7 = Properties.Resource._7yasuo;
        public Image image8 = Properties.Resource._8robotpacificrim;
        public Image image9 = Properties.Resource._9sieunhangao;
        public Image image10 = Properties.Resource._10robottraicay;
        public Image image11 = Properties.Resource.nv01;
        public Image image12 = Properties.Resource.nv02;
        public Image image13 = Properties.Resource.nv03;
        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private void HinhAnhSP_Load(object sender, EventArgs e)
        {

        }

    }
}
