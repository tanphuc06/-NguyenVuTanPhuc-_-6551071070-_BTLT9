-- CÂU 2: CSDL QL_NhaTro
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
