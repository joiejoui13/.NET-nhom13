using System;

namespace AssignmentApp.DTO
{
    public class Promotion
    {
        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; } = string.Empty;
        public int PhanTramGiamGia { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTaKhuyenMai { get; set; }
        public string? TrangThai { get; set; }
    }
}
