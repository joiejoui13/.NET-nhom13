USE CKNet;
GO

-- ========================================================
-- PHẦN 1: XÓA CÁC BẢNG CŨ (Theo thứ tự an toàn)
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
(7, N'Máy tính cầm tay', N'Máy tính bỏ túi học sinh, kế toán', N'Hoạt động', GETDATE()),
(8, N'Bảng - Phụ kiện', N'Bảng từ, nam châm, khăn lau', N'Hoạt động', GETDATE()),
(9, N'Thiết bị văn phòng', N'Chuột, bàn phím, USB', N'Hoạt động', GETDATE()),
(10, N'Quà tặng - Lưu niệm', N'Cúp, huy chương, đồ trang trí', N'Hoạt động', GETDATE());
SET IDENTITY_INSERT DanhMuc OFF;

-- 2. KhuyenMai 
SET IDENTITY_INSERT KhuyenMai ON;
INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayKetThuc, MoTaKhuyenMai, TrangThai) VALUES
(1, N'Back to School', 10, '2026-08-01', '2026-09-15', N'Ưu đãi tựu trường', N'Chưa diễn ra'),
(2, N'Sale Giữa Năm', 5, '2026-06-01', '2026-06-30', N'Khuyến mãi tháng 6', N'Chưa diễn ra'),
(3, N'Khách mua sỉ B2B', 15, '2026-01-01', '2026-12-31', N'Dành cho công ty đối tác', N'Đang diễn ra'),
(4, N'Tri ân Khách hàng', 20, '2026-11-15', '2026-11-25', N'Ngày nhà giáo VN', N'Chưa diễn ra'),
(5, N'Mừng Xuân Mới', 10, '2026-01-10', '2026-02-10', N'Lì xì đầu năm', N'Đã kết thúc'),
(6, N'Khuyến mãi Black Friday', 30, '2026-11-25', '2026-11-30', N'Ngày hội siêu sale', N'Chưa diễn ra'),
(7, N'Tuần lễ Vàng', 8, '2026-07-01', '2026-07-07', N'Giảm giá đầu tháng 7', N'Chưa diễn ra'),
(8, N'Ngày đôi 10/10', 10, '2026-10-10', '2026-10-15', N'Siêu sale tháng 10', N'Chưa diễn ra'),
(9, N'Ưu đãi thành viên mới', 5, '2026-01-01', '2026-12-31', N'Dành cho khách hàng mới', N'Đang diễn ra'),
(10, N'Sale xả kho cuối năm', 50, '2026-12-15', '2026-12-31', N'Thanh lý hàng tồn', N'Chưa diễn ra');
SET IDENTITY_INSERT KhuyenMai OFF;

-- 3. KhachHang
SET IDENTITY_INSERT KhachHang ON;
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiaChi, NgayTao) VALUES
(1, N'Trường THPT Chu Văn An', '0911222333', N'Tây Hồ, Hà Nội', GETDATE()),
(2, N'Công ty CP FPT', '0988777666', N'Cầu Giấy, Hà Nội', GETDATE()),
(3, N'Trần Minh Hoàng', '0900111222', N'Thanh Xuân, Hà Nội', GETDATE()),
(4, N'Nguyễn Thị Lan Anh', '0933444555', N'Đống Đa, Hà Nội', GETDATE()),
(5, N'Công ty TNHH Vạn Phát', '0944555666', N'Hoàn Kiếm, Hà Nội', GETDATE()),
(6, N'Lê Văn Luyện', '0912345678', N'Hai Bà Trưng, Hà Nội', GETDATE()),
(7, N'Phạm Thu Hương', '0987654321', N'Ba Đình, Hà Nội', GETDATE()),
(8, N'Trường Đại học Bách Khoa', '0966777888', N'Hai Bà Trưng, Hà Nội', GETDATE()),
(9, N'Ngân hàng Vietcombank', '0977888999', N'Hoàn Kiếm, Hà Nội', GETDATE()),
(10, N'Đoàn Văn Hậu', '0922333444', N'Nam Từ Liêm, Hà Nội', GETDATE());
SET IDENTITY_INSERT KhachHang OFF;

