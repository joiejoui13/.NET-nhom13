USE CKNet;
GO

-- ========================================================
-- PHẦN 1: XÓA CÁC BẢNG CŨ (Theo thứ tự an toàn)
-- ========================================================
IF OBJECT_ID('ChiTietDoiHang', 'U') IS NOT NULL DROP TABLE ChiTietDoiHang;
IF OBJECT_ID('DoiHang', 'U') IS NOT NULL DROP TABLE DoiHang;
IF OBJECT_ID('ChiTietTraHang', 'U') IS NOT NULL DROP TABLE ChiTietTraHang;
IF OBJECT_ID('TraHang', 'U') IS NOT NULL DROP TABLE TraHang;
IF OBJECT_ID('GiaoHang', 'U') IS NOT NULL DROP TABLE GiaoHang;
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
-- PHẦN 2: TẠO LẠI CẤU TRÚC BẢNG (Bản chốt)
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
    TenSanPham NVARCHAR(100) NOT NULL, -- Đã gộp tên thương hiệu vào đây
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
    LoaiHoaDon NVARCHAR(50) DEFAULT N'Đơn bán hàng', 
    LyDoHuy NVARCHAR(255), -- Cột mới để lưu lý do hủy đơn
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

CREATE TABLE TraHang (
    MaTraHang INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon INT,
    MaNguoiDung INT,
    LyDo NVARCHAR(255),
    TongTienHoan FLOAT,
    TrangThai NVARCHAR(50),
    NgayTra DATETIME,
    LoaiGiaoDich NVARCHAR(50) DEFAULT N'Trả hàng', 
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
GO

-- ========================================================
-- PHẦN 3: INSERT DỮ LIỆU MẪU ĐỒNG BỘ
-- ========================================================

-- 1. DanhMuc
SET IDENTITY_INSERT DanhMuc ON;
INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, TrangThai, NgayTao) VALUES
(1, N'Bút các loại', N'Bút bi, bút chì, bút dạ, bút ký', N'Hoạt động', GETDATE()),
(2, N'Sổ - Vở', N'Vở học sinh, sổ da, sổ tay', N'Hoạt động', GETDATE()),
(3, N'Giấy in - photo', N'Giấy A4, A3, giấy in nhiệt', N'Hoạt động', GETDATE()),
(4, N'Bìa - File hồ sơ', N'Bìa còng, bìa lá, kẹp rút', N'Hoạt động', GETDATE()),
(5, N'Dụng cụ học sinh', N'Thước kẻ, gọt bút chì, compa', N'Hoạt động', GETDATE()),
(6, N'Đồ dùng văn phòng', N'Dập ghim, băng dính, kéo, kẹp bướm', N'Hoạt động', GETDATE()),
(7, N'Máy tính cầm tay', N'Máy tính bỏ túi học sinh, kế toán', N'Hoạt động', GETDATE());
SET IDENTITY_INSERT DanhMuc OFF;

-- 2. KhuyenMai 
SET IDENTITY_INSERT KhuyenMai ON;
INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) VALUES
(1, N'Back to School', 10, '2026-08-01', '2026-09-15', N'Ưu đãi tựu trường', N'Chưa diễn ra'),
(2, N'Sale Giữa Năm', 5, '2026-06-01', '2026-06-30', N'Khuyến mãi tháng 6', N'Chưa diễn ra'),
(3, N'Khách mua sỉ B2B', 15, '2026-01-01', '2026-12-31', N'Dành cho công ty đối tác', N'Đang diễn ra');
SET IDENTITY_INSERT KhuyenMai OFF;

-- 3. KhachHang
SET IDENTITY_INSERT KhachHang ON;
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiaChi, NgayTao) VALUES
(1, N'Trường THPT X', '0911222333', N'Đống Đa, Hà Nội', GETDATE()),
(2, N'Công ty CP ABC', '0988777666', N'Cầu Giấy, Hà Nội', GETDATE()),
(3, N'Nguyễn Văn Học Sinh', '0900111222', N'Thanh Xuân, Hà Nội', GETDATE());
SET IDENTITY_INSERT KhachHang OFF;

-- 4. NguoiDung 
SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) VALUES
(1, N'Quản Lý Cửa Hàng', '0901000111', 'admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoạt động', GETDATE()),
(2, N'Thu Ngân 1', '0901000222', 'thungan@vpp.com', 'hashed_pass', 'SALES', N'Hoạt động', GETDATE()),
(3, N'Thủ Kho 1', '0901000333', 'kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoạt động', GETDATE());
SET IDENTITY_INSERT NguoiDung OFF;

