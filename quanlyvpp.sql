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