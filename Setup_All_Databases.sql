-- =======================================================
-- KHOI TAO TAT CA 4 CO SO DU LIEU CHO BAI TAP CHUONG 11
-- =======================================================

USE master;
GO

-- 1. DATABASE QL_ChiTieu (Cau 1)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QL_ChiTieu')
    DROP DATABASE QL_ChiTieu;
GO
CREATE DATABASE QL_ChiTieu;
GO
USE QL_ChiTieu;
GO

CREATE TABLE ChiTieu (
    MaCT INT IDENTITY(1,1) PRIMARY KEY,
    NgayChi DATE NOT NULL,
    NoiDung NVARCHAR(100) NOT NULL,
    LoaiChi NVARCHAR(30) NOT NULL,
    SoTien DECIMAL(18,0) NOT NULL,
    GhiChu NVARCHAR(200) NULL
);
GO

INSERT INTO ChiTieu (NgayChi, NoiDung, LoaiChi, SoTien, GhiChu) VALUES
('2026-10-01', N'Cơm trưa', N'Ăn uống', 35000, N'Cơm sườn bì chả'),
('2026-10-02', N'Xăng xe', N'Đi lại', 60000, N'Đổ xăng xe máy'),
('2026-10-03', N'Photo giáo trình', N'Học tập', 25000, N'Tài liệu môn C#'),
('2026-10-04', N'Xem phim CGV', N'Giải trí', 90000, N'Vé xem phim cuối tuần'),
('2026-10-05', N'Trà sữa Phúc Long', N'Ăn uống', 40000, N'Uống cùng bạn bè'),
('2026-10-06', N'Mua sắm áo thun', N'Cá nhân', 150000, N'Áo thun ngắn tay'),
('2026-10-07', N'Tiền điện tháng 9', N'Hóa đơn', 500000, N'Thanh toán qua ví MoMo'),
('2026-10-08', N'Ăn sáng bún bò', N'Ăn uống', 25000, N'Bún bò Huế'),
('2026-10-09', N'Sách tham khảo EF Core', N'Học tập', 120000, N'Nhà sách FAHASA'),
('2026-10-10', N'Bảo dưỡng xe máy', N'Đi lại', 180000, N'Thay nhớt và lọc gió'),
('2026-10-18', N'Cà phê với bạn bè', N'Ăn uống', 45000, N'Highlands Coffee');
GO

-- 2. DATABASE QL_NhaTro (Cau 2)
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QL_NhaTro')
    DROP DATABASE QL_NhaTro;
GO
CREATE DATABASE QL_NhaTro;
GO
USE QL_NhaTro;
GO

CREATE TABLE PhongTro (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong NVARCHAR(20) NOT NULL,
    GiaThue DECIMAL(18,0) NOT NULL,
    TinhTrang NVARCHAR(20) NOT NULL -- "Trống" / "Đang thuê"
);
GO

CREATE TABLE KhachThue (
    MaKhach INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(5) NOT NULL,
    NgaySinh DATE NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    MaTinh INT NOT NULL,
    TenTinh NVARCHAR(100) NOT NULL,
    MaPhuongXa INT NOT NULL,
    TenPhuongXa NVARCHAR(100) NOT NULL,
    DaDangKyTamTru BIT NOT NULL DEFAULT 0,
    MaPhong INT NOT NULL,
    CONSTRAINT FK_KhachThue_PhongTro FOREIGN KEY (MaPhong) REFERENCES PhongTro(MaPhong) ON DELETE CASCADE
);
GO

INSERT INTO PhongTro (TenPhong, GiaThue, TinhTrang) VALUES
(N'P01', 1500000, N'Đang thuê'),
(N'P02', 1600000, N'Đang thuê'),
(N'P03', 1800000, N'Đang thuê'),
(N'P04', 2000000, N'Đang thuê'),
(N'P05', 1700000, N'Đang thuê'),
(N'P06', 1900000, N'Trống');
GO

INSERT INTO KhachThue (HoTen, GioiTinh, NgaySinh, SoDienThoai, MaTinh, TenTinh, MaPhuongXa, TenPhuongXa, DaDangKyTamTru, MaPhong) VALUES
(N'Nguyễn Văn An', N'Nam', '2004-05-12', '0912345678', 1, N'Thành phố Hà Nội', 613, N'Phường Tây Tựu', 1, 3),
(N'Trần Thị Bình', N'Nữ', '2003-08-20', '0876543210', 1, N'Thành phố Hà Nội', 619, N'Phường Phú Diễn', 0, 1),
(N'Lê Văn Cường', N'Nam', '2002-11-15', '0901234567', 38, N'Tỉnh Thanh Hóa', 101, N'Phường Ba Đình', 0, 2),
(N'Phạm Minh Đức', N'Nam', '2001-03-09', '0987654321', 38, N'Tỉnh Thanh Hóa', 102, N'Phường Đông Vệ', 1, 4),
(N'Hoàng Thị Dung', N'Nữ', '2004-12-01', '0765432109', 24, N'Thành phố Bắc Ninh', 201, N'Phường Tiền An', 0, 5);
GO

-- 3. DATABASE QL_DaNgoai (Cau 3)
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QL_DaNgoai')
    DROP DATABASE QL_DaNgoai;
