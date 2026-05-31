using System;
using System.Collections.Generic;

namespace AssignmentApp.DTO.Models
{
    public class StockInReceipt
    {
        public int MaPhieuNhap { get; set; }
        public int MaNguoiDung { get; set; }
        public double TongTien { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayNhap { get; set; }
        
        // Bổ sung thêm trường tên người dùng để hiển thị lên lưới
        public string TenNguoiDung { get; set; }
    }

    // Mô hình cho chi tiết nhập hàng (dùng khi lưu CSDL và hiển thị Giỏ hàng)
    public class StockInDetailModel
    {
        public int MaPhieuNhap { get; set; }
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = "";
        public int SoLuong { get; set; }
        public double GiaNhap { get; set; }
        public double ThanhTien 
        {
            get 
            {
                return SoLuong * GiaNhap;
            }
        }
    }
}
