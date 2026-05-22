using System;

namespace AssignmentApp.DTO
{
    public class Order
    {
        public string MaHoaDon { get; set; } = string.Empty;
        public string? MaKhachHang { get; set; }
        public string MaNguoiDung { get; set; } = string.Empty;
        public string? MaKhuyenMai { get; set; }
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public string? MaGiaoHang { get; set; }
        public string? HinhThucThanhToan { get; set; }
        public DateTime NgayTao { get; set; }

        // Helper properties
        public string? TenKhachHang { get; set; }
        public string? TenNguoiDung { get; set; }
    }
}
