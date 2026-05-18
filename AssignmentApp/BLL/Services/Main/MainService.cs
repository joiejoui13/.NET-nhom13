using System;
using AssignmentApp.DAL.Repositories.Main;
using AssignmentApp.BLL.Session;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Main
{
    public class MainService : IMainService
    {
        private readonly IMainRepository _mainRepository;

        public MainService(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        // Nghiệp vụ đăng xuất (xóa Session)
        public void Logout()
        {
            UserSession.ClearSession();
        }

        // BLL xử lý nghiệp vụ và quyết định Panel Menu nào được phép hiển thị
        public MenuPermissions GetPermissions(string role)
        {
            var permissions = new MenuPermissions();
            string roleName = role?.Trim().ToUpper() ?? "";

            switch (roleName)
            {
                case "ADMIN":
                    permissions.ShowAdmin = true;
                    break;
                case "SALES":
                    permissions.ShowSales = true;
                    break;
                case "WAREHOUSE":
                    permissions.ShowWarehouse = true;
                    break;
            }
            return permissions;
        }
    }
}
