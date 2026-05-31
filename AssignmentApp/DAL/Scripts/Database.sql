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
(7, N'MÃ¡y tÃ­nh cáº§m tay', N'MÃ¡y tÃ­nh bá» tÃºi há»c sinh, káº¿ toÃ¡n', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(8, N'Báº£ng - Phá»¥ kiá»‡n', N'Báº£ng tá»«, nam chÃ¢m, khÄƒn lau', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(9, N'Thiáº¿t bá»‹ vÄƒn phÃ²ng', N'Chuá»™t, bÃ n phÃ­m, USB', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(10, N'QuÃ  táº·ng - LÆ°u niá»‡m', N'CÃºp, huy chÆ°Æ¡ng, Ä‘á»“ trang trÃ­', N'Hoáº¡t Ä‘á»™ng', GETDATE());
SET IDENTITY_INSERT DanhMuc OFF;

-- 2. KhuyenMai 
SET IDENTITY_INSERT KhuyenMai ON;
INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) VALUES
(1, N'Back to School', 10, '2026-08-01', '2026-09-15', N'Æ¯u Ä‘Ã£i tá»±u trÆ°á»ng', N'ChÆ°a diá»…n ra'),
(2, N'Sale Giá»¯a NÄƒm', 5, '2026-06-01', '2026-06-30', N'Khuyáº¿n mÃ£i thÃ¡ng 6', N'ChÆ°a diá»…n ra'),
(3, N'KhÃ¡ch mua sá»‰ B2B', 15, '2026-01-01', '2026-12-31', N'DÃ nh cho cÃ´ng ty Ä‘á»‘i tÃ¡c', N'Äang diá»…n ra'),
(4, N'Tri Ã¢n KhÃ¡ch hÃ ng', 20, '2026-11-15', '2026-11-25', N'NgÃ y nhÃ  giÃ¡o VN', N'ChÆ°a diá»…n ra'),
(5, N'Má»«ng XuÃ¢n Má»›i', 10, '2026-01-10', '2026-02-10', N'LÃ¬ xÃ¬ Ä‘áº§u nÄƒm', N'ÄÃ£ káº¿t thÃºc'),
(6, N'Khuyáº¿n mÃ£i Black Friday', 30, '2026-11-25', '2026-11-30', N'NgÃ y há»™i siÃªu sale', N'ChÆ°a diá»…n ra'),
(7, N'Tuáº§n lá»… VÃ ng', 8, '2026-07-01', '2026-07-07', N'Giáº£m giÃ¡ Ä‘áº§u thÃ¡ng 7', N'ChÆ°a diá»…n ra'),
(8, N'NgÃ y Ä‘Ã´i 10/10', 10, '2026-10-10', '2026-10-15', N'SiÃªu sale thÃ¡ng 10', N'ChÆ°a diá»…n ra'),
(9, N'Æ¯u Ä‘Ã£i thÃ nh viÃªn má»›i', 5, '2026-01-01', '2026-12-31', N'DÃ nh cho khÃ¡ch hÃ ng má»›i', N'Äang diá»…n ra'),
(10, N'Sale xáº£ kho cuá»‘i nÄƒm', 50, '2026-12-15', '2026-12-31', N'Thanh lÃ½ hÃ ng tá»“n', N'ChÆ°a diá»…n ra');
SET IDENTITY_INSERT KhuyenMai OFF;

-- 3. KhachHang
SET IDENTITY_INSERT KhachHang ON;
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiaChi, NgayTao) VALUES
(1, N'TrÆ°á»ng THPT Chu VÄƒn An', '0911222333', N'TÃ¢y Há»“, HÃ  Ná»™i', GETDATE()),
(2, N'CÃ´ng ty CP FPT', '0988777666', N'Cáº§u Giáº¥y, HÃ  Ná»™i', GETDATE()),
(3, N'Tráº§n Minh HoÃ ng', '0900111222', N'Thanh XuÃ¢n, HÃ  Ná»™i', GETDATE()),
(4, N'Nguyá»…n Thá»‹ Lan Anh', '0933444555', N'Äá»‘ng Äa, HÃ  Ná»™i', GETDATE()),
(5, N'CÃ´ng ty TNHH Váº¡n PhÃ¡t', '0944555666', N'HoÃ n Kiáº¿m, HÃ  Ná»™i', GETDATE()),
(6, N'LÃª VÄƒn Luyá»‡n', '0912345678', N'Hai BÃ  TrÆ°ng, HÃ  Ná»™i', GETDATE()),
(7, N'Pháº¡m Thu HÆ°Æ¡ng', '0987654321', N'Ba ÄÃ¬nh, HÃ  Ná»™i', GETDATE()),
(8, N'TrÆ°á»ng Äáº¡i há»c BÃ¡ch Khoa', '0966777888', N'Hai BÃ  TrÆ°ng, HÃ  Ná»™i', GETDATE()),
(9, N'NgÃ¢n hÃ ng Vietcombank', '0977888999', N'HoÃ n Kiáº¿m, HÃ  Ná»™i', GETDATE()),
(10, N'ÄoÃ n VÄƒn Háº­u', '0922333444', N'Nam Tá»« LiÃªm, HÃ  Ná»™i', GETDATE());
SET IDENTITY_INSERT KhachHang OFF;

-- 4. NguoiDung 
SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) VALUES
(1, N'Nguyá»…n VÄƒn TrÆ°á»Ÿng', '0901000111', 'admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(2, N'Tráº§n Thá»‹ BÃ­ch', '0901000222', 'bich.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(3, N'LÃª HoÃ ng PhÃºc', '0901000333', 'phuc.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(4, N'HoÃ ng Thanh Mai', '0912000444', 'mai.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(5, N'Äáº·ng ThÃ¡i SÆ¡n', '0923000555', 'son.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(6, N'Pháº¡m Tuáº¥n HÆ°ng', '0934000666', 'hung.admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(7, N'VÅ© PhÆ°Æ¡ng Tháº£o', '0945000777', 'thao.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(8, N'BÃ¹i Äá»©c Anh', '0956000888', 'anh.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(9, N'Há»“ Kim NgÃ¢n', '0967000999', 'ngan.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoáº¡t Ä‘á»™ng', GETDATE()),
(10, N'Trá»‹nh XuÃ¢n Lá»™c', '0978000000', 'loc.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoáº¡t Ä‘á»™ng', GETDATE());
SET IDENTITY_INSERT NguoiDung OFF;

-- 5. SanPham (Gáº¯n kÃ¨m ThÆ°Æ¡ng Hiá»‡u Ä‘á»ƒ Search LIKE)
SET IDENTITY_INSERT SanPham ON;
INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) VALUES
(1, N'BÃºt bi ThiÃªn Long TL-027 Xanh', 1, 3000, 5000, 1000, N'BÃºt quá»‘c dÃ¢n ngÃ²i 0.5mm', N'..\..\..\GUI\Resources\Images\Products\but-bi-xanh.webp', N'Äang bÃ¡n', GETDATE()),
(2, N'BÃºt kÃ½ máº¡ vÃ ng sang trá»ng', 1, 150000, 250000, 50, N'BÃºt doanh nhÃ¢n cao cáº¥p', N'..\..\..\GUI\Resources\Images\Products\BÃºt kÃ½ máº¡ vÃ ng sang trá»ng.jpg', N'Äang bÃ¡n', GETDATE()),
(3, N'Báº£ng viáº¿t bÃºt lÃ´ng cÃ³ tá»« tÃ­nh', 8, 350000, 450000, 30, N'Báº£ng vÄƒn phÃ²ng 80x120cm', N'..\..\..\GUI\Resources\Images\Products\Báº£ng viáº¿t bÃºt lÃ´ng cÃ³ tá»« tÃ­nh.jpg', N'Äang bÃ¡n', GETDATE()),
(4, N'Chuá»™t vÄƒn phÃ²ng tiáº¿t kiá»‡m pin', 9, 85000, 120000, 100, N'Chuá»™t khÃ´ng dÃ¢y Logitech', N'..\..\..\GUI\Resources\Images\Products\Chuá»™t vÄƒn phÃ²ng tiáº¿t kiá»‡m pin.png', N'Äang bÃ¡n', GETDATE()),
(5, N'Dá»¥ng cá»¥ báº¥m kim cá»¡ nhá»', 6, 15000, 22000, 200, N'Báº¥m kim Plus sá»‘ 10', N'..\..\..\GUI\Resources\Images\Products\Dá»¥ng cá»¥ báº¥m kim cá»¡ nhá».jpg', N'Äang bÃ¡n', GETDATE()),
(6, N'Sá»• bÃ¬a da tháº­t dÃ¹ng Ä‘i há»p', 2, 85000, 120000, 80, N'Sá»• da cao cáº¥p A5', N'..\..\..\GUI\Resources\Images\Products\Sá»• bÃ¬a da tháº­t dÃ¹ng Ä‘i há»p.jpg', N'Äang bÃ¡n', GETDATE()),
(7, N'Giáº¥y in cao cáº¥p Double A', 3, 65000, 80000, 500, N'Lá»‘c 500 tá» giáº¥y ThÃ¡i', N'..\..\..\GUI\Resources\Images\Products\Giáº¥y in cao cáº¥p.jpg', N'Äang bÃ¡n', GETDATE()),
(8, N'KÃ©o thÃ©p khÃ´ng gá»‰ cá»¡ lá»›n', 6, 35000, 50000, 120, N'KÃ©o cáº¯t giáº¥y chuyÃªn dá»¥ng', N'..\..\..\GUI\Resources\Images\Products\KÃ©o thÃ©p khÃ´ng gá»‰ cá»¡ lá»›n.png', N'Äang bÃ¡n', GETDATE()),
(9, N'BÃ¬a lÆ°u trá»¯ há»“ sÆ¡ dÃ y 7cm', 4, 25000, 35000, 300, N'BÃ¬a cÃ²ng ThiÃªn Long', N'..\..\..\GUI\Resources\Images\Products\BÃ¬a lÆ°u trá»¯ há»“ sÆ¡ dÃ y 7cm.jpg', N'Äang bÃ¡n', GETDATE()),
(10, N'Káº¹p tÃ i liá»‡u kim loáº¡i Ä‘en', 6, 8000, 12000, 400, N'Káº¹p bÆ°á»›m 25mm há»™p 12 cÃ¡i', N'..\..\..\GUI\Resources\Images\Products\Káº¹p tÃ i liá»‡u kim loáº¡i Ä‘en.jpg', N'Äang bÃ¡n', GETDATE()),
(11, N'BÃ¬a trong má»ng Ä‘á»±ng tÃ i liá»‡u', 4, 2000, 3500, 1000, N'BÃ¬a lÃ¡ A4 Plus', N'..\..\..\GUI\Resources\Images\Products\BÃ¬a trong má»ng Ä‘á»±ng tÃ i liá»‡u.jpg', N'Äang bÃ¡n', GETDATE()),
(12, N'MÃ¡y tÃ­nh khoa há»c chÃ­nh hÃ£ng', 7, 550000, 680000, 60, N'MÃ¡y tÃ­nh Casio FX-580VN', N'..\..\..\GUI\Resources\Images\Products\MÃ¡y tÃ­nh khoa há»c chÃ­nh hÃ£ng.webp', N'Äang bÃ¡n', GETDATE()),
(13, N'Má»±c chÃ¢m con dáº¥u cÃ´ng ty', 6, 12000, 18000, 150, N'Má»±c Ä‘á» Horse 30ml', N'..\..\..\GUI\Resources\Images\Products\Má»±c chÃ¢m con dáº¥u cÃ´ng ty.jpg', N'Äang bÃ¡n', GETDATE()),
(14, N'Giáº¥y nhá»› dáº¡ quang 5 mÃ u', 2, 10000, 15000, 400, N'Giáº¥y note deli dáº¡ quang', N'..\..\..\GUI\Resources\Images\Products\giáº¥y nhá»› dáº¡ quang.webp', N'Äang bÃ¡n', GETDATE()),
(15, N'Ká»‡ nhá»±a Ä‘á»±ng tÃ i liá»‡u Ä‘á»ƒ bÃ n', 4, 45000, 65000, 90, N'Ká»‡ 3 táº§ng rÃ¡p mica', N'..\..\..\GUI\Resources\Images\Products\Ká»‡ nhá»±a Ä‘á»±ng tÃ i liá»‡u Ä‘á»ƒ bÃ n.jpg', N'Äang bÃ¡n', GETDATE()),
(16, N'BÃºt dáº¡ ThiÃªn Long', 1, 4000, 6000, 250, N'BÃºt viáº¿t báº£ng tráº¯ng', N'..\..\..\GUI\Resources\Images\Products\BÃºt dáº¡ ThiÃªn Long.jpg', N'Äang bÃ¡n', GETDATE()),
(17, N'Chuá»‘t bÃºt chÃ¬ hÃ¬nh thÃº', 5, 5000, 10000, 150, N'Dá»¥ng cá»¥ chuá»‘t bÃºt chÃ¬ cho há»c sinh', N'..\..\..\GUI\Resources\Images\Products\chuá»‘t bÃºt chÃ¬.jpg', N'Äang bÃ¡n', GETDATE()),
(18, N'Compa Deli há»c sinh', 5, 12000, 18000, 100, N'Compa báº±ng thÃ©p khÃ´ng gá»‰', N'..\..\..\GUI\Resources\Images\Products\Compa Deli.webp', N'Äang bÃ¡n', GETDATE()),
(19, N'Dao rá»c giáº¥y vÄƒn phÃ²ng', 6, 8000, 15000, 200, N'Dao rá»c giáº¥y an toÃ n', N'..\..\..\GUI\Resources\Images\Products\Dao rá»c giáº¥y.png', N'Äang bÃ¡n', GETDATE()),
(20, N'Há»“ khÃ´ Deli dÃ¡n giáº¥y', 6, 4000, 8000, 300, N'Há»“ dÃ¡n dáº¡ng thá»i', N'..\..\..\GUI\Resources\Images\Products\há»“ khÃ´.jpg', N'Äang bÃ¡n', GETDATE()),
(21, N'Há»™p bÃºt nhá»±a Deli', 5, 25000, 40000, 80, N'Há»™p Ä‘á»±ng bÃºt nhiá»u ngÄƒn', N'..\..\..\GUI\Resources\Images\Products\há»™p bÃºt deli.jpg', N'Äang bÃ¡n', GETDATE()),
(22, N'Káº¹p giáº¥y mÃ u Deli', 6, 5000, 10000, 500, N'Há»™p 100 káº¹p giáº¥y nhiá»u mÃ u', N'..\..\..\GUI\Resources\Images\Products\káº¹p giáº¥y mÃ u.jpg', N'Äang bÃ¡n', GETDATE()),
(23, N'Sá»• lÃ² xo Há»“ng HÃ  A4', 2, 20000, 30000, 120, N'Sá»• ghi chÃ©p lÃ² xo dá»c', N'..\..\..\GUI\Resources\Images\Products\sá»• lÃ² xo há»“ng hÃ .webp', N'Äang bÃ¡n', GETDATE()),
(24, N'Táº©y chÃ¬ khÃ¡ng khuáº©n', 5, 3000, 6000, 400, N'Táº©y cao su khÃ¡ng khuáº©n an toÃ n', N'..\..\..\GUI\Resources\Images\Products\taychikhoangkhuan.webp', N'Äang bÃ¡n', GETDATE()),
(25, N'ThÆ°á»›c káº» 20cm', 5, 2500, 5000, 350, N'ThÆ°á»›c nhá»±a trong', N'..\..\..\GUI\Resources\Images\Products\ThÆ°á»›c káº» 20cm.jpg', N'Äang bÃ¡n', GETDATE()),
(26, N'ÃŠ ke Há»“ng HÃ ', 5, 3500, 7000, 200, N'Bá»™ thÆ°á»›c Ãª ke nhá»±a', N'..\..\..\GUI\Resources\Images\Products\ÃŠ ke Há»“ng HÃ .webp', N'Äang bÃ¡n', GETDATE());
SET IDENTITY_INSERT SanPham OFF;


-- 6. PhieuNhap & ChiTietNhapHang
SET IDENTITY_INSERT PhieuNhap ON;
INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, TongTien, TrangThai, NgayNhap) VALUES
(1, 3, 3000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(2, 5, 13000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(3, 8, 8500000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(4, 10, 4500000, N'Äang xá»­ lÃ½', GETDATE()),
(5, 3, 12000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(6, 5, 2500000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(7, 8, 9000000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(8, 10, 3200000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(9, 3, 6700000, N'ÄÃ£ hoÃ n thÃ nh', GETDATE()),
(10, 5, 15000000, N'Chá» duyá»‡t', GETDATE());
SET IDENTITY_INSERT PhieuNhap OFF;

SET IDENTITY_INSERT ChiTietNhapHang ON;
INSERT INTO ChiTietNhapHang (MaChiTietNhap, MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES
(1, 1, 1, 1000, 3000), 
(2, 2, 7, 200, 65000),
(3, 3, 12, 10, 550000),
(4, 3, 6, 20, 85000),
(5, 4, 9, 100, 25000),
(6, 4, 11, 500, 2000),
(7, 5, 4, 50, 85000),
(8, 5, 3, 10, 350000),
(9, 6, 14, 200, 10000),
(10, 7, 2, 30, 150000),
(11, 7, 8, 50, 35000),
(12, 8, 5, 100, 15000),
(13, 8, 10, 200, 8000),
(14, 9, 13, 100, 12000),
(15, 9, 15, 50, 45000);
SET IDENTITY_INSERT ChiTietNhapHang OFF;

-- 7. HoaDon (BÃ¡n hÃ ng, Äáº·t hÃ ng, Há»§y Ä‘Æ¡n)
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, PhuongThucThanhToan, TrangThai, LoaiHoaDon, LyDoHuy, NgayTao) VALUES
(1, 3, 2, NULL, 694000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(2, 2, 2, 3, 1360000, N'Chuyá»ƒn khoáº£n', N'Chá» xá»­ lÃ½', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, GETDATE()), 
(3, 1, 4, NULL, 45000, N'Tiá»n máº·t', N'ÄÃ£ há»§y', N'ÄÆ¡n bÃ¡n hÃ ng', N'KhÃ¡ch tháº¥y Ä‘áº¯t nÃªn khÃ´ng mua ná»¯a', GETDATE()),
(4, 5, 7, 3, 3500000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, GETDATE()),
(5, 4, 2, NULL, 125000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(6, 8, 9, 1, 850000, N'Tháº» tÃ­n dá»¥ng', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(7, 9, 2, NULL, 4500000, N'Chuyá»ƒn khoáº£n', N'Äang giao hÃ ng', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, GETDATE()),
(8, 6, 4, NULL, 50000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(9, 7, 7, NULL, 680000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(10, 10, 9, NULL, 150000, N'Momo', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, GETDATE()),
(11, 1, 2, NULL, 50000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-01-15 08:30:00'),
(12, 3, 4, NULL, 250000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-01-22 14:15:00'),
(13, 5, 2, 3, 1200000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, '2026-02-10 09:45:00'),
(14, 2, 7, NULL, 300000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-02-18 16:20:00'),
(15, 8, 9, NULL, 45000, N'Momo', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-03-05 10:10:00'),
(16, 4, 2, NULL, 550000, N'Tiá»n máº·t', N'ÄÃ£ há»§y', N'ÄÆ¡n bÃ¡n hÃ ng', N'KhÃ¡ch Ä‘á»•i Ã½', '2026-03-12 11:30:00'),
(17, 9, 4, NULL, 2200000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, '2026-03-25 13:00:00'),
(18, 7, 7, 1, 750000, N'Tháº» tÃ­n dá»¥ng', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-04-02 15:45:00'),
(19, 6, 9, NULL, 120000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-04-10 08:50:00'),
(20, 10, 2, NULL, 800000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-04-20 17:15:00'),
(21, 3, 4, NULL, 90000, N'Momo', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-01 09:20:00'),
(22, 1, 7, NULL, 350000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-05 14:10:00'),
(23, 5, 2, 3, 4500000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n Ä‘áº·t hÃ ng', NULL, '2026-05-10 10:00:00'),
(24, 2, 9, NULL, 60000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-15 16:30:00'),
(25, 8, 2, NULL, 150000, N'Tháº» tÃ­n dá»¥ng', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-18 11:45:00'),
(26, 4, 4, NULL, 280000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-20 13:20:00'),
(27, 9, 7, NULL, 1800000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ há»§y', N'ÄÆ¡n Ä‘áº·t hÃ ng', N'Thiáº¿u hÃ ng', '2026-05-22 09:10:00'),
(28, 7, 9, NULL, 400000, N'Momo', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-25 15:00:00'),
(29, 6, 2, NULL, 75000, N'Tiá»n máº·t', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-28 10:30:00'),
(30, 10, 4, 1, 950000, N'Chuyá»ƒn khoáº£n', N'ÄÃ£ hoÃ n thÃ nh', N'ÄÆ¡n bÃ¡n hÃ ng', NULL, '2026-05-30 14:00:00');
SET IDENTITY_INSERT HoaDon OFF;

-- 8. ChiTietHoaDon
SET IDENTITY_INSERT ChiTietHoaDon ON;
INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
(1, 1, 12, 1, 680000, 680000), 
(2, 1, 14, 1, 15000, 15000),     
(3, 1, 1, 1, 5000, 5000),     
(4, 2, 7, 20, 80000, 1600000), 
(5, 3, 6, 1, 120000, 120000),   
(6, 4, 7, 50, 80000, 4000000), 
(7, 4, 4, 5, 120000, 600000),
(8, 5, 2, 1, 250000, 250000),
(9, 6, 1, 100, 5000, 500000),
(10, 6, 11, 100, 3500, 350000),
(11, 7, 3, 10, 450000, 4500000),
(12, 8, 8, 1, 50000, 50000),
(13, 9, 12, 1, 680000, 680000),
(14, 10, 6, 1, 120000, 120000),
(15, 10, 9, 1, 35000, 35000),
(16, 11, 1, 10, 5000, 50000),
(17, 12, 2, 1, 250000, 250000),
(18, 13, 7, 15, 80000, 1200000),
(19, 14, 6, 2, 120000, 240000),
(20, 14, 25, 12, 5000, 60000),
(21, 15, 14, 3, 15000, 45000),
(22, 16, 12, 1, 550000, 550000),
(23, 17, 3, 4, 450000, 1800000),
(24, 17, 12, 1, 400000, 400000),
(25, 18, 2, 3, 250000, 750000),
(26, 19, 6, 1, 120000, 120000),
(27, 20, 7, 10, 80000, 800000),
(28, 21, 14, 6, 15000, 90000),
(29, 22, 21, 5, 40000, 200000),
(30, 22, 18, 5, 18000, 90000),
(31, 22, 20, 7, 8000, 56000),
(32, 23, 12, 5, 680000, 3400000),
(33, 23, 7, 10, 80000, 800000),
(34, 23, 4, 3, 100000, 300000),
(35, 24, 17, 6, 10000, 60000),
(36, 25, 23, 5, 30000, 150000),
(37, 26, 26, 40, 7000, 280000),
(38, 27, 12, 2, 680000, 1360000),
(39, 27, 2, 2, 220000, 440000),
(40, 28, 6, 2, 120000, 240000),
(41, 28, 7, 2, 80000, 160000),
(42, 29, 14, 5, 15000, 75000),
(43, 30, 12, 1, 680000, 680000),
(44, 30, 23, 9, 30000, 270000);
SET IDENTITY_INSERT ChiTietHoaDon OFF;

-- 9. GiaoHang 
SET IDENTITY_INSERT GiaoHang ON;
INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES
(1, 2, N'TÃ²a nhÃ  vÄƒn phÃ²ng Cáº§u Giáº¥y', N'Äang chuáº©n bá»‹ hÃ ng', NULL),
(2, 4, N'TÃ²a nhÃ  Váº¡n PhÃ¡t, HoÃ n Kiáº¿m', N'ÄÃ£ giao', GETDATE()),
(3, 7, N'Chi nhÃ¡nh Vietcombank HoÃ n Kiáº¿m', N'Äang giao hÃ ng', NULL);
SET IDENTITY_INSERT GiaoHang OFF;

-- 10. TraHang & ChiTietTraHang 
SET IDENTITY_INSERT TraHang ON;
INSERT INTO TraHang (MaTraHang, MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, TrangThai, LoaiGiaoDich, NgayTra) VALUES
(1, 1, 2, N'MÃ¡y tÃ­nh phÃ­m báº¥m bá»‹ káº¹t', 680000, N'HoÃ n táº¥t', N'Äá»•i 1:1', GETDATE()),
(2, 4, 7, N'Chuá»™t khÃ´ng nháº­n tÃ­n hiá»‡u', 120000, N'Äang xá»­ lÃ½', N'Tráº£ hÃ ng', GETDATE()),
(3, 6, 9, N'Giao nháº§m mÃ u bÃºt bi', 500000, N'HoÃ n táº¥t', N'Äá»•i 1:1', GETDATE());
SET IDENTITY_INSERT TraHang OFF;

SET IDENTITY_INSERT ChiTietTraHang ON;
INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang) VALUES
(1, 1, 12, 1, 680000, N'MÃ¡y nguyÃªn há»™p, lá»—i phÃ­m sá»‘ 5'),
(2, 2, 4, 1, 120000, N'KhÃ´ng lÃªn nguá»“n'),
(3, 3, 1, 100, 500000, N'Nháº§m bÃºt Ä‘en thay vÃ¬ xanh');
SET IDENTITY_INSERT ChiTietTraHang OFF;
GO

