using System;

namespace AssignmentApp.DTO
{
    public class Return
    {
        public int MaTraHang { get; set; }
        public int MaHoaDon { get; set; }
        public int MaNguoiDung { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public decimal TongTienHoan { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public DateTime NgayTra { get; set; }
        public string LoaiGiaoDich { get; set; } = "Trả hàng";

        // Các field dùng để hiển thị trên UI (JOIN)
        public string NhanVien { get; set; } = string.Empty;
        public string KhachHang { get; set; } = string.Empty;
    }
}
