using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau2_QuanLyNhaTro.Models;
using Cau2_QuanLyNhaTro.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau2_QuanLyNhaTro
{
    public partial class FormKhachThue : Form
    {
        private int _selectedId = -1;
        private bool _isLoadingAddress = false;

        public FormKhachThue()
        {
            InitializeComponent();
        }

        private async void FormKhachThue_Load(object sender, EventArgs e)
        {
            LoadPhongComboBox();
            LoadData();
            await LoadProvincesAsync();
        }

        private void LoadPhongComboBox()
        {
            try
            {
                using var db = new QL_NhaTroContext();
                var phongs = db.PhongTros.AsNoTracking().ToList();

                // ComboBox chọn phòng trong form nhập
                var listPhong = phongs.Select(p => new
                {
                    p.MaPhong,
                    DisplayText = $"{p.TenPhong} - {string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", p.GiaThue)}"
                }).ToList();

                cboPhong.DataSource = listPhong;
                cboPhong.DisplayMember = "DisplayText";
                cboPhong.ValueMember = "MaPhong";

                // ComboBox lọc theo phòng
                var listLocPhong = new List<object> { new { MaPhong = 0, DisplayText = "Tất cả" } };
                listLocPhong.AddRange(phongs.Select(p => new { p.MaPhong, DisplayText = p.TenPhong }));
                cboLocPhong.DataSource = listLocPhong;
                cboLocPhong.DisplayMember = "DisplayText";
                cboLocPhong.ValueMember = "MaPhong";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi nạp danh sách phòng");
                MessageBox.Show("Không thể tải danh sách phòng trọ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProvincesAsync()
        {
            try
            {
                _isLoadingAddress = true;
                cboTinh.Enabled = false;
                cboTinh.Text = "Đang tải…";
                lblStatus.Text = "Đang kết nối API tỉnh thành...";

                var provinces = await AddressApiService.GetProvincesAsync();

                cboTinh.DataSource = null;
                cboTinh.DisplayMember = "Name";
                cboTinh.ValueMember = "Code";
                cboTinh.DataSource = provinces;

                // Cập nhật combobox lọc theo tỉnh
                var locTinhList = new List<object> { new { Code = 0, Name = "Tất cả" } };
                locTinhList.AddRange(provinces.Select(p => new { p.Code, p.Name }));
                cboLocTinh.DataSource = locTinhList;
                cboLocTinh.DisplayMember = "Name";
                cboLocTinh.ValueMember = "Code";

                lblStatus.Text = $"Đã tải {provinces.Count} tỉnh/thành từ provinces.open-api.vn";
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                Log.Warning(ex, "Không thể kết nối API tỉnh thành provinces.open-api.vn");
                lblStatus.Text = "Không có kết nối mạng (Chế độ nhập tay)";
                MessageBox.Show("Không tải được danh sách địa chỉ, vui lòng kiểm tra mạng. Bạn có thể tự gõ tên tỉnh và phường/xã để lưu thông tin.",
                    "Cảnh báo kết nối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTinh.DataSource = null;
                cboTinh.Text = "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi không xác định khi tải danh sách tỉnh thành");
            }
            finally
            {
                _isLoadingAddress = false;
                cboTinh.Enabled = true;
            }
        }

        private async void cboTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingAddress) return;

            if (cboTinh.SelectedItem is ProvinceDto selectedProvince)
            {
                await LoadWardsAsync(selectedProvince.Code);
            }
            else if (cboTinh.SelectedValue is int code && code > 0)
            {
                await LoadWardsAsync(code);
            }
        }

        private async Task LoadWardsAsync(int provinceCode)
        {
            try
            {
                _isLoadingAddress = true;
                cboPhuongXa.Enabled = false;
                cboPhuongXa.Text = "Đang tải…";
                lblStatus.Text = $"Đang tải danh sách phường/xã (mã {provinceCode})...";

                var wards = await AddressApiService.GetWardsAsync(provinceCode);

                cboPhuongXa.DataSource = null;
                cboPhuongXa.DisplayMember = "Name";
                cboPhuongXa.ValueMember = "Code";
                cboPhuongXa.DataSource = wards;

                lblStatus.Text = $"Đã nạp {wards.Count} phường/xã.";
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                Log.Warning(ex, "Không thể tải phường/xã cho tỉnh mã {Code}", provinceCode);
                lblStatus.Text = "Lỗi mạng khi tải phường/xã (cho phép nhập tay)";
                MessageBox.Show("Không tải được danh sách phường/xã, bạn có thể tự nhập trực tiếp.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboPhuongXa.DataSource = null;
                cboPhuongXa.Text = "";
            }
            finally
            {
                _isLoadingAddress = false;
                cboPhuongXa.Enabled = true;
            }
        }

        private void LoadData()
        {
            try
            {
                using var db = new QL_NhaTroContext();
                var query = db.KhachThues.Include(k => k.MaPhongNavigation).AsNoTracking().AsQueryable();

                // Lọc từ khóa
                var keyword = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(k => k.HoTen.Contains(keyword) || k.SoDienThoai.Contains(keyword));
                }

                // Lọc theo phòng
                if (cboLocPhong.SelectedValue is int maPhongLoc && maPhongLoc > 0)
                {
                    query = query.Where(k => k.MaPhong == maPhongLoc);
                }

                // Lọc theo tỉnh
                if (cboLocTinh.SelectedValue is int maTinhLoc && maTinhLoc > 0)
                {
                    query = query.Where(k => k.MaTinh == maTinhLoc);
                }

                var list = query.OrderByDescending(k => k.MaKhach).ToList();

                var displayList = list.Select(k => new
                {
                    k.MaKhach,
                    k.HoTen,
                    k.GioiTinh,
                    k.NgaySinh,
                    k.SoDienThoai,
                    TenPhong = k.MaPhongNavigation != null ? k.MaPhongNavigation.TenPhong : "",
                    GiaThue = k.MaPhongNavigation != null ? k.MaPhongNavigation.GiaThue : 0,
                    GiaThueFormatted = k.MaPhongNavigation != null 
                        ? string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", k.MaPhongNavigation.GiaThue) : "",
                    QueQuan = $"{k.TenPhuongXa} - {k.TenTinh}",
                    k.DaDangKyTamTru,
                    k.MaPhong,
                    k.MaTinh,
                    k.TenTinh,
                    k.MaPhuongXa,
                    k.TenPhuongXa
                }).ToList();

                dgvKhachThue.DataSource = displayList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi truy vấn danh sách khách thuê");
                MessageBox.Show("Có lỗi xảy ra khi truy vấn dữ liệu khách thuê!", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachThue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvKhachThue.Rows.Count) return;

            var row = dgvKhachThue.Rows[e.RowIndex];
            _selectedId = Convert.ToInt32(row.Cells["colMa"].Value);

            txtHoTen.Text = row.Cells["colHoTen"].Value?.ToString() ?? "";
            txtSoDienThoai.Text = row.Cells["colSDT"].Value?.ToString() ?? "";

            // Lấy từ data source
            if (dgvKhachThue.DataSource is IEnumerable<dynamic> list)
            {
                var item = list.ElementAt(e.RowIndex);
                if (item.GioiTinh == "Nữ")
                    rdoNu.Checked = true;
                else
                    rdoNam.Checked = true;

                if (item.NgaySinh is DateOnly dob)
                {
                    dtpNgaySinh.Value = dob.ToDateTime(TimeOnly.MinValue);
                }

                cboPhong.SelectedValue = (int)item.MaPhong;

                _isLoadingAddress = true;
                cboTinh.Text = item.TenTinh;
                cboPhuongXa.Text = item.TenPhuongXa;
                _isLoadingAddress = false;

                chkTamTru.Checked = (bool)item.DaDangKyTamTru;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách thuê!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            if (cboPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPhong.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboTinh.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Tỉnh/Thành!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTinh.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboPhuongXa.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Phường/Xã!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPhuongXa.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using var db = new QL_NhaTroContext();

                int maPhong = Convert.ToInt32(cboPhong.SelectedValue);

                int maTinh = 0;
                string tenTinh = cboTinh.Text.Trim();
                if (cboTinh.SelectedItem is ProvinceDto prov)
                {
                    maTinh = prov.Code;
                    tenTinh = prov.Name;
                }

                int maPhuong = 0;
                string tenPhuong = cboPhuongXa.Text.Trim();
                if (cboPhuongXa.SelectedItem is WardDto ward)
                {
                    maPhuong = ward.Code;
                    tenPhuong = ward.Name;
                }

                var khach = new KhachThue
                {
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                    NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value.Date),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    MaPhong = maPhong,
                    MaTinh = maTinh,
                    TenTinh = tenTinh,
                    MaPhuongXa = maPhuong,
                    TenPhuongXa = tenPhuong,
                    DaDangKyTamTru = chkTamTru.Checked
                };

                db.KhachThues.Add(khach);

                // Cập nhật trạng thái phòng thành "Đang thuê"
                var phong = db.PhongTros.FirstOrDefault(p => p.MaPhong == maPhong);
                if (phong != null)
                {
                    phong.TinhTrang = "Đang thuê";
                }

                db.SaveChanges();

                Log.Information("Thêm khách thuê thành công: MaKhach={MaKhach}, HoTen={HoTen}, Phong={Phong}",
                    khach.MaKhach, khach.HoTen, phong?.TenPhong);

                MessageBox.Show("Thêm mới khách thuê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetInput();
                LoadPhongComboBox();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thêm khách thuê vào CSDL");
                MessageBox.Show("Có lỗi xảy ra khi lưu khách thuê!", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách thuê từ danh sách để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                using var db = new QL_NhaTroContext();
                var khach = db.KhachThues.FirstOrDefault(k => k.MaKhach == _selectedId);
                if (khach == null)
                {
                    MessageBox.Show("Khách thuê không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                int oldMaPhong = khach.MaPhong;
                int newMaPhong = Convert.ToInt32(cboPhong.SelectedValue);

                khach.HoTen = txtHoTen.Text.Trim();
                khach.GioiTinh = rdoNam.Checked ? "Nam" : "Nữ";
                khach.NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value.Date);
                khach.SoDienThoai = txtSoDienThoai.Text.Trim();
                khach.MaPhong = newMaPhong;

                if (cboTinh.SelectedItem is ProvinceDto prov)
                {
                    khach.MaTinh = prov.Code;
                    khach.TenTinh = prov.Name;
                }
                else
                {
                    khach.TenTinh = cboTinh.Text.Trim();
                }

                if (cboPhuongXa.SelectedItem is WardDto ward)
                {
                    khach.MaPhuongXa = ward.Code;
                    khach.TenPhuongXa = ward.Name;
                }
                else
                {
                    khach.TenPhuongXa = cboPhuongXa.Text.Trim();
                }

                khach.DaDangKyTamTru = chkTamTru.Checked;

                // Cập nhật trạng thái phòng mới thành Đang thuê
                var newPhong = db.PhongTros.FirstOrDefault(p => p.MaPhong == newMaPhong);
                if (newPhong != null) newPhong.TinhTrang = "Đang thuê";

                db.SaveChanges();

                // Nếu chuyển phòng, kiểm tra phòng cũ có còn ai không
                if (oldMaPhong != newMaPhong)
                {
                    bool conNguoi = db.KhachThues.Any(k => k.MaPhong == oldMaPhong);
                    if (!conNguoi)
                    {
                        var oldPhong = db.PhongTros.FirstOrDefault(p => p.MaPhong == oldMaPhong);
                        if (oldPhong != null)
                        {
                            oldPhong.TinhTrang = "Trống";
                            db.SaveChanges();
                        }
                    }
                }

                Log.Information("Cập nhật khách thuê thành công: MaKhach={MaKhach}, HoTen={HoTen}",
                    khach.MaKhach, khach.HoTen);

                MessageBox.Show("Cập nhật thông tin khách thuê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetInput();
                LoadPhongComboBox();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi cập nhật khách thuê MaKhach={MaKhach}", _selectedId);
                MessageBox.Show("Có lỗi xảy ra khi cập nhật khách thuê!", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách thuê từ danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách thuê mã {_selectedId} không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new QL_NhaTroContext();
                var khach = db.KhachThues.FirstOrDefault(k => k.MaKhach == _selectedId);
                if (khach != null)
                {
                    int maPhong = khach.MaPhong;
                    db.KhachThues.Remove(khach);
                    db.SaveChanges();

                    // Kiểm tra xem phòng còn người thuê không; nếu không còn thì chuyển về "Trống"
                    bool conNguoi = db.KhachThues.Any(k => k.MaPhong == maPhong);
                    if (!conNguoi)
                    {
                        var phong = db.PhongTros.FirstOrDefault(p => p.MaPhong == maPhong);
                        if (phong != null)
                        {
                            phong.TinhTrang = "Trống";
                            db.SaveChanges();
                        }
                    }

                    Log.Information("Xóa khách thuê thành công: MaKhach={MaKhach}", _selectedId);
                    MessageBox.Show("Đã xóa khách thuê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ResetInput();
                LoadPhongComboBox();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xóa khách thuê MaKhach={MaKhach}", _selectedId);
                MessageBox.Show("Có lỗi xảy ra khi xóa khách thuê!", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void ResetInput()
        {
            _selectedId = -1;
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            rdoNam.Checked = true;
            dtpNgaySinh.Value = new DateTime(2002, 1, 1);
            if (cboPhong.Items.Count > 0) cboPhong.SelectedIndex = 0;
            if (cboTinh.Items.Count > 0) cboTinh.SelectedIndex = -1;
            cboTinh.Text = "";
            cboPhuongXa.DataSource = null;
            cboPhuongXa.Text = "";
            chkTamTru.Checked = false;
            dgvKhachThue.ClearSelection();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnBoLoc_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLocPhong.SelectedIndex = 0;
            cboLocTinh.SelectedIndex = 0;
            LoadData();
        }

        private void btnGayLoiThu_Click(object sender, EventArgs e)
        {
            // Cố tình ném ngoại lệ chưa bắt để kiểm tra cơ chế Global Exception Handler
            throw new InvalidOperationException("Đây là ngoại lệ thử nghiệm để kiểm thử bộ xử lý lỗi toàn cục!");
        }
    }
}
