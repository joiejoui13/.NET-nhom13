using System;

namespace AssignmentApp.DTO
{
    public class Customer
    {
        public string MaKhachHang { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public int DiemTichLuy { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
