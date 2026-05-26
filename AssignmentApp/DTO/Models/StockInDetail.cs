using System;

namespace AssignmentApp.DTO
{
    public class StockInDetail
    {
        public string MaChiTietPhieuNhap { get; set; } = string.Empty;
        public string MaPhieuNhap { get; set; } = string.Empty;
        public string MaSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }

        // Helper properties for display
        public string TenSanPham { get; set; } = string.Empty;
        public decimal ThanhTien => SoLuong * GiaNhap;
    }
}