GO
CREATE DATABASE QL_DaNgoai;
GO
USE QL_DaNgoai;
GO

CREATE TABLE ChuyenDi (
    MaChuyen INT IDENTITY(1,1) PRIMARY KEY,
    TenChuyen NVARCHAR(100) NOT NULL,
    DiaDiem NVARCHAR(100) NOT NULL,
    NgayDi DATE NOT NULL,
    SoNguoi INT NOT NULL,
    NganSach DECIMAL(18,0) NOT NULL,
    PhuongTien NVARCHAR(30) NOT NULL,
    DaHoanThanh BIT NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(200) NULL
);
GO

INSERT INTO ChuyenDi (TenChuyen, DiaDiem, NgayDi, SoNguoi, NganSach, PhuongTien, DaHoanThanh, GhiChu) VALUES
(N'Cắm trại đồi chè', N'Đà Lạt', '2026-10-25', 6, 3500000, N'Ô tô', 0, N'Chuẩn bị lều bạt và áo ấm'),
(N'Khám phá nóc nhà Đông Dương', N'Sa Pa', '2026-10-28', 4, 5000000, N'Xe khách', 0, N'Leo Fansipan'),
(N'Nghỉ dưỡng cuối tuần', N'Vũng Tàu', '2026-10-23', 8, 4000000, N'Ô tô', 0, N'Tắm biển và hải sản'),
(N'Thăm phố cổ về đêm', N'Hội An', '2026-11-01', 5, 4500000, N'Tàu hỏa', 0, N'Thả hoa đăng và chụp ảnh'),
(N'Lặn ngắm san hô', N'Phú Quốc', '2026-11-05', 4, 8000000, N'Xe khách', 0, N'Tour 4 đảo'),
(N'Tràng An Bái Đính', N'Ninh Bình', '2026-10-22', 10, 3000000, N'Ô tô', 0, N'Chèo thuyền ngắm cảnh'),
(N'Mùa hoa mận nở', N'Mộc Châu', '2026-11-10', 6, 3800000, N'Xe máy', 0, N'Phượt ngắm cảnh'),
(N'Tắm biển Cát Bà', N'Hải Phòng', '2026-10-15', 5, 3200000, N'Xe khách', 1, N'Đã hoàn thành tốt đẹp');
GO

-- 4. DATABASE QL_TraSua (Cau 4)
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QL_TraSua')
    DROP DATABASE QL_TraSua;
GO
CREATE DATABASE QL_TraSua;
GO
USE QL_TraSua;
GO

CREATE TABLE DanhMucMon (
    MaDM INT IDENTITY(1,1) PRIMARY KEY,
    TenDM NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE MonUong (
    MaMon INT IDENTITY(1,1) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    GiaBan DECIMAL(18,0) NOT NULL,
    DangBan BIT NOT NULL DEFAULT 1,
    MaDM INT NOT NULL,
    CONSTRAINT FK_MonUong_DanhMucMon FOREIGN KEY (MaDM) REFERENCES DanhMucMon(MaDM)
);
GO

CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,0) NOT NULL,
    HinhThucTT NVARCHAR(20) NOT NULL, -- 'Tiền mặt' / 'Chuyển khoản'
    DaThanhToan BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE ChiTietHoaDon (
    MaHD INT NOT NULL,
    MaMon INT NOT NULL,
    SoLy INT NOT NULL,
    DonGia DECIMAL(18,0) NOT NULL,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHD, MaMon),
    CONSTRAINT FK_ChiTiet_HoaDon FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) ON DELETE CASCADE,
    CONSTRAINT FK_ChiTiet_MonUong FOREIGN KEY (MaMon) REFERENCES MonUong(MaMon)
);
GO

INSERT INTO DanhMucMon (TenDM) VALUES
(N'Trà sữa'),
(N'Trà trái cây'),
(N'Topping'),
(N'Đồ ăn vặt');
GO

INSERT INTO MonUong (TenMon, GiaBan, DangBan, MaDM) VALUES
(N'Trà sữa trân châu', 35000, 1, 1),
(N'Trà sữa matcha', 40000, 1, 1),
(N'Hồng trà sữa', 32000, 1, 1),
(N'Trà sữa khoai môn', 38000, 1, 1),
(N'Trà đào cam sả', 42000, 1, 2),
(N'Trà vải hoa hồng', 45000, 1, 2),
(N'Trà xoài macchiato', 40000, 1, 2),
(N'Trân châu đen', 5000, 1, 3),
(N'Trân châu trắng', 5000, 1, 3),
(N'Thạch phô mai', 10000, 1, 3),
(N'Bánh tráng trộn', 20000, 1, 4),
(N'Khoai tây lắc phô mai', 25000, 1, 4),
(N'Cá viên chiên', 25000, 1, 4);
GO

INSERT INTO HoaDon (NgayLap, TongTien, HinhThucTT, DaThanhToan) VALUES
(GETDATE(), 120000, N'Chuyển khoản', 1),
(GETDATE(), 85000, N'Tiền mặt', 1);
GO

INSERT INTO ChiTietHoaDon (MaHD, MaMon, SoLy, DonGia) VALUES
(1, 1, 2, 35000),
(1, 2, 1, 40000),
(1, 9, 2, 5000),
(2, 5, 1, 42000),
(2, 6, 1, 43000);
GO
