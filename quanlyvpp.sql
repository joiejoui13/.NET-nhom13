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
('DM001', N'Bút - Viết các loại', N'Bút bi, bút chì, bút ký cao cấp, bút dạ quang', '2026-01-01'),
('DM002', N'Sổ - Tập học sinh', N'Sổ tay, sổ lò xo, tập học sinh 96 trang, 200 trang', '2026-01-02'),
('DM003', N'Giấy in - Giấy photo', N'Giấy A4, A3, giấy in liên tục, giấy decal', '2026-01-03'),
('DM004', N'File hồ sơ - Khay kệ', N'Bìa còng, bìa lá, khay tài liệu nhựa, kệ mica', '2026-01-04'),
('DM005', N'Dụng cụ cắt - Dán', N'Kéo văn phòng, dao rọc giấy, băng keo, hồ dán', '2026-01-05'),
('DM006', N'Dụng cụ bấm kim - Ghim', N'Máy bấm kim, kim bấm, ghim kẹp giấy, ghim bọc nhựa', '2026-01-06'),
('DM007', N'Máy tính bỏ túi', N'Máy tính Casio học sinh, máy tính kế toán đại sảnh', '2026-01-07'),
('DM008', N'Mực văn phòng', N'Mực máy in, mực dấu, mực bút viết bảng', '2026-01-08'),
('DM009', N'Dụng cụ học sinh', N'Thước kẻ, gôm tẩy, hộp bút, compa', '2026-01-09'),
('DM010', N'Thiết bị đóng sổ', N'Lò xo đóng sổ, bìa kiếng, máy đóng sách', '2026-01-10'),
('DM011', N'Màu vẽ - Mỹ thuật', N'Màu sáp, màu nước, cọ vẽ, bút lông màu', '2026-01-11'),
('DM012', N'Đông dấu - Bảng tên', N'Khay mực dấu, con dấu hộp, bảng tên nhân viên', '2026-01-12'),
('DM013', N'Bảng văn phòng', N'Bảng từ trắng, bảng mica, bút viết bảng', '2026-01-13'),
('DM014', N'Nhu yếu phẩm văn phòng', N'Khăn giấy, nước rửa tay, ly nhựa, trà, cà phê', '2026-01-14'),
('DM015', N'Thiết bị lưu trữ lẻ', N'Băng thẻ nhớ, hộp đựng card, rổ nhựa', '2026-01-15'),
('DM016', N'Quà lưu niệm', N'Thiệp chúc mừng, túi quà, giấy gói quà', '2026-01-16'),
('DM017', N'Văn phòng phẩm da', N'Cặp da hội nghị, hộp cắm bút bọc da', '2026-01-17'),
('DM018', N'Dây đeo thẻ - Phụ kiện', N'Dây đeo thẻ vip, bao thẻ nhựa cứng', '2026-01-18'),
('DM019', N'Pin các loại', N'Pin AA, AAA, pin Panasonic cho thiết bị', '2026-01-19'),
('DM020', N'Đèn bàn học sinh', N'Đèn LED chống cận thị, đèn kẹp bàn', '2026-01-20'),
('DM021', N'Sách tham khảo', N'Sách bài tập, từ điển, sách kỹ năng văn phòng', '2026-01-21'),
('DM022', N'Bao thư - Túi giấy', N'Bao thư A4, bao thư nhỏ, túi xi măng chuyển phát', '2026-01-22'),
('DM023', N'Phấn - Lau bảng', N'Phấn không bụi, mút lau bảng từ tính', '2026-01-23'),
('DM024', N'Sổ kế toán chuyên dụng', N'Sổ cái, sổ thu chi, phiếu xuất kho in sẵn', '2026-01-24'),
('DM025', N'Dụng cụ vệ sinh máy', N'Xịt bụi bàn phím, gel vệ sinh thiết bị văn phòng', '2026-01-25'),
('DM026', N'Lịch để bàn', N'Lịch chữ A, lịch block văn phòng xuân mới', '2026-01-26'),
('DM027', N'Cặp học sinh - Balo', N'Cặp chống gù, balo vải dù siêu nhẹ', '2026-01-27'),
('DM028', N'Băng keo chuyên dụng', N'Băng keo 2 mặt, băng keo mút xốp, băng keo vải', '2026-01-28'),
('DM029', N'Đất sét - Đồ chơi đất', N'Đất nặn tạo hình cho trẻ em mầm học', '2026-01-29'),
('DM030', N'Văn phòng phẩm thông minh', N'Kẹp giữ dây cáp, kệ kê tay chống mỏi văn phòng', '2026-01-30');
GO

