using System;

namespace AssignmentApp.DTO
{
    public class OrderDetail
    {
        public string MaChiTiet { get; set; } = string.Empty;
        public string MaHoaDon { get; set; } = string.Empty;
        public string MaSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        
        // Helper property
        public string? TenSanPham { get; set; }
    }
}
