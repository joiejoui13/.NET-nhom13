using System;

namespace AssignmentApp.DTO
{
    public class Return
    {
        public string MaTraHang { get; set; } = string.Empty;
        public string MaHoaDon { get; set; } = string.Empty;
        public DateTime NgayTra { get; set; }
        public string? LyDo { get; set; }
        public decimal TongTienHoan { get; set; }
        public string? MaNguoiDung { get; set; }
    }
}
