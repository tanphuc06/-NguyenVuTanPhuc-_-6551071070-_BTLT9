# BÀI TẬP CHƯƠNG 11: PUBLISH APP & TÍCH HỢP API

Thư mục này chứa đầy đủ **4 bài tập** được xây dựng bằng **C# WinForms (.NET 8.0)** kết hợp **Entity Framework Core Database First**, **Serilog**, **ConfigurationBuilder (strongly-typed)**, đóng gói **Publish win-x64** và tích hợp các **Web API thực tế**.

---

## 📁 CẤU TRÚC THƯ MỤC DỰ ÁN

```text
BaiTap_Chuong11_Publish_App_API/
├── BaiTap_Chuong11.sln              # Solution Visual Studio chuẩn mở cả 4 project
├── Setup_All_Databases.sql          # File SQL duy nhất tạo nhanh cả 4 CSDL
│
├── Cau1_SoChiTieu/                  # [CÂU 1] Sổ chi tiêu cá nhân
│   ├── QL_ChiTieu.sql               # Script CSDL QL_ChiTieu & 10+ dòng mẫu
│   ├── FormChiTieu.cs               # Giao diện & nghiệp vụ CRUD, lọc, tính tổng
│   ├── Configuration/AppSettings.cs # Configuration strongly-typed
│   ├── appsettings.json             # Connection string
│   ├── appsettings.Development.json # Ghi đè khi lập trình
│   ├── appsettings.example.json     # File mẫu không lộ mật khẩu
│   ├── .gitignore                   # Chặn file cấu hình nhạy cảm / logs / bin
│   └── bin/publish/                 # Thư mục đã Publish Framework-Dependent win-x64
│
├── Cau2_QuanLyNhaTro/               # [CÂU 2] Quản lý nhà trọ & API Tỉnh/Phường-Xã
│   ├── QL_NhaTro.sql                # Script CSDL PhongTro + KhachThue (FK)
│   ├── FormKhachThue.cs             # Giao diện quản lý khách thuê liên kết phòng
│   ├── Services/AddressApiService.cs# Tích hợp API provinces.open-api.vn
│   └── Program.cs                   # Bắt lỗi toàn cục (ThreadException, UnhandledException)
│
├── Cau3_KeHoachDaNgoai/             # [CÂU 3] Kế hoạch dã ngoại & API Thời tiết
│   ├── QL_DaNgoai.sql               # Script CSDL ChuyenDi (8 chuyến mẫu)
│   ├── FormChuyenDi.cs              # Giao diện lập kế hoạch dã ngoại
│   ├── Services/WeatherApiService.cs# Tích hợp API Open-Meteo (Geocoding + Forecast)
│   └── Services/UserSettingsService.cs # Lưu đa ngôn ngữ vào %LocalAppData%
│
└── Cau4_QuanLyTraSua/               # [CÂU 4] Quán trà sữa & VietQR Payment
    ├── QL_TraSua.sql                # Script CSDL 4 bảng (DanhMuc, Mon, HoaDon, ChiTiet)
    ├── FormBanHang.cs               # Giao diện POS bán hàng, Thực đơn, Hóa đơn
    ├── Services/VietQRService.cs    # Tích hợp API mã QR thanh toán VietQR
    └── appsettings.json             # Cấu hình BankId, AccountNo, ShopName
```

---

## 🛠️ HƯỚNG DẪN CÀI ĐẶT & CHẠY ỨNG DỤNG

### Bước 1: Khởi tạo Cơ sở dữ liệu (SQL Server)
Mở SQL Server Management Studio (SSMS) hoặc chạy lệnh thông qua `sqlcmd`:
```powershell
sqlcmd -S . -E -C -i "Setup_All_Databases.sql"
```
Script sẽ tự động tạo và nạp dữ liệu mẫu cho cả 4 CSDL:
1. `QL_ChiTieu`
2. `QL_NhaTro`
3. `QL_DaNgoai`
4. `QL_TraSua`

### Bước 2: Mở và chạy từng bài tập
Bạn có thể mở `BaiTap_Chuong11.sln` trong Visual Studio 2022 / VS Code, hoặc dùng terminal:

- **Chạy Câu 1 (Sổ chi tiêu cá nhân):**
  ```powershell
  dotnet run --project Cau1_SoChiTieu/Cau1_SoChiTieu.csproj
  ```

