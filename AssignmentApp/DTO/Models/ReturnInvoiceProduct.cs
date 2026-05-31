namespace AssignmentApp.DTO
{
    public class ReturnInvoiceProduct
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SLMua { get; set; }
        public int DaTra { get; set; }
        public decimal DonGia { get; set; }
        public string Anh { get; set; } = string.Empty;
    }
}