-- ====================================================================================
-- 2. INSERT DATA FOR BẢNG [SanPham] (30 dòng, Tồn kho > 100)
-- ====================================================================================
INSERT INTO [dbo].[SanPham] ([MaSanPham], [TenSanPham], [MaDanhMuc], [GiaBan], [GiaNhap], [SoLuongTon], [MoTa], [TrangThai], [NgayTao]) VALUES
('SP001', N'Bút bi Thiên Long TL-027', 'DM001', 4000, 2500, 1200, N'Hộp 20 cây, nét chữ thanh mảnh', N'Kinh Doanh', '2026-01-10'),
('SP002', N'Sổ lò xo Campus A5', 'DM002', 25000, 17000, 350, N'80 trang, giấy chống lóa', N'Kinh Doanh', '2026-01-11'),
('SP003', N'Giấy in Double A A4 70gsm', 'DM003', 75000, 60000, 500, N'Thùng 5 ram, trắng sáng không kẹt giấy', N'Kinh Doanh', '2026-01-12'),
('SP004', N'Bìa còng Plus 7cm A4', 'DM004', 45000, 32000, 220, N'Lưu trữ tài liệu số lượng lớn', N'Kinh Doanh', '2026-01-13'),
('SP005', N'Kéo văn phòng Deli 6009', 'DM005', 18000, 11000, 180, N'Lưỡi thép không gỉ, cán nhựa êm', N'Kinh Doanh', '2026-01-14'),
('SP006', N'Máy bấm kim Deli 0305', 'DM006', 35000, 24000, 140, N'Bấm tối đa 25 tờ giấy', N'Kinh Doanh', '2026-01-15'),
('SP007', N'Máy tính Casio FX-580VN X', 'DM007', 650000, 560000, 130, N'Máy tính khoa học cho học sinh sinh viên', N'Kinh Doanh', '2026-01-16'),
('SP008', N'Mực dấu đóng Trodat 7011', 'DM008', 28000, 19000, 160, N'Lọ 28ml màu đỏ tươi', N'Kinh Doanh', '2026-01-17'),
('SP009', N'Thước kẻ nhôm Deli 30cm', 'DM009', 12000, 7000, 400, N'Vạch chia rõ nét chống mờ', N'Kinh Doanh', '2026-01-18'),
('SP010', N'Bìa kiếng đóng sách A4 mỏng', 'DM010', 55000, 38000, 250, N'Xấp 100 tờ làm bìa sổ', N'Kinh Doanh', '2026-01-19'),
('SP011', N'Hộp màu sáp Colokit 24 màu', 'DM011', 42000, 29000, 190, N'Màu mịn, không độc hại cho trẻ', N'Kinh Doanh', '2026-01-20'),
('SP012', N'Con dấu tên khay mực lật', 'DM012', 80000, 50000, 115, N'Thiết kế dấu đóng liền mực tiện dụng', N'Kinh Doanh', '2026-01-21'),
('SP013', N'Bút viết bảng Thiên Long WB-02', 'DM013', 10000, 6500, 600, N'Bút lông bảng mực đậm dễ xóa', N'Kinh Doanh', '2026-01-22'),
('SP014', N'Hộp khăn giấy rút Pulppy', 'DM014', 22000, 15000, 300, N'Khăn giấy lụa 2 lớp mịn màng', N'Kinh Doanh', '2026-01-23'),
('SP015', N'Hộp đựng namecard mica', 'DM015', 30000, 18000, 150, N'Kệ mica 1 ngăn để bàn làm việc', N'Kinh Doanh', '2026-01-24'),
('SP016', N'Túi quà giấy Kraft vintage', 'DM016', 15000, 8000, 450, N'Túi quai xách thân thiện môi trường', N'Kinh Doanh', '2026-01-25'),
('SP017', N'Cặp da tài liệu đại hội', 'DM017', 180000, 130000, 120, N'Da tổng hợp sang trọng có khóa kéo', N'Kinh Doanh', '2026-01-26'),
('SP018', N'Bao thẻ nhựa dẻo đứng', 'DM018', 3000, 1500, 1000, N'Bảo vệ thẻ nhân viên, học sinh', N'Kinh Doanh', '2026-01-27'),
('SP019', N'Pin AAA Panasonic Hippo', 'DM019', 7000, 4000, 800, N'Vỉ 2 viên cho remote, chuột không dây', N'Kinh Doanh', '2026-01-28'),
('SP020', N'Đèn bàn LED chống cận Deli', 'DM020', 210000, 160000, 105, N'Tích hợp khay cắm bút thông minh', N'Kinh Doanh', '2026-01-29'),
('SP021', N'Từ điển Anh - Việt bỏ túi', 'DM021', 45000, 32000, 140, N'Nhà xuất bản Giáo Dục mới nhất', N'Kinh Doanh', '2026-01-30'),
('SP022', N'Bao thư trắng A4 dày', 'DM022', 2000, 1000, 1500, N'Xấp 50 cái gửi công văn', N'Kinh Doanh', '2026-01-31'),
('SP023', N'Hộp phấn viết bảng không bụi', 'DM023', 15000, 9000, 260, N'Phấn trắng Thiên Long mượt mà', N'Kinh Doanh', '2026-02-01'),
('SP024', N'Sổ cái kế toán khổ dọc A4', 'DM024', 35000, 23000, 175, N'Dày 100 trang bìa cứng', N'Kinh Doanh', '2026-02-02'),
('SP025', N'Chai xịt vệ sinh màn hình LCD', 'DM025', 40000, 25000, 210, N'Kèm khăn lau sợi nhân tạo siêu mịn', N'Kinh Doanh', '2026-02-03'),
('SP026', N'Lịch để bàn chữ A Tết', 'DM026', 30000, 18000, 320, N'Giấy cứng cáp hình ảnh sắc nét', N'Kinh Doanh', '2026-02-04'),
('SP027', N'Balo học sinh siêu nhẹ Miti', 'DM027', 280000, 195000, 110, N'Chống thấm nước cho học sinh tiểu học', N'Kinh Doanh', '2026-02-05'),
('SP028', N'Băng keo 2 mặt 2F 10y', 'DM028', 6000, 3500, 700, N'Độ dính cao tiện dụng đóng gói', N'Kinh Doanh', '2026-02-06'),
('SP029', N'Vỉ đất nặn 12 màu kèm khuôn', 'DM029', 24000, 16000, 230, N'Mềm dẻo, kích thích tư duy sáng tạo', N'Kinh Doanh', '2026-02-07'),
('SP030', N'Đệm kê tay bàn phím silicon', 'DM030', 65000, 45000, 165, N'Chống mỏi cổ tay cho dân văn phòng', N'Kinh Doanh', '2026-02-08');
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
('KM001', 5, '2026-01-01', '2026-02-01', N'Giảm nhẹ đầu năm', N'HAPPYNEWYEAR', N'Hết Hạn'),
('KM002', 10, '2026-02-10', '2026-02-16', N'Mùa tựu trường/tình yêu', N'VALENTINE', N'Hết Hạn'),
('KM003', 15, '2026-03-05', '2026-03-10', N'Ưu đãi ngành mỹ thuật nữ', N'WOMANDAY', N'Hết Hạn'),
('KM004', 20, '2026-04-28', '2026-05-05', N'Đại lễ giải phóng 30/4', N'DAILE304', N'Hoạt Động'),
('KM005', 8, '2026-05-15', '2026-06-15', N'Giảm giá sắm đồ hè', N'HEALOHA', N'Hoạt Động'),
('KM006', 12, '2026-06-01', '2026-06-03', N'Giảm giá tập màu cho thiếu nhi', N'KIDSGAME', N'Hoạt Động'),
('KM007', 5, '2026-01-01', '2026-12-31', N'Đơn đầu tiên mua sỉ văn phòng', N'WELCOME', N'Hoạt Động'),
('KM008', 3, '2026-01-01', '2026-12-31', N'Giảm thành viên đồng', N'COPPERMEMBER', N'Hoạt Động'),
('KM009', 7, '2026-01-01', '2026-12-31', N'Giảm thành viên bạc', N'SILVERMEMBER', N'Hoạt Động'),
('KM010', 10, '2026-01-01', '2026-12-31', N'Giảm doanh nghiệp vàng', N'GOLDMEMBER', N'Hoạt Động'),
('KM011', 2, '2026-02-01', '2026-02-28', N'Khuyến mãi tháng 2', N'SALEKHOITHANG2', N'Hết Hạn'),
('KM012', 4, '2026-03-01', '2026-03-31', N'Khuyến mãi tháng 3', N'SALEKHOITHANG3', N'Hết Hạn'),
('KM013', 6, '2026-04-01', '2026-04-30', N'Khuyến mãi tháng 4', N'SALEKHOITHANG4', N'Hết Hạn'),
('KM014', 8, '2026-05-01', '2026-05-31', N'Khuyến mãi tháng 5', N'SALEKHOITHANG5', N'Hoạt Động'),
('KM015', 10, '2026-06-01', '2026-06-30', N'Khuyến mãi tháng 6', N'SALEKHOITHANG6', N'Hoạt Động'),
('KM016', 12, '2026-07-01', '2026-07-31', N'Khuyến mãi tháng 7', N'SALEKHOITHANG7', N'Hoạt Động'),
('KM017', 14, '2026-08-01', '2026-08-31', N'Mùa tựu trường mua sắm rầm rộ', N'SALEKHOITHANG8', N'Hoạt Động'),
('KM018', 15, '2026-09-01', '2026-09-30', N'Khuyến mãi tháng 9', N'SALEKHOITHANG9', N'Hoạt Động'),
('KM019', 18, '2026-10-01', '2026-10-31', N'Mừng ngày nhà giáo sớm', N'SALEKHOITHANG10', N'Hoạt Động'),
('KM020', 25, '2026-11-20', '2026-11-30', N'Xả kho tưng bừng BlackFriday', N'BLACKFRIDAY', N'Hoạt Động'),
('KM021', 30, '2026-12-10', '2026-12-25', N'Cuối năm dọn văn phòng sạch sẽ', N'CHRISTMAS', N'Hoạt Động'),
('KM022', 5, '2026-05-01', '2026-05-10', N'Combo thước kẻ + bút chì', N'COMBOACC', N'Hoạt Động'),
('KM023', 10, '2026-05-01', '2026-05-15', N'Ưu đãi mua trọn gói bàn học', N'BUILDPC', N'Hoạt Động'),
('KM024', 6, '2026-05-01', '2026-05-31', N'Mã giảm độc quyền App di động', N'APPSALE', N'Hoạt Động'),
('KM025', 5, '2026-05-01', '2026-05-31', N'Mã dành riêng cho đêm muộn', 'MIDNIGHT', N'Hoạt Động'),
('KM026', 4, '2026-05-01', '2026-05-31', N'Giảm giá ngày giữa tuần', N'MIDWEEK', N'Hoạt Động'),
('KM027', 7, '2026-05-01', '2026-05-31', N'Giảm giá cuối tuần xả stress', N'WEEKEND', N'Hoạt Động'),
('KM028', 11, '2026-11-11', '2026-11-12', N'Siêu sale độc thân văn phòng', N'SINGLEDAY', N'Hoạt Động'),
('KM029', 12, '2026-12-12', '2026-12-13', N'Siêu sale cuối cùng năm 12/12', N'LASTSALE', N'Hoạt Động'),
('KM030', 5, '2026-01-01', '2026-12-31', N'Khuyến mãi mừng sinh nhật khách', N'BIRTHDAY', N'Hoạt Động');
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
('HD001', 'KH001', 'ND003', 'KM007', 80000, 4000, 'Tien Mat', '2026-03-01'),
('HD002', 'KH002', 'ND003', 'KM010', 50000, 5000, 'Tien Mat', '2026-03-02'),
('HD003', 'KH003', 'ND006', 'KM007', 750000, 37500, 'Chuyen Khoan', '2026-03-03'),
('HD004', 'KH004', 'ND006', 'KM007', 180000, 9000, 'Tien Mat', '2026-03-04'),
('HD005', 'KH005', 'ND003', 'KM007', 36000, 1800, 'Tien Mat', '2026-03-05'),
('HD006', 'KH006', 'ND003', 'KM008', 70000, 2100, 'Chuyen Khoan', '2026-03-06'),
('HD007', 'KH007', 'ND006', 'KM010', 650000, 65000, 'Chuyen Khoan', '2026-03-07'),
('HD008', 'KH008', 'ND006', 'KM007', 140000, 7000, 'Tien Mat', '2026-03-08'),
('HD009', 'KH009', 'ND003', 'KM007', 12000, 600, 'Tien Mat', '2026-03-09'),
('HD010', 'KH010', 'ND003', 'KM010', 550000, 55000, 'Chuyen Khoan', '2026-03-10'),
('HD011', 'KH011', 'ND006', 'KM007', 84000, 4200, 'Tien Mat', '2026-03-11'),
('HD012', 'KH012', 'ND006', 'KM007', 160000, 8000, 'Chuyen Khoan', '2026-03-12'),
('HD013', 'KH013', 'ND003', 'KM007', 30000, 1500, 'Tien Mat', '2026-03-13'),
('HD014', 'KH014', 'ND003', 'KM007', 66000, 3300, 'Tien Mat', '2026-03-14'),
('HD015', 'KH015', 'ND006', 'KM007', 150000, 7500, 'Tien Mat', '2026-03-15'),
('HD016', 'KH016', 'ND006', 'KM007', 75000, 3750, 'Tien Mat', '2026-03-16'),
('HD017', 'KH017', 'ND003', 'KM010', 180000, 18000, 'Chuyen Khoan', '2026-03-17'),
('HD018', 'KH018', 'ND003', 'KM007', 15000, 750, 'Tien Mat', '2026-03-18'),
('HD019', 'KH019', 'ND006', 'KM007', 28000, 1400, 'Tien Mat', '2026-03-19'),
('HD020', 'KH020', 'ND006', 'KM007', 210000, 10500, 'Chuyen Khoan', '2026-03-20'),
('HD021', 'KH021', 'ND003', 'KM010', 45000, 4500, 'Tien Mat', '2026-03-21'),
('HD022', 'KH022', 'ND003', 'KM007', 10000, 500, 'Tien Mat', '2026-03-22'),
('HD023', 'KH023', 'ND006', 'KM010', 45000, 4500, 'Chuyen Khoan', '2026-03-23'),
('HD024', 'KH024', 'ND006', 'KM007', 35000, 1750, 'Tien Mat', '2026-03-24'),
('HD025', 'KH025', 'ND003', 'KM007', 120000, 6000, 'Chuyen Khoan', '2026-03-25'),
('HD026', 'KH026', 'ND003', 'KM010', 30000, 3000, 'Tien Mat', '2026-03-26'),
('HD027', 'KH027', 'ND006', 'KM007', 280000, 14000, 'Chuyen Khoan', '2026-03-27'),
('HD028', 'KH028', 'ND006', 'KM007', 30000, 1500, 'Tien Mat', '2026-03-28'),
('HD029', 'KH029', 'ND003', 'KM007', 48000, 2400, 'Tien Mat', '2026-03-29'),
('HD030', 'KH030', 'ND003', 'KM010', 65000, 6500, 'Tien Mat', '2026-03-30');
GO

