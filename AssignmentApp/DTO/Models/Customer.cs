using System;

namespace AssignmentApp.DTO
{
    public class Customer
    {
        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayTao { get; set; }
        public string? TrangThai { get; set; }
    }
}
