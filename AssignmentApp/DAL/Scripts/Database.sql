Create Database CKNet
go
use CKNet
go
-- 1. Bảng Danh Mục
CREATE TABLE DanhMuc (
    MaDanhMuc VARCHAR(50) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE()
);

-- 2. Bảng Khuyến Mãi
CREATE TABLE KhuyenMai (
    MaKhuyenMai VARCHAR(50) PRIMARY KEY,
    TenKhuyenMai NVARCHAR(100) NOT NULL,
    PhanTramGiamGia INT,
    NgayBatDau DATETIME,
    NgayHetHan DATETIME,
    MoTaKhuyenMai NVARCHAR(255),
    TrangThai NVARCHAR(50)
);

-- 3. Bảng Khách Hàng
CREATE TABLE KhachHang (
    MaKhachHang VARCHAR(50) PRIMARY KEY,
    TenKhachHang NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(20),
    DiemTichLuy INT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE()
);

-- 4. Bảng Người Dùng (Giới hạn 3 vai trò: ADMIN, SALES, WAREHOUSE)
CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(50) PRIMARY KEY,
    TenNguoiDung NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(20),
    Email VARCHAR(100),
    MatKhau VARCHAR(255) NOT NULL,
    VaiTro VARCHAR(20) NOT NULL, 
    TrangThai NVARCHAR(50),
    NgayTao DATETIME DEFAULT GETDATE(),
    CONSTRAINT CK_NguoiDung_VaiTro CHECK (VaiTro IN ('ADMIN', 'SALES', 'WAREHOUSE'))
);

-- 5. Bảng Sản Phẩm
CREATE TABLE SanPham (
    MaSanPham VARCHAR(50) PRIMARY KEY,
    TenSanPham NVARCHAR(100) NOT NULL,
    MaDanhMuc VARCHAR(50),
    GiaBan DECIMAL(18, 2),
    GiaNhap DECIMAL(18, 2),
    SoLuongTon INT DEFAULT 0,
    MoTa NVARCHAR(255),
    Anh NVARCHAR(255),
    TrangThai NVARCHAR(50),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc)
);