-- ====================================================================================
-- 7. INSERT DATA FOR BẢNG [GiaoHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[GiaoHang] ([MaGiaoHang], [MaHoaDon], [DiaChiGiao], [TrangThaiGiao], [NgayGiao]) VALUES
('GH001', 'HD001', N'Trường THPT Thanh Xuân, Hà Nội', 'Thanh Cong', '2026-03-02'),
('GH002', 'HD002', N'Đại học Bách Khoa, Quận 10, TP HCM', 'Thanh Cong', '2026-03-03'),
('GH003', 'HD003', N'Công ty CP Đầu Tư Hải Châu, Đà Nẵng', 'Thanh Cong', '2026-03-04'),
('GH004', 'HD004', N'Trung tâm Anh ngữ Quy Nhơn', 'Thanh Cong', '2026-03-05'),
('GH005', 'HD005', N'65 Lý Tự Trọng, Ninh Kiều, Cần Thơ', 'Thanh Cong', '2026-03-06'),
('GH006', 'HD006', N'Ủy ban nhân dân Quận Ngô Quyền, Hải Phòng', 'Thanh Cong', '2026-03-07'),
('GH007', 'HD007', N'Trường tiểu học Quang Trung, Vinh', 'Thanh Cong', '2026-03-08'),
('GH008', 'HD008', N'Khách sạn Novotel Nha Trang', 'Thanh Cong', '2026-03-09'),
('GH009', 'HD009', N'Văn phòng công chứng Nguyễn Chí Thanh, HN', 'Thanh Cong', '2026-03-10'),
('GH010', 'HD010', N'Ngân hàng Vietcombank Biên Hòa', 'Thanh Cong', '2026-03-11'),
('GH011', 'HD011', N'Trường THCS Lê Hồng Phong, Vũng Tàu', 'Thanh Cong', '2026-03-12'),
('GH012', 'HD012', N'Bảo hiểm xã hội tỉnh Thừa Thiên Huế', 'Thanh Cong', '2026-03-13'),
('GH013', 'HD013', N'Chi cục thuế TP Buôn Ma Thuột', 'Thanh Cong', '2026-03-14'),
('GH014', 'HD014', N'Khách sạn Hồng Phú, Đà Lạt', 'Thanh Cong', '2026-03-15'),
('GH015', 'HD015', N'Trường Đại học An Giang, Long Xuyên', 'Thanh Cong', '2026-03-16'),
('GH016', 'HD016', N'Tòa nhà Becamex, Thủ Dầu Một, Bình Dương', 'Thanh Cong', '2026-03-17'),
('GH017', 'HD017', N'Sở Giáo dục & Đào tạo Bình Thuận', 'Thanh Cong', '2026-03-18'),
('GH018', 'HD018', N'Bệnh viện Đa khoa tỉnh Gia Lai', 'Thanh Cong', '2026-03-19'),
('GH019', 'HD019', N'Công ty Môi trường Đô thị Thanh Hóa', 'Thanh Cong', '2026-03-20'),
('GH020', 'HD020', N'Trường mầm non Sao Mai, Bắc Giang', 'Thanh Cong', '2026-03-21'),
('GH021', 'HD021', N'Đại học Thái Nguyên, Thái Nguyên', 'Thanh Cong', '2026-03-22'),
('GH022', 'HD022', N'Bưu điện tỉnh Đồng Tháp', 'Thanh Cong', '2026-03-23'),
('GH023', 'HD023', N'Tòa án nhân dân tỉnh Sơn La', 'Thanh Cong', '2026-03-24'),
('GH024', 'HD024', N'Sở Tài chính tỉnh Quảng Bình', 'Thanh Cong', '2026-03-25'),
('GH025', 'HD025', N'Phòng Giáo dục huyện Hòa An, Cao Bằng', 'Thanh Cong', '2026-03-26'),
('GH026', 'HD026', N'Ủy ban Mặt trận Tổ quốc Phú Yên', 'Thanh Cong', '2026-03-27'),
('GH027', 'HD027', N'Trường THPT Nguyễn Đình Chiểu, Mỹ Tho', 'Thanh Cong', '2026-03-28'),
('GH028', 'HD028', N'Đài Phát thanh Truyền hình Sóc Trăng', 'Thanh Cong', '2026-03-29'),
('GH029', 'HD029', N'Kho bạc nhà nước tỉnh Nam Định', 'Thanh Cong', '2026-03-30'),
('GH030', 'HD030', N'Trung tâm y tế thành phố Vĩnh Yên', 'Thanh Cong', '2026-03-31');
GO

-- ====================================================================================
-- 8. INSERT DATA FOR BẢNG [ChiTietHoaDon] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietHoaDon] ([MaChiTiet], [MaHoaDon], [MaSanPham], [SoLuong], [DonGia], [ThanhTien]) VALUES
('CTHD001', 'HD001', 'SP001', 20, 4000, 80000),
('CTHD002', 'HD002', 'SP002', 2, 25000, 50000),
('CTHD003', 'HD003', 'SP003', 10, 75000, 750000),
('CTHD004', 'HD004', 'SP004', 4, 45000, 180000),
('CTHD005', 'HD005', 'SP005', 2, 18000, 36000),
('CTHD006', 'HD006', 'SP006', 2, 35000, 70000),
('CTHD007', 'HD007', 'SP007', 1, 650000, 650000),
('CTHD008', 'HD008', 'SP008', 5, 28000, 140000),
('CTHD009', 'HD009', 'SP009', 1, 12000, 12000),
('CTHD010', 'HD010', 'SP010', 10, 55000, 550000),
('CTHD011', 'HD011', 'SP011', 2, 42000, 84000),
('CTHD012', 'HD012', 'SP012', 2, 80000, 160000),
('CTHD013', 'HD013', 'SP013', 3, 10000, 30000),
('CTHD014', 'HD014', 'SP014', 3, 22000, 66000),
('CTHD015', 'HD015', 'SP015', 5, 30000, 150000),
('CTHD016', 'HD016', 'SP016', 5, 15000, 75000),
('CTHD017', 'HD017', 'SP017', 1, 180000, 180000),
('CTHD018', 'HD018', 'SP018', 5, 3000, 15000),
('CTHD019', 'HD019', 'SP019', 4, 7000, 28000),
('CTHD020', 'HD020', 'SP020', 1, 210000, 210000),
('CTHD021', 'HD021', 'SP021', 1, 45000, 45000),
('CTHD022', 'HD022', 'SP022', 5, 2000, 10000),
('CTHD023', 'HD023', 'SP023', 3, 15000, 45000),
('CTHD024', 'HD024', 'SP024', 1, 35000, 35000),
('CTHD025', 'HD025', 'SP025', 3, 40000, 120000),
('CTHD026', 'HD026', 'SP026', 1, 30000, 30000),
('CTHD027', 'HD027', 'SP027', 1, 280000, 280000),
('CTHD028', 'HD028', 'SP028', 5, 6000, 30000),
('CTHD029', 'HD029', 'SP029', 2, 24000, 48000),
('CTHD030', 'HD030', 'SP030', 1, 65000, 65000);
GO

