using System;
using System.Collections.Generic;

namespace AssignmentApp.DTO
{
    public class StockIn
    {
        public string MaPhieuNhap { get; set; } = string.Empty;
        public string? MaNguoiDung { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TongTien { get; set; }
    }
}
