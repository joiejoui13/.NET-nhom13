using System;

namespace AssignmentApp.DTO
{
    public class Product
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public double GiaBan { get; set; }
        public double GiaNhap { get; set; }
        public int SoLuongTon { get; set; }
        public string? MoTa { get; set; }
        public string? Anh { get; set; }
        public string? TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