-- ====================================================================================
-- 9. INSERT DATA FOR BẢNG [PhieuNhap] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[PhieuNhap] ([MaPhieuNhap], [MaNguoiDung], [NgayNhap], [TongTien]) VALUES
('PN001', 'ND004', '2026-02-10', 500000),
('PN002', 'ND004', '2026-02-11', 340000),
('PN003', 'ND007', '2026-02-12', 1200000),
('PN004', 'ND007', '2026-02-13', 640000),
('PN005', 'ND004', '2026-02-14', 220000),
('PN006', 'ND004', '2026-02-15', 480000),
('PN007', 'ND007', '2026-02-16', 11200000),
('PN008', 'ND007', '2026-02-17', 380000),
('PN009', 'ND004', '2026-02-18', 140000),
('PN010', 'ND004', '2026-02-19', 760000),
('PN011', 'ND007', '2026-02-20', 580000),
('PN012', 'ND007', '2026-02-21', 1000000),
('PN013', 'ND004', '2026-02-22', 130000),
('PN014', 'ND004', '2026-02-23', 300000),
('PN015', 'ND007', '2026-02-24', 360000),
('PN016', 'ND007', '2026-02-25', 160000),
('PN017', 'ND004', '2026-02-26', 2600000),
('PN018', 'ND004', '2026-02-27', 30000),
('PN019', 'ND007', '2026-02-28', 80000),
('PN020', 'ND007', '2026-03-01', 3200000),
('PN021', 'ND004', '2026-03-02', 640000),
('PN022', 'ND004', '2026-03-03', 20000),
('PN023', 'ND007', '2026-03-04', 180000),
('PN024', 'ND007', '2026-03-05', 460000),
('PN025', 'ND004', '2026-03-06', 500000),
('PN026', 'ND004', '2026-03-07', 360000),
('PN027', 'ND007', '2026-03-08', 3900000),
('PN028', 'ND007', '2026-03-09', 70000),
('PN029', 'ND004', '2026-03-10', 320000),
('PN030', 'ND004', '2026-03-11', 900000);
GO

