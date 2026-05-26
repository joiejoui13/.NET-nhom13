namespace AssignmentApp.DTO
{
    public class ReturnDetail
    {
        public string MaChiTietTra { get; set; } = string.Empty;
        public string MaTraHang { get; set; } = string.Empty;
        public string MaSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal TienHoan { get; set; }

        // Binding helper
        public string TenSanPham { get; set; } = string.Empty;
    }
}
