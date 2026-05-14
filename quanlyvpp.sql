USE master;
GO
-- Nếu database cũ đang bận, ép đóng kết nối để DROP không bị kẹt
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LAPNPT')
BEGIN
    ALTER DATABASE LAPNPT SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LAPNPT;
END
GO

CREATE DATABASE LAPNPT;
GO
USE LAPNPT;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 1. BẢNG DANH MỤC
CREATE TABLE [dbo].[DanhMuc](
	[MaDanhMuc] [varchar](20) NOT NULL,
	[TenDanhMuc] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](255) NOT NULL,
	[NgayTao] [datetime] NOT NULL,
 CONSTRAINT [PK_DanhMuc] PRIMARY KEY CLUSTERED ([MaDanhMuc] ASC)
) ON [PRIMARY]
GO

-- 2. BẢNG SẢN PHẨM
CREATE TABLE [dbo].[SanPham](
	[MaSanPham] [varchar](20) NOT NULL,
	[TenSanPham] [nvarchar](150) NOT NULL,
	[MaDanhMuc] [varchar](20) NOT NULL,
	[GiaBan] [float] NOT NULL,
	[GiaNhap] [float] NOT NULL,
	[SoLuongTon] [int] NOT NULL,
	[MoTa] [nvarchar](255) NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
	[NgayTao] [datetime] NOT NULL,
 CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED ([MaSanPham] ASC)
) ON [PRIMARY]
GO

-- 3. BẢNG KHÁCH HÀNG
CREATE TABLE [dbo].[KhachHang](
	[MaKhachHang] [varchar](20) NOT NULL,
	[TenKhachHang] [nvarchar](100) NOT NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[DiemTichLuy] [int] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
 CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED ([MaKhachHang] ASC)
) ON [PRIMARY]
GO

-- 4. BẢNG KHUYẾN MÃI
CREATE TABLE [dbo].[KhuyenMai](
	[MaKhuyenMai] [varchar](20) NOT NULL,
	[PhanTramGiamGia] [float] NOT NULL,
	[NgayBatDau] [datetime] NOT NULL,
	[NgayHetHan] [datetime] NOT NULL,
	[MoTaKhuyenMai] [nvarchar](500) NOT NULL,
	[TenKhuyenMai] [nvarchar](255) NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KhuyenMai] PRIMARY KEY CLUSTERED ([MaKhuyenMai] ASC)
) ON [PRIMARY]
GO

-- 5. BẢNG NGƯỜI DÙNG (NHÂN VIÊN/ADMIN)
CREATE TABLE [dbo].[NguoiDung](
	[MaNguoiDung] [varchar](20) NOT NULL,
	[TenNguoiDung] [nvarchar](100) NOT NULL,
	[SoDienThoai] [nvarchar](15) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[MatKhau] [varchar](255) NOT NULL,
	[VaiTro] [varchar](20) NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
	[NgayTao] [datetime] NOT NULL,
 CONSTRAINT [PK_NguoiDung] PRIMARY KEY CLUSTERED ([MaNguoiDung] ASC)
) ON [PRIMARY]
GO

-- 6. BẢNG HÓA ĐƠN (Đã bỏ cột MaGiaoHang lỗi logic)
CREATE TABLE [dbo].[HoaDon](
	[MaHoaDon] [varchar](20) NOT NULL,
	[MaKhachHang] [varchar](20) NOT NULL,
	[MaNguoiDung] [varchar](20) NOT NULL,
	[MaKhuyenMai] [varchar](20) NOT NULL,
	[TongTien] [float] NOT NULL,
	[GiamGia] [float] NOT NULL,
	[HinhThucThanhToan] [varchar](50) NOT NULL,
	[NgayTao] [datetime] NOT NULL,
 CONSTRAINT [PK_HoaDon] PRIMARY KEY CLUSTERED ([MaHoaDon] ASC)
) ON [PRIMARY]
GO

-- 7. BẢNG GIAO HÀNG (Liên kết trực tiếp tới MaHoaDon)
CREATE TABLE [dbo].[GiaoHang](
	[MaGiaoHang] [varchar](20) NOT NULL,
	[MaHoaDon] [varchar](20) NOT NULL,
	[DiaChiGiao] [nvarchar](255) NOT NULL,
	[TrangThaiGiao] [varchar](50) NOT NULL,
	[NgayGiao] [datetime] NOT NULL,
 CONSTRAINT [PK_GiaoHang] PRIMARY KEY CLUSTERED ([MaGiaoHang] ASC)
) ON [PRIMARY]
GO

-- 8. BẢNG CHI TIẾT HÓA ĐƠN
CREATE TABLE [dbo].[ChiTietHoaDon](
	[MaChiTiet] [varchar](20) NOT NULL,
	[MaHoaDon] [varchar](20) NOT NULL,
	[MaSanPham] [varchar](20) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[DonGia] [float] NOT NULL,
	[ThanhTien] [float] NOT NULL,
 CONSTRAINT [PK_ChiTietHoaDon] PRIMARY KEY CLUSTERED ([MaChiTiet] ASC)
) ON [PRIMARY]
GO

