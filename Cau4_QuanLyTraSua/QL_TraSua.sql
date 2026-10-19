-- CÂU 4: CSDL QL_TraSua
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