-- 6. Bảng Lịch Sử Tồn Kho
CREATE TABLE LichSuTonKho (
    MaLichSu VARCHAR(50) PRIMARY KEY,
    MaSanPham VARCHAR(50),
    SoLuongThayDoi INT,
    Loai NVARCHAR(50),
    Ngay DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

-- 7. Bảng Phiếu Nhập
CREATE TABLE PhieuNhap (
    MaPhieuNhap VARCHAR(50) PRIMARY KEY,
    MaNguoiDung VARCHAR(50),
    NgayNhap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

-- 8. Bảng Chi Tiết Phiếu Nhập
CREATE TABLE ChiTietPhieuNhap (
    MaChiTietPhieuNhap VARCHAR(50) PRIMARY KEY,
    MaPhieuNhap VARCHAR(50),
    MaSanPham VARCHAR(50),
    SoLuong INT,
    GiaNhap DECIMAL(18, 2),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

-- 9. Bảng Hóa Đơn
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(50) PRIMARY KEY,
    MaKhachHang VARCHAR(50),
    MaNguoiDung VARCHAR(50),
    MaKhuyenMai VARCHAR(50),
    TongTien DECIMAL(18, 2),
    GiamGia DECIMAL(18, 2),
    MaGiaoHang VARCHAR(50), 
    HinhThucThanhToan NVARCHAR(50),
    TrangThai NVARCHAR(50),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai)
);

-- 10. Bảng Chi Tiết Hóa Đơn
CREATE TABLE ChiTietHoaDon (
    MaChiTiet VARCHAR(50) PRIMARY KEY,
    MaHoaDon VARCHAR(50),
    MaSanPham VARCHAR(50),
    SoLuong INT,
    DonGia DECIMAL(18, 2),
    ThanhTien DECIMAL(18, 2),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

-- 11. Bảng Giao Hàng
CREATE TABLE GiaoHang (
    MaGiaoHang VARCHAR(50) PRIMARY KEY,
    MaHoaDon VARCHAR(50),
    DiaChiGiao NVARCHAR(255),
    TrangThaiGiao NVARCHAR(50),
    NgayGiao DATETIME,
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon)
);

-- 12. Bảng Trả Hàng
CREATE TABLE TraHang (
    MaTraHang VARCHAR(50) PRIMARY KEY,
    MaHoaDon VARCHAR(50),
    NgayTra DATETIME DEFAULT GETDATE(),
    LyDo NVARCHAR(255),
    TongTienHoan DECIMAL(18, 2),
    MaNguoiDung VARCHAR(50),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

-- 13. Bảng Chi Tiết Trả Hàng
CREATE TABLE ChiTietTraHang (
    MaChiTietTra VARCHAR(50) PRIMARY KEY,
    MaTraHang VARCHAR(50),
    MaSanPham VARCHAR(50),
    SoLuong INT,
    TienHoan DECIMAL(18, 2),
    FOREIGN KEY (MaTraHang) REFERENCES TraHang(MaTraHang),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

-- 14. Bảng Đổi Hàng
CREATE TABLE DoiHang (
    MaDoiHang VARCHAR(50) PRIMARY KEY,
    MaHoaDon VARCHAR(50),
    NgayDoi DATETIME DEFAULT GETDATE(),
    MaNguoiDung VARCHAR(50),
    LyDo NVARCHAR(255),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

-- 15. Bảng Chi Tiết Đổi Hàng
CREATE TABLE ChiTietDoiHang (
    MaChiTietDoi VARCHAR(50) PRIMARY KEY,
    MaDoiHang VARCHAR(50),
    MaSanPhamCu VARCHAR(50),
    MaSanPhamMoi VARCHAR(50),
    SoLuong INT,
    ChenhLechGia DECIMAL(18, 2),
    FOREIGN KEY (MaDoiHang) REFERENCES DoiHang(MaDoiHang),
    FOREIGN KEY (MaSanPhamCu) REFERENCES SanPham(MaSanPham),
    FOREIGN KEY (MaSanPhamMoi) REFERENCES SanPham(MaSanPham)
);
Go

-- ========================================================
-- 1. DỮ LIỆU BẢNG: DanhMuc (15 dòng)
-- ========================================================
INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, NgayTao) VALUES
('DM001', N'Bút các loại', N'Bút bi, bút máy, bút ký', '2026-01-10'),
('DM002', N'Giấy in & Phân trang', N'Giấy A4, A3, giấy note', '2026-01-11'),
('DM003', N'Bìa hồ sơ', N'Bìa còng, bìa lá, bìa trình ký', '2026-01-12'),
('DM004', N'Sổ tay & Tập', N'Sổ da, sổ lò xo, tập học sinh', '2026-01-13'),
('DM005', N'Dụng cụ cắt dán', N'Kéo, dao rọc giấy, hồ dán, băng keo', '2026-01-14'),
('DM006', N'Bấm kim & Kim bấm', N'Dụng cụ bấm kim các loại', '2026-01-15'),
('DM007', N'Dấu mộc & Mực', N'Mực in, mực dấu, con dấu', '2026-01-16'),
('DM008', N'Kẹp ghim & Kẹp bướm', N'Kẹp tài liệu các cỡ', '2026-01-17'),
('DM009', N'Thiết bị máy tính', N'Chuột, bàn phím văn phòng', '2026-01-18'),
('DM010', N'Máy tính bỏ túi', N'Máy tính Casio, Vinacal', '2026-01-19'),
('DM011', N'Bảng văn phòng', N'Bảng từ trắng, bảng ghim', '2026-01-20'),
('DM012', N'Khay kệ rổ mica', N'Khay đựng tài liệu để bàn', '2026-01-21'),
('DM013', N'Bảng tên & Dây đeo', N'Thẻ nhân viên, dây đeo thẻ', '2026-01-22'),
('DM014', N'Vệ sinh văn phòng', N'Xịt phòng, nước lau sàn', '2026-01-23'),
('DM015', N'Quà tặng văn phòng', N'Quà tặng doanh nghiệp', '2026-01-24');

-- ========================================================
-- 2. DỮ LIỆU BẢNG: KhuyenMai (15 dòng)
-- ========================================================
INSERT INTO KhuyenMai (MaKhuyenMai, TenKhuyenMai, PhanTramGiamGia, NgayBatDau, NgayHetHan, MoTaKhuyenMai, TrangThai) VALUES
('KM001', N'Khuyến mãi Tết', 10, '2026-02-01', '2026-02-15', N'Giảm giá dịp Tết Nguyên Đán', N'Hết hạn'),
('KM002', N'Valentine Day', 14, '2026-02-13', '2026-02-15', N'Ưu đãi ngày lễ tình nhân', N'Hết hạn'),
('KM003', N'Quốc tế Phụ nữ', 8, '2026-03-05', '2026-03-10', N'Giảm giá cho khách hàng nữ', N'Hết hạn'),
('KM004', N'Chào hè rực rỡ', 15, '2026-05-01', '2026-05-31', N'Săn deal giải nhiệt mùa hè', N'Đang diễn ra'),
('KM005', N'Sinh nhật cửa hàng', 20, '2026-06-01', '2026-06-05', N'Mừng cửa hàng tròn 5 tuổi', N'Chưa diễn ra'),
('KM006', N'Ngày đôi 6/6', 6, '2026-06-06', '2026-06-06', N'Siêu sale giữa năm', N'Chưa diễn ra'),
('KM007', N'Back to School', 12, '2026-08-15', '2026-09-05', N'Ưu đãi cho học sinh sinh viên', N'Chưa diễn ra'),
('KM008', N'Trung thu đoàn viên', 5, '2026-09-20', '2026-09-25', N'Giảm giá thiết bị gia dụng', N'Chưa diễn ra'),
('KM009', N'Ngày đôi 10/10', 10, '2026-10-10', '2026-10-10', N'Lễ hội mua sắm tháng 10', N'Chưa diễn ra'),
('KM010', N'Black Friday', 30, '2026-11-25', '2026-11-28', N'Xả kho lớn nhất năm', N'Chưa diễn ra'),
('KM011', N'Giáng sinh an lành', 15, '2026-12-20', '2026-12-25', N'Quà tặng mùa Noel', N'Chưa diễn ra'),
('KM012', N'Chào năm mới', 25, '2026-12-30', '2027-01-03', N'Đón chào năm mới rực rỡ', N'Chưa diễn ra'),
('KM013', N'Tri ân khách VIP', 18, '2026-05-10', '2026-05-20', N'Dành riêng cho hạng Kim Cương', N'Đang diễn ra'),
('KM014', N'Xả kho công nghệ', 40, '2026-05-15', '2026-05-25', N'Dọn kho các mẫu cũ', N'Đang diễn ra'),
('KM015', N'Mid-Night Sale', 5, '2026-05-18', '2026-05-19', N'Giảm giá mua đêm từ 12h-2h', N'Đang diễn ra');

-- ========================================================
-- 3. DỮ LIỆU BẢNG: KhachHang (15 dòng)
-- ========================================================
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, DiemTichLuy, NgayTao) VALUES
('KH001', N'Nguyễn Văn A', '0912345671', 120, '2026-01-05'),
('KH002', N'Trần Thị B', '0912345672', 50, '2026-01-12'),
('KH003', N'Lê Hoàng C', '0912345673', 340, '2026-01-20'),
('KH004', N'Phạm Minh D', '0912345674', 0, '2026-02-02'),
('KH005', N'Hoàng Lệ E', '0912345675', 85, '2026-02-15'),
('KH006', N'Vũ Văn F', '0912345676', 520, '2026-03-01'),
('KH007', N'Đặng Thu G', '0912345677', 15, '2026-03-14'),
('KH008', N'Bùi Tiến H', '0912345678', 190, '2026-03-25'),
('KH009', N'Đỗ Minh I', '0912345679', 45, '2026-04-02'),
('KH010', N'Ngô Cao K', '0912345680', 710, '2026-04-10'),
('KH011', N'Dương Thúy L', '0912345681', 0, '2026-04-18'),
('KH012', N'Lý Văn M', '0912345682', 110, '2026-04-29'),
('KH013', N'Phan Hồng N', '0912345683', 25, '2026-05-02'),
('KH014', N'Tô Anh O', '0912345684', 1300, '2026-05-10'),
('KH015', N'Trịnh Xuân P', '0912345685', 95, '2026-05-16');

-- ========================================================
-- 4. DỮ LIỆU BẢNG: NguoiDung (15 dòng - CHUẨN VAI TRÒ)
-- ========================================================
INSERT INTO NguoiDung (MaNguoiDung, TenNguoiDung, SoDienThoai, Email, MatKhau, VaiTro, TrangThai, NgayTao) VALUES
('ND001', N'Lê Quản Trị', '0901111221', 'admin@store.com', 'pass123', 'ADMIN', N'Hoạt động', '2026-01-01'),
('ND002', N'Nguyễn Thu Ngân 1', '0901111222', 'ngan1@store.com', 'pass123', 'SALES', N'Hoạt động', '2026-01-02'),
('ND003', N'Trần Thủ Kho 1', '0901111223', 'kho1@store.com', 'pass123', 'WAREHOUSE', N'Hoạt động', '2026-01-02'),
('ND004', N'Phạm Bán Hàng 1', '0901111224', 'sales1@store.com', 'pass123', 'SALES', N'Hoạt động', '2026-01-03'),
('ND005', N'Hoàng Thu Ngân 2', '0901111225', 'ngan2@store.com', 'pass123', 'SALES', N'Hoạt động', '2026-01-15'),
('ND006', N'Vũ Thủ Kho 2', '0901111226', 'kho2@store.com', 'pass123', 'WAREHOUSE', N'Hoạt động', '2026-01-15'),
('ND007', N'Đặng Bán Hàng 2', '0901111227', 'sales2@store.com', 'pass123', 'SALES', N'Nghỉ việc', '2026-01-16'),
('ND008', N'Bùi Chăm Sóc KH', '0901111228', 'cskh@store.com', 'pass123', 'SALES', N'Hoạt động', '2026-02-01'),
('ND009', N'Đỗ Kế Toán Trưởng', '0901111229', 'ketoan@store.com', 'pass123', 'ADMIN', N'Hoạt động', '2026-02-01'),
('ND010', N'Ngô Điều Phối Giao', '0901111230', 'shipper1@store.com', 'pass123', 'WAREHOUSE', N'Hoạt động', '2026-02-10'),
('ND011', N'Dương Kiểm Kho', '0901111231', 'shipper2@store.com', 'pass123', 'WAREHOUSE', N'Hoạt động', '2026-02-10'),
('ND012', N'Lý Giám Đốc Điều Hành', '0901111232', 'manager@store.com', 'pass123', 'ADMIN', N'Hoạt động', '2026-03-01'),
('ND013', N'Phan Bán Hàng 3', '0901111233', 'sales3@store.com', 'pass123', 'SALES', N'Hoạt động', '2026-03-15'),
('ND014', N'Tô Thu Ngân 3', '0901111234', 'ngan3@store.com', 'pass123', 'SALES', N'Tạm khóa', '2026-04-01'),
('ND015', N'Trịnh Kỹ Thuật Kho', '0901111235', 'tech@store.com', 'pass123', 'WAREHOUSE', N'Hoạt động', '2026-04-10');
-- ========================================================
-- Mã hóa hàm băm mật khẩu
-- ========================================================
UPDATE NguoiDung
SET MatKhau = '$2a$11$/8NjLZAiQlZSczFphButv.5yu4tp3LO5mFPCQUcxLmygJWfTcCYUO';
-- ========================================================
-- 5. DỮ LIỆU BẢNG: SanPham (15 dòng)
-- ========================================================
INSERT INTO SanPham (MaSanPham, TenSanPham, MaDanhMuc, GiaBan, GiaNhap, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) VALUES
('SP001', N'Bút bi Thiên Long TL-027', 'DM001', 5000, 3500, 500, N'Bút bi phổ thông mực xanh', 'but-bi-xanh.webp', N'Đang bán', '2026-01-15'),
('SP002', N'Bút ký cao cấp Parker', 'DM001', 550000, 450000, 30, N'Bút ký mạ vàng sang trọng', 'Bút ký mạ vàng sang trọng.jpg', N'Đang bán', '2026-01-16'),
('SP003', N'Giấy in Double A A4 70gsm', 'DM002', 65000, 55000, 200, N'Giấy in cao cấp, thùng 5 gram', 'Giấy in cao cấp.jpg', N'Đang bán', '2026-01-17'),
('SP004', N'Giấy note vàng 3x3', 'DM002', 12000, 8000, 150, N'Giấy nhớ dạ quang', 'giấy nhớ dạ quang.webp', N'Đang bán', '2026-01-18'),
('SP005', N'Bìa còng Thiên Long 7F', 'DM003', 35000, 25000, 100, N'Bìa lưu trữ hồ sơ dày 7cm', 'Bìa lưu trữ hồ sơ dày 7cm.jpg', N'Đang bán', '2026-01-19'),
('SP006', N'Bìa lá nhựa A4', 'DM003', 2000, 1200, 1000, N'Bìa trong mỏng đựng tài liệu', 'Bìa trong mỏng đựng tài liệu.jpg', N'Đang bán', '2026-01-20'),
('SP007', N'Sổ da cao cấp A5', 'DM004', 120000, 85000, 50, N'Sổ bìa da thật dùng đi họp', 'Sổ bìa da thật dùng đi họp.jpg', N'Đang bán', '2026-01-21'),
('SP008', N'Kéo cắt giấy văn phòng lớn', 'DM005', 45000, 30000, 80, N'Kéo thép không gỉ cỡ lớn', 'Kéo thép không gỉ cỡ lớn.png', N'Đang bán', '2026-01-22'),
('SP009', N'Bấm kim Plus số 10', 'DM006', 25000, 18000, 120, N'Dụng cụ bấm kim cỡ nhỏ', 'Dụng cụ bấm kim cỡ nhỏ.jpg', N'Đang bán', '2026-01-23'),
('SP010', N'Mực dấu đỏ Trodat', 'DM007', 30000, 22000, 60, N'Mực châm con dấu công ty', 'Mực châm con dấu công ty.jpg', N'Đang bán', '2026-01-24'),
('SP011', N'Kẹp bướm 25mm (Hộp 12 cái)', 'DM008', 15000, 10000, 200, N'Kẹp tài liệu kim loại đen', 'Kẹp tài liệu kim loại đen.jpg', N'Đang bán', '2026-01-25'),
('SP012', N'Chuột không dây Logitech M170', 'DM009', 180000, 130000, 40, N'Chuột văn phòng tiết kiệm pin', 'Chuột văn phòng tiết kiệm pin.png', N'Đang bán', '2026-01-26'),
('SP013', N'Máy tính Casio FX-580VNX', 'DM010', 650000, 550000, 25, N'Máy tính khoa học chính hãng', 'Máy tính khoa học chính hãng.webp', N'Đang bán', '2026-01-27'),
('SP014', N'Bảng từ trắng 1.2 x 2.4m', 'DM011', 1200000, 950000, 10, N'Bảng viết bút lông có từ tính', 'Bảng viết bút lông có từ tính.jpg', N'Đang bán', '2026-01-28'),
('SP015', N'Khay mica rổ 3 tầng', 'DM012', 85000, 65000, 70, N'Kệ nhựa đựng tài liệu để bàn', 'Kệ nhựa đựng tài liệu để bàn.jpg', N'Đang bán', '2026-01-29');

-- ========================================================
-- 6. DỮ LIỆU BẢNG: LichSuTonKho (15 dòng)
-- ========================================================
INSERT INTO LichSuTonKho (MaLichSu, MaSanPham, SoLuongThayDoi, Loai, Ngay) VALUES
('LS001', 'SP001', 50, N'Nhập kho', '2026-01-15'),
('LS002', 'SP002', 35, N'Nhập kho', '2026-01-16'),
('LS003', 'SP001', -2, N'Xuất kho bán', '2026-02-02'),
('LS004', 'SP003', 20, N'Nhập kho', '2026-01-17'),
('LS005', 'SP006', 150, N'Nhập kho', '2026-01-20'),
('LS006', 'SP006', -10, N'Xuất kho bán', '2026-02-05'),
('LS007', 'SP010', 20, N'Nhập kho', '2026-01-24'),
('LS008', 'SP014', 10, N'Nhập kho', '2026-01-28'),
('LS009', 'SP014', -3, N'Xuất hủy hàng lỗi', '2026-03-10'),
('LS010', 'SP005', 25, N'Nhập kho', '2026-01-19'),
('LS011', 'SP002', -5, N'Xuất kho bán', '2026-02-20'),
('LS012', 'SP008', 30, N'Nhập kho', '2026-01-22'),
('LS013', 'SP015', 25, N'Nhập kho', '2026-01-29'),
('LS014', 'SP015', -3, N'Xuất kho bán', '2026-04-05'),
('LS015', 'SP011', 15, N'Nhập kho', '2026-01-25');

-- ========================================================
-- 7. DỮ LIỆU BẢNG: PhieuNhap (15 dòng)
-- ========================================================
INSERT INTO PhieuNhap (MaPhieuNhap, MaNguoiDung, NgayNhap, TongTien) VALUES
('PN001', 'ND003', '2026-01-15', 1300000000),
('PN002', 'ND003', '2026-01-16', 840000000),
('PN003', 'ND003', '2026-01-17', 440000000),
('PN004', 'ND003', '2026-01-18', 280000000),
('PN005', 'ND003', '2026-01-19', 437500000),
('PN006', 'ND006', '2026-01-20', 52500000),
('PN007', 'ND006', '2026-01-21', 222000000),
('PN008', 'ND006', '2026-01-22', 57000000),
('PN009', 'ND006', '2026-01-23', 36000000),
('PN010', 'ND006', '2026-01-24', 120000000),
('PN011', 'ND003', '2026-01-25', 40500000),
('PN012', 'ND003', '2026-01-26', 78400000),
('PN013', 'ND003', '2026-01-27', 60000000),
('PN014', 'ND006', '2026-01-28', 65000000),
('PN015', 'ND006', '2026-01-29', 220000000);

-- ========================================================
-- 8. DỮ LIỆU BẢNG: ChiTietPhieuNhap (15 dòng)
-- ========================================================
INSERT INTO ChiTietPhieuNhap (MaChiTietPhieuNhap, MaPhieuNhap, MaSanPham, SoLuong, GiaNhap) VALUES
('CTPN001', 'PN001', 'SP001', 50, 26000000),
('CTPN002', 'PN002', 'SP002', 35, 24000000),
('CTPN003', 'PN003', 'SP003', 20, 22000000),
('CTPN004', 'PN004', 'SP004', 10, 28000000),
('CTPN005', 'PN005', 'SP005', 25, 17500000),
('CTPN006', 'PN006', 'SP006', 150, 350000),
('CTPN007', 'PN007', 'SP007', 12, 18500000),
('CTPN008', 'PN008', 'SP008', 30, 1900000),
('CTPN009', 'PN009', 'SP009', 80, 450000),
('CTPN010', 'PN010', 'SP010', 20, 6000000),
('CTPN011', 'PN011', 'SP011', 15, 2700000),
('CTPN012', 'PN012', 'SP012', 8, 9800000),
('CTPN013', 'PN013', 'SP013', 5, 12000000),
('CTPN014', 'PN014', 'SP014', 10, 6500000),
('CTPN015', 'PN015', 'SP015', 25, 8800000);

-- ========================================================
-- 9. DỮ LIỆU BẢNG: HoaDon (15 dòng)
-- ========================================================
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, GiamGia, MaGiaoHang, HinhThucThanhToan, TrangThai, NgayTao) VALUES
('HD001', 'KH001', 'ND002', 'KM001', 27000000, 3000000, 'GH001', N'Tiền mặt', N'Đã giao', '2026-02-02'),
('HD002', 'KH002', 'ND002', NULL, 600000, 0, NULL, N'Thẻ ngân hàng', N'Hoàn thành', '2026-02-05'),
('HD003', 'KH003', 'ND005', 'KM002', 24080000, 3920000, 'GH002', N'Chuyển khoản', N'Đã giao', '2026-02-14'),
('HD004', 'KH005', 'ND005', 'KM003', 18400000, 1600000, 'GH003', N'Chuyển khoản', N'Đã giao', '2026-03-08'),
('HD005', 'KH006', 'ND002', NULL, 7500000, 0, NULL, N'Tiền mặt', N'Hủy', '2026-03-12'),
('HD006', 'KH008', 'ND013', NULL, 1400000, 0, 'GH004', N'Ví điện tử', N'Đã giao', '2026-03-28'),
('HD007', 'KH010', 'ND013', NULL, 32000000, 0, 'GH005', N'Thẻ ngân hàng', N'Đã giao', '2026-04-12'),
('HD008', 'KH012', 'ND002', NULL, 10500000, 0, 'GH006', N'Chuyển khoản', N'Đã giao', '2026-04-30'),
('HD009', 'KH014', 'ND005', 'KM004', 51000000, 9000000, 'GH007', N'Chuyển khoản', N'Đang giao', '2026-05-02'),
('HD010', 'KH015', 'ND005', 'KM004', 21250000, 3750000, 'GH008', N'Tiền mặt', N'Đang giao', '2026-05-12'),
('HD011', 'KH004', 'ND002', 'KM013', 24600000, 5400000, 'GH009', N'Thẻ ngân hàng', N'Chờ xử lý', '2026-05-14'),
('HD012', 'KH007', 'ND002', 'KM014', 420000, 280000, NULL, N'Tiền mặt', N'Hoàn thành', '2026-05-15'),
('HD013', 'KH009', 'ND013', 'KM015', 7125000, 375000, NULL, N'Ví điện tử', N'Chờ xử lý', '2026-05-18'),
('HD014', 'KH011', 'ND013', NULL, 3500000, 0, 'GH010', N'Tiền mặt', N'Đang giao', '2026-05-18'),
('HD015', 'KH013', 'ND005', NULL, 12000000, 0, 'GH011', N'Chuyển khoản', N'Chờ xử lý', '2026-05-18');

-- ========================================================
-- 10. DỮ LIỆU BẢNG: ChiTietHoaDon (15 dòng)
-- ========================================================
INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
('CTHD001', 'HD001', 'SP001', 1, 30000000, 30000000),
('CTHD002', 'HD002', 'SP006', 1, 600000, 600000),
('CTHD003', 'HD003', 'SP002', 1, 28000000, 28000000),
('CTHD004', 'HD004', 'SP005', 1, 20000000, 20000000),
('CTHD005', 'HD005', 'SP010', 1, 7500000, 7500000),
('CTHD006', 'HD006', 'SP009', 2, 700000, 1400000),
('CTHD007', 'HD007', 'SP004', 1, 32000000, 32000000),
('CTHD008', 'HD008', 'SP015', 1, 10500000, 10500000),
('CTHD009', 'HD009', 'SP001', 2, 30000000, 60000000),
('CTHD010', 'HD010', 'SP003', 1, 26000000, 26000000),
('CTHD011', 'HD011', 'SP001', 1, 30000000, 30000000),
('CTHD012', 'HD012', 'SP006', 1, 600000, 600000),
('CTHD013', 'HD013', 'SP010', 1, 7500000, 7500000),
('CTHD014', 'HD014', 'SP011', 1, 3500000, 3500000),
('CTHD015', 'HD015', 'SP012', 1, 12000000, 12000000);

-- ========================================================
-- 11. DỮ LIỆU BẢNG: GiaoHang (15 dòng)
-- ========================================================
INSERT INTO GiaoHang (MaGiaoHang, MaHoaDon, DiaChiGiao, TrangThaiGiao, NgayGiao) VALUES
('GH001', 'HD001', N'123 Nguyễn Trãi, Thanh Xuân, Hà Nội', N'Đã giao', '2026-02-03'),
('GH002', 'HD003', N'456 Lê Lợi, Quận 1, TP HCM', N'Đã giao', '2026-02-16'),
('GH003', 'HD004', N'789 Điện Biên Phủ, Đà Nẵng', N'Đã giao', '2026-03-09'),
('GH004', 'HD006', N'12 Trần Hưng Đạo, Cần Thơ', N'Đã giao', '2026-03-30'),
('GH005', 'HD007', N'88 Quang Trung, Hải Phòng', N'Đã giao', '2026-04-14'),
('GH006', 'HD008', N'55 Lê Hồng Phong, Vinh', N'Đã giao', '2026-05-01'),
('GH007', 'HD009', N'22 Bùi Thị Xuân, Đà Lạt', N'Đã giao', '2026-05-04'),
('GH008', 'HD010', N'101 Hùng Vương, Nha Trang', N'Đã giao', '2026-05-14'),
('GH009', 'HD011', N'15 Hoà Bình, Biên Hoà', N'Đang giao', NULL),
('GH010', 'HD014', N'67 Nguyễn Huệ, TP HCM', N'Đang chuẩn bị hàng', NULL),
('GH011', 'HD015', N'34 Lý Tự Trọng, Cần Thơ', N'Đang chuẩn bị hàng', NULL),
('GH012', NULL, N'Địa chỉ chờ xử lý đơn phụ 1', N'Chưa giao', NULL),
('GH013', NULL, N'Địa chỉ chờ xử lý đơn phụ 2', N'Chưa giao', NULL),
('GH014', NULL, N'Địa chỉ chờ xử lý đơn phụ 3', N'Chưa giao', NULL),
('GH015', NULL, N'Địa chỉ chờ xử lý đơn phụ 4', N'Chưa giao', NULL);

-- ========================================================
-- 12. DỮ LIỆU BẢNG: TraHang (15 dòng)
-- ========================================================
INSERT INTO TraHang (MaTraHang, MaHoaDon, NgayTra, LyDo, TongTienHoan, MaNguoiDung) VALUES
('TH001', 'HD001', '2026-02-04', N'Bút bi bị tắc mực', 27000000, 'ND008'),
('TH002', 'HD002', '2026-02-06', N'Khách đổi ý không muốn mua nữa', 600000, 'ND008'),
('TH003', 'HD005', '2026-03-13', N'Giấy in bị ẩm khi giao', 7500000, 'ND008'),
('TH004', 'HD007', '2026-04-15', N'Bìa còng bị gãy chốt', 32000000, 'ND008'),
('TH005', 'HD012', '2026-05-16', N'Kéo cắt giấy bị rỉ sét', 420000, 'ND008'),
('TH006', 'HD003', '2026-02-18', N'Giao sai màu bìa hồ sơ', 24080000, 'ND008'),
('TH007', 'HD004', '2026-03-10', N'Bảng từ trắng bị xước bề mặt', 18400000, 'ND008'),
('TH008', 'HD006', '2026-04-01', N'Khách hàng không hài lòng chất lượng giấy note', 1400000, 'ND008'),
('TH009', 'HD008', '2026-05-02', N'Máy tính Casio bấm không ăn', 10500000, 'ND008'),
('TH010', 'HD009', '2026-05-05', N'Sổ da bị rách mép lúc giao', 25500000, 'ND008'),
('TH011', 'HD010', '2026-05-15', N'Khách chê mẫu mã khay rổ mica', 21250000, 'ND008'),
('TH012', 'HD011', '2026-05-16', N'Hủy đơn do chờ vận chuyển lâu', 24600000, 'ND008'),
('TH013', 'HD013', '2026-05-18', N'Chuột máy tính bị đơ trượt', 7125000, 'ND008'),
('TH014', 'HD014', '2026-05-19', N'Mực dấu bị khô hỏng', 3500000, 'ND008'),
('TH015', 'HD015', '2026-05-19', N'Bấm kim bị kẹt lò xo', 12000000, 'ND008');

-- ========================================================
-- 13. DỮ LIỆU BẢNG: ChiTietTraHang (15 dòng)
-- ========================================================
INSERT INTO ChiTietTraHang (MaChiTietTra, MaTraHang, MaSanPham, SoLuong, TienHoan) VALUES
('CTTH001', 'TH001', 'SP001', 1, 27000000),
('CTTH002', 'TH002', 'SP006', 1, 600000),
('CTTH003', 'TH003', 'SP010', 1, 7500000),
('CTTH004', 'TH004', 'SP004', 1, 32000000),
('CTTH005', 'TH005', 'SP006', 1, 420000),
('CTTH006', 'TH006', 'SP002', 1, 24080000),
('CTTH007', 'TH007', 'SP005', 1, 18400000),
('CTTH008', 'TH008', 'SP009', 2, 1400000),
('CTTH009', 'TH009', 'SP015', 1, 10500000),
('CTTH010', 'TH010', 'SP001', 1, 25500000),
('CTTH011', 'TH011', 'SP003', 1, 21250000),
('CTTH012', 'TH012', 'SP001', 1, 24600000),
('CTTH013', 'TH013', 'SP010', 1, 7125000),
('CTTH014', 'TH014', 'SP011', 1, 3500000),
('CTTH015', 'TH015', 'SP012', 1, 12000000);

-- ========================================================
-- 14. DỮ LIỆU BẢNG: DoiHang (15 dòng)
-- ========================================================
INSERT INTO DoiHang (MaDoiHang, MaHoaDon, NgayDoi, MaNguoiDung, LyDo) VALUES
('DH001', 'HD001', '2026-02-05', 'ND008', N'Khách muốn đổi sang bút màu đen'),
('DH002', 'HD003', '2026-02-15', 'ND008', N'Lỗi tắc mực đổi cây mới tương đương'),
('DH003', 'HD004', '2026-03-10', 'ND008', N'Giấy in có lấm chấm mốc'),
('DH004', 'HD005', '2026-03-14', 'ND008', N'Đổi sang mẫu bìa còng dày hơn'),
('DH005', 'HD007', '2026-04-16', 'ND008', N'Bấm kim ghim bị kẹt chốt'),
('DH006', 'HD006', '2026-04-02', 'ND008', N'Đổi loại khay rổ sang mica dẻo'),
('DH007', 'HD008', '2026-05-02', 'ND008', N'Đổi dao rọc giấy cán chống trượt'),
('DH008', 'HD009', '2026-05-05', 'ND008', N'Hộp giấy móp méo nặng cần đổi nguyên vẹn'),
('DH009', 'HD010', '2026-05-14', 'ND008', N'Đổi sổ da A5 sang loại có khóa số'),
('DH010', 'HD011', '2026-05-16', 'ND008', N'Mực dấu bị chảy lem nắp'),
('DH011', 'HD012', '2026-05-17', 'ND008', N'Lỗi chuột không dây mất tín hiệu USB'),
('DH012', 'HD013', '2026-05-18', 'ND008', N'Kẹp bướm bị móp méo gãy chốt'),
('DH013', 'HD014', '2026-05-19', 'ND008', N'Máy tính casio liệt phím màn hình'),
('DH014', 'HD015', '2026-05-19', 'ND008', N'Bảng từ bong tróc góc bo nhôm'),
('DH015', 'HD002', '2026-02-07', 'ND008', N'Bìa lá nhựa dán mép bị lỗi hở keo');

-- ========================================================
-- 15. DỮ LIỆU BẢNG: ChiTietDoiHang (15 dòng)
-- ========================================================
INSERT INTO ChiTietDoiHang (MaChiTietDoi, MaDoiHang, MaSanPhamCu, MaSanPhamMoi, SoLuong, ChenhLechGia) VALUES
('CTDH001', 'DH001', 'SP001', 'SP001', 1, 0),
('CTDH002', 'DH002', 'SP002', 'SP002', 1, 0),
('CTDH003', 'DH003', 'SP005', 'SP005', 1, 0),
('CTDH004', 'DH004', 'SP010', 'SP010', 1, 0),
('CTDH005', 'DH005', 'SP004', 'SP004', 1, 0),
('CTDH006', 'DH006', 'SP009', 'SP009', 1, 0),
('CTDH007', 'DH007', 'SP015', 'SP015', 1, 0),
('CTDH008', 'DH008', 'SP001', 'SP001', 1, 0),
('CTDH009', 'DH009', 'SP003', 'SP004', 1, 6000000),
('CTDH010', 'DH010', 'SP001', 'SP001', 1, 0),
('CTDH011', 'DH011', 'SP006', 'SP006', 1, 0),
('CTDH012', 'DH012', 'SP010', 'SP010', 1, 0),
('CTDH013', 'DH013', 'SP011', 'SP011', 1, 0),
('CTDH014', 'DH014', 'SP012', 'SP012', 1, 0),
('CTDH015', 'DH015', 'SP006', 'SP006', 1, 0);