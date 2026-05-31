using System;

namespace AssignmentApp.DTO.Models
{
    public class SalesReportRow
    {
        public string MaHoaDon { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string TenNguoiDung { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public decimal TongTien { get; set; }
        public string HinhThucThanhToan { get; set; } = string.Empty;
    }

    public class RevenueTrendRow
    {
        public string Period { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
    }

    public class TopProductRow
    {
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class OrderStatusRow
    {
        public string TrangThai { get; set; } = string.Empty;
        public int SoLuong { get; set; }
    }
}
