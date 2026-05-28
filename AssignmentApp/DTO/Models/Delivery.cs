using System;

namespace AssignmentApp.DTO
{
    public class Delivery
    {
        public string MaGiaoHang { get; set; } = string.Empty;
        public string MaHoaDon { get; set; } = string.Empty;
        public string? DiaChiGiao { get; set; }
        public string? TrangThaiGiao { get; set; }
        public DateTime? NgayGiao { get; set; }
    }
}
