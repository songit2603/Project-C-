using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Forms;
using BLL;

namespace GUI
{
    public partial class frmMain : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        int PanelWidth;
        string ucName;
        Label lblPhanTram;
        System.Windows.Forms.ProgressBar ProgressBar;
        Task taskLoadProgressBar;
        Task taskLoadUserControl;
        private object senderLocal;
        public frmMain()
        {
            InitializeComponent();
            random = new Random();
            PanelWidth = panelLeft.Width;
        }

        public void AddControl(string ucname)
        {
            ucName = ucname;
            //Xóa các uc trước khi thêm uc mới vào
            foreach (Form uc in panelControls.Controls.OfType<Form>())
            {
                panelControls.Controls.Remove(uc);
            }
            //Add vào ProgressBar từ ucProgressBar
            ucProgressbar ucprogress = new ucProgressbar();
            ucprogress.Dock = DockStyle.Fill;
            panelControls.Controls.Add(ucprogress);
            panelControls.Controls["ucProgressBar"].BringToFront();
            //Ánh xạ lên biến toàn cục
            ProgressBar = ucprogress.progressLoading;
            lblPhanTram = ucprogress.lblPhanTram;

            taskLoadProgressBar = new Task(LoadProgressBar);
            taskLoadUserControl = new Task(LoadUserControl);
            taskLoadProgressBar.Start();
            taskLoadUserControl.Start();
        }
        public void LoadProgressBar()
        {
            for (int i = 0; i <= 100; i++)
            {
                //Tách ra khỏi Thread chính
                ProgressBar.Invoke(new MethodInvoker(delegate
                {
                    ProgressBar.Value = i;
                }));

                //Tách ra khỏi Thread chính
                lblPhanTram.Invoke(new MethodInvoker(delegate
                {
                    lblPhanTram.Text = i.ToString() + "%";
                }));
            }
        }
        public void LoadUserControl()
        {
            taskLoadProgressBar.Wait();
            panelControls.Invoke(new MethodInvoker(delegate ()
            {
                switch (ucName)
                {
                    case "ucTrangChu":
                        {
                            frmTrangChu ucTC = new frmTrangChu();
                            AddControlsIntoPanel(ucTC);
                        }
                        break;
                    case "ucSanPham":
                        {
                            frmSanPham ucSP = new frmSanPham();
                            AddControlsIntoPanel(ucSP);
                        }
                        break;
                    case "ucBanSanPham":
                        {
                            frmBanSanPham ucBH = new frmBanSanPham();
                            AddControlsIntoPanel(ucBH);
                        }
                        break;
                    case "ucKhachHang":
                        {
                            frmKhachHang ucKH = new frmKhachHang();
                            AddControlsIntoPanel(ucKH);
                        }
                        break;
                    case "ucNhanVien":
                        {
                            frmNhanVien ucNV = new frmNhanVien();
                            AddControlsIntoPanel(ucNV);
                        }
                        break;
                }
            }));

        }
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }
        private void activeButton(object sender)
        {
            if (sender != null)
            {
                if (currentButton != (Button)sender)
                {
                    disableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)sender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        private void disableButton()
        {
            foreach (Control previousBtn in panelLeft.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.White;
                    previousBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = string.Empty;
                this.ControlBox = false;
                btnTrangChu.PerformClick();
                //btnENTITY.PerformClick();
                DateTime dt = DateTime.Now;
                lblTime.Text = dt.ToString("HH:MM:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                //AddData themdulieu = new AddData();
                //themdulieu.themdulieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OpenChildForm(Form childForm, object sender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeButton(sender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelControls.Controls.Add(childForm);
            this.panelControls.Tag = childForm;
            childForm.Show();
        }
        private void AddControlsIntoPanel(Form c)
        {
            c.Dock = DockStyle.Fill;
            panelControls.Controls.Clear();      
            OpenChildForm(c, senderLocal);
        }
        private void check_reset(Button button)
        {
            foreach (Control control in panelLeft.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button btn = (Button)control;
                    if (btn.Text != button.Text)
                    {
                        if (btn.BackColor != Color.White)
                        {
                            btn.BackColor = Color.FromArgb(17, 145, 249);
                            btn.ForeColor = Color.White;
                            //btn.Image = (Image)Properties.Resources.ResourceManager.GetObject(btn.AccessibleName + "_blue");
                        }
                        btn.FlatAppearance.MouseDownBackColor = Color.White;
                    }
                }
            }
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            senderLocal = sender;
            Cursor = Cursors.AppStarting;
            AddControl("ucTrangChu");
            check_reset(btnTrangChu);
            Cursor = Cursors.Default;
        }

        private void btnBanSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                senderLocal = sender;
                Cursor = Cursors.AppStarting;
                AddControl("ucBanSanPham");
                check_reset(btnTrangChu);
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            senderLocal = sender;
            Cursor = Cursors.AppStarting;
            AddControl("ucSanPham");
            check_reset(btnTrangChu);
            Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void OpenNotifyForm(Form childForm, object sender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeButton(sender);
            activeForm = childForm;
            childForm.Show();
        }
        private void btnENTITY_Click(object sender, EventArgs e)
        {
/*            try
            {*/
/*                lblDangKhoiTao.Visible = true;
                lblDangKhoiTao.ForeColor = Color.Red;
                DataRun.GetInstance.themdulieu();
                lblDangKhoiTao.Text = "Khởi tạo thành công!!!";
                lblDangKhoiTao.ForeColor = Color.LightGreen;
                OpenNotifyForm(new Forms.frmPopUpNoti(), sender);*/
/*            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblDangKhoiTao.Visible = false;
            }*/
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                senderLocal = sender;
                Cursor = Cursors.AppStarting;
                AddControl("ucNhanVien");
                check_reset(btnTrangChu);
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            try
            {
                senderLocal = sender;
                Cursor = Cursors.AppStarting;
                AddControl("ucKhachHang");
                check_reset(btnTrangChu);
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