-- 5. SanPham (Gắn kèm Thương Hiệu để Search LIKE)
SET IDENTITY_INSERT SanPham ON;
INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, TrangThai, NgayTao) VALUES
(1, N'Bút bi Thiên Long TL-027 Xanh', 1, 3000, 5000, 1000, N'Bút quốc dân ngòi 0.5mm', N'Đang bán', GETDATE()),
(2, N'Bút dạ quang Deli Macaron', 1, 8000, 12000, 300, N'Bút highlight màu pastel', N'Đang bán', GETDATE()),
(3, N'Bút máy Hồng Hà Nét Hoa', 1, 35000, 45000, 150, N'Bút luyện chữ đẹp', N'Đang bán', GETDATE()),
(4, N'Vở kẻ ngang Hồng Hà 72 trang', 2, 6000, 9000, 800, N'Giấy chống lóa mắt', N'Đang bán', GETDATE()),
(5, N'Vở ô ly Campus 96 trang', 2, 8500, 12000, 500, N'Gáy keo đa lớp siêu bền', N'Đang bán', GETDATE()),
(6, N'Sổ da cao cấp Hải Tiến A5', 2, 45000, 65000, 100, N'Sổ tay doanh nhân bìa đen', N'Đang bán', GETDATE()),
(7, N'Giấy in Double A A4 70gsm', 3, 65000, 80000, 200, N'Lốc 500 tờ giấy Thái', N'Đang bán', GETDATE()),
(8, N'Giấy in Paper One A4 80gsm', 3, 70000, 85000, 150, N'Giấy in định lượng dày dặn', N'Đang bán', GETDATE()),
(9, N'Bìa còng Thiên Long 7cm', 4, 25000, 35000, 120, N'Bìa còng lưu trữ hồ sơ A4', N'Đang bán', GETDATE()),
(10, N'Băng dính trong Deli 5cm', 6, 12000, 18000, 400, N'Cuộn lớn 100 yard', N'Đang bán', GETDATE()),
(11, N'Dập ghim Plus số 10', 6, 28000, 40000, 80, N'Dập ghim Nhật Bản', N'Đang bán', GETDATE()),
(12, N'Máy tính Casio FX-580VN X', 7, 550000, 680000, 50, N'Máy tính khoa học chuẩn GD', N'Đang bán', GETDATE());
SET IDENTITY_INSERT SanPham OFF;

-- 6. PhieuNhap & ChiTietNhapHang
SET IDENTITY_INSERT PhieuNhap ON;
INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, TongTien, TrangThai, NgayNhap) VALUES
(1, 3, 3000000, N'Đã hoàn thành', GETDATE()),
(2, 3, 13000000, N'Đã hoàn thành', GETDATE());
SET IDENTITY_INSERT PhieuNhap OFF;

SET IDENTITY_INSERT ChiTietNhapHang ON;
INSERT INTO ChiTietNhapHang (MaChiTietNhap, MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES
(1, 1, 1, 1000, 3000), -- Nhập 1000 bút Thiên Long
(2, 2, 7, 200, 65000); -- Nhập 200 ram giấy Double A
SET IDENTITY_INSERT ChiTietNhapHang OFF;

-- 7. HoaDon (Bán hàng, Đặt hàng, Hủy đơn)
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, PhuongThucThanhToan, TrangThai, LoaiHoaDon, LyDoHuy, NgayTao) VALUES
(1, 3, 2, NULL, 694000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(2, 2, 2, 3, 1360000, N'Chuyển khoản', N'Chờ xử lý', N'Đơn đặt hàng', NULL, GETDATE()), 
(3, 1, 2, NULL, 45000, N'Tiền mặt', N'Đã hủy', N'Đơn bán hàng', N'Khách thấy đắt nên không mua nữa', GETDATE());
SET IDENTITY_INSERT HoaDon OFF;

-- 8. ChiTietHoaDon
SET IDENTITY_INSERT ChiTietHoaDon ON;
INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
-- Đơn 1: Mua lẻ máy tính và vở
(1, 1, 12, 1, 680000, 680000), 
(2, 1, 4, 1, 9000, 9000),     
(3, 1, 1, 1, 5000, 5000),     

-- Đơn 2: Mua sỉ giấy in cho công ty
(4, 2, 7, 20, 80000, 1600000), 

-- Đơn 3: Khách định mua sổ da nhưng hủy
(5, 3, 3, 1, 45000, 45000);   
SET IDENTITY_INSERT ChiTietHoaDon OFF;

-- 9. GiaoHang (Cho đơn đặt hàng công ty)
SET IDENTITY_INSERT GiaoHang ON;
INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES
(1, 2, N'Tòa nhà văn phòng Cầu Giấy', N'Đang chuẩn bị hàng', NULL);
SET IDENTITY_INSERT GiaoHang OFF;

-- 10. TraHang & ChiTietTraHang (Lỗi sản phẩm)
SET IDENTITY_INSERT TraHang ON;
INSERT INTO TraHang (MaTraHang, MaHoaDon,MaTraHang, MaNguoiDung, LyDo, TongTienHoan, TrangThai, LoaiGiaoDich, NgayTra) VALUES
(1, 1, Null, 2, N'Máy tính phím bấm bị kẹt', 680000, N'Hoàn tất', N'Đổi 1:1', GETDATE());
SET IDENTITY_INSERT TraHang OFF;

SET IDENTITY_INSERT ChiTietTraHang ON;
INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang) VALUES
(1, 1, 12, 1, 680000, N'Máy nguyên hộp, lỗi phím số 5');
SET IDENTITY_INSERT ChiTietTraHang OFF;
GO