using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau3_KeHoachDaNgoai.Models;
using Cau3_KeHoachDaNgoai.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau3_KeHoachDaNgoai
{
    public partial class FormChuyenDi : Form
    {
        private int _selectedId = -1;
        private string _currentLang = "vi-VN"; // "vi-VN" hoặc "en-US"
        private WeatherInfo? _lastWeatherInfo;

        public FormChuyenDi()
        {
            InitializeComponent();
        }

        private void FormChuyenDi_Load(object sender, EventArgs e)
        {
            // Tải ngôn ngữ đã lưu từ %LocalAppData%
            var settings = UserSettingsService.Load();
            _currentLang = settings.Language ?? "vi-VN";
            cboNgonNgu.SelectedIndex = _currentLang == "en-US" ? 1 : 0;

            if (cboPhuongTien.Items.Count > 0) cboPhuongTien.SelectedIndex = 1; // Ô tô

            var today = DateTime.Today;
            dtpNgayDi.Value = today.AddDays(3);
            dtpTuNgay.Value = today;
            dtpDenNgay.Value = today.AddMonths(2);

            ApplyLanguage();
            LoadData();
        }

        private void cboNgonNgu_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cboNgonNgu.SelectedIndex == 1 ? "en-US" : "vi-VN";
            if (_currentLang != selected)
            {
                _currentLang = selected;
                UserSettingsService.Save(new UserSettings { Language = _currentLang });
                ApplyLanguage();
                LoadData();
                UpdateWeatherDisplay();
            }
        }

        private void ApplyLanguage()
        {
            bool isEn = _currentLang == "en-US";

            this.Text = isEn ? "Weekend Picnic Planner" : "Kế hoạch dã ngoại cuối tuần";
            lblTitle.Text = isEn ? "Weekend Picnic Planner" : "Kế hoạch dã ngoại cuối tuần";
            lblNgonNgu.Text = isEn ? "Language:" : "Ngôn ngữ:";

            grpThongTin.Text = isEn ? "Trip Information" : "Thông tin chuyến đi";
            lblTenChuyen.Text = isEn ? "Trip Name:" : "Tên chuyến:";
            lblDiaDiem.Text = isEn ? "Location:" : "Địa điểm:";
            lblNgayDi.Text = isEn ? "Departure:" : "Ngày đi:";
            lblSoNguoi.Text = isEn ? "People:" : "Số người:";
            lblNganSach.Text = isEn ? "Budget:" : "Ngân sách:";
            lblPhuongTien.Text = isEn ? "Transport:" : "Phương tiện:";
            chkHoanThanh.Text = isEn ? "Completed" : "Đã hoàn thành";

            btnThem.Text = isEn ? "Add" : "Thêm";
            btnSua.Text = isEn ? "Edit" : "Sửa";
            btnXoa.Text = isEn ? "Delete" : "Xóa";
            btnLamMoi.Text = isEn ? "Refresh" : "Làm mới";

            grpThoiTiet.Text = isEn ? "Weather Forecast" : "Dự báo thời tiết";
            btnXemThoiTiet.Text = isEn ? "Check Weather" : "Xem thời tiết";
            lblThoiTietTitle.Text = isEn ? "Select a trip to view forecast" : "Chọn chuyến đi để xem dự báo";
            lblNhietDo.Text = isEn ? "Temperature:" : "Nhiệt độ:";
            lblKhaNangMua.Text = isEn ? "Rain chance:" : "Khả năng mưa:";
            lblCanhBao.Text = isEn ? "⚠️ Rain gear recommended or consider rescheduling" : "⚠️ Nên mang áo mưa hoặc cân nhắc dời lịch";

            txtTimKiem.PlaceholderText = isEn ? "Search by name or place..." : "Tìm theo tên hoặc địa điểm...";
            chkLocChuaHoanThanh.Text = isEn ? "Incomplete trips only" : "Chỉ hiện chuyến chưa hoàn thành";
            lblTuNgay.Text = isEn ? "From:" : "Từ ngày:";
            lblDenNgay.Text = isEn ? "To:" : "Đến ngày:";
            btnTim.Text = isEn ? "Search" : "Tìm kiếm";
            btnBoLoc.Text = isEn ? "Clear" : "Bỏ lọc";

            // Tiêu đề cột DataGridView
            colMa.HeaderText = isEn ? "ID" : "Mã";
            colTenChuyen.HeaderText = isEn ? "Trip Name" : "Tên chuyến";
            colDiaDiem.HeaderText = isEn ? "Location" : "Địa điểm";
            colNgayDi.HeaderText = isEn ? "Departure Date" : "Ngày đi";
            colSoNguoi.HeaderText = isEn ? "People" : "Số người";
            colNganSach.HeaderText = isEn ? "Budget" : "Ngân sách";
            colPhuongTien.HeaderText = isEn ? "Transport" : "Phương tiện";
            colHoanThanh.HeaderText = isEn ? "Completed" : "Hoàn thành";

            // Cập nhật các lựa chọn phương tiện
            int prevIdx = cboPhuongTien.SelectedIndex;
            cboPhuongTien.Items.Clear();
            if (isEn)
            {
                cboPhuongTien.Items.AddRange(new object[] { "Motorbike", "Car", "Coach", "Train" });
            }
            else
            {
                cboPhuongTien.Items.AddRange(new object[] { "Xe máy", "Ô tô", "Xe khách", "Tàu hỏa" });
            }
            cboPhuongTien.SelectedIndex = prevIdx >= 0 && prevIdx < cboPhuongTien.Items.Count ? prevIdx : 0;
        }

        private void LoadData()
        {
            try
            {
                using var db = new QL_DaNgoaiContext();
                var query = db.ChuyenDis.AsNoTracking().AsQueryable();

                var kw = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(kw))
                {
                    query = query.Where(c => c.TenChuyen.Contains(kw) || c.DiaDiem.Contains(kw));
                }

                if (chkLocChuaHoanThanh.Checked)
                {
                    query = query.Where(c => !c.DaHoanThanh);
                }

                var tuNgay = DateOnly.FromDateTime(dtpTuNgay.Value.Date);
                var denNgay = DateOnly.FromDateTime(dtpDenNgay.Value.Date);
                query = query.Where(c => c.NgayDi >= tuNgay && c.NgayDi <= denNgay);

                var list = query.OrderBy(c => c.NgayDi).ToList();

                var culture = new CultureInfo(_currentLang);
                var displayList = list.Select(c => new
                {
                    c.MaChuyen,
                    c.TenChuyen,
                    c.DiaDiem,
                    c.NgayDi,
                    NgayDiStr = c.NgayDi.ToString("dd/MM/yyyy", culture),
                    c.SoNguoi,
                    c.NganSach,
                    NganSachFormatted = _currentLang == "en-US" 
                        ? string.Format(culture, "{0:C0}", c.NganSach) 
                        : string.Format(culture, "{0:N0} đ", c.NganSach),
                    c.PhuongTien,
                    c.DaHoanThanh,
                    c.GhiChu
                }).ToList();

                dgvChuyenDi.DataSource = displayList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải danh sách chuyến đi");
                ShowError(_currentLang == "en-US" ? "Failed to load trips!" : "Không thể tải danh sách chuyến đi!");
            }
        }

        private async void dgvChuyenDi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChuyenDi.Rows.Count) return;

            var row = dgvChuyenDi.Rows[e.RowIndex];
            _selectedId = Convert.ToInt32(row.Cells["colMa"].Value);
            txtTenChuyen.Text = row.Cells["colTenChuyen"].Value?.ToString() ?? "";
            txtDiaDiem.Text = row.Cells["colDiaDiem"].Value?.ToString() ?? "";

            var ngayStr = row.Cells["colNgayDi"].Value?.ToString();
            if (DateTime.TryParseExact(ngayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            {
                dtpNgayDi.Value = d;
            }

            if (int.TryParse(row.Cells["colSoNguoi"].Value?.ToString(), out var sn))
            {
                numSoNguoi.Value = Math.Max(numSoNguoi.Minimum, sn);
            }

            if (dgvChuyenDi.DataSource is IEnumerable<dynamic> list)
            {
                var item = list.ElementAt(e.RowIndex);
                numNganSach.Value = Math.Min(numNganSach.Maximum, Math.Max(numNganSach.Minimum, (decimal)item.NganSach));
                chkHoanThanh.Checked = (bool)item.DaHoanThanh;

                // Đồng bộ phương tiện
                string pt = (string)item.PhuongTien;
                MapTransportToCombo(pt);
            }

            // Tự động kiểm tra thời tiết khi chọn dòng
            await FetchWeatherAsync();
        }

        private void MapTransportToCombo(string pt)
        {
            if (string.IsNullOrEmpty(pt)) return;

            if (_currentLang == "en-US")
            {
                if (pt.Contains("máy") || pt.Contains("Motorbike")) cboPhuongTien.SelectedItem = "Motorbike";
                else if (pt.Contains("tô") || pt.Contains("Car")) cboPhuongTien.SelectedItem = "Car";
                else if (pt.Contains("khách") || pt.Contains("Coach")) cboPhuongTien.SelectedItem = "Coach";
                else if (pt.Contains("hỏa") || pt.Contains("Train")) cboPhuongTien.SelectedItem = "Train";
            }
            else
            {
                if (pt.Contains("máy") || pt.Contains("Motorbike")) cboPhuongTien.SelectedItem = "Xe máy";
                else if (pt.Contains("tô") || pt.Contains("Car")) cboPhuongTien.SelectedItem = "Ô tô";
                else if (pt.Contains("khách") || pt.Contains("Coach")) cboPhuongTien.SelectedItem = "Xe khách";
                else if (pt.Contains("hỏa") || pt.Contains("Train")) cboPhuongTien.SelectedItem = "Tàu hỏa";
            }
        }

        private async void btnXemThoiTiet_Click(object sender, EventArgs e)
        {
            await FetchWeatherAsync();
        }

        private async Task FetchWeatherAsync()
        {
            var diaDiem = txtDiaDiem.Text.Trim();
            if (string.IsNullOrEmpty(diaDiem))
            {
                ShowWarning(_currentLang == "en-US" ? "Please enter a location first!" : "Vui lòng nhập địa điểm chuyến đi!");
                return;
            }

            var ngayDi = DateOnly.FromDateTime(dtpNgayDi.Value.Date);

            try
            {
                btnXemThoiTiet.Enabled = false;
                prgLoading.Visible = true;
                lblThoiTietTitle.Text = _currentLang == "en-US" 
                    ? $"Forecasting weather for {diaDiem} on {ngayDi:dd/MM/yyyy}..." 
                    : $"Đang lấy dự báo thời tiết ngày {ngayDi:dd/MM/yyyy} - {diaDiem}...";

                _lastWeatherInfo = await WeatherApiService.GetWeatherForecastAsync(diaDiem, ngayDi);
                UpdateWeatherDisplay();
            }
            catch (KeyNotFoundException knf)
            {
                Log.Warning(knf, "Không tìm thấy địa danh '{DiaDiem}'", diaDiem);
                lblWeatherIcon.Text = "❓";
                lblWeatherDesc.Text = _currentLang == "en-US" ? "Location not found" : "Không tìm thấy địa điểm";
                lblNhietDo.Text = _currentLang == "en-US" ? "Try using unaccented name (e.g., Da Lat)" : "Gợi ý: Thử gõ tên không dấu như 'Da Lat'";
                lblKhaNangMua.Text = "";
                pnlCanhBao.Visible = false;
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                Log.Warning(ex, "Lỗi kết nối thời tiết Open-Meteo");
                lblWeatherIcon.Text = "⚠️";
                lblWeatherDesc.Text = _currentLang == "en-US" ? "Network error" : "Lỗi kết nối mạng";
                lblNhietDo.Text = _currentLang == "en-US" ? "Cannot reach weather service" : "Không kết nối được máy chủ thời tiết";
                lblKhaNangMua.Text = "";
                pnlCanhBao.Visible = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tra cứu thời tiết");
            }
            finally
            {
                prgLoading.Visible = false;
                btnXemThoiTiet.Enabled = true;
            }
        }

        private void UpdateWeatherDisplay()
        {
            if (_lastWeatherInfo == null) return;

            bool isEn = _currentLang == "en-US";
            var ngayDi = DateOnly.FromDateTime(dtpNgayDi.Value.Date);
            var diaDiem = txtDiaDiem.Text.Trim();

            lblThoiTietTitle.Text = isEn 
                ? $"Forecast for {ngayDi:dd/MM/yyyy} - {diaDiem}" 
                : $"Dự báo thời tiết ngày {ngayDi:dd/MM/yyyy} - {diaDiem}";

            if (!_lastWeatherInfo.HasForecast)
            {
                lblWeatherIcon.Text = "📅";
                lblWeatherDesc.Text = isEn ? "No forecast data" : "Chưa có dữ liệu dự báo";
                lblNhietDo.Text = isEn ? "(Forecast available within 16 days)" : "(Chỉ dự báo tối đa 16 ngày tới)";
                lblKhaNangMua.Text = "";
                pnlCanhBao.Visible = false;
                return;
            }

            lblWeatherIcon.Text = _lastWeatherInfo.WeatherIcon;
            lblWeatherDesc.Text = isEn ? _lastWeatherInfo.WeatherDescriptionEn : _lastWeatherInfo.WeatherDescriptionVi;
            lblNhietDo.Text = (isEn ? "Temperature: " : "Nhiệt độ: ") + $"{_lastWeatherInfo.TempMin:N0}°C - {_lastWeatherInfo.TempMax:N0}°C";
            lblKhaNangMua.Text = (isEn ? "Rain chance: " : "Khả năng mưa: ") + $"{_lastWeatherInfo.RainProbability}%";

            if (_lastWeatherInfo.IsHighRainRisk)
            {
                pnlCanhBao.Visible = true;
            }
            else
            {
                pnlCanhBao.Visible = false;
            }
        }

        private bool ValidateInput(bool isAdding)
        {
            bool isEn = _currentLang == "en-US";

            if (string.IsNullOrWhiteSpace(txtTenChuyen.Text))
            {
                ShowWarning(isEn ? "Please enter a trip name!" : "Vui lòng nhập tên chuyến đi!");
                txtTenChuyen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text))
            {
                ShowWarning(isEn ? "Please enter a location!" : "Vui lòng nhập địa điểm!");
                txtDiaDiem.Focus();
                return false;
            }

            if (isAdding && dtpNgayDi.Value.Date < DateTime.Today)
            {
                ShowWarning(isEn ? "Departure date cannot be in the past!" : "Ngày đi không được nhỏ hơn ngày hôm nay khi thêm mới!");
                dtpNgayDi.Focus();
                return false;
            }

            if (numSoNguoi.Value < 1)
            {
                ShowWarning(isEn ? "Number of people must be at least 1!" : "Số người phải lớn hơn hoặc bằng 1!");
                numSoNguoi.Focus();
                return false;
            }

            if (numNganSach.Value < 0)
            {
                ShowWarning(isEn ? "Budget must be at least 0!" : "Ngân sách phải lớn hơn hoặc bằng 0!");
                numNganSach.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(true)) return;

            try
            {
                using var db = new QL_DaNgoaiContext();
                var item = new ChuyenDi
                {
                    TenChuyen = txtTenChuyen.Text.Trim(),
                    DiaDiem = txtDiaDiem.Text.Trim(),
                    NgayDi = DateOnly.FromDateTime(dtpNgayDi.Value.Date),
                    SoNguoi = (int)numSoNguoi.Value,
                    NganSach = numNganSach.Value,
                    PhuongTien = cboPhuongTien.SelectedItem?.ToString() ?? "Ô tô",
                    DaHoanThanh = chkHoanThanh.Checked
                };

                db.ChuyenDis.Add(item);
                db.SaveChanges();

                Log.Information("Thêm chuyến đi thành công: MaChuyen={MaChuyen}, TenChuyen={TenChuyen}", item.MaChuyen, item.TenChuyen);

                ShowInfo(_currentLang == "en-US" ? "Trip added successfully!" : "Thêm chuyến đi thành công!");
                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thêm chuyến đi");
                ShowError(_currentLang == "en-US" ? "Failed to add trip!" : "Không thể thêm chuyến đi!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                ShowWarning(_currentLang == "en-US" ? "Please select a trip from the list to edit!" : "Vui lòng chọn một chuyến đi từ danh sách để sửa!");
                return;
            }

            if (!ValidateInput(false)) return;

            try
            {
                using var db = new QL_DaNgoaiContext();
                var item = db.ChuyenDis.FirstOrDefault(c => c.MaChuyen == _selectedId);
                if (item == null)
                {
                    ShowError(_currentLang == "en-US" ? "Trip not found!" : "Chuyến đi không tồn tại!");
                    LoadData();
                    return;
                }

                item.TenChuyen = txtTenChuyen.Text.Trim();
                item.DiaDiem = txtDiaDiem.Text.Trim();
                item.NgayDi = DateOnly.FromDateTime(dtpNgayDi.Value.Date);
                item.SoNguoi = (int)numSoNguoi.Value;
                item.NganSach = numNganSach.Value;
                item.PhuongTien = cboPhuongTien.SelectedItem?.ToString() ?? "Ô tô";
                item.DaHoanThanh = chkHoanThanh.Checked;

                db.SaveChanges();

                Log.Information("Sửa chuyến đi thành công: MaChuyen={MaChuyen}", item.MaChuyen);

                ShowInfo(_currentLang == "en-US" ? "Trip updated successfully!" : "Cập nhật chuyến đi thành công!");
                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi sửa chuyến đi");
                ShowError(_currentLang == "en-US" ? "Failed to update trip!" : "Không thể sửa chuyến đi!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                ShowWarning(_currentLang == "en-US" ? "Please select a trip to delete!" : "Vui lòng chọn chuyến đi để xóa!");
                return;
            }

            string question = _currentLang == "en-US" 
                ? $"Are you sure you want to delete trip #{_selectedId}?" 
                : $"Bạn có chắc chắn muốn xóa chuyến đi mã {_selectedId} không?";
            string title = _currentLang == "en-US" ? "Confirm Delete" : "Xác nhận xóa";

            if (MessageBox.Show(question, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using var db = new QL_DaNgoaiContext();
                var item = db.ChuyenDis.FirstOrDefault(c => c.MaChuyen == _selectedId);
                if (item != null)
                {
                    db.ChuyenDis.Remove(item);
                    db.SaveChanges();
                    Log.Information("Xóa chuyến đi thành công: MaChuyen={MaChuyen}", _selectedId);
                    ShowInfo(_currentLang == "en-US" ? "Trip deleted successfully!" : "Đã xóa chuyến đi thành công!");
                }

                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xóa chuyến đi");
                ShowError(_currentLang == "en-US" ? "Failed to delete trip!" : "Không thể xóa chuyến đi!");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void ResetInput()
        {
            _selectedId = -1;
            txtTenChuyen.Clear();
            txtDiaDiem.Clear();
            dtpNgayDi.Value = DateTime.Today.AddDays(3);
            numSoNguoi.Value = 4;
            numNganSach.Value = 3000000;
            if (cboPhuongTien.Items.Count > 0) cboPhuongTien.SelectedIndex = 0;
            chkHoanThanh.Checked = false;
            dgvChuyenDi.ClearSelection();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnBoLoc_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            chkLocChuaHoanThanh.Checked = false;
            var today = DateTime.Today;
            dtpTuNgay.Value = today;
            dtpDenNgay.Value = today.AddMonths(2);
            LoadData();
        }

        private void ShowInfo(string msg) => MessageBox.Show(msg, _currentLang == "en-US" ? "Notification" : "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowWarning(string msg) => MessageBox.Show(msg, _currentLang == "en-US" ? "Warning" : "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowError(string msg) => MessageBox.Show(msg, _currentLang == "en-US" ? "Error" : "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
