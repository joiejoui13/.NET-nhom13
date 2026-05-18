using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using AssignmentApp.BLL.Services.Security;
using AssignmentApp.BLL.Session;
using AssignmentApp.DAL.Core;
using AssignmentApp.GUI.Forms;

namespace AssignmentApp.GUI
{
    public partial class frmAuth : Base.frmBase
    {
        private readonly AuthService _authService;

        public frmAuth()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            if (DbContext.Ketnoi())
            {
                lblConnect.Text = "SERVER CONNECTION: SECURE";
                lblConnect.FillColor = System.Drawing.Color.FromArgb(192, 255, 192);
            }
            else
            {
                lblConnect.Text = "SERVER CONNECTION: FAILED";
                lblConnect.FillColor = System.Drawing.Color.FromArgb(255, 192, 192);
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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang xác thực...";

                // Gọi tầng BLL
                var userDto = await _authService.LoginAsync(user, pass);

                if (userDto != null)
                {
                    // Lưu Session
                    UserSession.CurrentUser = userDto;
                    UserSession.LoginTime = DateTime.Now;

                    string role = userDto.VaiTro?.Trim().ToUpper();
                    if (role == "SALES" || role == "ADMIN" || role == "WAREHOUSE")
                    {
                        MessageBox.Show($"Đăng nhập thành công với quyền {role}!");
                        this.Hide();
                        frmMain main = new frmMain();
                        main.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Vai trò không hợp lệ!");
                    }
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi kết nối: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "LOGIN";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUser.Text = "";
            txtPass.Text = "";
        }

        private void btnCancel_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon thoat khong?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
              == System.Windows.Forms.DialogResult.Yes)
                Application.Exit();
        }
    }
}