-- ====================================================================================
-- 10. INSERT DATA FOR BẢNG [ChiTietPhieuNhap] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietPhieuNhap] ([MaChiTietNhap], [MaPhieuNhap], [MaSanPham], [SoLuong], [GiaNhap]) VALUES
('CTPN001', 'PN001', 'SP001', 200, 2500),
('CTPN002', 'PN002', 'SP002', 20, 17000),
('CTPN003', 'PN003', 'SP003', 20, 60000),
('CTPN004', 'PN004', 'SP004', 20, 32000),
('CTPN005', 'PN005', 'SP005', 20, 11000),
('CTPN006', 'PN006', 'SP006', 20, 24000),
('CTPN007', 'PN007', 'SP007', 20, 560000),
('CTPN008', 'PN008', 'SP008', 20, 19000),
('CTPN009', 'PN009', 'SP009', 20, 7000),
('CTPN010', 'PN010', 'SP010', 20, 38000),
('CTPN011', 'PN011', 'SP011', 20, 29000),
('CTPN012', 'PN012', 'SP012', 20, 50000),
('CTPN013', 'PN013', 'SP013', 20, 65000),
('CTPN014', 'PN014', 'SP014', 20, 15000),
('CTPN015', 'PN015', 'SP015', 20, 18000),
('CTPN016', 'PN016', 'SP016', 20, 8000),
('CTPN017', 'PN017', 'SP017', 20, 130000),
('CTPN018', 'PN018', 'SP018', 20, 1500),
('CTPN019', 'PN019', 'SP019', 20, 4000),
('CTPN020', 'PN020', 'SP020', 20, 160000),
('CTPN021', 'PN021', 'SP021', 20, 32000),
('CTPN022', 'PN022', 'SP022', 20, 1000),
('CTPN023', 'PN023', 'SP023', 20, 9000),
('CTPN024', 'PN024', 'SP024', 20, 23000),
('CTPN025', 'PN025', 'SP025', 20, 25000),
('CTPN026', 'PN026', 'SP026', 20, 18000),
('CTPN027', 'PN027', 'SP027', 20, 195000),
('CTPN028', 'PN028', 'SP028', 20, 3500),
('CTPN029', 'PN029', 'SP029', 20, 16000),
('CTPN030', 'PN030', 'SP030', 20, 45000);
GO

