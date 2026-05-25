using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Session;
using AssignmentApp.DTO;
using Microsoft.Extensions.DependencyInjection;
using AssignmentApp.BLL.Services.Main;
using AssignmentApp.GUI.UserControls.Sales;
using AssignmentApp.GUI.UserControls.Admin;
using AssignmentApp.GUI.UserControls.Warehouse;

namespace AssignmentApp.GUI.Forms
{
    public partial class frmMain : Base.frmBase
    {
        private readonly IMainService _mainService;

        public frmMain(IMainService mainService)
        {
            InitializeComponent();
            _mainService = mainService;
            this.FormClosing += frmMain_FormClosing;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var user = UserSession.CurrentUser;
            ApplyAuthorization(user.VaiTro);

            picLogo.ImageLocation = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources\Anhlogo.png");
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.Padding = new Padding(0); // Đảm bảo không bị lề thụt vào


        }

        private void ApplyAuthorization(string role)
        {
            // Yêu cầu BLL (MainService) kiểm tra quyền hạn
            var permissions = _mainService.GetPermissions(role);

            // GUI chỉ nhận kết quả (true/false) từ BLL và thay đổi UI tương ứng
            pnlMenuAdmin.Visible = permissions.ShowAdmin;
            pnlMenuSales.Visible = permissions.ShowSales;
            pnlMenuWarehouse.Visible = permissions.ShowWarehouse;

            // Đẩy menu tương ứng lên trên cùng
            if (permissions.ShowAdmin) pnlMenuAdmin.BringToFront();
            if (permissions.ShowSales) pnlMenuSales.BringToFront();
            if (permissions.ShowWarehouse) pnlMenuWarehouse.BringToFront();

            // Xóa trung tâm và hiển thị vai trò trên Header
            pnlContainer.Controls.Clear();
            lblTitle.Text = $"QUYỀN TRUY CẬP: {(role?.Trim().ToUpper() ?? "")}";
        }

        private void HighlightButton(object senderButton)
        {
            if (senderButton == null) return;

            // Reset màu cho tất cả các nút
            Panel[] menus = { pnlMenuAdmin, pnlMenuSales, pnlMenuWarehouse };
            foreach (var panel in menus)
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn)
                    {
                        btn.FillColor = System.Drawing.Color.Transparent;
                        btn.ForeColor = System.Drawing.Color.FromArgb(160, 164, 177); // Màu chữ xám mặc định
                    }
                }
            }

            // Đặt màu xanh cho nút được bấm
            if (senderButton is Guna.UI2.WinForms.Guna2Button clickedBtn)
            {
                clickedBtn.FillColor = System.Drawing.Color.FromArgb(0, 126, 249); // Nền màu xanh
                clickedBtn.ForeColor = System.Drawing.Color.White; // Chữ màu trắng cho nổi
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            pnlContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(uc);
        }
        // ==========================================
        // EVENTS CHO MENU ADMIN
        // ==========================================
        private void btnAdmin_Reports_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucReports());
        }

        private void btnAdmin_Users_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucUserManagement());
        }

        private void btnAdmin_Promo_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucPromotion());
        }

        // ==========================================
        // EVENTS CHO MENU SALES
        // ==========================================
        private void btnSales_Orders_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucOrderManagement(defaultToPOS: false));
        }

        private void btnSales_Delivery_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucDelivery());
        }

        private void btnSales_Returns_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucReturn());
        }

        private void btnSales_Customers_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucCustomer());
        }

        // ==========================================
        // EVENTS CHO MENU WAREHOUSE
        // ==========================================
        private void btnWarehouse_Goods_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucProductList());
        }

        private void btnWarehouse_Category_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucCategory());
        }

        private void btnWarehouse_StockIn_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucStockIn());
        }

        private void btnWarehouse_Inventory_Click(object sender, EventArgs e)
        {
            HighlightButton(sender);
            LoadUserControl(new ucInventory());
        }

        private bool _isLoggingOut = false;

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = AssignmentApp.GUI.Utils.MsgBox.Show(this,
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận",
                Guna.UI2.WinForms.MessageDialogButtons.YesNo,
                Guna.UI2.WinForms.MessageDialogIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Bật cờ hiệu đăng xuất để bỏ qua hộp thoại xác nhận đóng ứng dụng
                _isLoggingOut = true;

                // Gọi xuống BLL (MainService) để xử lý nghiệp vụ đăng xuất
                _mainService.Logout();

                // Mở lại trang Login sử dụng DI Container
                this.Hide();
                frmAuth login = Program.ServiceProvider.GetRequiredService<frmAuth>();
                login.ShowDialog();
                this.Close();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đang trong quá trình đăng xuất, bỏ qua xác nhận đóng
            if (_isLoggingOut) return;

            DialogResult result = AssignmentApp.GUI.Utils.MsgBox.Show(this,
                "Bạn có chắc chắn muốn đóng ứng dụng không?",
                "Xác nhận đóng",
                Guna.UI2.WinForms.MessageDialogButtons.YesNo,
                Guna.UI2.WinForms.MessageDialogIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy lệnh đóng Form
            }
        }

        private void pnlMenuAdmin_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