- **Chạy Câu 2 (Quản lý nhà trọ):**
  ```powershell
  dotnet run --project Cau2_QuanLyNhaTro/Cau2_QuanLyNhaTro.csproj
  ```

- **Chạy Câu 3 (Kế hoạch dã ngoại):**
  ```powershell
  dotnet run --project Cau3_KeHoachDaNgoai/Cau3_KeHoachDaNgoai.csproj
  ```

- **Chạy Câu 4 (Bán hàng quán trà sữa):**
  ```powershell
  dotnet run --project Cau4_QuanLyTraSua/Cau4_QuanLyTraSua.csproj
  ```

---

## 🎯 CHI TIẾT TỪNG BÀI TẬP & TÍNH NĂNG ĐẠT ĐƯỢC

### 1. Câu 1: Sổ chi tiêu cá nhân
* **EF Core Database First**: Đã scaffold `QL_ChiTieuContext` và model `ChiTieu`.
* **Giao diện & CRUD**: Đầy đủ `DateTimePicker` (ngày chi), `TextBox` (nội dung), `ComboBox` (loại chi), `NumericUpDown` (số tiền), `TextBox` nhiều dòng (ghi chú), `DataGridView`.
* **Ràng buộc kiểm tra hợp lệ**: Nội dung không để trống, số tiền > 0, ngày chi không vượt quá hôm nay, xác nhận Yes/No trước khi xóa.
* **Tìm kiếm & Lọc**: Tìm theo nội dung, loại chi, khoảng ngày (Từ ngày - Đến ngày). Label *Tổng chi: ... đ* tự động tính toán lại theo danh sách lọc.
* **Cấu hình (9.2)**: Tách connection string sang `appsettings.json`, ghi đè bằng `appsettings.Development.json`, đọc thông qua `ConfigurationBuilder` vào lớp `AppSettings` strongly-typed. Có kèm `appsettings.example.json` và `.gitignore`.
* **Ghi log Serilog (9.3)**: Ghi file hàng ngày vào thư mục `Logs/log-yyyyMMdd.txt` cạnh file `.exe`. Mức *Information* khi Thêm/Sửa/Xóa (kèm MaCT), mức *Error* khi thao tác CSDL gặp sự cố.
* **Đóng gói (9.1)**: Đã publish thành công gói Framework-Dependent win-x64 tại `Cau1_SoChiTieu/bin/publish`.

---

### 2. Câu 2: Quản lý nhà trọ
* **EF Core Database First**: 2 bảng có khóa ngoại `PhongTro` và `KhachThue`, kiểm tra đầy đủ navigation property (`MaPhongNavigation` và `KhachThues`).
* **Tích hợp API Tỉnh/Phường-Xã (9.4)**:
  - Khi form load: Gọi bất đồng bộ GET danh sách tỉnh thành từ `https://provinces.open-api.vn/api/v2/p/` nạp vào ComboBox tỉnh.
  - Khi chọn một tỉnh: Gọi GET `https://provinces.open-api.vn/api/v2/p/{code}?depth=2` nạp danh sách phường/xã tương ứng.
  - Dùng `HttpClient` dùng chung với timeout 10s, cơ chế bộ nhớ đệm (cache) giúp không gọi lại API thừa.
  - Khóa ComboBox và hiển thị trạng thái "Đang tải…" trong lúc chờ.
* **Chịu lỗi mạng & Fallback (9.4)**: Khi mất mạng hoặc API timeout, bắt `HttpRequestException` / `TaskCanceledException`, hiện thông báo thân thiện và cho phép người dùng tự gõ tay tên Tỉnh/Phường-Xã để lưu khách thuê bình thường.
* **Nghiệp vụ liên kết**: Thêm khách vào phòng thì phòng tự động chuyển *"Đang thuê"*; khi xóa khách cuối cùng của phòng thì phòng chuyển lại *"Trống"*.
* **Xử lý lỗi toàn cục (9.3)**: Cấu hình `Application.SetUnhandledExceptionMode`, `Application.ThreadException`, `AppDomain.CurrentDomain.UnhandledException`. Có nút *"Gây lỗi thử"* để kiểm tra bắt ngoại lệ và ghi log Serilog mà app không bị crash.

