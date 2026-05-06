using AssignmentApp.Class;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;
// Cac nut su dung trong form
// Label
// TextBox
// materialButton
// uiGroupBox
// uiSymbolLabel
// hopeGroupBox
// materialButton

namespace AssignmentApp.Page
{
    public partial class frmAuth : MaterialForm
    {
        public frmAuth()
        {
            InitializeComponent();
            //Sua mau cac nut cua thu vien materialSkin phai dung lẹnh nay
            //var materialSkinManager = MaterialSkinManager.Instance;
            //materialSkinManager.AddFormToManage(this);
            //materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);)
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            if (Functions.Ketnoi())
            {
                lblConnect.Text = "SERVER CONNECTION: SECURE";
                lblConnect.SymbolColor = System.Drawing.Color.FromArgb(0, 112, 112);
            }
            else
            {
                lblConnect.Text = "SERVER CONNECTION: FAILED";
                lblConnect.SymbolColor = System.Drawing.Color.FromArgb(255, 0, 0);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiSymbolLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                return;
            }
            // Gọi hàm lấy quyền từ Database
            string sql = $"SELECT Vaitro FROM tblNhanvien WHERE CCCD = '{user}' AND Matkhau = '{pass}'";

            string vaitro = Functions.GetFieldValues(sql);
        
            if (vaitro == "Pos")
            {
                MessageBox.Show("Đăng nhập thành công với quyền POS!");
                this.Hide(); // Ẩn form đăng nhập
                RolePos.Pos frmPos = new RolePos.Pos(); // Tạo form POS
                frmPos.ShowDialog();
                this.Close(); // Đóng hẳn khi form POS tắt
            }
            else if (vaitro == "ADMIN")
            {
                // Code mở form Admin của bạn ở đây
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUser.Text = "";
            txtPass.Text = "";
        }
    }
}
