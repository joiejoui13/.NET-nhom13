USE CKNet;
GO

-- ========================================================
-- PHáº¦N 1: XÃ“A CÃC Báº¢NG CÅ¨ (Theo thá»© tá»± an toÃ n)
-- ========================================================
IF OBJECT_ID('ChiTietDoiHang', 'U') IS NOT NULL DROP TABLE ChiTietDoiHang;
IF OBJECT_ID('DoiHang', 'U') IS NOT NULL DROP TABLE DoiHang;
IF OBJECT_ID('ChiTietTraHang', 'U') IS NOT NULL DROP TABLE ChiTietTraHang;
IF OBJECT_ID('GiaoHang', 'U') IS NOT NULL DROP TABLE GiaoHang;
IF OBJECT_ID('TraHang', 'U') IS NOT NULL DROP TABLE TraHang;
IF OBJECT_ID('ChiTietHoaDon', 'U') IS NOT NULL DROP TABLE ChiTietHoaDon;
IF OBJECT_ID('HoaDon', 'U') IS NOT NULL DROP TABLE HoaDon;
IF OBJECT_ID('ChiTietNhapHang', 'U') IS NOT NULL DROP TABLE ChiTietNhapHang;
IF OBJECT_ID('ChiTietPhieuNhap', 'U') IS NOT NULL DROP TABLE ChiTietPhieuNhap;
IF OBJECT_ID('PhieuNhap', 'U') IS NOT NULL DROP TABLE PhieuNhap;
IF OBJECT_ID('LichSuNhapKho', 'U') IS NOT NULL DROP TABLE LichSuNhapKho;
IF OBJECT_ID('LichSuTonKho', 'U') IS NOT NULL DROP TABLE LichSuTonKho;
IF OBJECT_ID('SanPham', 'U') IS NOT NULL DROP TABLE SanPham;
IF OBJECT_ID('NguoiDung', 'U') IS NOT NULL DROP TABLE NguoiDung;
IF OBJECT_ID('KhachHang', 'U') IS NOT NULL DROP TABLE KhachHang;
IF OBJECT_ID('KhuyenMai', 'U') IS NOT NULL DROP TABLE KhuyenMai;
IF OBJECT_ID('DanhMuc', 'U') IS NOT NULL DROP TABLE DanhMuc;
GO

-- ========================================================
-- PHáº¦N 2: Táº O Láº I Cáº¤U TRÃšC Báº¢NG (Báº£n chá»‘t)
-- ========================================================
CREATE TABLE DanhMuc (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(250),
    TrangThai NVARCHAR(50),
    NgayTao DATETIME,
    NgayCapNhat DATETIME
);

CREATE TABLE KhuyenMai (
    MaKhuyenMai INT IDENTITY(1,1) PRIMARY KEY,
    TenKhuyenMai NVARCHAR(100) NOT NULL,
    PhanTramGiamGia FLOAT,
    MoTaKhuyenMai NVARCHAR(250),
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai NVARCHAR(50)
);

CREATE TABLE KhachHang (
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
    TenKhachHang NVARCHAR(100),
    SoDienThoai VARCHAR(15),
    Email VARCHAR(100),
    DiaChi NVARCHAR(255),
    NgayTao DATETIME
);

CREATE TABLE NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    TenNguoiDung NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    VaiTro NVARCHAR(50) NOT NULL,
    TrangThai NVARCHAR(50),
    NgayTao DATETIME
);

CREATE TABLE SanPham (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(100) NOT NULL, -- ÄÃ£ gá»™p tÃªn thÆ°Æ¡ng hiá»‡u vÃ o Ä‘Ã¢y
    MaDanhMuc INT,
    GiaNhap FLOAT,
    GiaBan FLOAT,
    SoLuongTon INT,
    MoTa NVARCHAR(250),
    Anh NVARCHAR(255),
    TrangThai NVARCHAR(50),
    NgayTao DATETIME,
    NgayCapNhat DATETIME,
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc)
);

