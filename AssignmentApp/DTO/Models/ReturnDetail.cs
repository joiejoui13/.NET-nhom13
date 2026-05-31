namespace AssignmentApp.DTO
{
    public class ReturnDetail
    {
        public int MaChiTietTra { get; set; }
        public int MaTraHang { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal TienHoan { get; set; }
        public string TinhTrang { get; set; } = string.Empty;

        // Binding helper (UI display fields)
        public string TenSanPham { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
    }
}