-- ====================================================================================
-- 11. INSERT DATA FOR BẢNG [DoiHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[DoiHang] ([MaDoiHang], [MaHoaDon], [NgayDoi], [MaNguoiDung], [LyDo]) VALUES
('DH001', 'HD004', '2026-03-05', 'ND010', N'Bìa còng bị rách một góc bên mép'),
('DH002', 'HD005', '2026-03-06', 'ND010', N'Kéo có tay nhựa lỏng'),
('DH003', 'HD006', '2026-03-07', 'ND010', N'Máy bấm ghim kẹt lò xo bên trong'),
('DH004', 'HD008', '2026-03-09', 'ND010', N'Mực dấu đóng bị khô màu'),
('DH005', 'HD009', '2026-03-10', 'ND010', N'Thước nhôm xước vạch phân cấp'),
('DH006', 'HD012', '2026-03-14', 'ND010', N'Dấu đóng sai font yêu cầu lẻ'),
('DH007', 'HD013', '2026-03-15', 'ND010', N'Bút bảng ngòi tà mực ra yếu'),
('DH008', 'HD017', '2026-03-18', 'ND010', N'Cặp da lỗi khóa kéo kẹt cứng'),
('DH009', 'HD018', '2026-03-19', 'ND010', N'Bao thẻ nhựa rách viền nhựa dẻo'),
('DH010', 'HD019', '2026-03-20', 'ND010', N'Pin Hippo chảy dung dịch'),
('DH011', 'HD020', '2026-03-21', 'ND010', N'Đèn LED cắm điện không sáng bừng'),
('DH012', 'HD022', '2026-03-23', 'ND010', N'Phấn viết bể vụn nhiều viên'),
('DH013', 'HD023', '2026-03-24', 'ND010', N'Sổ cái lem mực in lề hàng'),
('DH014', 'HD024', '2026-03-25', 'ND010', N'Chai xịt vỡ vòi xịt phun sương'),
('DH015', 'HD025', '2026-03-26', 'ND010', N'Lịch để bàn nhăn góc giấy nền'),
('DH016', 'HD026', '2026-03-27', 'ND010', N'Balo đứt đường chỉ gài vai đeo'),
('DH017', 'HD027', '2026-03-28', 'ND010', N'Băng keo hai mặt hết keo khô reo'),
('DH018', 'HD028', '2026-03-29', 'ND010', N'Đất nặn khô cứng khó nhào nặn'),
('DH019', 'HD004', '2026-04-01', 'ND010', N'Khách đổi ý muốn lấy bìa dày hơn'),
('DH020', 'HD005', '2026-04-02', 'ND010', N'Đổi sản phẩm cùng loại đổi mẫu mã'),
('DH021', 'HD006', '2026-04-03', 'ND010', N'Đổi trả kim bấm cùng màu xanh'),
('DH022', 'HD008', '2026-04-04', 'ND010', N'Đổi lọ mực lấy màu xanh biển'),
('DH023', 'HD009', '2026-04-05', 'ND010', N'Thước bị cong nhẹ do vận chuyển'),
('DH024', 'HD012', '2026-04-06', 'ND010', N'Khắc lại con dấu chức danh mới'),
('DH025', 'HD013', '2026-04-07', 'ND010', N'Đổi bút viết bảng đen sang đỏ'),
('DH026', 'HD017', '2026-04-08', 'ND010', N'Cặp da bị trầy xước nhẹ bề mặt'),
('DH027', 'HD018', '2026-04-09', 'ND010', N'Đổi bao thẻ đứng sang bao ngang'),
('DH028', 'HD019', '2026-04-10', 'ND010', N'Pin lắp không vừa khay thiết bị'),
('DH029', 'HD020', '2026-04-11', 'ND010', N'Đèn LED lỗi nút cảm ứng bật mở'),
('DH030', 'HD022', '2026-04-12', 'ND010', N'Đổi phấn trắng sang phấn màu lẻ');
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
('TH001', 'HD001', '2026-03-05', N'Khách mua nhầm loại bút bi chữ thanh', 80000, 'ND002'),
('TH002', 'HD002', '2026-03-06', N'Sổ Campus nhầm kích thước khổ giấy', 50000, 'ND002'),
('TH003', 'HD003', '2026-03-07', N'Hủy mua sỉ giấy Double A do tìm nơi rẻ hơn', 750000, 'ND002'),
('TH004', 'HD007', '2026-03-12', N'Máy tính Casio bị cấn vỡ màn hình LCD', 650000, 'ND002'),
('TH005', 'HD010', '2026-03-15', N'Bìa kiếng mua dư số lượng sự kiện', 550000, 'ND002'),
('TH006', 'HD011', '2026-03-16', N'Hộp sáp màu nứt gãy hết một nửa vỉ', 84000, 'ND002'),
('TH007', 'HD014', '2026-03-20', N'Khăn giấy Pulppy ẩm mốc bao bì đóng gói', 66000, 'ND002'),
('TH008', 'HD015', '2026-03-21', N'Kệ mica nứt vỡ đường góc cạnh', 150000, 'ND002'),
('TH009', 'HD016', '2026-03-22', N'Túi quà giấy rách quai xách lỏng lẻo', 75000, 'ND002'),
('TH010', 'HD021', '2026-03-27', N'Từ điển rách trang mục lục cứu chữ', 45000, 'ND002'),
('TH011', 'HD029', '2026-04-02', N'Đất nặn nghe mùi nhựa hôi nồng', 48000, 'ND002'),
('TH012', 'HD030', '2026-04-03', N'Kê silicon rách rỉ lớp gel đệm', 65000, 'ND002'),
('TH013', 'HD001', '2026-04-05', N'Trả hàng hoàn tiền do mua trùng lắp', 80000, 'ND002'),
('TH014', 'HD002', '2026-04-06', N'Trả hàng theo nhu cầu công ty thay đổi', 50000, 'ND002'),
('TH015', 'HD003', '2026-04-07', N'Đơn hàng giấy in bị hủy thầu nội bộ', 750000, 'ND002'),
('TH016', 'HD007', '2026-04-12', N'Casio lỗi phím bấm không nảy số', 650000, 'ND002'),
('TH017', 'HD010', '2026-04-15', N'Bìa kiếng sai độ dày mỏng mong muốn', 550000, 'ND002'),
('TH018', 'HD011', '2026-04-16', N'Trả màu sáp lấy lại tiền mặt', 84000, 'ND002'),
('TH019', 'HD014', '2026-04-20', N'Giấy lau bị rách lõi các cuộn trong', 66000, 'ND002'),
('TH020', 'HD015', '2026-04-21', N'Mica bị ố vàng loang lổ nhựa', 150000, 'ND002'),
('TH021', 'HD016', '2026-04-22', N'Không thích mẫu túi kraft này nữa', 75000, 'ND002'),
('TH022', 'HD021', '2026-04-27', N'Sách in nhòe chữ lỗi nhà xuất bản', 45000, 'ND002'),
('TH023', 'HD029', '2026-05-02', N'Sản phẩm lỗi chất liệu dính tay', 48000, 'ND002'),
('TH024', 'HD030', '2026-05-03', N'Đệm kê tay quá cao cấn cổ tay', 65000, 'ND002'),
('TH025', 'HD001', '2026-05-05', N'Lỗi đóng gói từ xưởng sỉ Thiên Long', 80000, 'ND002'),
('TH026', 'HD002', '2026-05-06', N'Khách hàng hoàn trả tập Campus', 50000, 'ND002'),
('TH027', 'HD003', '2026-05-07', N'Kho hết chỗ chứa hoàn trả giấy bớt', 750000, 'ND002'),
('TH028', 'HD007', '2026-05-12', N'Máy không lên nguồn pin năng lượng', 650000, 'ND002'),
('TH029', 'HD010', '2026-05-15', N'Đổi kế hoạch không dùng bìa kiếng', 550000, 'ND002'),
('TH030', 'HD011', '2026-05-16', N'Màu sáp bị chảy do nắng nóng ship', 84000, 'ND002');
GO

-- ====================================================================================
-- 14. INSERT DATA FOR BẢNG [ChiTietTraHang] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[ChiTietTraHang] ([MaChiTietTra], [MaTraHang], [MaSanPham], [SoLuong], [TienHoan]) VALUES
('CTTH001', 'TH001', 'SP001', 20, 80000),
('CTTH002', 'TH002', 'SP002', 2, 50000),
('CTTH003', 'TH003', 'SP003', 10, 750000),
('CTTH004', 'TH004', 'SP007', 1, 650000),
('CTTH005', 'TH005', 'SP010', 10, 550000),
('CTTH006', 'TH006', 'SP011', 2, 84000),
('CTTH007', 'TH007', 'SP014', 3, 66000),
('CTTH008', 'TH008', 'SP015', 5, 150000),
('CTTH009', 'TH009', 'SP016', 5, 75000),
('CTTH010', 'TH010', 'SP021', 1, 45000),
('CTTH011', 'TH011', 'SP029', 2, 48000),
('CTTH012', 'TH012', 'SP030', 1, 65000),
('CTTH013', 'TH013', 'SP001', 20, 80000),
('CTTH014', 'TH014', 'SP002', 2, 50000),
('CTTH015', 'TH015', 'SP003', 10, 750000),
('CTTH016', 'TH016', 'SP007', 1, 650000),
('CTTH017', 'TH017', 'SP010', 10, 550000),
('CTTH018', 'TH018', 'SP011', 2, 84000),
('CTTH019', 'TH019', 'SP014', 3, 66000),
('CTTH020', 'TH020', 'SP015', 5, 150000),
('CTTH021', 'TH021', 'SP016', 5, 75000),
('CTTH022', 'TH022', 'SP021', 1, 45000),
('CTTH023', 'TH023', 'SP029', 2, 48000),
('CTTH024', 'TH024', 'SP030', 1, 65000),
('CTTH025', 'TH025', 'SP001', 20, 80000),
('CTTH026', 'TH026', 'SP002', 2, 50000),
('CTTH027', 'TH027', 'SP003', 10, 750000),
('CTTH028', 'TH028', 'SP007', 1, 650000),
('CTTH029', 'TH029', 'SP010', 10, 550000),
('CTTH030', 'TH030', 'SP011', 2, 84000);
GO

-- ====================================================================================
-- 15. INSERT DATA FOR BẢNG [LichSuTonKho] (30 dòng)
-- ====================================================================================
INSERT INTO [dbo].[LichSuTonKho] ([MaLichSu], [MaSanPham], [SoLuongThayDoi], [Loai], [Ngay]) VALUES
('LS001', 'SP001', 1000, 'Nhap Kho', '2026-02-10'),
('LS002', 'SP002', 300, 'Nhap Kho', '2026-02-11'),
('LS003', 'SP003', 400, 'Nhap Kho', '2026-02-12'),
('LS004', 'SP004', 200, 'Nhap Kho', '2026-02-13'),
('LS005', 'SP005', 150, 'Nhap Kho', '2026-02-14'),
('LS006', 'SP001', -20, 'Xuat Ban', '2026-03-01'),
('LS007', 'SP002', -2, 'Xuat Ban', '2026-03-02'),
('LS008', 'SP003', -10, 'Xuat Ban', '2026-03-03'),
('LS009', 'SP004', -4, 'Xuat Ban', '2026-03-04'),
('LS010', 'SP005', -2, 'Xuat Ban', '2026-03-05'),
('LS011', 'SP004', 1, 'Nhan Doi', '2026-03-05'),
('LS012', 'SP004', -1, 'Tra Doi', '2026-03-05'),
('LS013', 'SP005', 1, 'Nhan Doi', '2026-03-06'),
('LS014', 'SP005', -1, 'Tra Doi', '2026-03-06'),
('LS015', 'SP001', 20, 'Khach Tra', '2026-03-05'),
('LS016', 'SP002', 2, 'Khach Tra', '2026-03-06'),
('LS017', 'SP003', 10, 'Khach Tra', '2026-03-07'),
('LS018', 'SP006', 150, 'Nhap Kho', '2026-02-15'),
('LS019', 'SP007', 120, 'Nhap Kho', '2026-02-16'),
('LS020', 'SP008', 150, 'Nhap Kho', '2026-02-17'),
('LS021', 'SP009', 350, 'Nhap Kho', '2026-02-18'),
('LS022', 'SP010', 220, 'Nhap Kho', '2026-02-19'),
('LS023', 'SP006', -2, 'Xuat Ban', '2026-03-06'),
('LS024', 'SP007', -1, 'Xuat Ban', '2026-03-07'),
('LS025', 'SP008', -5, 'Xuat Ban', '2026-03-08'),
('LS026', 'SP009', -1, 'Xuat Ban', '2026-03-09'),
('LS027', 'SP010', -10, 'Xuat Ban', '2026-03-10'),
('LS028', 'SP006', 1, 'Nhan Doi', '2026-03-07'),
('LS029', 'SP008', 1, 'Nhan Doi', '2026-03-09'),
('LS030', 'SP009', 1, 'Nhan Doi', '2026-03-10');
GO
