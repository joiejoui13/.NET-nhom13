using System;

namespace AssignmentApp.DTO
{
    public class Delivery
    {
        public int MaGiaoHang { get; set; }
        public int? MaHoaDon { get; set; }
        public int? MaTraHang { get; set; }
        public string? DiaChiGiao { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayGiao { get; set; }
    }
}
