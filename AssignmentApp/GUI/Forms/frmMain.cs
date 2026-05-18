using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Session;
using AssignmentApp.GUI.UserControls.Sales;
using AssignmentApp.GUI.UserControls.Admin;
using AssignmentApp.GUI.UserControls.Warehouse;

namespace AssignmentApp.GUI.Forms
{
    public partial class frmMain : Base.frmBase
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var user = UserSession.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Vui lòng đăng nhập!");
                this.Close();
                return;
            }
            picLogo.ImageLocation = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources\Anhlogo.png");
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.Padding = new Padding(0); // Đảm bảo không bị lề thụt vào
           
            ApplyAuthorization(user.VaiTro);
        }

        private void ApplyAuthorization(string role)
        {
            pnlMenuAdmin.Visible = false;
            pnlMenuSales.Visible = false;
            pnlMenuWarehouse.Visible = false;

            switch (role?.Trim().ToUpper())
            {
                case "ADMIN":
                    pnlMenuAdmin.Visible = true;
                    pnlMenuAdmin.BringToFront();
                    LoadUserControl(new ucReports(), "BÁO CÁO TỔNG HỢP");
                    break;

                case "SALES":
                    pnlMenuSales.Visible = true;
                    pnlMenuSales.BringToFront();
                    LoadUserControl(new ucPOS(), "HỆ THỐNG BÁN HÀNG (POS)");
                    break;

                case "WAREHOUSE":
                    pnlMenuWarehouse.Visible = true;
                    pnlMenuWarehouse.BringToFront();
                    LoadUserControl(new ucStockIn(), "QUẢN LÝ NHẬP KHO");
                    break;
            }
        }

        private void LoadUserControl(UserControl uc, string title)
        {
            lblTitle.Text = title;
            pnlContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(uc);
        }
        // ==========================================
        // EVENTS CHO MENU ADMIN
        // ==========================================
        private void btnAdmin_Reports_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucReports(), "BÁO CÁO TỔNG HỢP");
        }

        private void btnAdmin_Users_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucUserManagement(), "QUẢN LÝ NHÂN VIÊN");
        }

        private void btnAdmin_Promo_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucPromotion(), "QUẢN LÝ KHUYẾN MÃI");
        }

        // ==========================================
        // EVENTS CHO MENU SALES
        // ==========================================
        private void btnSales_POS_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucPOS(), "HỆ THỐNG BÁN HÀNG (POS)");
        }

        private void btnSales_Orders_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucOrderManagement(), "DANH SÁCH ĐƠN HÀNG");
        }

        private void btnSales_Delivery_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucDelivery(), "QUẢN LÝ GIAO HÀNG");
        }

        private void btnSales_Returns_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucReturns(), "TRẢ HÀNG / ĐỔI TRẢ");
        }

        private void btnSales_Customers_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucCustomer(), "DANH SÁCH KHÁCH HÀNG");
        }

        // ==========================================
        // EVENTS CHO MENU WAREHOUSE
        // ==========================================
        private void btnWarehouse_Goods_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucProductList(), "DANH MỤC HÀNG HÓA");
        }

        private void btnWarehouse_Category_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucCategory(), "PHÂN LOẠI SẢN PHẨM");
        }

        private void btnWarehouse_StockIn_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucStockIn(), "QUẢN LÝ NHẬP KHO");
        }

        private void btnWarehouse_Inventory_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucInventory(), "KIỂM TRA TỒN KHO");
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            UserSession.ClearSession();
            frmAuth login = new frmAuth();
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Khởi động lại ứng dụng, tự động quay về Form chạy đầu tiên (Login)
                Application.Restart();
            }
        }
    }
}
