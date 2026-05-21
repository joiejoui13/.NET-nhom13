USE [TestChamnetCK]
GO

/****** Object:  Table [dbo].[tblNhanvien] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblNhanvien]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblNhanvien](
	[MaNV] [nvarchar](10) NOT NULL,
	[TenNV] [nvarchar](50) NULL,
	[CCCD] [nvarchar](50) NULL,
	[Matkhau] [nvarchar](50) NULL,
	[Vaitro] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblNhanvien] PRIMARY KEY CLUSTERED ([MaNV] ASC)
)
END
GO

-- Xóa dữ liệu cũ để tránh trùng lặp khi chạy lại
DELETE FROM [dbo].[tblNhanvien]
GO

-- Chèn 3 tài khoản mẫu cho 3 vai trò (Dùng tên Role làm CCCD để test nhanh)
INSERT [dbo].[tblNhanvien] ([MaNV], [TenNV], [CCCD], [Matkhau], [Vaitro]) VALUES (N'NV01', N'Admin Manager', N'ADMIN', N'123', N'ADMIN')
INSERT [dbo].[tblNhanvien] ([MaNV], [TenNV], [CCCD], [Matkhau], [Vaitro]) VALUES (N'NV02', N'Sales Staff', N'SALES', N'123', N'SALES')
INSERT [dbo].[tblNhanvien] ([MaNV], [TenNV], [CCCD], [Matkhau], [Vaitro]) VALUES (N'NV03', N'Warehouse Staff', N'WAREHOUSE', N'123', N'WAREHOUSE')
GO

/****** Object:  Table [dbo].[tblHangHoa] (Mẫu cho Warehouse) ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblHangHoa]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[tblHangHoa](
        [MaHang] [nvarchar](10) NOT NULL PRIMARY KEY,
        [TenHang] [nvarchar](100) NULL,
        [MaLoai] [nvarchar](10) NULL,
        [SoLuong] [int] DEFAULT 0,
        [DonGiaBan] [decimal](18, 2) DEFAULT 0
    )
END
GO

