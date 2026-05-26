using System;

namespace AssignmentApp.DTO
{
    public class InventoryLog
    {
        public string MaLichSu { get; set; } = string.Empty;
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuongThayDoi { get; set; }
        public string Loai { get; set; } = string.Empty;
        public DateTime Ngay { get; set; }
    }
}
