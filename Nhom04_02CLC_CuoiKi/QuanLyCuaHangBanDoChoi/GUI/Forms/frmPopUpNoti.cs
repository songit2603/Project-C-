using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmPopUpNoti : Form
    {
        public frmPopUpNoti()
        {
            InitializeComponent();
        }

        private void Formloading_Load(object sender, EventArgs e)
        {
            this.Top = 720;
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
        }

        private void close_Tick(object sender, EventArgs e)
        {
            if(this.Opacity>0)
            {
                this.Opacity-=0.05;
            }
            else
            {
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            close.Start();
        }
    }
}