---

### 3. Câu 3: Kế hoạch dã ngoại
* **EF Core Database First**: Bảng `ChuyenDi` với 8 chuyến mẫu có ngày đi từ vài ngày tới vài tuần.
* **Tích hợp API Thời tiết Open-Meteo (9.4)**:
  - Bước 1: Geocoding chuyển địa danh thành tọa độ (lat/long) qua `https://geocoding-api.open-meteo.com/v1/search?name={DiaDiem}&count=1&language=vi`.
  - Bước 2: Lấy dự báo 16 ngày qua `https://api.open-meteo.com/v1/forecast?...&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max`.
  - Khớp đúng ngày đi trong mảng `daily.time` để hiển thị: biểu tượng thời tiết (☀️, ⛅, 🌧️, ⛈️, 🌫️), nhiệt độ thấp nhất - cao nhất (°C), khả năng mưa (%).
  - Nếu xác suất mưa ≥ 60%: tự động tô nền cam và hiện cảnh báo *"Nên mang áo mưa hoặc cân nhắc dời lịch"*.
  - Nếu ngày đi vượt quá 16 ngày: hiển thị *"Chưa có dữ liệu dự báo"*.
  - Xử lý gợi ý khi không tìm thấy địa danh (như gõ không dấu "Da Lat") và lỗi mạng nhẹ nhàng.
* **Đa ngôn ngữ Runtime (Tiếng Việt / English)**:
  - Chuyển đổi toàn bộ nhãn, tiêu đề cột lưới, nút bấm, thông báo và mô tả thời tiết ngay lập tức không cần khởi động lại.
  - Định dạng ngày tháng và tiền tệ chuẩn theo `CultureInfo` (`vi-VN` vs `en-US`).
  - Tự động lưu và khôi phục ngôn ngữ đã chọn qua file `usersettings.json` trong `%LocalAppData%\KeHoachDaNgoai`.

---

### 4. Câu 4: Bán hàng quán trà sữa (VietQR Payment)
* **EF Core Database First**: 4 bảng `DanhMucMon`, `MonUong`, `HoaDon`, `ChiTietHoaDon`.
* **Giao diện Bán hàng (POS) & Giỏ hàng**:
  - Chọn danh mục -> danh sách món thuộc danh mục.
  - Chọn số ly -> Thêm vào giỏ hàng: tự động cộng dồn số lượng nếu món đã có, nút xóa dòng, tổng tiền tự cập nhật tức thì.
* **Cấu hình thanh toán có kiểu (9.2)**: Đọc mục `"Payment"` từ `appsettings.json` gồm `BankId`, `AccountNo`, `AccountName`, `Template`, `ShopName` vào lớp `PaymentConfig` strongly-typed, không hard-code thông tin ngân hàng trong mã nguồn.
* **Tạo mã VietQR chuyển khoản (9.4)**:
  - Ghép địa chỉ `https://img.vietqr.io/image/{BankId}-{AccountNo}-{Template}.png?amount={TongTien}&addInfo={NoiDung}&accountName={AccountName}`.
  - Tải ảnh qua `HttpClient.GetByteArrayAsync` và hiển thị trên `PictureBox`.
  - Hiển thị kèm số tiền và nội dung chuyển khoản bằng chữ (ví dụ: `TRASUA HD1025`).
  - Nút *"Đã nhận tiền"*: Lưu đồng thời `HoaDon` và các dòng `ChiTietHoaDon` trong cùng một transaction/SaveChanges.
  - Nút *"Tiền mặt"*: Thanh toán và lưu hóa đơn trực tiếp không cần quét QR.
  - **Chịu lỗi & Bảo mật Log (9.3)**: Ghi log Information khi tạo QR (mã HD, số tiền - che số tài khoản dạng `******4321`), ghi Error khi mất mạng và tự động vẽ thẻ chuyển khoản offline có đầy đủ STK để khách chuyển tay.
* **Quản lý thực đơn**: Thêm/Sửa/Xóa món uống, tìm kiếm theo tên món, lọc theo danh mục.
* **Lịch sử hóa đơn & Doanh thu**: Lọc theo ngày, chọn một hóa đơn thì lưới thứ hai hiện danh sách chi tiết các món, nhãn *Doanh thu ngày* tự cập nhật chính xác.
