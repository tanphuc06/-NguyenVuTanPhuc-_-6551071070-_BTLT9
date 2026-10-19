using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Cau1_SoChiTieu.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau1_SoChiTieu
{
    public partial class FormChiTieu : Form
    {
        private int _selectedId = -1;

        public FormChiTieu()
        {
            InitializeComponent();
        }

        private void FormChiTieu_Load(object sender, EventArgs e)
        {
            try
            {
                cboLoaiChi.SelectedIndex = 0;
                cboLocLoaiChi.SelectedIndex = 0;

                // Mặc định khoảng ngày tìm kiếm: từ đầu tháng đến cuối tháng hiện tại
                var today = DateTime.Today;
                dtpNgayChi.Value = today;
                dtpTuNgay.Value = new DateTime(today.Year, today.Month, 1);
                dtpDenNgay.Value = today;

                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi khởi động FormChiTieu");
                MessageBox.Show("Không thể tải dữ liệu chi tiêu. Vui lòng thử lại sau!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                using var db = new QL_ChiTieuContext();
                var query = db.ChiTieus.AsNoTracking().AsQueryable();

                // Lọc theo nội dung
                var keyword = txtTimNoiDung.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x => x.NoiDung.Contains(keyword));
                }

                // Lọc theo loại chi
                var loai = cboLocLoaiChi.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(loai) && loai != "Tất cả")
                {
                    query = query.Where(x => x.LoaiChi == loai);
                }

                // Lọc theo khoảng ngày
                var tuNgay = DateOnly.FromDateTime(dtpTuNgay.Value.Date);
                var denNgay = DateOnly.FromDateTime(dtpDenNgay.Value.Date);
                query = query.Where(x => x.NgayChi >= tuNgay && x.NgayChi <= denNgay);

                var list = query.OrderByDescending(x => x.NgayChi)
                                .ThenByDescending(x => x.MaCt)
                                .ToList();

                var displayList = list.Select(x => new
                {
                    x.MaCt,
                    NgayChiStr = x.NgayChi.ToString("dd/MM/yyyy"),
                    x.NoiDung,
                    x.LoaiChi,
                    SoTien = x.SoTien,
                    SoTienFormatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} đ", x.SoTien),
                    x.GhiChu
                }).ToList();

                dgvChiTieu.DataSource = displayList;

                // Tính tổng chi hiển thị
                decimal tongChi = list.Sum(x => x.SoTien);
                lblTongChi.Text = string.Format(new CultureInfo("vi-VN"), "Tổng chi: {0:N0} đ", tongChi);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi truy vấn CSDL chi tiêu");
                MessageBox.Show("Có lỗi xảy ra khi truy vấn dữ liệu chi tiêu!", 
                    "Lỗi kết nối CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvChiTieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTieu.Rows.Count) return;

            var row = dgvChiTieu.Rows[e.RowIndex];
            if (row.Cells["colMa"].Value != null)
            {
                _selectedId = Convert.ToInt32(row.Cells["colMa"].Value);
                txtNoiDung.Text = row.Cells["colNoiDung"].Value?.ToString() ?? "";
                
                var loai = row.Cells["colLoaiChi"].Value?.ToString();
                if (!string.IsNullOrEmpty(loai))
                {
                    cboLoaiChi.SelectedItem = loai;
                }

                var ngayStr = row.Cells["colNgayChi"].Value?.ToString();
                if (DateTime.TryParseExact(ngayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                {
                    dtpNgayChi.Value = d;
                }

                if (decimal.TryParse(row.Cells["colSoTien"].Value?.ToString(), out var tien))
                {
                    numSoTien.Value = Math.Min(numSoTien.Maximum, Math.Max(numSoTien.Minimum, tien));
                }

                txtGhiChu.Text = row.Cells["colGhiChu"].Value?.ToString() ?? "";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNoiDung.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung chi tiêu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNoiDung.Focus();
                return false;
            }

            if (numSoTien.Value <= 0)
            {
                MessageBox.Show("Số tiền phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoTien.Focus();
                return false;
            }

            if (dtpNgayChi.Value.Date > DateTime.Today)
            {
                MessageBox.Show("Ngày chi không được lớn hơn ngày hiện tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayChi.Focus();
                return false;
            }

            if (cboLoaiChi.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại chi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiChi.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using var db = new QL_ChiTieuContext();
                var item = new ChiTieu
                {
                    NgayChi = DateOnly.FromDateTime(dtpNgayChi.Value.Date),
                    NoiDung = txtNoiDung.Text.Trim(),
                    LoaiChi = cboLoaiChi.SelectedItem!.ToString()!,
                    SoTien = numSoTien.Value,
                    GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
                };

                db.ChiTieus.Add(item);
                db.SaveChanges();

                Log.Information("Thêm khoản chi thành công: MaCT={MaCT}, NoiDung={NoiDung}, SoTien={SoTien}", 
                    item.MaCt, item.NoiDung, item.SoTien);

                MessageBox.Show("Thêm mới khoản chi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thêm khoản chi vào CSDL");
                MessageBox.Show("Không thể thêm khoản chi. Vui lòng kiểm tra lại kết nối CSDL!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một khoản chi từ danh sách để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                using var db = new QL_ChiTieuContext();
                var item = db.ChiTieus.FirstOrDefault(x => x.MaCt == _selectedId);
                if (item == null)
                {
                    MessageBox.Show("Khoản chi không tồn tại hoặc đã bị xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                item.NgayChi = DateOnly.FromDateTime(dtpNgayChi.Value.Date);
                item.NoiDung = txtNoiDung.Text.Trim();
                item.LoaiChi = cboLoaiChi.SelectedItem!.ToString()!;
                item.SoTien = numSoTien.Value;
                item.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();

                db.SaveChanges();

                Log.Information("Sửa khoản chi thành công: MaCT={MaCT}, NoiDung={NoiDung}, SoTien={SoTien}", 
                    item.MaCt, item.NoiDung, item.SoTien);

                MessageBox.Show("Cập nhật khoản chi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi cập nhật khoản chi MaCT={MaCT}", _selectedId);
                MessageBox.Show("Không thể sửa khoản chi. Vui lòng kiểm tra lại kết nối CSDL!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một khoản chi từ danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khoản chi mã {_selectedId} không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new QL_ChiTieuContext();
                var item = db.ChiTieus.FirstOrDefault(x => x.MaCt == _selectedId);
                if (item != null)
                {
                    db.ChiTieus.Remove(item);
                    db.SaveChanges();

                    Log.Information("Xóa khoản chi thành công: MaCT={MaCT}", _selectedId);
                    MessageBox.Show("Đã xóa khoản chi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ResetInput();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xóa khoản chi MaCT={MaCT}", _selectedId);
                MessageBox.Show("Không thể xóa khoản chi. Vui lòng kiểm tra lại kết nối CSDL!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void ResetInput()
        {
            _selectedId = -1;
            txtNoiDung.Clear();
            txtGhiChu.Clear();
            numSoTien.Value = 0;
            dtpNgayChi.Value = DateTime.Today;
            if (cboLoaiChi.Items.Count > 0) cboLoaiChi.SelectedIndex = 0;
            dgvChiTieu.ClearSelection();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnBoLoc_Click(object sender, EventArgs e)
        {
            txtTimNoiDung.Clear();
            cboLocLoaiChi.SelectedIndex = 0;
            var today = DateTime.Today;
            dtpTuNgay.Value = new DateTime(today.Year, 1, 1);
            dtpDenNgay.Value = today;
            LoadData();
        }
    }
}