-- 4. NguoiDung 
SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) VALUES
(1, N'Nguyễn Văn Trưởng', '0901000111', 'admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoạt động', GETDATE()),
(2, N'Trần Thị Bích', '0901000222', 'bich.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoạt động', GETDATE()),
(3, N'Lê Hoàng Phúc', '0901000333', 'phuc.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoạt động', GETDATE()),
(4, N'Hoàng Thanh Mai', '0912000444', 'mai.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoạt động', GETDATE()),
(5, N'Đặng Thái Sơn', '0923000555', 'son.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoạt động', GETDATE()),
(6, N'Phạm Tuấn Hưng', '0934000666', 'hung.admin@vpp.com', 'hashed_pass', 'ADMIN', N'Hoạt động', GETDATE()),
(7, N'Vũ Phương Thảo', '0945000777', 'thao.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoạt động', GETDATE()),
(8, N'Bùi Đức Anh', '0956000888', 'anh.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoạt động', GETDATE()),
(9, N'Hồ Kim Ngân', '0967000999', 'ngan.sales@vpp.com', 'hashed_pass', 'SALES', N'Hoạt động', GETDATE()),
(10, N'Trịnh Xuân Lộc', '0978000000', 'loc.kho@vpp.com', 'hashed_pass', 'WAREHOUSE', N'Hoạt động', GETDATE());
SET IDENTITY_INSERT NguoiDung OFF;

-- 5. SanPham (Gắn kèm Thương Hiệu để Search LIKE)
SET IDENTITY_INSERT SanPham ON;
INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) VALUES
(1, N'Bút bi Thiên Long TL-027 Xanh', 1, 3000, 5000, 1000, N'Bút quốc dân ngòi 0.5mm', N'..\..\..\GUI\Resources\Images\Products\but-bi-xanh.webp', N'Đang bán', GETDATE()),
(2, N'Bút ký mạ vàng sang trọng', 1, 150000, 250000, 50, N'Bút doanh nhân cao cấp', N'..\..\..\GUI\Resources\Images\Products\Bút ký mạ vàng sang trọng.jpg', N'Đang bán', GETDATE()),
(3, N'Bảng viết bút lông có từ tính', 8, 350000, 450000, 30, N'Bảng văn phòng 80x120cm', N'..\..\..\GUI\Resources\Images\Products\Bảng viết bút lông có từ tính.jpg', N'Đang bán', GETDATE()),
(4, N'Chuột văn phòng tiết kiệm pin', 9, 85000, 120000, 100, N'Chuột không dây Logitech', N'..\..\..\GUI\Resources\Images\Products\Chuột văn phòng tiết kiệm pin.png', N'Đang bán', GETDATE()),
(5, N'Dụng cụ bấm kim cỡ nhỏ', 6, 15000, 22000, 200, N'Bấm kim Plus số 10', N'..\..\..\GUI\Resources\Images\Products\Dụng cụ bấm kim cỡ nhỏ.jpg', N'Đang bán', GETDATE()),
(6, N'Sổ bìa da thật dùng đi họp', 2, 85000, 120000, 80, N'Sổ da cao cấp A5', N'..\..\..\GUI\Resources\Images\Products\Sổ bìa da thật dùng đi họp.jpg', N'Đang bán', GETDATE()),
(7, N'Giấy in cao cấp Double A', 3, 65000, 80000, 500, N'Lốc 500 tờ giấy Thái', N'..\..\..\GUI\Resources\Images\Products\Giấy in cao cấp.jpg', N'Đang bán', GETDATE()),
(8, N'Kéo thép không gỉ cỡ lớn', 6, 35000, 50000, 120, N'Kéo cắt giấy chuyên dụng', N'..\..\..\GUI\Resources\Images\Products\Kéo thép không gỉ cỡ lớn.png', N'Đang bán', GETDATE()),
(9, N'Bìa lưu trữ hồ sơ dày 7cm', 4, 25000, 35000, 300, N'Bìa còng Thiên Long', N'..\..\..\GUI\Resources\Images\Products\Bìa lưu trữ hồ sơ dày 7cm.jpg', N'Đang bán', GETDATE()),
(10, N'Kẹp tài liệu kim loại đen', 6, 8000, 12000, 400, N'Kẹp bướm 25mm hộp 12 cái', N'..\..\..\GUI\Resources\Images\Products\Kẹp tài liệu kim loại đen.jpg', N'Đang bán', GETDATE()),
(11, N'Bìa trong mỏng đựng tài liệu', 4, 2000, 3500, 1000, N'Bìa lá A4 Plus', N'..\..\..\GUI\Resources\Images\Products\Bìa trong mỏng đựng tài liệu.jpg', N'Đang bán', GETDATE()),
(12, N'Máy tính khoa học chính hãng', 7, 550000, 680000, 60, N'Máy tính Casio FX-580VN', N'..\..\..\GUI\Resources\Images\Products\Máy tính khoa học chính hãng.webp', N'Đang bán', GETDATE()),
(13, N'Mực châm con dấu công ty', 6, 12000, 18000, 150, N'Mực đỏ Horse 30ml', N'..\..\..\GUI\Resources\Images\Products\Mực châm con dấu công ty.jpg', N'Đang bán', GETDATE()),
(14, N'Giấy nhớ dạ quang 5 màu', 2, 10000, 15000, 400, N'Giấy note deli dạ quang', N'..\..\..\GUI\Resources\Images\Products\giấy nhớ dạ quang.webp', N'Đang bán', GETDATE()),
(15, N'Kệ nhựa đựng tài liệu để bàn', 4, 45000, 65000, 90, N'Kệ 3 tầng ráp mica', N'..\..\..\GUI\Resources\Images\Products\Kệ nhựa đựng tài liệu để bàn.jpg', N'Đang bán', GETDATE()),
(16, N'Bút dạ Thiên Long', 1, 4000, 6000, 250, N'Bút viết bảng trắng', N'..\..\..\GUI\Resources\Images\Products\Bút dạ Thiên Long.jpg', N'Đang bán', GETDATE()),
(17, N'Chuốt bút chì hình thú', 5, 5000, 10000, 150, N'Dụng cụ chuốt bút chì cho học sinh', N'..\..\..\GUI\Resources\Images\Products\chuốt bút chì.jpg', N'Đang bán', GETDATE()),
(18, N'Compa Deli học sinh', 5, 12000, 18000, 100, N'Compa bằng thép không gỉ', N'..\..\..\GUI\Resources\Images\Products\Compa Deli.webp', N'Đang bán', GETDATE()),
(19, N'Dao rọc giấy văn phòng', 6, 8000, 15000, 200, N'Dao rọc giấy an toàn', N'..\..\..\GUI\Resources\Images\Products\Dao rọc giấy.png', N'Đang bán', GETDATE()),
(20, N'Hồ khô Deli dán giấy', 6, 4000, 8000, 300, N'Hồ dán dạng thỏi', N'..\..\..\GUI\Resources\Images\Products\hồ khô.jpg', N'Đang bán', GETDATE()),
(21, N'Hộp bút nhựa Deli', 5, 25000, 40000, 80, N'Hộp đựng bút nhiều ngăn', N'..\..\..\GUI\Resources\Images\Products\hộp bút deli.jpg', N'Đang bán', GETDATE()),
(22, N'Kẹp giấy màu Deli', 6, 5000, 10000, 500, N'Hộp 100 kẹp giấy nhiều màu', N'..\..\..\GUI\Resources\Images\Products\kẹp giấy màu.jpg', N'Đang bán', GETDATE()),
(23, N'Sổ lò xo Hồng Hà A4', 2, 20000, 30000, 120, N'Sổ ghi chép lò xo dọc', N'..\..\..\GUI\Resources\Images\Products\sổ lò xo hồng hà.webp', N'Đang bán', GETDATE()),
(24, N'Tẩy chì kháng khuẩn', 5, 3000, 6000, 400, N'Tẩy cao su kháng khuẩn an toàn', N'..\..\..\GUI\Resources\Images\Products\taychikhoangkhuan.webp', N'Đang bán', GETDATE()),
(25, N'Thước kẻ 20cm', 5, 2500, 5000, 350, N'Thước nhựa trong', N'..\..\..\GUI\Resources\Images\Products\Thước kẻ 20cm.jpg', N'Đang bán', GETDATE()),
(26, N'Ê ke Hồng Hà', 5, 3500, 7000, 200, N'Bộ thước ê ke nhựa', N'..\..\..\GUI\Resources\Images\Products\Ê ke Hồng Hà.webp', N'Đang bán', GETDATE());
SET IDENTITY_INSERT SanPham OFF;