-- 9. BẢNG PHIẾU NHẬP VÀ CHI TIẾT NHẬP
CREATE TABLE [dbo].[PhieuNhap](
	[MaPhieuNhap] [varchar](20) NOT NULL,
	[MaNguoiDung] [varchar](20) NOT NULL,
	[NgayNhap] [datetime] NOT NULL,
	[TongTien] [float] NOT NULL,
 CONSTRAINT [PK_PhieuNhap] PRIMARY KEY CLUSTERED ([MaPhieuNhap] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ChiTietPhieuNhap](
	[MaChiTietNhap] [varchar](20) NOT NULL,
	[MaPhieuNhap] [varchar](20) NOT NULL,
	[MaSanPham] [varchar](20) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[GiaNhap] [float] NOT NULL,
 CONSTRAINT [PK_ChiTietPhieuNhap] PRIMARY KEY CLUSTERED ([MaChiTietNhap] ASC)
) ON [PRIMARY]
GO

-- 10. BẢNG ĐỔI HÀNG VÀ CHI TIẾT ĐỔI HÀNG (Sửa SoLuong thành kiểu INT)
CREATE TABLE [dbo].[DoiHang](
	[MaDoiHang] [varchar](20) NOT NULL,
	[MaHoaDon] [varchar](20) NOT NULL,
	[NgayDoi] [datetime] NOT NULL,
	[MaNguoiDung] [varchar](20) NOT NULL,
	[LyDo] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DoiHang] PRIMARY KEY CLUSTERED ([MaDoiHang] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ChiTietDoiHang](
	[MaChiTietDoi] [varchar](20) NOT NULL,
	[MaDoiHang] [varchar](20) NOT NULL,
	[MaSanPhamCu] [varchar](20) NOT NULL,
	[MaSanPhamMoi] [varchar](20) NOT NULL,
	[SoLuong] [int] NOT NULL, -- Đã sửa đổi từ VARCHAR thành INT
	[ChenhLechGia] [float] NOT NULL,
 CONSTRAINT [PK_ChiTietDoiHang] PRIMARY KEY CLUSTERED ([MaChiTietDoi] ASC)
) ON [PRIMARY]
GO

-- 11. BẢNG TRẢ HÀNG VÀ CHI TIẾT TRẢ HÀNG
CREATE TABLE [dbo].[TraHang](
	[MaTraHang] [varchar](20) NOT NULL,
	[MaHoaDon] [varchar](20) NOT NULL,
	[NgayTra] [datetime] NOT NULL,
	[LyDo] [nvarchar](255) NOT NULL,
	[TongTienHoan] [float] NOT NULL,
	[MaNguoiDung] [varchar](20) NOT NULL,
 CONSTRAINT [PK_TraHang] PRIMARY KEY CLUSTERED ([MaTraHang] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ChiTietTraHang](
	[MaChiTietTra] [varchar](20) NOT NULL,
	[MaTraHang] [varchar](20) NOT NULL,
	[MaSanPham] [varchar](20) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[TienHoan] [float] NOT NULL,
 CONSTRAINT [PK_ChiTietTraHang] PRIMARY KEY CLUSTERED ([MaChiTietTra] ASC)
) ON [PRIMARY]
GO

-- 12. BẢNG LỊCH SỬ TỒN KHO
CREATE TABLE [dbo].[LichSuTonKho](
	[MaLichSu] [varchar](20) NOT NULL,
	[MaSanPham] [varchar](20) NOT NULL,
	[SoLuongThayDoi] [int] NOT NULL,
	[Loai] [varchar](20) NOT NULL,
	[Ngay] [datetime] NOT NULL,
 CONSTRAINT [PK_LichSuTonKho] PRIMARY KEY CLUSTERED ([MaLichSu] ASC)
) ON [PRIMARY]
GO


-- ====================================================================================
-- TẠO CÁC RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEYS)
-- ====================================================================================

-- Khóa ngoại cho SanPham
ALTER TABLE [dbo].[SanPham] WITH CHECK ADD CONSTRAINT [FK_SanPham_DanhMuc] FOREIGN KEY([MaDanhMuc])
REFERENCES [dbo].[DanhMuc] ([MaDanhMuc])
GO

-- Khóa ngoại cho HoaDon
ALTER TABLE [dbo].[HoaDon] WITH CHECK ADD CONSTRAINT [FK_HoaDon_KhachHang] FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KhachHang] ([MaKhachHang])
GO
ALTER TABLE [dbo].[HoaDon] WITH CHECK ADD CONSTRAINT [FK_HoaDon_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[HoaDon] WITH CHECK ADD CONSTRAINT [FK_HoaDon_KhuyenMai] FOREIGN KEY([MaKhuyenMai])
REFERENCES [dbo].[KhuyenMai] ([MaKhuyenMai])
GO

-- Khóa ngoại cho GiaoHang
ALTER TABLE [dbo].[GiaoHang] WITH CHECK ADD CONSTRAINT [FK_GiaoHang_HoaDon] FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO

-- Khóa ngoại cho ChiTietHoaDon
ALTER TABLE [dbo].[ChiTietHoaDon] WITH CHECK ADD CONSTRAINT [FK_ChiTietHoaDon_HoaDon] FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO
ALTER TABLE [dbo].[ChiTietHoaDon] WITH CHECK ADD CONSTRAINT [FK_ChiTietHoaDon_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO

-- Khóa ngoại cho PhieuNhap và ChiTietPhieuNhap
ALTER TABLE [dbo].[PhieuNhap] WITH CHECK ADD CONSTRAINT [FK_PhieuNhap_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] WITH CHECK ADD CONSTRAINT [FK_ChiTietPhieuNhap_PhieuNhap] FOREIGN KEY([MaPhieuNhap])
REFERENCES [dbo].[PhieuNhap] ([MaPhieuNhap])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] WITH CHECK ADD CONSTRAINT [FK_ChiTietPhieuNhap_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO

-- Khóa ngoại cho DoiHang và ChiTietDoiHang (Đã bổ sung khóa ngoại SanPhamCu và SanPhamMoi)
ALTER TABLE [dbo].[DoiHang] WITH CHECK ADD CONSTRAINT [FK_DoiHang_HoaDon] FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO
ALTER TABLE [dbo].[DoiHang] WITH CHECK ADD CONSTRAINT [FK_DoiHang_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[ChiTietDoiHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietDoiHang_DoiHang] FOREIGN KEY([MaDoiHang])
REFERENCES [dbo].[DoiHang] ([MaDoiHang])
GO
ALTER TABLE [dbo].[ChiTietDoiHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietDoiHang_SanPhamCu] FOREIGN KEY([MaSanPhamCu])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[ChiTietDoiHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietDoiHang_SanPhamMoi] FOREIGN KEY([MaSanPhamMoi])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO

-- Khóa ngoại cho TraHang và ChiTietTraHang
ALTER TABLE [dbo].[TraHang] WITH CHECK ADD CONSTRAINT [FK_TraHang_HoaDon] FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO
ALTER TABLE [dbo].[TraHang] WITH CHECK ADD CONSTRAINT [FK_TraHang_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[ChiTietTraHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietTraHang_TraHang] FOREIGN KEY([MaTraHang])
REFERENCES [dbo].[TraHang] ([MaTraHang])
GO
ALTER TABLE [dbo].[ChiTietTraHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietTraHang_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO

-- Khóa ngoại cho LichSuTonKho
ALTER TABLE [dbo].[LichSuTonKho] WITH CHECK ADD CONSTRAINT [FK_LichSuTonKho_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO

USE LAPNPT;
GO
-- ====================================================================================
-- 1. INSERT DATA FOR BẢNG [DanhMuc] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[DanhMuc] ([MaDanhMuc], [TenDanhMuc], [MoTa], [NgayTao]) VALUES
('DM001', N'Laptop Gaming', N'Laptop hiệu năng cao cho game thủ', '2026-01-01'),
('DM002', N'Laptop Văn Phòng', N'Laptop mỏng nhẹ, pin lâu', '2026-01-02'),
('DM003', N'Laptop Đồ Họa', N'Laptop màn hình chuẩn màu, cấu hình mạnh', '2026-01-03'),
('DM004', N'Bàn Phím Cơ', N'Bàn phím cơ các loại switch', '2026-01-04'),
('DM005', N'Chuột Máy Tính', N'Chuột gaming và chuột văn phòng', '2026-01-05'),
('DM006', N'Tai Nghe Máy Tính', N'Tai nghe chụp tai và nhét tai', '2026-01-06'),
('DM007', N'Màn Hình Máy Tính', N'Màn hình từ 24 inch đến 32 inch', '2026-01-07'),
('DM008', N'Ram Máy Tính', N'Ram DDR4, DDR5 cho PC và Laptop', '2026-01-08'),
('DM009', N'Ổ Cứng SSD/HDD', N'Ổ cứng tốc độ cao dữ liệu lớn', '2026-01-09'),
('DM010', N'Card Đồ Họa (VGA)', N'Card màn hình NVIDIA và AMD', '2026-01-10'),
('DM011', N'Nguồn Máy Tính (PSU)', N'Nguồn công suất thực từ 450W trở lên', '2026-01-11'),
('DM012', N'Vỏ CaSe Máy Tính', N'Vỏ case có kính cường lực và LED', '2026-01-12'),
('DM013', N'Tản Nhiệt CPU', N'Tản nhiệt khí và tản nhiệt nước AIO', '2026-01-13'),
('DM014', N'Mainboard (Bo Mạch Chủ)', N'Bo mạch chủ Intel và AMD', '2026-01-14'),
('DM015', N'Bộ Vi Xử Lý (CPU)', N'CPU Intel Core và AMD Ryzen', '2026-01-15'),
('DM016', N'Bàn Ghế Gaming', N'Ghế công thái học và ghế gaming', '2026-01-16'),
('DM017', N'Cáp Chuyển Đổi', N'Cáp HDMI, DisplayPort, Type-C', '2026-01-17'),
('DM018', N'Hub Mở Rộng USB', N'Bộ chia cổng kết nối đa năng', '2026-01-18'),
('DM019', N'Lót Chuột (Pad)', N'Lót chuột cỡ lớn di mượt mà', '2026-01-19'),
('DM020', N'Loa Máy Tính', N'Hệ thống âm thanh để bàn sinh động', '2026-01-20'),
('DM021', N'Webcam Máy Tính', N'Webcam học trực tuyến và livestream', '2026-01-21'),
('DM022', N'Bút Trình Chiếu', N'Thiết bị hỗ trợ thuyết trình từ xa', '2026-01-22'),
('DM023', N'Giá Đỡ Laptop', N'Kệ tản nhiệt nâng chiều cao laptop', '2026-01-23'),
('DM024', N'Thiết Bị Mạng (Router)', N'Cục phát Wifi tốc độ cao', '2026-01-24'),
('DM025', N'USB Flash Drive', N'Ổ lưu trữ di động nhỏ gọn', '2026-01-25'),
('DM026', N'Thẻ Nhớ MicroSD', N'Thẻ nhớ cho điện thoại, camera', '2026-01-26'),
('DM027', N'Balo Laptop', N'Balo chống sốc, chống nước', '2026-01-27'),
('DM028', N'Bộ Vệ Sinh Máy Tính', N'Dụng cụ thổi bụi và lau màn hình', '2026-01-28'),
('DM029', N'Micro Thu Âm', N'Micro chuyên dụng cho podcast và stream', '2026-01-29'),
('DM030', N'Tay Cầm Chơi Game', N'Gamepad không dây cho PC/Console', '2026-01-30');
GO

-- ====================================================================================
-- 2. INSERT DATA FOR BẢNG [SanPham] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[SanPham] ([MaSanPham], [TenSanPham], [MaDanhMuc], [GiaBan], [GiaNhap], [SoLuongTon], [MoTa], [TrangThai], [NgayTao]) VALUES
('SP001', N'Laptop ASUS ROG Strix', 'DM001', 32000000, 27000000, 15, N'Ryzen 7, RTX 4060', N'Kinh Doanh', '2026-01-10'),
('SP002', N'Laptop Dell Inspiron 14', 'DM002', 15500000, 13000000, 25, N'Core i5, 16GB RAM', N'Kinh Doanh', '2026-01-11'),
('SP003', N'MacBook Pro 14 M3', 'DM003', 45000000, 40000000, 10, N'Apple M3 Pro, 18GB', N'Kinh Doanh', '2026-01-12'),
('SP004', N'Bàn Phím AKKO 3068B', 'DM004', 1800000, 1300000, 40, N'Bluetooth, Keycap PBT', N'Kinh Doanh', '2026-01-13'),
('SP005', N'Chuột Logitech G502 Hero', 'DM005', 1200000, 850000, 50, N'Mắt đọc 25K DPI, có dây', N'Kinh Doanh', '2026-01-14'),
('SP006', N'Tai nghe Razer BlackShark', 'DM006', 2300000, 1700000, 30, N'Âm thanh vòm 7.1', N'Kinh Doanh', '2026-01-15'),
('SP007', N'Màn hình ASUS ProArt 27', 'DM007', 8900000, 7500000, 12, N'2K, IPS, 100% sRGB', N'Kinh Doanh', '2026-01-16'),
('SP008', N'Ram Kingston Fury 16GB', 'DM008', 1100000, 850000, 80, N'Bus 3200MHz DDR4', N'Kinh Doanh', '2026-01-17'),
('SP009', N'SSD Samsung 990 Pro 1TB', 'DM009', 2800000, 2200000, 35, N'PCIe Gen 4x4 NVMe', N'Kinh Doanh', '2026-01-18'),
('SP010', N'VGA ASUS ROG RTX 4070', 'DM010', 19500000, 17000000, 8, N'12GB GDDR6X', N'Kinh Doanh', '2026-01-19'),
('SP011', N'Nguồn Corsair RM750e', 'DM011', 2600000, 2000000, 20, N'750W 80 Plus Gold', N'Kinh Doanh', '2026-01-20'),
('SP012', N'Vỏ Case MSI Gungnir', 'DM012', 1700000, 1200000, 15, N'Mid Tower, kèm 4 quạt ARGB', N'Kinh Doanh', '2026-01-21'),
('SP013', N'Tản nước AIO Deepcool LT720', 'DM013', 3100000, 2400000, 14, N'Rad 360mm hiệu năng cao', N'Kinh Doanh', '2026-01-22'),
('SP014', N'Mainboard ASUS TUF B760M', 'DM014', 3800000, 3100000, 18, N'Socket LGA1700 Intel', N'Kinh Doanh', '2026-01-23'),
('SP015', N'CPU Intel Core i7 14700K', 'DM015', 10500000, 9200000, 11, N'20 nhân 28 luồng', N'Kinh Doanh', '2026-01-24'),
('SP016', N'Ghế Công Thái Học Sihoo M57', 'DM016', 3600000, 2800000, 9, N'Đệm lưới thoáng khí', N'Kinh Doanh', '2026-01-25'),
('SP017', N'Cáp HDMI Baseus 4K 2m', 'DM017', 150000, 80000, 120, N'Dây bện dù siêu bền', N'Kinh Doanh', '2026-01-26'),
('SP018', N'Hub Type-C Ugreen 6 in 1', 'DM018', 450000, 300000, 65, N'Xuất HDMI và thẻ nhớ', N'Kinh Doanh', '2026-01-27'),
('SP019', N'Lót Chuột SteelSeries QcK L', 'DM019', 390000, 250000, 90, N'Vải dệt mịn màng', N'Kinh Doanh', '2026-01-28'),
('SP020', N'Loa Logitech Z213 2.1', 'DM020', 650000, 480000, 22, N'Có cục âm trầm riêng', N'Kinh Doanh', '2026-01-29'),
('SP021', N'Webcam Logitech C922 Pro', 'DM021', 2100000, 1600000, 16, N'Full HD 1080p 60fps', N'Kinh Doanh', '2026-01-30'),
('SP022', N'Bút Trình Chiếu Logitech R400', 'DM022', 400000, 280000, 45, N'Tia laser đỏ trực quan', N'Kinh Doanh', '2026-01-31'),
('SP023', N'Giá Đỡ Laptop Hợp Kim Nhôm', 'DM023', 250000, 150000, 70, N'Gấp gọn đa năng', N'Kinh Doanh', '2026-02-01'),
('SP024', N'Router Wifi 6 TP-Link AX10', 'DM024', 1150000, 850000, 28, N'Tốc độ 1500Mbps', N'Kinh Doanh', '2026-02-02'),
('SP025', N'USB Kingston DataTraveler 64GB', 'DM025', 160000, 100000, 150, N'Chuẩn USB 3.2 nhanh chóng', N'Kinh Doanh', '2026-02-03'),
('SP026', N'Thẻ nhớ Sandisk Ultra 128GB', 'DM026', 290000, 180000, 110, N'Tốc độ đọc 120MB/s', N'Kinh Doanh', '2026-02-04'),
('SP027', N'Balo Gaming Acer Predator', 'DM027', 990000, 700000, 24, N'Chống nước, đựng vừa lap 17 inch', N'Kinh Doanh', '2026-02-05'),
('SP028', N'Bộ vệ sinh 4 món gia đình', 'DM028', 50000, 20000, 300, N'Nước xịt, bóng thổi, chổi, khăn', N'Kinh Doanh', '2026-02-06'),
('SP029', N'Micro Elgato Wave 3', 'DM029', 3800000, 3100000, 7, N'Condenser cao cấp cho streamer', N'Kinh Doanh', '2026-02-07'),
('SP030', N'Tay Cầm Xbox Wireless Controller', 'DM030', 1450000, 1150000, 33, N'Màu đen Robot Black chính hãng', N'Kinh Doanh', '2026-02-08');
GO

-- ====================================================================================
-- 3. INSERT DATA FOR BẢNG [KhachHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[KhachHang] ([MaKhachHang], [TenKhachHang], [SoDienThoai], [DiemTichLuy], [NgayTao]) VALUES
('KH001', N'Nguyễn Văn An', '0901234567', 120, '2026-02-01'),
('KH002', N'Trần Thị Bình', '0912345678', 450, '2026-02-02'),
('KH003', N'Lê Minh Cường', '0923456789', 50, '2026-02-03'),
('KH004', N'Phạm Hồng Dương', '0934567890', 890, '2026-02-04'),
('KH005', N'Hoàng Thu Giang', '0945678901', 0, '2026-02-05'),
('KH006', N'Đỗ Quốc Hùng', '0956789012', 340, '2026-02-06'),
('KH007', N'Vũ Thúy Hương', '0967890123', 1500, '2026-02-07'),
('KH008', N'Bùi Quang Khải', '0978901234', 60, '2026-02-08'),
('KH009', N'Ngô Mai Linh', '0989012345', 210, '2026-02-09'),
('KH010', N'Đặng Tiến Minh', '0990123456', 750, '2026-02-10'),
('KH011', N'Lý Bảo Ngọc', '0812345678', 5, '2026-02-11'),
('KH012', N'Phan Thanh Phong', '0823456789', 130, '2026-02-12'),
('KH013', N'Đinh Văn Quân', '0834567890', 420, '2026-02-13'),
('KH014', N'Tạ Minh Sơn', '0845678901', 95, '2026-02-14'),
('KH015', N'Trịnh Tuyết Trinh', '0856789012', 0, '2026-02-15'),
('KH016', N'Vương Trọng Uy', '0867890123', 680, '2026-02-16'),
('KH017', N'Tô Kim Vỹ', '0878901234', 1100, '2026-02-17'),
('KH018', N'Đoàn Đức Xuân', '0888901235', 25, '2026-02-18'),
('KH019', N'Lâm Minh Yến', '0899012345', 300, '2026-02-19'),
('KH020', N'Cao Thành Long', '0701234567', 160, '2026-02-20'),
('KH021', N'Hồ Ngọc Hà', '0762345678', 550, '2026-02-21'),
('KH022', N'Mai Đức Chung', '0773456789', 80, '2026-02-22'),
('KH023', N'Trương Mỹ Linh', '0784567890', 920, '2026-02-23'),
('KH024', N'Diệp Tấn Lộc', '0795678901', 110, '2026-02-24'),
('KH025', N'Lương Gia Huy', '0326789012', 40, '2026-02-25'),
('KH026', N'Thái Văn Đạt', '0337890123', 1350, '2026-02-26'),
('KH027', N'Quách Thu Thảo', '0348901234', 0, '2026-02-27'),
('KH028', N'Nghiêm Xuân Trường', '0359012345', 270, '2026-02-28'),
('KH029', N'Hứa Minh Đạt', '0361234567', 180, '2026-03-01'),
('KH030', N'Đường Yên', '0372345678', 610, '2026-03-02');
GO

-- ====================================================================================
-- 4. INSERT DATA FOR BẢNG [KhuyenMai] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[KhuyenMai] ([MaKhuyenMai], [PhanTramGiamGia], [NgayBatDau], [NgayHetHan], [MoTaKhuyenMai], [TenKhuyenMai], [TrangThai]) VALUES
('KM001', 5, '2026-01-01', '2026-02-01', N'Giảm 5% Chào Năm Mới', N'HAPPYNEWYEAR', N'Hết Hạn'),
('KM002', 10, '2026-02-10', '2026-02-16', N'Giảm Valentine ấm áp', N'VALENTINE', N'Hết Hạn'),
('KM003', 15, '2026-03-05', '2026-03-10', N'Giảm ngày quốc tế phụ nữ', N'WOMANDAY', N'Hết Hạn'),
('KM004', 20, '2026-04-28', '2026-05-05', N'Đại lễ giải phóng 30/4', N'DAILE304', N'Hoạt Động'),
('KM005', 8, '2026-05-15', '2026-06-15', N'Giảm giá chào hè sôi động', N'HEALOHA', N'Hoạt Động'),
('KM006', 12, '2026-06-01', '2026-06-03', N'Quốc tế thiếu nhi mua sắm', N'KIDSGAME', N'Hoạt Động'),
('KM007', 5, '2026-01-01', '2026-12-31', N'Ưu đãi cho đơn hàng đầu tiên', N'WELCOME', N'Hoạt Động'),
('KM008', 3, '2026-01-01', '2026-12-31', N'Giảm thành viên đồng', N'COPPERMEMBER', N'Hoạt Động'),
('KM009', 7, '2026-01-01', '2026-12-31', N'Giảm thành viên bạc', N'SILVERMEMBER', N'Hoạt Động'),
('KM010', 10, '2026-01-01', '2026-12-31', N'Giảm thành viên vàng', N'GOLDMEMBER', N'Hoạt Động'),
('KM011', 2, '2026-02-01', '2026-02-28', N'Khuyến mãi tháng 2', N'SALEKHOITHANG2', N'Hết Hạn'),
('KM012', 4, '2026-03-01', '2026-03-31', N'Khuyến mãi tháng 3', N'SALEKHOITHANG3', N'Hết Hạn'),
('KM013', 6, '2026-04-01', '2026-04-30', N'Khuyến mãi tháng 4', N'SALEKHOITHANG4', N'Hết Hạn'),
('KM014', 8, '2026-05-01', '2026-05-31', N'Khuyến mãi tháng 5', N'SALEKHOITHANG5', N'Hoạt Động'),
('KM015', 10, '2026-06-01', '2026-06-30', N'Khuyến mãi tháng 6', N'SALEKHOITHANG6', N'Hoạt Động'),
('KM016', 12, '2026-07-01', '2026-07-31', N'Khuyến mãi tháng 7', N'SALEKHOITHANG7', N'Hoạt Động'),
('KM017', 14, '2026-08-01', '2026-08-31', N'Khuyến mãi tháng 8', N'SALEKHOITHANG8', N'Hoạt Động'),
('KM018', 15, '2026-09-01', '2026-09-30', N'Khuyến mãi tháng 9', N'SALEKHOITHANG9', N'Hoạt Động'),
('KM019', 18, '2026-10-01', '2026-10-31', N'Khuyến mãi tháng 10', N'SALEKHOITHANG10', N'Hoạt Động'),
('KM020', 25, '2026-11-20', '2026-11-30', N'Xả kho tưng bừng BlackFriday', N'BLACKFRIDAY', N'Hoạt Động'),
('KM021', 30, '2026-12-10', '2026-12-25', N'Đón Giáng sinh rinh quà khủng', N'CHRISTMAS', N'Hoạt Động'),
('KM022', 5, '2026-05-01', '2026-05-10', N'Ưu đãi mua kèm bàn phím chuột', N'COMBOACC', N'Hoạt Động'),
('KM023', 10, '2026-05-01', '2026-05-15', N'Ưu đãi build PC giảm sâu', N'BUILDPC', N'Hoạt Động'),
('KM024', 6, '2026-05-01', '2026-05-31', N'Mã giảm độc quyền App di động', N'APPSALE', N'Hoạt Động'),
('KM025', 5, '2026-05-01', '2026-05-31', N'Mã dành riêng cho đêm muộn', 'MIDNIGHT', N'Hoạt Động'),
('KM026', 4, '2026-05-01', '2026-05-31', N'Giảm giá ngày giữa tuần', N'MIDWEEK', N'Hoạt Động'),
('KM027', 7, '2026-05-01', '2026-05-31', N'Giảm giá cuối tuần xả stress', N'WEEKEND', N'Hoạt Động'),
('KM028', 11, '2026-11-11', '2026-11-12', N'Siêu sale độc thân 11/11', N'SINGLEDAY', N'Hoạt Động'),
('KM029', 12, '2026-12-12', '2026-12-13', N'Siêu sale cuối cùng năm 12/12', N'LASTSALE', N'Hoạt Động'),
('KM030', 5, '2026-01-01', '2026-12-31', N'Khuyến mãi mừng thọ sinh nhật', N'BIRTHDAY', N'Hoạt Động');
GO

-- ====================================================================================
-- 5. INSERT DATA FOR BẢNG [NguoiDung] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[NguoiDung] ([MaNguoiDung], [TenNguoiDung], [SoDienThoai], [Email], [MatKhau], [VaiTro], [TrangThai], [NgayTao]) VALUES
('ND001', N'Phan Văn Admin', '0901112223', 'admin@lapnpt.com', 'admin_hash_123', 'Admin', N'Hoạt Động', '2026-01-01'),
('ND002', N'Lê Thị Quản Lý', '0902223334', 'manager@lapnpt.com', 'manager_hash_123', 'Manager', N'Hoạt Động', '2026-01-02'),
('ND003', N'Nguyễn Thu Ngân 1', '0903334445', 'cashier1@lapnpt.com', 'cashier_hash_1', 'Staff', N'Hoạt Động', '2026-01-05'),
('ND004', N'Trần Minh Kho 1', '0904445556', 'store1@lapnpt.com', 'store_hash_1', 'Staff', N'Hoạt Động', '2026-01-05'),
('ND005', N'Hoàng Văn Bán Hàng', '0905556667', 'sale1@lapnpt.com', 'sale_hash_1', 'Staff', N'Hoạt Động', '2026-01-06'),
('ND006', N'Vũ Thị Ngân 2', '0906667778', 'cashier2@lapnpt.com', 'cashier_hash_2', 'Staff', N'Hoạt Động', '2026-01-10'),
('ND007', N'Đặng Văn Kho 2', '0907778889', 'store2@lapnpt.com', 'store_hash_2', 'Staff', N'Hoạt Động', '2026-01-12'),
('ND008', N'Bùi Thị Kế Toán', '0908889990', 'accountant@lapnpt.com', 'acc_hash_123', 'Staff', N'Hoạt Động', '2026-01-15'),
('ND009', N'Nguyễn Văn Giao Hàng', '0909990001', 'shipper1@lapnpt.com', 'ship_hash_1', 'Staff', N'Hoạt Động', '2026-01-20'),
('ND010', N'Trần Văn Kỹ Thuật', '0901239876', 'tech1@lapnpt.com', 'tech_hash_1', 'Staff', N'Hoạt Động', '2026-01-22'),
('ND011', N'Lê Minh Tú', '0911001122', 'tule@lapnpt.com', 'pass_hash_11', 'Staff', N'Nghỉ Việc', '2026-01-25'),
('ND012', N'Phạm Thành Nam', '0912002233', 'nampham@lapnpt.com', 'pass_hash_12', 'Staff', N'Hoạt Động', '2026-02-01'),
('ND013', N'Đỗ Diệu Thúy', '0913003344', 'thuydo@lapnpt.com', 'pass_hash_13', 'Staff', N'Hoạt Động', '2026-02-02'),
('ND014', N'Nguyễn Hoàng Long', '0914004455', 'longnguyen@lapnpt.com', 'pass_hash_14', 'Staff', N'Hoạt Động', '2026-02-03'),
('ND015', N'Ngô Quốc Bảo', '0915005566', 'baongo@lapnpt.com', 'pass_hash_15', 'Staff', N'Hoạt Động', '2026-02-04'),
('ND016', N'Vũ Hoàng Yến', '0916006677', 'yenvu@lapnpt.com', 'pass_hash_16', 'Staff', N'Hoạt Động', '2026-02-05'),
('ND017', N'Dương Chí Dũng', '0917007788', 'dungduong@lapnpt.com', 'pass_hash_17', 'Staff', N'Hoạt Động', '2026-02-06'),
('ND018', N'Lý Thiên Kim', '0918008899', 'kimly@lapnpt.com', 'pass_hash_18', 'Staff', N'Hoạt Động', '2026-02-07'),
('ND019', N'Phan Thanh Bình', '0919009900', 'binhphan@lapnpt.com', 'pass_hash_19', 'Staff', N'Hoạt Động', '2026-02-08'),
('ND020', N'Trịnh Công Sơn', '0920001122', 'sontrinh@lapnpt.com', 'pass_hash_20', 'Staff', N'Hoạt Động', '2026-02-09'),
('ND021', N'Cao Văn Thắng', '0921002233', 'thangcao@lapnpt.com', 'pass_hash_21', 'Staff', N'Hoạt Động', '2026-02-10'),
('ND022', N'Hà Thị Thảo', '0922003344', 'thaoha@lapnpt.com', 'pass_hash_22', 'Staff', N'Hoạt Động', '2026-02-11'),
('ND023', N'Đoàn Văn Hậu', '0923004455', 'haudoan@lapnpt.com', 'pass_hash_23', 'Staff', N'Hoạt Động', '2026-02-12'),
('ND024', N'Trần Đình Trọng', '0924005566', 'trongtran@lapnpt.com', 'pass_hash_24', 'Staff', N'Hoạt Động', '2026-02-13'),
('ND025', N'Nguyễn Quang Hải', '0925006677', 'hainguyen@lapnpt.com', 'pass_hash_25', 'Staff', N'Hoạt Động', '2026-02-14'),
('ND026', N'Phan Văn Đức', '0926007788', 'ducphan@lapnpt.com', 'pass_hash_26', 'Staff', N'Hoạt Động', '2026-02-15'),
('ND027', N'Bùi Tiến Dũng', '0927008899', 'dungbui@lapnpt.com', 'pass_hash_27', 'Staff', N'Hoạt Động', '2026-02-16'),
('ND028', N'Nguyễn Công Phượng', '0928009900', 'phuongnguyen@lapnpt.com', 'pass_hash_28', 'Staff', N'Hoạt Động', '2026-02-17'),
('ND029', N'Lương Xuân Trường', '0929000011', 'truongluong@lapnpt.com', 'pass_hash_29', 'Staff', N'Hoạt Động', '2026-02-18'),
('ND030', N'Đặng Văn Lâm', '0930001122', 'lamdang@lapnpt.com', 'pass_hash_30', 'Staff', N'Hoạt Động', '2026-02-19');
GO

-- ====================================================================================
-- 6. INSERT DATA FOR BẢNG [HoaDon] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[HoaDon] ([MaHoaDon], [MaKhachHang], [MaNguoiDung], [MaKhuyenMai], [TongTien], [GiamGia], [HinhThucThanhToan], [NgayTao]) VALUES
('HD001', 'KH001', 'ND003', 'KM007', 32000000, 1600000, 'Chuyen Khoan', '2026-03-01'),
('HD002', 'KH002', 'ND003', 'KM010', 15500000, 1550000, 'Tien Mat', '2026-03-02'),
('HD003', 'KH003', 'ND006', 'KM007', 45000000, 2250000, 'Chuyen Khoan', '2026-03-03'),
('HD004', 'KH004', 'ND006', 'KM007', 1800000, 90000, 'Tien Mat', '2026-03-04'),
('HD005', 'KH005', 'ND003', 'KM007', 1200000, 60000, 'The Tin Dung', '2026-03-05'),
('HD006', 'KH006', 'ND003', 'KM008', 2300000, 69000, 'Chuyen Khoan', '2026-03-06'),
('HD007', 'KH007', 'ND006', 'KM010', 8900000, 890000, 'Tien Mat', '2026-03-07'),
('HD008', 'KH008', 'ND006', 'KM007', 1100000, 55000, 'Chuyen Khoan', '2026-03-08'),
('HD009', 'KH009', 'ND003', 'KM007', 2800000, 140000, 'Tien Mat', '2026-03-09'),
('HD010', 'KH010', 'ND003', 'KM010', 19500000, 1950000, 'The Tin Dung', '2026-03-10'),
('HD011', 'KH011', 'ND006', 'KM007', 2600000, 130000, 'Chuyen Khoan', '2026-03-11'),
('HD012', 'KH012', 'ND006', 'KM007', 1700000, 85000, 'Tien Mat', '2026-03-12'),
('HD013', 'KH013', 'ND003', 'KM007', 3100000, 155000, 'Chuyen Khoan', '2026-03-13'),
('HD014', 'KH014', 'ND003', 'KM007', 3800000, 190000, 'Tien Mat', '2026-03-14'),
('HD015', 'KH015', 'ND006', 'KM007', 10500000, 525000, 'The Tin Dung', '2026-03-15'),
('HD016', 'KH016', 'ND006', 'KM007', 3600000, 180000, 'Chuyen Khoan', '2026-03-16'),
('HD017', 'KH017', 'ND003', 'KM010', 150000, 15000, 'Tien Mat', '2026-03-17'),
('HD018', 'KH018', 'ND003', 'KM007', 450000, 22500, 'Chuyen Khoan', '2026-03-18'),
('HD019', 'KH019', 'ND006', 'KM007', 390000, 19500, 'Tien Mat', '2026-03-19'),
('HD020', 'KH020', 'ND006', 'KM007', 650000, 32500, 'The Tin Dung', '2026-03-20'),
('HD021', 'KH021', 'ND003', 'KM010', 2100000, 210000, 'Chuyen Khoan', '2026-03-21'),
('HD022', 'KH022', 'ND003', 'KM007', 400000, 20000, 'Tien Mat', '2026-03-22'),
('HD023', 'KH023', 'ND006', 'KM010', 250000, 25000, 'Chuyen Khoan', '2026-03-23'),
('HD024', 'KH024', 'ND006', 'KM007', 1150000, 57500, 'Tien Mat', '2026-03-24'),
('HD025', 'KH025', 'ND003', 'KM007', 160000, 8000, 'The Tin Dung', '2026-03-25'),
('HD026', 'KH026', 'ND003', 'KM010', 290000, 29000, 'Chuyen Khoan', '2026-03-26'),
('HD027', 'KH027', 'ND006', 'KM007', 990000, 49500, 'Tien Mat', '2026-03-27'),
('HD028', 'KH028', 'ND006', 'KM007', 50000, 2500, 'Chuyen Khoan', '2026-03-28'),
('HD029', 'KH029', 'ND003', 'KM007', 3800000, 190000, 'Tien Mat', '2026-03-29'),
('HD030', 'KH030', 'ND003', 'KM010', 1450000, 145000, 'The Tin Dung', '2026-03-30');
GO

-- ====================================================================================
-- 7. INSERT DATA FOR BẢNG [GiaoHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[GiaoHang] ([MaGiaoHang], [MaHoaDon], [DiaChiGiao], [TrangThaiGiao], [NgayGiao]) VALUES
('GH001', 'HD001', N'123 Nguyễn Trãi, Thanh Xuân, Hà Nội', 'Thanh Cong', '2026-03-02'),
('GH002', 'HD002', N'456 Lê Lợi, Quận 1, TP HCM', 'Thanh Cong', '2026-03-03'),
('GH003', 'HD003', N'789 Hùng Vương, Hải Châu, Đà Nẵng', 'Thanh Cong', '2026-03-04'),
('GH004', 'HD004', N'12 Trần Hưng Đạo, Quy Nhơn', 'Thanh Cong', '2026-03-05'),
('GH005', 'HD005', N'65 Lý Tự Trọng, Cần Thơ', 'Thanh Cong', '2026-03-06'),
('GH006', 'HD006', N'99 Điện Biên Phủ, Hải Phòng', 'Thanh Cong', '2026-03-07'),
('GH007', 'HD007', N'102 Quang Trung, Vinh, Nghệ An', 'Thanh Cong', '2026-03-08'),
('GH008', 'HD008', N'40 Võ Văn Kiệt, Nha Trang', 'Thanh Cong', '2026-03-09'),
('GH009', 'HD009', N'88 Nguyễn Chí Thanh, Hà Nội', 'Thanh Cong', '2026-03-10'),
('GH010', 'HD010', N'15 Cách Mạng Tháng 8, Biên Hòa', 'Thanh Cong', '2026-03-11'),
('GH011', 'HD011', N'222 Lê Hồng Phong, Vũng Tàu', 'Thanh Cong', '2026-03-12'),
('GH012', 'HD012', N'74 Kinh Dương Vương, Huế', 'Thanh Cong', '2026-03-13'),
('GH013', 'HD013', N'53 Hoàng Diệu, Buôn Ma Thuột', 'Thanh Cong', '2026-03-14'),
('GH014', 'HD014', N'91 Trần Phú, Đà Lạt', 'Thanh Cong', '2026-03-15'),
('GH015', 'HD015', N'16 Nguyễn Văn Linh, Long Xuyên', 'Thanh Cong', '2026-03-16'),
('GH016', 'HD016', N'304 Đại Lộ Bình Dương, Thủ Dầu Một', 'Thanh Cong', '2026-03-17'),
('GH017', 'HD017', N'85 Lê Duẩn, Phan Thiết', 'Thanh Cong', '2026-03-18'),
('GH018', 'HD018', N'19 Hùng Vương, Pleiku', 'Thanh Cong', '2026-03-19'),
('GH019', 'HD019', N'62 Trần Phú, Thanh Hóa', 'Thanh Cong', '2026-03-20'),
('GH020', 'HD020', N'143 Lê Lợi, Bắc Giang', 'Thanh Cong', '2026-03-21'),
('GH021', 'HD021', N'25 Lương Ngọc Quyến, Thái Nguyên', 'Thanh Cong', '2026-03-22'),
('GH022', 'HD022', N'77 Nguyễn Huệ, Cao Lãnh', 'Thanh Cong', '2026-03-23'),
('GH023', 'HD023', N'39 Tô Hiệu, Sơn La', 'Thanh Cong', '2026-03-24'),
('GH024', 'HD024', N'18 Trần Hưng Đạo, Đồng Hới', 'Thanh Cong', '2026-03-25'),
('GH025', 'HD025', N'92 Kim Đồng, Cao Bằng', 'Thanh Cong', '2026-03-26'),
('GH026', 'HD026', N'48 Nguyễn Tất Thành, Tuy Hòa', 'Thanh Cong', '2026-03-27'),
('GH027', 'HD027', N'66 Lý Thường Kiệt, Mỹ Tho', 'Thanh Cong', '2026-03-28'),
('GH028', 'HD028', N'103 Bạch Đằng, Sóc Trăng', 'Thanh Cong', '2026-03-29'),
('GH029', 'HD029', N'57 Hoàng Văn Thụ, Nam Định', 'Thanh Cong', '2026-03-30'),
('GH030', 'HD030', N'11 Trưng Trắc, Vĩnh Yên', 'Thanh Cong', '2026-03-31');
GO

-- ====================================================================================
-- 8. INSERT DATA FOR BẢNG [ChiTietHoaDon] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietHoaDon] ([MaChiTiet], [MaHoaDon], [MaSanPham], [SoLuong], [DonGia], [ThanhTien]) VALUES
('CTHD001', 'HD001', 'SP001', 1, 32000000, 32000000),
('CTHD002', 'HD002', 'SP002', 1, 15500000, 15500000),
('CTHD003', 'HD003', 'SP003', 1, 45000000, 45000000),
('CTHD004', 'HD004', 'SP004', 1, 1800000, 1800000),
('CTHD005', 'HD005', 'SP005', 1, 1200000, 1200000),
('CTHD006', 'HD006', 'SP006', 1, 2300000, 2300000),
('CTHD007', 'HD007', 'SP007', 1, 8900000, 8900000),
('CTHD008', 'HD008', 'SP008', 1, 1100000, 1100000),
('CTHD009', 'HD009', 'SP009', 1, 2800000, 2800000),
('CTHD010', 'HD010', 'SP010', 1, 19500000, 19500000),
('CTHD011', 'HD011', 'SP011', 1, 2600000, 2600000),
('CTHD012', 'HD012', 'SP012', 1, 1700000, 1700000),
('CTHD013', 'HD013', 'SP013', 1, 3100000, 3100000),
('CTHD014', 'HD014', 'SP014', 1, 3800000, 3800000),
('CTHD015', 'HD015', 'SP015', 1, 10500000, 10500000),
('CTHD016', 'HD016', 'SP016', 1, 3600000, 3600000),
('CTHD017', 'HD017', 'SP017', 1, 150000, 150000),
('CTHD018', 'HD018', 'SP018', 1, 450000, 450000),
('CTHD019', 'HD019', 'SP019', 1, 390000, 390000),
('CTHD020', 'HD020', 'SP020', 1, 650000, 650000),
('CTHD021', 'HD021', 'SP021', 1, 2100000, 2100000),
('CTHD022', 'HD022', 'SP022', 1, 400000, 400000),
('CTHD023', 'HD023', 'SP023', 1, 250000, 250000),
('GHHD024', 'HD024', 'SP024', 1, 1150000, 1150000),
('CTHD025', 'HD025', 'SP025', 1, 160000, 160000),
('CTHD026', 'HD026', 'SP026', 1, 290000, 290000),
('CTHD027', 'HD027', 'SP027', 1, 990000, 990000),
('CTHD028', 'HD028', 'SP028', 1, 50000, 50000),
('CTHD029', 'HD029', 'SP029', 1, 3800000, 3800000),
('CTHD030', 'HD030', 'SP030', 1, 1450000, 1450000);
GO

-- ====================================================================================
-- 9. INSERT DATA FOR BẢNG [PhieuNhap] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[PhieuNhap] ([MaPhieuNhap], [MaNguoiDung], [NgayNhap], [TongTien]) VALUES
('PN001', 'ND004', '2026-02-10', 54000000),
('PN002', 'ND004', '2026-02-11', 26000000),
('PN003', 'ND007', '2026-02-12', 80000000),
('PN004', 'ND007', '2026-02-13', 1300000),
('PN005', 'ND004', '2026-02-14', 850000),
('PN006', 'ND004', '2026-02-15', 1700000),
('PN007', 'ND007', '2026-02-16', 15000000),
('PN008', 'ND007', '2026-02-17', 850000),
('PN009', 'ND004', '2026-02-18', 2200000),
('PN010', 'ND004', '2026-02-19', 34000000),
('PN011', 'ND007', '2026-02-20', 4000000),
('PN012', 'ND007', '2026-02-21', 2400000),
('PN013', 'ND004', '2026-02-22', 4800000),
('PN014', 'ND004', '2026-02-23', 6200000),
('PN015', 'ND007', '2026-02-24', 18400000),
('PN016', 'ND007', '2026-02-25', 5600000),
('PN017', 'ND004', '2026-02-26', 160000),
('PN018', 'ND004', '2026-02-27', 600000),
('PN019', 'ND007', '2026-02-28', 500000),
('PN020', 'ND007', '2026-03-01', 960000),
('PN021', 'ND004', '2026-03-02', 3200000),
('PN022', 'ND004', '2026-03-03', 560000),
('PN023', 'ND007', '2026-03-04', 300000),
('PN024', 'ND007', '2026-03-05', 1700000),
('PN025', 'ND004', '2026-03-06', 200000),
('PN026', 'ND004', '2026-03-07', 360000),
('PN027', 'ND007', '2026-03-08', 1400000),
('PN028', 'ND007', '2026-03-09', 40000),
('PN029', 'ND004', '2026-03-10', 6200000),
('PN030', 'ND004', '2026-03-11', 2300000);
GO

-- ====================================================================================
-- 10. INSERT DATA FOR BẢNG [ChiTietPhieuNhap] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietPhieuNhap] ([MaChiTietNhap], [MaPhieuNhap], [MaSanPham], [SoLuong], [GiaNhap]) VALUES
('CTPN001', 'PN001', 'SP001', 2, 27000000),
('CTPN002', 'PN002', 'SP002', 2, 13000000),
('CTPN003', 'PN003', 'SP003', 2, 40000000),
('CTPN004', 'PN004', 'SP004', 1, 1300000),
('CTPN005', 'PN005', 'SP005', 1, 850000),
('CTPN006', 'PN006', 'SP006', 1, 1700000),
('CTPN007', 'PN007', 'SP007', 2, 7500000),
('CTPN008', 'PN008', 'SP008', 1, 850000),
('CTPN009', 'PN009', 'SP009', 1, 2200000),
('CTPN010', 'PN010', 'SP010', 2, 17000000),
('CTPN011', 'PN011', 'SP011', 2, 2000000),
('CTPN012', 'PN012', 'SP012', 2, 1200000),
('CTPN013', 'PN013', 'SP013', 2, 2400000),
('CTPN014', 'PN014', 'SP014', 2, 3100000),
('CTPN015', 'PN015', 'SP015', 2, 9200000),
('CTPN016', 'PN016', 'SP016', 2, 2800000),
('CTPN017', 'PN017', 'SP017', 2, 80000),
('CTPN018', 'PN018', 'SP018', 2, 300000),
('CTPN019', 'PN019', 'SP019', 2, 250000),
('CTPN020', 'PN020', 'SP020', 2, 480000),
('CTPN021', 'PN021', 'SP021', 2, 1600000),
('CTPN022', 'PN022', 'SP022', 2, 280000),
('CTPN023', 'PN023', 'SP023', 2, 150000),
('CTPN024', 'PN024', 'SP024', 2, 850000),
('CTPN025', 'PN025', 'SP025', 2, 100000),
('CTPN026', 'PN026', 'SP026', 2, 180000),
('CTPN027', 'PN027', 'SP027', 2, 700000),
('CTPN028', 'PN028', 'SP028', 2, 20000),
('CTPN029', 'PN029', 'SP029', 2, 3100000),
('CTPN030', 'PN030', 'SP030', 2, 1150000);
GO

-- ====================================================================================
-- 11. INSERT DATA FOR BẢNG [DoiHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[DoiHang] ([MaDoiHang], [MaHoaDon], [NgayDoi], [MaNguoiDung], [LyDo]) VALUES
('DH001', 'HD004', '2026-03-05', 'ND010', N'Phím chập chờn nút Space'),
('DH002', 'HD005', '2026-03-06', 'ND010', N'Chuột click đúp nhẹ'),
('DH003', 'HD006', '2026-03-07', 'ND010', N'Tai nghe rè bên trái'),
('DH004', 'HD008', '2026-03-09', 'ND010', N'Ram không nhận bus đủ'),
('DH005', 'HD009', '2026-03-10', 'ND010', N'SSD không nhận phân vùng'),
('DH006', 'HD012', '2026-03-14', 'ND010', N'Vỏ case bị móp nhẹ góc'),
('DH007', 'HD013', '2026-03-15', 'ND010', N'Tản nhiệt kêu to lạ thường'),
('DH008', 'HD017', '2026-03-18', 'ND010', N'Cáp xuất hình bị nhiễu'),
('DH009', 'HD018', '2026-03-19', 'ND010', N'Hub lỏng cổng kết nối'),
('DH010', 'HD019', '2026-03-20', 'ND010', N'Lót chuột bị xước viền'),
('DH011', 'HD020', '2026-03-21', 'ND010', N'Loa mất tiếng một bên'),
('DH012', 'HD022', '2026-03-23', 'ND010', N'Bút laser không sáng'),
('DH013', 'HD023', '2026-03-24', 'ND010', N'Giá đỡ lỏng ốc vít'),
('DH014', 'HD024', '2026-03-25', 'ND010', N'Router rớt mạng liên tục'),
('DH015', 'HD025', '2026-03-26', 'ND010', N'USB không nhận máy tính'),
('DH016', 'HD026', '2026-03-27', 'ND010', N'Thẻ nhớ lỗi định dạng'),
('DH017', 'HD027', '2026-03-28', 'ND010', N'Balo tuột đường chỉ may'),
('DH018', 'HD028', '2026-03-29', 'ND010', N'Nước lau bị rò rỉ'),
('DH019', 'HD004', '2026-04-01', 'ND010', N'Khách muốn đổi sang loại cao cấp hơn'),
('DH020', 'HD005', '2026-04-02', 'ND010', N'Đổi trả bảo hành định kỳ'),
('DH021', 'HD006', '2026-04-03', 'ND010', N'Lỗi âm thanh vòm không hoạt động'),
('DH022', 'HD008', '2026-04-04', 'ND010', N'Đổi lấy thanh RAM cùng loại'),
('DH023', 'HD009', '2026-04-05', 'ND010', N'Tốc độ đọc ghi giảm mạnh'),
('DH024', 'HD012', '2026-04-06', 'ND010', N'Kính cường lực bị trầy xước'),
('DH025', 'HD013', '2026-04-07', 'ND010', N'Lỗi bơm nước không chạy'),
('DH026', 'HD017', '2026-04-08', 'ND010', N'Đầu HDMI bị lỏng gãy'),
('DH027', 'HD018', '2026-04-09', 'ND010', N'Cổng sạc không truyền điện'),
('DH028', 'HD019', '2026-04-10', 'ND010', N'Đổi sang màu khác theo ý thích'),
('DH029', 'HD020', '2026-04-11', 'ND010', N'Lỗi rè cục bass trầm'),
('DH030', 'HD022', '2026-04-12', 'ND010', N'Nút bấm nhảy trang bị kẹt');
GO

-- ====================================================================================
-- 12. INSERT DATA FOR BẢNG [ChiTietDoiHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietDoiHang] ([MaChiTietDoi], [MaDoiHang], [MaSanPhamCu], [MaSanPhamMoi], [SoLuong], [ChenhLechGia]) VALUES
('CTDH001', 'DH001', 'SP004', 'SP004', 1, 0),
('CTDH002', 'DH002', 'SP005', 'SP005', 1, 0),
('CTDH003', 'DH003', 'SP006', 'SP006', 1, 0),
('CTDH004', 'DH004', 'SP008', 'SP008', 1, 0),
('CTDH005', 'DH005', 'SP009', 'SP009', 1, 0),
('CTDH006', 'DH006', 'SP012', 'SP012', 1, 0),
('CTDH007', 'DH007', 'SP013', 'SP013', 1, 0),
('CTDH008', 'DH008', 'SP017', 'SP017', 1, 0),
('CTDH009', 'DH009', 'SP018', 'SP018', 1, 0),
('CTDH010', 'DH010', 'SP019', 'SP019', 1, 0),
('CTDH011', 'DH011', 'SP020', 'SP020', 1, 0),
('CTDH012', 'DH012', 'SP022', 'SP022', 1, 0),
('CTDH013', 'DH013', 'SP023', 'SP023', 1, 0),
('CTDH014', 'DH014', 'SP024', 'SP024', 1, 0),
('CTDH015', 'DH015', 'SP025', 'SP025', 1, 0),
('CTDH016', 'DH016', 'SP026', 'SP026', 1, 0),
('CTDH017', 'DH017', 'SP027', 'SP027', 1, 0),
('CTDH018', 'DH018', 'SP028', 'SP028', 1, 0),
('CTDH019', 'DH019', 'SP004', 'SP004', 1, 0),
('CTDH020', 'DH020', 'SP005', 'SP005', 1, 0),
('CTDH021', 'DH021', 'SP006', 'SP006', 1, 0),
('CTDH022', 'DH022', 'SP008', 'SP008', 1, 0),
('CTDH023', 'DH023', 'SP009', 'SP009', 1, 0),
('CTDH024', 'DH024', 'SP012', 'SP012', 1, 0),
('CTDH025', 'DH025', 'SP013', 'SP013', 1, 0),
('CTDH026', 'DH026', 'SP017', 'SP017', 1, 0),
('CTDH027', 'DH027', 'SP018', 'SP018', 1, 0),
('CTDH028', 'DH028', 'SP019', 'SP019', 1, 0),
('CTDH029', 'DH029', 'SP020', 'SP020', 1, 0),
('CTDH030', 'DH030', 'SP022', 'SP022', 1, 0);
GO

-- ====================================================================================
-- 13. INSERT DATA FOR BẢNG [TraHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[TraHang] ([MaTraHang], [MaHoaDon], [NgayTra], [LyDo], [TongTienHoan], [MaNguoiDung]) VALUES
('TH001', 'HD001', '2026-03-05', N'Khách không đủ tiền lấy, trả cọc hoàn tiền', 32000000, 'ND002'),
('TH002', 'HD002', '2026-03-06', N'Giao nhầm mã máy, khách trả luôn', 15500000, 'ND002'),
('TH003', 'HD003', '2026-03-07', N'Macbook lỗi màn hình sọc, hết hàng đổi', 45000000, 'ND002'),
('TH004', 'HD007', '2026-03-12', N'Màn hình sai lệch màu nặng so với mô tả', 8900000, 'ND002'),
('TH005', 'HD010', '2026-03-15', N'Card đồ họa không vừa case máy tính', 19500000, 'ND002'),
('TH006', 'HD011', '2026-03-16', N'Nguồn sụt áp sập nguồn liên tục', 2600000, 'ND002'),
('TH007', 'HD014', '2026-03-20', N'Mainboard lỗi chân socket', 3800000, 'ND002'),
('TH008', 'HD015', '2026-03-21', N'CPU nóng quá nhiệt bốc khói', 10500000, 'ND002'),
('TH009', 'HD016', '2026-03-22', N'Ghế gãy bánh xe khi mới ngồi', 3600000, 'ND002'),
('TH010', 'HD021', '2026-03-27', N'Webcam mờ không đúng độ phân giải', 2100000, 'ND002'),
('TH011', 'HD029', '2026-04-02', N'Micro thu nhiều tạp âm không lọc được', 3800000, 'ND002'),
('TH012', 'HD030', '2026-04-03', N'Tay cầm nút bấm bị liệt hoàn toàn', 1450000, 'ND002'),
('TH013', 'HD001', '2026-04-05', N'Khách đổi ý không muốn mua nữa', 32000000, 'ND002'),
('TH014', 'HD002', '2026-04-06', N'Lỗi bo mạch hệ thống nguồn lap', 15500000, 'ND002'),
('TH015', 'HD003', '2026-04-07', N'Trả hàng theo chính sách 7 ngày đầu', 45000000, 'ND002'),
('TH016', 'HD007', '2026-04-12', N'Màn bị hở sáng nặng góc dưới', 8900000, 'ND002'),
('TH017', 'HD010', '2026-04-15', N'Khách mua tặng nhưng người nhận không thích', 19500000, 'ND002'),
('TH018', 'HD011', '2026-04-16', N'Nổ tụ nguồn bốc mùi khét', 2600000, 'ND002'),
('TH019', 'HD014', '2026-04-20', N'Không tương thích với thanh RAM cũ', 3800000, 'ND002'),
('TH020', 'HD015', '2026-04-21', N'Xung đột phần cứng liên tục', 10500000, 'ND002'),
('TH021', 'HD016', '2026-04-22', N'Da ghế bị bong tróc loang lổ', 3600000, 'ND002'),
('TH022', 'HD021', '2026-04-27', N'Lỗi không kết nối được qua cổng USB', 2100000, 'ND002'),
('TH023', 'HD029', '2026-05-02', N'Thiếu phụ kiện dây đi kèm', 3800000, 'ND002'),
('TH024', 'HD030', '2026-05-03', N'Cần analog bị trôi hướng liên tục', 1450000, 'ND002'),
('TH025', 'HD001', '2026-05-05', N'Lỗi phần cứng không khắc phục được', 32000000, 'ND002'),
('TH026', 'HD002', '2026-05-06', N'Khách hàng không hài lòng chất lượng loa lap', 15500000, 'ND002'),
('TH027', 'HD003', '2026-05-07', N'Hủy hợp đồng mua bán doanh nghiệp', 45000000, 'ND002'),
('TH028', 'HD007', '2026-05-12', N'Màn hình có trên 5 điểm chết màu', 8900000, 'ND002'),
('TH029', 'HD010', '2026-05-15', N'Đổi sang dòng VGA khác phân khúc', 19500000, 'ND002'),
('TH030', 'HD011', '2026-05-16', N'Quạt nguồn không quay gây nóng', 2600000, 'ND002');
GO

-- ====================================================================================
-- 14. INSERT DATA FOR BẢNG [ChiTietTraHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietTraHang] ([MaChiTietTra], [MaTraHang], [MaSanPham], [SoLuong], [TienHoan]) VALUES
('CTTH001', 'TH001', 'SP001', 1, 32000000),
('CTTH002', 'TH002', 'SP002', 1, 15500000),
('CTTH003', 'TH003', 'SP003', 1, 45000000),
('CTTH004', 'TH004', 'SP007', 1, 8900000),
('CTTH005', 'TH005', 'SP010', 1, 19500000),
('CTTH006', 'TH006', 'SP011', 1, 2600000),
('CTTH007', 'TH007', 'SP014', 1, 3800000),
('CTTH008', 'TH008', 'SP015', 1, 10500000),
('CTTH009', 'TH009', 'SP016', 1, 3600000),
('CTTH010', 'TH010', 'SP021', 1, 2100000),
('CTTH011', 'TH011', 'SP029', 1, 3800000),
('CTTH012', 'TH012', 'SP030', 1, 1450000),
('CTTH013', 'TH013', 'SP001', 1, 32000000),
('CTTH014', 'TH014', 'SP002', 1, 15500000),
('CTTH015', 'TH015', 'SP003', 1, 45000000),
('CTTH016', 'TH016', 'SP007', 1, 8900000),
('CTTH017', 'TH017', 'SP010', 1, 19500000),
('CTTH018', 'TH018', 'SP011', 1, 2600000),
('CTTH019', 'TH019', 'SP014', 1, 3800000),
('CTTH020', 'TH020', 'SP015', 1, 10500000),
('CTTH021', 'TH021', 'SP016', 1, 3600000),
('CTTH022', 'TH022', 'SP021', 1, 2100000),
('CTTH023', 'TH023', 'SP029', 1, 3800000),
('CTTH024', 'TH024', 'SP030', 1, 1450000),
('CTTH025', 'TH025', 'SP001', 1, 32000000),
('CTTH026', 'TH026', 'SP002', 1, 15500000),
('CTTH027', 'TH027', 'SP003', 1, 45000000),
('CTTH028', 'TH028', 'SP007', 1, 8900000),
('CTTH029', 'TH029', 'SP010', 1, 19500000),
('CTTH030', 'TH030', 'SP011', 1, 2600000);
GO

-- ====================================================================================
-- 15. INSERT DATA FOR BẢNG [LichSuTonKho] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[LichSuTonKho] ([MaLichSu], [MaSanPham], [SoLuongThayDoi], [Loai], [Ngay]) VALUES
('LS001', 'SP001', 10, 'Nhap Kho', '2026-02-10'),
('LS002', 'SP002', 20, 'Nhap Kho', '2026-02-11'),
('LS003', 'SP003', 5, 'Nhap Kho', '2026-02-12'),
('LS004', 'SP004', 30, 'Nhap Kho', '2026-02-13'),
('LS005', 'SP005', 40, 'Nhap Kho', '2026-02-14'),
('LS006', 'SP001', -1, 'Xuat Ban', '2026-03-01'),
('LS007', 'SP002', -1, 'Xuat Ban', '2026-03-02'),
('LS008', 'SP003', -1, 'Xuat Ban', '2026-03-03'),
('LS009', 'SP004', -1, 'Xuat Ban', '2026-03-04'),
('LS010', 'SP005', -1, 'Xuat Ban', '2026-03-05'),
('LS011', 'SP004', 1, 'Nhan Doi', '2026-03-05'),
('LS012', 'SP004', -1, 'Tra Doi', '2026-03-05'),
('LS013', 'SP005', 1, 'Nhan Doi', '2026-03-06'),
('LS014', 'SP005', -1, 'Tra Doi', '2026-03-06'),
('LS015', 'SP001', 1, 'Khach Tra', '2026-03-05'),
('LS016', 'SP002', 1, 'Khach Tra', '2026-03-06'),
('LS017', 'SP003', 1, 'Khach Tra', '2026-03-07'),
('LS018', 'SP006', 15, 'Nhap Kho', '2026-02-15'),
('LS019', 'SP007', 10, 'Nhap Kho', '2026-02-16'),
('LS020', 'SP008', 50, 'Nhap Kho', '2026-02-17'),
('LS021', 'SP009', 25, 'Nhap Kho', '2026-02-18'),
('LS022', 'SP010', 8, 'Nhap Kho', '2026-02-19'),
('LS023', 'SP006', -1, 'Xuat Ban', '2026-03-06'),
('LS024', 'SP007', -1, 'Xuat Ban', '2026-03-07'),
('LS025', 'SP008', -1, 'Xuat Ban', '2026-03-08'),
('LS026', 'SP009', -1, 'Xuat Ban', '2026-03-09'),
('LS027', 'SP010', -1, 'Xuat Ban', '2026-03-10'),
('LS028', 'SP006', 1, 'Nhan Doi', '2026-03-07'),
('LS029', 'SP008', 1, 'Nhan Doi', '2026-03-09'),
('LS030', 'SP009', 1, 'Nhan Doi', '2026-03-10');
GO