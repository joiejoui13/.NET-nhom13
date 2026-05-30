USE CKNet;
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNguoiDung, MaKhuyenMai, TongTien, PhuongThucThanhToan, TrangThai, LoaiHoaDon, LyDoHuy, NgayTao) VALUES
(4, 1, 2, NULL, 150000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(day, -2, GETDATE())),
(5, 2, 2, NULL, 240000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, DATEADD(day, -3, GETDATE())),
(6, 3, 2, NULL, 50000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(day, -5, GETDATE())),
(7, 1, 2, NULL, 450000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, DATEADD(day, -10, GETDATE())),
(8, 2, 2, NULL, 120000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(day, -15, GETDATE())),
(9, 3, 2, NULL, 300000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(day, -20, GETDATE())),
(10, 1, 2, NULL, 75000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(month, -1, GETDATE())),
(11, 2, 2, NULL, 680000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, DATEADD(month, -1, GETDATE())),
(12, 3, 2, NULL, 40000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(month, -2, GETDATE())),
(13, 1, 2, NULL, 160000, N'Chuyển khoản', N'Đã hoàn thành', N'Đơn bán hàng', NULL, DATEADD(month, -2, GETDATE())),
(14, 2, 2, NULL, 85000, N'Tiền mặt', N'Đã hoàn thành', N'Đơn đặt hàng', NULL, DATEADD(month, -3, GETDATE())),
(15, 3, 2, NULL, 500000, N'Chuyển khoản', N'Chờ xử lý', N'Đơn đặt hàng', NULL, GETDATE());
SET IDENTITY_INSERT HoaDon OFF;

SET IDENTITY_INSERT ChiTietHoaDon ON;
INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
(6, 4, 1, 30, 5000, 150000),
(7, 5, 2, 20, 12000, 240000),
(8, 6, 1, 10, 5000, 50000),
(9, 7, 3, 10, 45000, 450000),
(10, 8, 4, 10, 12000, 120000),
(11, 9, 6, 5, 60000, 300000),
(12, 10, 5, 5, 15000, 75000),
(13, 11, 12, 1, 680000, 680000),
(14, 12, 7, 1, 40000, 40000),
(15, 13, 8, 2, 80000, 160000),
(16, 14, 8, 1, 85000, 85000),
(17, 15, 6, 8, 62500, 500000);
SET IDENTITY_INSERT ChiTietHoaDon OFF;