-- 6. PhieuNhap & ChiTietNhapHang
SET IDENTITY_INSERT PhieuNhap ON;
INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, TongTien, TrangThai, NgayNhap) VALUES
(1, 3, 3000000, N'Đã hoàn thành', GETDATE()),
(2, 5, 13000000, N'Đã hoàn thành', GETDATE()),
(3, 8, 8500000, N'Đã hoàn thành', GETDATE()),
(4, 10, 4500000, N'Đang xử lý', GETDATE()),
(5, 3, 12000000, N'Đã hoàn thành', GETDATE()),
(6, 5, 2500000, N'Đã hoàn thành', GETDATE()),
(7, 8, 9000000, N'Đã hoàn thành', GETDATE()),
(8, 10, 3200000, N'Đã hoàn thành', GETDATE()),
(9, 3, 6700000, N'Đã hoàn thành', GETDATE()),
(10, 5, 15000000, N'Chờ duyệt', GETDATE());
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

-- 7. HoaDon (Bán hàng, Đặt hàng, Hủy đơn)
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, PhuongThucThanhToan, TrangThai, LoaiHoaDon, LyDoHuy, NgayTao) VALUES
(1, 3, 2, NULL, 694000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(2, 2, 2, 3, 1360000, N'Chuyển khoản', N'Chờ xử lý', N'Đơn đặt hàng', NULL, GETDATE()), 
(3, 1, 4, NULL, 45000, N'Tiền mặt', N'Đã hủy', N'Đơn bán hàng', N'Khách thấy đắt nên không mua nữa', GETDATE()),
(4, 5, 7, 3, 3500000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, GETDATE()),
(5, 4, 2, NULL, 125000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(6, 8, 9, 1, 850000, N'Thẻ tín dụng', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(7, 9, 2, NULL, 4500000, N'Chuyển khoản', N'Đang giao hàng', N'Đơn đặt hàng', NULL, GETDATE()),
(8, 6, 4, NULL, 50000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(9, 7, 7, NULL, 680000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(10, 10, 9, NULL, 150000, N'Momo', N'Đã hoàn thành', N'Đơn bán hàng', NULL, GETDATE()),
(11, 1, 2, NULL, 50000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-01-15 08:30:00'),
(12, 3, 4, NULL, 250000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-01-22 14:15:00'),
(13, 5, 2, 3, 1200000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, '2026-02-10 09:45:00'),
(14, 2, 7, NULL, 300000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-02-18 16:20:00'),
(15, 8, 9, NULL, 45000, N'Momo', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-03-05 10:10:00'),
(16, 4, 2, NULL, 550000, N'Tiền mặt', N'Đã hủy', N'Đơn bán hàng', N'Khách đổi ý', '2026-03-12 11:30:00'),
(17, 9, 4, NULL, 2200000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, '2026-03-25 13:00:00'),
(18, 7, 7, 1, 750000, N'Thẻ tín dụng', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-04-02 15:45:00'),
(19, 6, 9, NULL, 120000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-04-10 08:50:00'),
(20, 10, 2, NULL, 800000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-04-20 17:15:00'),
(21, 3, 4, NULL, 90000, N'Momo', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-01 09:20:00'),
(22, 1, 7, NULL, 350000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-05 14:10:00'),
(23, 5, 2, 3, 4500000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, '2026-05-10 10:00:00'),
(24, 2, 9, NULL, 60000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-15 16:30:00'),
(25, 8, 2, NULL, 150000, N'Thẻ tín dụng', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-18 11:45:00'),
(26, 4, 4, NULL, 280000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-20 13:20:00'),
(27, 9, 7, NULL, 1800000, N'Chuyển khoản', N'Đã hủy', N'Đơn đặt hàng', N'Thiếu hàng', '2026-05-22 09:10:00'),
(28, 7, 9, NULL, 400000, N'Momo', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-25 15:00:00'),
(29, 6, 2, NULL, 75000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-28 10:30:00'),
(30, 10, 4, 1, 950000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, '2026-05-30 14:00:00');
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
(1, 2, N'Tòa nhà văn phòng Cầu Giấy', N'Đang chuẩn bị hàng', NULL),
(2, 4, N'Tòa nhà Vạn Phát, Hoàn Kiếm', N'Đã giao', GETDATE()),
(3, 7, N'Chi nhánh Vietcombank Hoàn Kiếm', N'Đang giao hàng', NULL);
SET IDENTITY_INSERT GiaoHang OFF;

-- 10. TraHang & ChiTietTraHang 
SET IDENTITY_INSERT TraHang ON;
INSERT INTO TraHang (MaTraHang, MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, TrangThai, LoaiGiaoDich, NgayTra) VALUES
(1, 1, 2, N'Máy tính phím bấm bị kẹt', 680000, N'Hoàn tất', N'Đổi 1:1', GETDATE()),
(2, 4, 7, N'Chuột không nhận tín hiệu', 120000, N'Đang xử lý', N'Trả hàng', GETDATE()),
(3, 6, 9, N'Giao nhầm màu bút bi', 500000, N'Hoàn tất', N'Đổi 1:1', GETDATE());
SET IDENTITY_INSERT TraHang OFF;

SET IDENTITY_INSERT ChiTietTraHang ON;
INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang) VALUES
(1, 1, 12, 1, 680000, N'Máy nguyên hộp, lỗi phím số 5'),
(2, 2, 4, 1, 120000, N'Không lên nguồn'),
(3, 3, 1, 100, 500000, N'Nhầm bút đen thay vì xanh');
SET IDENTITY_INSERT ChiTietTraHang OFF;
GO
