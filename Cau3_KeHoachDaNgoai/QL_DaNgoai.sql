-- CÂU 3: CSDL QL_DaNgoai
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
