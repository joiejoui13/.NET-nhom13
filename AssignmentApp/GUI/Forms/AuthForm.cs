using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using AssignmentApp.BLL.Services.Main;
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
            this.AcceptButton = btnLogin; // Khi ấn Enter sẽ tự động kích hoạt nút Login
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
                AssignmentApp.GUI.Utils.MsgBox.Show(this, "Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Yêu cầu", Guna.UI2.WinForms.MessageDialogButtons.OK, Guna.UI2.WinForms.MessageDialogIcon.Warning);
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
                        AssignmentApp.GUI.Utils.MsgBox.Show(this, $"Đăng nhập thành công với quyền {role}!", "Thành công", Guna.UI2.WinForms.MessageDialogButtons.OK, Guna.UI2.WinForms.MessageDialogIcon.Information);
                        this.Hide();
                        frmMain main = new frmMain();
                        main.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        AssignmentApp.GUI.Utils.MsgBox.Show(this, "Vai trò không hợp lệ!", "Lỗi", Guna.UI2.WinForms.MessageDialogButtons.OK, Guna.UI2.WinForms.MessageDialogIcon.Error);
                    }
                }
                else
                {
                    AssignmentApp.GUI.Utils.MsgBox.Show(this, "Sai tài khoản hoặc mật khẩu!", "Lỗi", Guna.UI2.WinForms.MessageDialogButtons.OK, Guna.UI2.WinForms.MessageDialogIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AssignmentApp.GUI.Utils.MsgBox.Show(this, "Đã xảy ra lỗi kết nối: " + ex.Message, "Lỗi hệ thống", Guna.UI2.WinForms.MessageDialogButtons.OK, Guna.UI2.WinForms.MessageDialogIcon.Error);
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
            var result = AssignmentApp.GUI.Utils.MsgBox.Show(this, "Bạn có muốn thoát không?", "Thông báo", Guna.UI2.WinForms.MessageDialogButtons.YesNo, Guna.UI2.WinForms.MessageDialogIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
                Application.Exit();
        }
        //ProcessCmdKey là một phương thức ảo (Virtual Method) được định nghĩa sẵn bởi Microsoft sâu bên trong lớp cha System.Windows.Forms.Form (Form cơ bản của hệ thống).
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Nhấn mũi tên Lên -> Nhảy lên ô Username
            if (keyData == Keys.Up)
            {
                txtUser.Focus();
                txtUser.SelectAll(); // Bôi đen để gõ đè nhanh
                return true; // Đã xử lý xong phím này
            }
            // Nhấn mũi tên Xuống -> Nhảy xuống ô Password
            else if (keyData == Keys.Down)
            {
                txtPass.Focus();
                txtPass.SelectAll(); // Bôi đen để gõ đè nhanh
                return true; // Đã xử lý xong phím này
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
