using System;

namespace AssignmentApp.DTO
{
    // DTO chứa quyền hạn hiển thị menu của hệ thống
    public class MenuPermissions
    {
        public bool ShowAdmin { get; set; } = false;
        public bool ShowSales { get; set; } = false;
        public bool ShowWarehouse { get; set; } = false;
    }
}
