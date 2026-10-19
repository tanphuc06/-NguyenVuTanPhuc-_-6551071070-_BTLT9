-- CÂU 1: CSDL QL_ChiTieu
USE master;
GO
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
