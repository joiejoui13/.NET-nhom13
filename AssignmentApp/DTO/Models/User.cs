using System;

namespace AssignmentApp.DTO
{
    public class User
    {
        public int MaNguoiDung { get; set; }
        public string TenNguoiDung { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
    }
}