CREATE TABLE LichSuNhapKho (
    MaLichSu INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT,
    Thoigian DATETIME,
    ThayDoi INT,
    SoLuongTruoc INT,
    SoLuongSau INT,
    LoaiGiaoDich NVARCHAR(50),
    MaThamChieu INT,
    TrangThai NVARCHAR(50),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE PhieuNhap (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT,
    TongTien FLOAT,
    TrangThai NVARCHAR(50),
    NgayNhap DATETIME,
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE ChiTietNhapHang (
    MaChiTietNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap INT,
    MaSanPham INT,
    SoLuong INT,
    DonGia FLOAT,
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE HoaDon (
    MaHoaDon INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT,
    MaNguoiDung INT,
    MaKhuyenMai INT,
    TongTien FLOAT,
    TrangThai NVARCHAR(50),
    NgayTao DATETIME,
    PhuongThucThanhToan NVARCHAR(50),
    LoaiHoaDon NVARCHAR(50) DEFAULT N'ÄÆ¡n bÃ¡n hÃ ng', 
    LyDoHuy NVARCHAR(255), -- Cá»™t má»›i Ä‘á»ƒ lÆ°u lÃ½ do há»§y Ä‘Æ¡n
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai)
);

CREATE TABLE ChiTietHoaDon (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon INT,
    MaSanPham INT,
    SoLuong INT,
    DonGia FLOAT,
    ThanhTien FLOAT,
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE TraHang (
    MaTraHang INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon INT,
    MaNguoiDung INT,
    LyDo NVARCHAR(255),
    TongTienHoan FLOAT,
    TrangThai NVARCHAR(50),
    NgayTra DATETIME,
    LoaiGiaoDich NVARCHAR(50) DEFAULT N'Tráº£ hÃ ng', 
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE ChiTietTraHang (
    MaChiTietTra INT IDENTITY(1,1) PRIMARY KEY,
    MaTraHang INT,
    MaSanPham INT,
    SoLuong INT,
    TienHoan FLOAT,
    TinhTrang NVARCHAR(250),
    FOREIGN KEY (MaTraHang) REFERENCES TraHang(MaTraHang),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE GiaoHang (
    MaGiaoHang INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon INT,
    MaTraHang INT,
    DiaChiGiao NVARCHAR(255),
    TrangThaiGiao NVARCHAR(50),
    NgayGiao DATETIME,
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaTraHang) REFERENCES TraHang(MaTraHang)
);
GO

-- ========================================================
-- PHáº¦N 3: INSERT Dá»® LIá»†U MáºªU Äá»’NG Bá»˜
-- ========================================================

-- 1. DanhMuc
SET IDENTITY_INSERT DanhMuc ON;
INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, TrangThai, NgayTao) VALUES
(1, N'BÃºt cÃ¡c loáº¡i', N'BÃºt bi, bÃºt chÃ¬, bÃºt dáº¡, bÃºt kÃ½', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(2, N'Sá»• - Vá»Ÿ', N'Vá»Ÿ há»c sinh, sá»• da, sá»• tay', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(3, N'Giáº¥y in - photo', N'Giáº¥y A4, A3, giáº¥y in nhiá»‡t', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(4, N'BÃ¬a - File há»“ sÆ¡', N'BÃ¬a cÃ²ng, bÃ¬a lÃ¡, káº¹p rÃºt', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(5, N'Dá»¥ng cá»¥ há»c sinh', N'ThÆ°á»›c káº», gá»t bÃºt chÃ¬, compa', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(6, N'Äá»“ dÃ¹ng vÄƒn phÃ²ng', N'Dáº­p ghim, bÄƒng dÃ­nh, kÃ©o, káº¹p bÆ°á»›m', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(7, N'MÃ¡y tÃ­nh cáº§m tay', N'MÃ¡y tÃ­nh bá» tÃºi há»c sinh, káº¿ toÃ¡n', N'Hoáº¡t Ä‘á»™ng', GETDATE());
SET IDENTITY_INSERT DanhMuc OFF;

-- 2. KhuyenMai 
SET IDENTITY_INSERT KhuyenMai ON;
INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) VALUES
(1, N'Back to School', 10, '2026-08-01', '2026-09-15', N'Æ¯u Ä‘Ã£i tá»±u trÆ°á»ng', N'ChÆ°a diá»…n ra'),
(2, N'Sale Giá»¯a NÄƒm', 5, '2026-06-01', '2026-06-30', N'Khuyáº¿n mÃ£i thÃ¡ng 6', N'ChÆ°a diá»…n ra'),
(3, N'KhÃ¡ch mua sá»‰ B2B', 15, '2026-01-01', '2026-12-31', N'DÃ nh cho cÃ´ng ty Ä‘á»‘i tÃ¡c', N'Äang diá»…n ra');
SET IDENTITY_INSERT KhuyenMai OFF;

-- 3. KhachHang
SET IDENTITY_INSERT KhachHang ON;
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiaChi, NgayTao) VALUES
(1, N'TrÆ°á»ng THPT X', '0911222333', N'Äá»‘ng Äa, HÃ  Ná»™i', GETDATE()),
(2, N'CÃ´ng ty CP ABC', '0988777666', N'Cáº§u Giáº¥y, HÃ  Ná»™i', GETDATE()),
(3, N'Nguyá»…n VÄƒn Há»c Sinh', '0900111222', N'Thanh XuÃ¢n, HÃ  Ná»™i', GETDATE());
SET IDENTITY_INSERT KhachHang OFF;

-- 4. NguoiDung 
SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) VALUES
(1, N'Quáº£n LÃ½ Cá»­a HÃ ng', '0901000111', 'admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(2, N'Thu NgÃ¢n 1', '0901000222', 'thungan@vpp.com', 'hashed_pass', 'SALES', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(3, N'Thá»§ Kho 1', '0901000333', 'kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoáº¡t Ä‘á»™ng', GETDATE());
SET IDENTITY_INSERT NguoiDung OFF;

-- 5. SanPham (Gáº¯n kÃ¨m ThÆ°Æ¡ng Hiá»‡u Ä‘á»ƒ Search LIKE)
SET IDENTITY_INSERT SanPham ON;
INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, TrangThai, NgayTao) VALUES
(1, N'BÃºt bi ThiÃªn Long TL-027 Xanh', 1, 3000, 5000, 1000, N'BÃºt quá»‘c dÃ¢n ngÃ²i 0.5mm', N'Äang bÃ¡n', GETDATE()),
(2, N'BÃºt dáº¡ quang Deli Macaron', 1, 8000, 12000, 300, N'BÃºt highlight mÃ u pastel', N'Äang bÃ¡n', GETDATE()),
(3, N'BÃºt mÃ¡y Há»“ng HÃ  NÃ©t Hoa', 1, 35000, 45000, 150, N'BÃºt luyá»‡n chá»¯ Ä‘áº¹p', N'Äang bÃ¡n', GETDATE()),
(4, N'Vá»Ÿ káº» ngang Há»“ng HÃ  72 trang', 2, 6000, 9000, 800, N'Giáº¥y chá»‘ng lÃ³a máº¯t', N'Äang bÃ¡n', GETDATE()),
(5, N'Vá»Ÿ Ã´ ly Campus 96 trang', 2, 8500, 12000, 500, N'GÃ¡y keo Ä‘a lá»›p siÃªu bá»n', N'Äang bÃ¡n', GETDATE()),
(6, N'Sá»• da cao cáº¥p Háº£i Tiáº¿n A5', 2, 45000, 65000, 100, N'Sá»• tay doanh nhÃ¢n bÃ¬a Ä‘en', N'Äang bÃ¡n', GETDATE()),
(7, N'Giáº¥y in Double A A4 70gsm', 3, 65000, 80000, 200, N'Lá»‘c 500 tá» giáº¥y ThÃ¡i', N'Äang bÃ¡n', GETDATE()),
(8, N'Giáº¥y in Paper One A4 80gsm', 3, 70000, 85000, 150, N'Giáº¥y in Ä‘á»‹nh lÆ°á»£ng dÃ y dáº·n', N'Äang bÃ¡n', GETDATE()),
(9, N'BÃ¬a cÃ²ng ThiÃªn Long 7cm', 4, 25000, 35000, 120, N'BÃ¬a cÃ²ng lÆ°u trá»¯ há»“ sÆ¡ A4', N'Äang bÃ¡n', GETDATE()),
(10, N'BÄƒng dÃ­nh trong Deli 5cm', 6, 12000, 18000, 400, N'Cuá»™n lá»›n 100 yard', N'Äang bÃ¡n', GETDATE()),
(11, N'Dáº­p ghim Plus sá»‘ 10', 6, 28000, 40000, 80, N'Dáº­p ghim Nháº­t Báº£n', N'Äang bÃ¡n', GETDATE()),
(12, N'MÃ¡y tÃ­nh Casio FX-580VN X', 7, 550000, 680000, 50, N'MÃ¡y tÃ­nh khoa há»c chuáº©n GD', N'Äang bÃ¡n', GETDATE());
SET IDENTITY_INSERT SanPham OFF;

-- 6. PhieuNhap & ChiTietNhapHang
SET IDENTITY_INSERT PhieuNhap ON;
INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, TongTien, TrangThai, NgayNhap) VALUES
(1, 3, 3000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(2, 3, 13000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE());
SET IDENTITY_INSERT PhieuNhap OFF;

SET IDENTITY_INSERT ChiTietNhapHang ON;
INSERT INTO ChiTietNhapHang (MaChiTietNhap, MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES
(1, 1, 1, 1000, 3000), -- Nháº­p 1000 bÃºt ThiÃªn Long
(2, 2, 7, 200, 65000); -- Nháº­p 200 ram giáº¥y Double A
SET IDENTITY_INSERT ChiTietNhapHang OFF;

-- 7. HoaDon (BÃ¡n hÃ ng, Äáº·t hÃ ng, Há»§y Ä‘Æ¡n)
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, PhuongThucThanhToan, TrangThai, LoaiHoaDon, LyDoHuy, NgayTao) VALUES
(1, 3, 2, NULL, 694000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(2, 2, 2, 3, 1360000, N'Chuyá»ƒn khoáº£n', N'Chá» xá»­ lÃ½', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, GETDATE()), 
(3, 1, 2, NULL, 45000, N'Tiá»n máº·t', N'ÄÃ£ há»§y', N'ÄÆ¡n bÃ¡n hÃ ng', N'KhÃ¡ch tháº¥y Ä‘áº¯t nÃªn khÃ´ng mua ná»¯a', GETDATE());
SET IDENTITY_INSERT HoaDon OFF;

-- 8. ChiTietHoaDon
SET IDENTITY_INSERT ChiTietHoaDon ON;
INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
-- ÄÆ¡n 1: Mua láº» mÃ¡y tÃ­nh vÃ  vá»Ÿ
(1, 1, 12, 1, 680000, 680000), 
(2, 1, 4, 1, 9000, 9000),     
(3, 1, 1, 1, 5000, 5000),     

-- ÄÆ¡n 2: Mua sá»‰ giáº¥y in cho cÃ´ng ty
(4, 2, 7, 20, 80000, 1600000), 

-- ÄÆ¡n 3: KhÃ¡ch Ä‘á»‹nh mua sá»• da nhÆ°ng há»§y
(5, 3, 3, 1, 45000, 45000);   
SET IDENTITY_INSERT ChiTietHoaDon OFF;

-- 9. GiaoHang (Cho Ä‘Æ¡n Ä‘áº·t hÃ ng cÃ´ng ty)
SET IDENTITY_INSERT GiaoHang ON;
INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES
(1, 2, N'TÃ²a nhÃ  vÄƒn phÃ²ng Cáº§u Giáº¥y', N'Äang chuáº©n bá»‹ hÃ ng', NULL);
SET IDENTITY_INSERT GiaoHang OFF;

-- 10. TraHang & ChiTietTraHang (Lá»—i sáº£n pháº©m)
SET IDENTITY_INSERT TraHang ON;
INSERT INTO TraHang (MaTraHang, MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, TrangThai, LoaiGiaoDich, NgayTra) VALUES
(1, 1, 2, N'MÃ¡y tÃ­nh phÃ­m báº¥m bá»‹ káº¹t', 680000, N'HoÃ n táº¥t', N'Äá»•i 1:1', GETDATE());
SET IDENTITY_INSERT TraHang OFF;

SET IDENTITY_INSERT ChiTietTraHang ON;
INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang) VALUES
(1, 1, 12, 1, 680000, N'MÃ¡y nguyÃªn há»™p, lá»—i phÃ­m sá»‘ 5');
SET IDENTITY_INSERT ChiTietTraHang OFF;
GO
