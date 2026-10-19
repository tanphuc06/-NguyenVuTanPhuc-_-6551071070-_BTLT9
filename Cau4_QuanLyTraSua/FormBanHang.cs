using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau4_QuanLyTraSua.Configuration;
using Cau4_QuanLyTraSua.Models;
using Cau4_QuanLyTraSua.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau4_QuanLyTraSua
{
    public class CartItem
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
        public string DonGiaFormatted => string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", DonGia);
        public string ThanhTienFormatted => string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", ThanhTien);
    }

    public partial class FormBanHang : Form
    {
        private readonly BindingList<CartItem> _cart = new();
        private readonly PaymentConfig _paymentConfig;
        private int _selectedThucDonId = -1;
        private int _selectedHoaDonId = -1;
        private string _pendingPaymentMethod = "Tiền mặt";
        private int _nextTempInvoiceId = 1024;

        public FormBanHang()
        {
            InitializeComponent();
            var settings = AppSettings.Load();
            _paymentConfig = settings.Payment ?? new PaymentConfig();
        }

        private void FormBanHang_Load(object sender, EventArgs e)
        {
            dgvGioHang.AutoGenerateColumns = false;
            dgvGioHang.DataSource = _cart;

            lblQRTenQuan.Text = _paymentConfig.ShopName;
            lblQRThongTinTK.Text = $"STK: {_paymentConfig.AccountNo} ({_paymentConfig.BankId})";

            LoadCategories();
            LoadThucDon();
            LoadHoaDonList();
        }

        #region TAB 1: BÁN HÀNG & VIETQR

        private void LoadCategories()
        {
            try
            {
                using var db = new QL_TraSuaContext();
                var dms = db.DanhMucMons.AsNoTracking().ToList();

                // ComboBox chọn danh mục bán hàng
                cboBanHangDanhMuc.DataSource = new List<DanhMucMon>(dms);
                cboBanHangDanhMuc.DisplayMember = "TenDm";
                cboBanHangDanhMuc.ValueMember = "MaDm";

                // ComboBox quản lý thực đơn
                cboThucDonDanhMuc.DataSource = new List<DanhMucMon>(dms);
                cboThucDonDanhMuc.DisplayMember = "TenDm";
                cboThucDonDanhMuc.ValueMember = "MaDm";

                // ComboBox lọc thực đơn
                var locList = new List<DanhMucMon> { new DanhMucMon { MaDm = 0, TenDm = "Tất cả danh mục" } };
                locList.AddRange(dms);
                cboLocDMMon.DataSource = locList;
                cboLocDMMon.DisplayMember = "TenDm";
                cboLocDMMon.ValueMember = "MaDm";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi nạp danh mục món");
            }
        }

        private void cboBanHangDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBanHangDanhMuc.SelectedValue is int maDm && maDm > 0)
            {
                LoadMonByDanhMuc(maDm);
            }
        }

        private void LoadMonByDanhMuc(int maDm)
        {
            try
            {
                using var db = new QL_TraSuaContext();
                var mons = db.MonUongs
                    .Where(m => m.MaDm == maDm && m.DangBan == true)
                    .AsNoTracking()
                    .ToList();

                var displayItems = mons.Select(m => new
                {
                    m.MaMon,
                    m.TenMon,
                    m.GiaBan,
                    DisplayText = $"{m.TenMon} - {string.Format(new CultureInfo("vi-VN"), "{0:N0}", m.GiaBan)}"
                }).ToList();

                lstMonUong.DataSource = displayItems;
                lstMonUong.DisplayMember = "DisplayText";
                lstMonUong.ValueMember = "MaMon";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải món uống theo danh mục {MaDM}", maDm);
            }
        }

        private void btnThemVaoDon_Click(object sender, EventArgs e)
        {
            if (lstMonUong.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một món uống từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dynamic selected = lstMonUong.SelectedItem;
            int maMon = selected.MaMon;
            string tenMon = selected.TenMon;
            decimal giaBan = selected.GiaBan;
            int soLy = (int)numSoLy.Value;

            // Kiểm tra cộng dồn nếu món đã có
            var exist = _cart.FirstOrDefault(c => c.MaMon == maMon);
            if (exist != null)
            {
                exist.SoLuong += soLy;
                dgvGioHang.Refresh();
            }
            else
            {
                _cart.Add(new CartItem
                {
                    MaMon = maMon,
                    TenMon = tenMon,
                    SoLuong = soLy,
                    DonGia = giaBan
                });
            }

            numSoLy.Value = 1;
            UpdateTotalAmount();
        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow?.DataBoundItem is CartItem item)
            {
                _cart.Remove(item);
                UpdateTotalAmount();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa trong giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateTotalAmount()
        {
            decimal total = _cart.Sum(c => c.ThanhTien);
            lblTongTienDon.Text = string.Format(new CultureInfo("vi-VN"), "Tổng tiền: {0:N0} đ", total);
            lblQRSoTien.Text = string.Format(new CultureInfo("vi-VN"), "Số tiền: {0:N0} đ", total);
        }

        private async void btnThanhToanChuyenKhoan_Click(object sender, EventArgs e)
        {
            decimal total = _cart.Sum(c => c.ThanhTien);
            if (total <= 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng chọn món trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _pendingPaymentMethod = "Chuyển khoản";
            _nextTempInvoiceId++;

            string noiDung = $"TRASUA HD{_nextTempInvoiceId}";
            lblQRNoiDung.Text = $"Nội dung: {noiDung}";
            lblQRSoTien.Text = string.Format(new CultureInfo("vi-VN"), "Số tiền: {0:N0} đ", total);

            btnThanhToanChuyenKhoan.Enabled = false;
            try
            {
                var result = await VietQRService.GenerateVietQRAsync(_paymentConfig, total, noiDung, _nextTempInvoiceId);
                picQR.Image = result.QrImage;

                if (!result.Success)
                {
                    MessageBox.Show("Không thể tải ảnh QR trực tuyến. Bạn có thể xem số tài khoản hiển thị để khách chuyển tay hoặc chọn Tiền mặt.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xử lý thanh toán QR");
            }
            finally
            {
                btnThanhToanChuyenKhoan.Enabled = true;
            }
        }

        private void btnTienMat_Click(object sender, EventArgs e)
        {
            decimal total = _cart.Sum(c => c.ThanhTien);
            if (total <= 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng chọn món trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _pendingPaymentMethod = "Tiền mặt";
            picQR.Image = null;
            lblQRNoiDung.Text = "Thanh toán bằng Tiền mặt";
            lblQRSoTien.Text = string.Format(new CultureInfo("vi-VN"), "Số tiền: {0:N0} đ", total);

            var confirm = MessageBox.Show($"Xác nhận thanh toán TIỀN MẶT {total:N0} đ và in hóa đơn?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                LuuHoaDon("Tiền mặt");
            }
        }

        private void btnDaNhanTien_Click(object sender, EventArgs e)
        {
            decimal total = _cart.Sum(c => c.ThanhTien);
            if (total <= 0)
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Xác nhận ĐÃ NHẬN TIỀN ({_pendingPaymentMethod}: {total:N0} đ) để lưu hóa đơn?",
                "Xác nhận đã nhận tiền", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                LuuHoaDon(_pendingPaymentMethod);
            }
        }

        private void LuuHoaDon(string hinhThuc)
        {
            try
            {
                decimal total = _cart.Sum(c => c.ThanhTien);

                using var db = new QL_TraSuaContext();
                
                var hd = new HoaDon
                {
                    NgayLap = DateTime.Now,
                    TongTien = total,
                    HinhThucTt = hinhThuc,
                    DaThanhToan = true
                };

                db.HoaDons.Add(hd);
                db.SaveChanges(); // Tạo MaHD

                foreach (var item in _cart)
                {
                    db.ChiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        MaHd = hd.MaHd,
                        MaMon = item.MaMon,
                        SoLy = item.SoLuong,
                        DonGia = item.DonGia
                    });
                }

                db.SaveChanges(); // Lưu chi tiết

                Log.Information("Lưu hóa đơn thành công: MaHD={MaHD}, TongTien={TongTien}, HinhThuc={HinhThuc}",
                    hd.MaHd, hd.TongTien, hd.HinhThucTt);

                MessageBox.Show($"Thanh toán thành công hóa đơn #{hd.MaHd}!\nTổng tiền: {hd.TongTien:N0} đ\nHình thức: {hd.HinhThucTt}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Dọn giỏ hàng
                _cart.Clear();
                UpdateTotalAmount();
                picQR.Image = null;
                lblQRNoiDung.Text = "Nội dung: TRASUA HD...";

                // Cập nhật lại tab hóa đơn
                LoadHoaDonList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi lưu hóa đơn vào CSDL");
                MessageBox.Show("Không thể lưu hóa đơn. Vui lòng kiểm tra lại CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region TAB 2: QUẢN LÝ THỰC ĐƠN

        private void LoadThucDon()
        {
            try
            {
                using var db = new QL_TraSuaContext();
                var query = db.MonUongs.Include(m => m.MaDmNavigation).AsNoTracking().AsQueryable();

                var kw = txtTimTenMon.Text.Trim();
                if (!string.IsNullOrEmpty(kw))
                {
                    query = query.Where(m => m.TenMon.Contains(kw));
                }

                if (cboLocDMMon.SelectedValue is int maDmLoc && maDmLoc > 0)
                {
                    query = query.Where(m => m.MaDm == maDmLoc);
                }

                var list = query.OrderBy(m => m.MaDm).ThenBy(m => m.TenMon).ToList();

                var displayList = list.Select(m => new
                {
                    m.MaMon,
                    m.TenMon,
                    TenDM = m.MaDmNavigation != null ? m.MaDmNavigation.TenDm : "",
                    m.GiaBan,
                    GiaBanFormatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", m.GiaBan),
                    m.DangBan,
                    m.MaDm
                }).ToList();

                dgvThucDon.DataSource = displayList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi nạp danh sách thực đơn");
            }
        }

        private void dgvThucDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvThucDon.Rows.Count) return;

            var row = dgvThucDon.Rows[e.RowIndex];
            _selectedThucDonId = Convert.ToInt32(row.Cells["colTD_MaMon"].Value);
            txtTenMon.Text = row.Cells["colTD_TenMon"].Value?.ToString() ?? "";

            if (dgvThucDon.DataSource is IEnumerable<dynamic> list)
            {
                var item = list.ElementAt(e.RowIndex);
                cboThucDonDanhMuc.SelectedValue = (int)item.MaDm;
                numGiaBan.Value = Math.Min(numGiaBan.Maximum, Math.Max(numGiaBan.Minimum, (decimal)item.GiaBan));
                chkDangBan.Checked = (bool)item.DangBan;
            }
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return;
            }

            if (cboThucDonDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new QL_TraSuaContext();
                var item = new MonUong
                {
                    TenMon = txtTenMon.Text.Trim(),
                    MaDm = Convert.ToInt32(cboThucDonDanhMuc.SelectedValue),
                    GiaBan = numGiaBan.Value,
                    DangBan = chkDangBan.Checked
                };

                db.MonUongs.Add(item);
                db.SaveChanges();

                Log.Information("Thêm món mới thành công: MaMon={MaMon}, TenMon={TenMon}", item.MaMon, item.TenMon);
                MessageBox.Show("Thêm món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetThucDonInput();
                LoadThucDon();
                cboBanHangDanhMuc_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thêm món");
                MessageBox.Show("Không thể thêm món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaMon_Click(object sender, EventArgs e)
        {
            if (_selectedThucDonId <= 0)
            {
                MessageBox.Show("Vui lòng chọn món để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var db = new QL_TraSuaContext();
                var item = db.MonUongs.FirstOrDefault(m => m.MaMon == _selectedThucDonId);
                if (item != null)
                {
                    item.TenMon = txtTenMon.Text.Trim();
                    item.MaDm = Convert.ToInt32(cboThucDonDanhMuc.SelectedValue);
                    item.GiaBan = numGiaBan.Value;
                    item.DangBan = chkDangBan.Checked;

                    db.SaveChanges();
                    Log.Information("Sửa món thành công: MaMon={MaMon}", item.MaMon);
                    MessageBox.Show("Cập nhật món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetThucDonInput();
                    LoadThucDon();
                    cboBanHangDanhMuc_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi sửa món");
                MessageBox.Show("Không thể sửa món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            if (_selectedThucDonId <= 0)
            {
                MessageBox.Show("Vui lòng chọn món để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Xác nhận xóa món mã {_selectedThucDonId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using var db = new QL_TraSuaContext();
                var item = db.MonUongs.FirstOrDefault(m => m.MaMon == _selectedThucDonId);
                if (item != null)
                {
                    db.MonUongs.Remove(item);
                    db.SaveChanges();
                    Log.Information("Xóa món thành công: MaMon={MaMon}", _selectedThucDonId);
                    MessageBox.Show("Đã xóa món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetThucDonInput();
                    LoadThucDon();
                    cboBanHangDanhMuc_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xóa món (có thể món đã nằm trong hóa đơn)");
                MessageBox.Show("Không thể xóa món này vì đã có dữ liệu hóa đơn liên quan! Bạn có thể chuyển sang trạng thái Ngừng bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamMoiMon_Click(object sender, EventArgs e)
        {
            ResetThucDonInput();
        }

        private void ResetThucDonInput()
        {
            _selectedThucDonId = -1;
            txtTenMon.Clear();
            numGiaBan.Value = 30000;
            chkDangBan.Checked = true;
            if (cboThucDonDanhMuc.Items.Count > 0) cboThucDonDanhMuc.SelectedIndex = 0;
            dgvThucDon.ClearSelection();
        }

        private void btnTimMon_Click(object sender, EventArgs e)
        {
            LoadThucDon();
        }

        private void btnBoLocMon_Click(object sender, EventArgs e)
        {
            txtTimTenMon.Clear();
            cboLocDMMon.SelectedIndex = 0;
            LoadThucDon();
        }

        #endregion

        #region TAB 3: DANH SÁCH HÓA ĐƠN & DOANH THU

        private void btnLocHD_Click(object sender, EventArgs e)
        {
            LoadHoaDonList();
        }

        private void LoadHoaDonList()
        {
            try
            {
                using var db = new QL_TraSuaContext();
                var date = dtpLocNgayHD.Value.Date;
                var nextDate = date.AddDays(1);

                var query = db.HoaDons
                    .Where(h => h.NgayLap >= date && h.NgayLap < nextDate)
                    .OrderByDescending(h => h.NgayLap)
                    .ToList();

                var displayList = query.Select(h => new
                {
                    h.MaHd,
                    NgayLapStr = h.NgayLap.ToString("dd/MM/yyyy HH:mm:ss"),
                    HinhThucTt = h.HinhThucTt,
                    TongTien = h.TongTien,
                    TongTienFormatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", h.TongTien)
                }).ToList();

                dgvHoaDon.DataSource = displayList;

                decimal doanhThu = query.Sum(h => h.TongTien);
                lblDoanhThuNgay.Text = string.Format(new CultureInfo("vi-VN"), "Doanh thu ngày {0:dd/MM/yyyy}: {1:N0} đ", date, doanhThu);

                dgvChiTietHD.DataSource = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi nạp danh sách hóa đơn");
            }
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvHoaDon.Rows.Count) return;

            var row = dgvHoaDon.Rows[e.RowIndex];
            _selectedHoaDonId = Convert.ToInt32(row.Cells["colHD_MaHD"].Value);

            LoadChiTietHoaDon(_selectedHoaDonId);
        }

        private void LoadChiTietHoaDon(int maHd)
        {
            try
            {
                using var db = new QL_TraSuaContext();
                var list = db.ChiTietHoaDons
                    .Include(ct => ct.MaMonNavigation)
                    .Where(ct => ct.MaHd == maHd)
                    .ToList();

                var displayList = list.Select(ct => new
                {
                    TenMon = ct.MaMonNavigation != null ? ct.MaMonNavigation.TenMon : $"Món #{ct.MaMon}",
                    ct.SoLy,
                    DonGiaFormatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", ct.DonGia),
                    ThanhTienFormatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", ct.SoLy * ct.DonGia)
                }).ToList();

                dgvChiTietHD.DataSource = displayList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi nạp chi tiết hóa đơn {MaHD}", maHd);
            }
        }

        #endregion
    }
}
