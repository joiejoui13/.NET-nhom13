using System;

namespace AssignmentApp.DTO
{
    public class InventoryLog
    {
        public int MaLichSu { get; set; }
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int ThayDoi { get; set; }
        public int SoLuongTruoc { get; set; }
        public int SoLuongSau { get; set; }
        public string LoaiGiaoDich { get; set; } = string.Empty;
        public int MaThamChieu { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public DateTime Thoigian { get; set; }
    }
}
