using System;

namespace AssignmentApp.DTO
{
    public class Category
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
