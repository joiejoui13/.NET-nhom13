using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : UserControl
    {
        private readonly BLL.Services.Sales.IDeliveryService _deliveryService;
        private bool isAddingNew = false;

        public ucDelivery()
        {
            InitializeComponent();
            _deliveryService = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<BLL.Services.Sales.IDeliveryService>(Program.ServiceProvider);
        }

        private async void ucDelivery_Load(object sender, EventArgs e)
        {
            // Thiết lập ComboBox trạng thái
            cboTrangThaiGiao.Items.Clear();
            cboTrangThaiGiao.Items.AddRange(new object[] { "Chờ giao", "Đang giao", "Đã giao", "Đã hủy" });
            cboTrangThaiGiao.SelectedIndex = 0;

            // Thiết lập sự kiện cho RadioButtons
            guna2CustomRadioButton1.CheckedChanged += RadioButton_CheckedChanged;
            guna2CustomRadioButton2.CheckedChanged += RadioButton_CheckedChanged;

            // Mặc định chọn Mã hóa đơn
            guna2CustomRadioButton1.Checked = true;

            // Đổi tên cột trong GridView
            dgvDeliveries.Columns["colMaHoaDon"].HeaderText = "Mã HĐ/ĐH";

            dtpNgayGiao.ValueChanged -= dtpNgayGiao_ValueChanged;
            dtpNgayGiao.ValueChanged += dtpNgayGiao_ValueChanged;

            await LoadDataAsync();
            ResetState();
        }

        private void dtpNgayGiao_ValueChanged(object sender, EventArgs e)
        {
            if (dtpNgayGiao.CustomFormat == " ")
            {
                dtpNgayGiao.Format = DateTimePickerFormat.Short;
            }
        }

        private async Task LoadDataAsync()
        {
            dgvDeliveries.Rows.Clear();
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveriesAsync();
                foreach (var d in deliveries)
                {
                    string hienThiMa = d.MaHoaDon.HasValue ? "HĐ: " + d.MaHoaDon : "ĐH: " + d.MaTraHang;
                    string ngayGiao = d.NgayGiao.HasValue ? d.NgayGiao.Value.ToString("dd/MM/yyyy") : "";

                    dgvDeliveries.Rows.Add(d.MaGiaoHang.ToString(), hienThiMa, d.DiaChiGiao ?? "", d.TrangThaiGiao ?? "", ngayGiao);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void ResetValues()
        {
            txtMaGiaoHang.Text = "";
            txtMaHoaDon.Text = "";
            guna2TextBox1.Text = "";
            txtDiaChiGiao.Text = "";
            
            cboTrangThaiGiao.SelectedIndex = -1;
            dtpNgayGiao.Format = DateTimePickerFormat.Short;
            dtpNgayGiao.Value = DateTime.Now;

            guna2CustomRadioButton1.Checked = true;
        }

        private void ToggleInputs(bool isEnabled)
        {
            guna2CustomRadioButton1.Enabled = isEnabled;
            guna2CustomRadioButton2.Enabled = isEnabled;

            if (isEnabled)
            {
                if (guna2CustomRadioButton1.Checked)
                {
                    txtMaHoaDon.Enabled = true;
                    guna2TextBox1.Enabled = false;
                }
                else
                {
                    txtMaHoaDon.Enabled = false;
                    guna2TextBox1.Enabled = true;
                }
            }
            else
            {
                txtMaHoaDon.Enabled = false;
                guna2TextBox1.Enabled = false;
            }

            txtDiaChiGiao.Enabled = isEnabled;
            cboTrangThaiGiao.Enabled = isEnabled;
            dtpNgayGiao.Enabled = isEnabled;
        }

        private void ResetState()
        {
            isAddingNew = false;
            ResetValues();

            txtMaGiaoHang.Enabled = false;
            ToggleInputs(false);

            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
            btnRefresh.Enabled = true;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Chỉ đổi trạng thái input khi form đang mở khóa nhập liệu (txtDiaChiGiao mở)
            if (txtDiaChiGiao.Enabled)
            {
                if (guna2CustomRadioButton1.Checked)
                {
                    txtMaHoaDon.Enabled = true;
                    guna2TextBox1.Enabled = false;
                    guna2TextBox1.Text = "";
                }
                else
                {
                    txtMaHoaDon.Enabled = false;
                    guna2TextBox1.Enabled = true;
                    txtMaHoaDon.Text = "";
                }
            }
        }

        private async void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDeliveries.Rows[e.RowIndex];
                string idStr = row.Cells["colMaGiaoHang"].Value?.ToString();
                txtMaGiaoHang.Text = idStr;

                try
                {
                    if (int.TryParse(idStr, out int maGH))
                    {
                        var d = await _deliveryService.GetDeliveryByIdAsync(maGH);
                        if (d != null)
                        {
                            if (d.MaHoaDon.HasValue)
                            {
                                guna2CustomRadioButton1.Checked = true;
                                txtMaHoaDon.Text = d.MaHoaDon.ToString();
                                guna2TextBox1.Text = "";
                            }
                            else
                            {
                                guna2CustomRadioButton2.Checked = true;
                                guna2TextBox1.Text = d.MaTraHang.ToString();
                                txtMaHoaDon.Text = "";
                            }
                            txtDiaChiGiao.Text = d.DiaChiGiao;
                            cboTrangThaiGiao.Text = d.TrangThaiGiao ?? "Chờ giao";
                            if (d.NgayGiao.HasValue) dtpNgayGiao.Value = d.NgayGiao.Value;
                        }
                    }
                }
                catch { }

                ToggleInputs(true);
                txtMaGiaoHang.Enabled = false;

                btnAdd.Enabled = false;
                btnSearch.Enabled = true;
                btnRefresh.Enabled = true;
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                btnSave.Enabled = false;
                btnCancel.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            ResetValues();
            ToggleInputs(true);

            txtMaGiaoHang.Text = "Tự động sinh";
            txtMaGiaoHang.Enabled = false; 

            dtpNgayGiao.Enabled = false; // Tự động lấy ngày hiện tại
            cboTrangThaiGiao.Text = "Chờ giao"; // Mặc định

            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;

            if (txtMaHoaDon.Enabled) txtMaHoaDon.Focus();
            else guna2TextBox1.Focus();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaGiaoHang.Text) || isAddingNew || txtMaGiaoHang.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một đơn giao hàng để chỉnh sửa!");
                return;
            }

            var delivery = new DTO.Delivery
            {
                MaGiaoHang = int.Parse(txtMaGiaoHang.Text),
                DiaChiGiao = txtDiaChiGiao.Text.Trim(),
                TrangThaiGiao = cboTrangThaiGiao.Text,
                NgayGiao = dtpNgayGiao.Value
            };

            if (guna2CustomRadioButton1.Checked)
            {
                if (int.TryParse(txtMaHoaDon.Text.Trim(), out int maHD)) delivery.MaHoaDon = maHD;
            }
            else
            {
                if (int.TryParse(guna2TextBox1.Text.Trim(), out int maDH)) delivery.MaTraHang = maDH;
            }

            try
            {
                await _deliveryService.UpdateDeliveryAsync(delivery);
                MessageBox.Show("Lưu thay đổi thành công!");
                await LoadDataAsync();
                ResetState();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY"))
                    MessageBox.Show("Mã hóa đơn hoặc Mã phiếu trả hàng không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!isAddingNew) return;

            var delivery = new DTO.Delivery
            {
                DiaChiGiao = txtDiaChiGiao.Text.Trim(),
                TrangThaiGiao = cboTrangThaiGiao.Text
            };

            if (guna2CustomRadioButton1.Checked)
            {
                if (int.TryParse(txtMaHoaDon.Text.Trim(), out int maHD)) delivery.MaHoaDon = maHD;
                else { MessageBox.Show("Mã hóa đơn không hợp lệ!"); return; }
            }
            else
            {
                if (int.TryParse(guna2TextBox1.Text.Trim(), out int maDH)) delivery.MaTraHang = maDH;
                else { MessageBox.Show("Mã phiếu trả hàng không hợp lệ!"); return; }
            }

            try
            {
                await _deliveryService.AddDeliveryAsync(delivery);
                MessageBox.Show("Thêm đơn giao hàng thành công! Mã giao hàng đã được tự động sinh.");
                await LoadDataAsync();
                ResetState();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY"))
                    MessageBox.Show("Mã hóa đơn hoặc Mã phiếu trả hàng không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaGiaoHang.Text) || txtMaGiaoHang.Text == "Tự động sinh")
            {
                MessageBox.Show("Vui lòng chọn một đơn giao hàng để xóa!");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa (chuyển trạng thái sang Đã hủy) đơn giao hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _deliveryService.SoftDeleteDeliveryAsync(int.Parse(txtMaGiaoHang.Text));
                    MessageBox.Show("Đã xóa (hủy) đơn giao hàng thành công!");
                    await LoadDataAsync();
                    ResetState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
            ResetState();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtMaGiaoHang.Enabled == false)
            {
                ResetValues();
                ToggleInputs(true);
                txtMaGiaoHang.Enabled = true; 

                btnCancel.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                
                dtpNgayGiao.Format = DateTimePickerFormat.Custom;
                dtpNgayGiao.CustomFormat = " ";

                MessageBox.Show("Chế độ tìm kiếm đã BẬT!\nVui lòng nhập các tiêu chí cần lọc vào ô nhập liệu rồi bấm 'Tìm Kiếm' lần nữa.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaGiaoHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMaGiaoHang.Text.Trim()) &&
                string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()) &&
                string.IsNullOrEmpty(guna2TextBox1.Text.Trim()) &&
                cboTrangThaiGiao.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập/chọn ít nhất một thông tin (Mã GH, Mã HĐ, Mã ĐH, Trạng thái) để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvDeliveries.Rows.Clear();
            int? maGH = null, maHD = null, maTH = null;
            if (int.TryParse(txtMaGiaoHang.Text.Trim(), out int temp1)) maGH = temp1;
            if (int.TryParse(txtMaHoaDon.Text.Trim(), out int temp2)) maHD = temp2;
            if (int.TryParse(guna2TextBox1.Text.Trim(), out int temp3)) maTH = temp3;
            
            string status = cboTrangThaiGiao.SelectedIndex != -1 ? cboTrangThaiGiao.Text : null;

            try
            {
                var deliveries = await _deliveryService.SearchDeliveriesAsync(maGH, maHD, maTH, status);
                foreach (var d in deliveries)
                {
                    string hienThiMa = d.MaHoaDon.HasValue ? "HĐ: " + d.MaHoaDon : "ĐH: " + d.MaTraHang;
                    string ngayGiao = d.NgayGiao.HasValue ? d.NgayGiao.Value.ToString("dd/MM/yyyy") : "";

                    dgvDeliveries.Rows.Add(d.MaGiaoHang.ToString(), hienThiMa, d.DiaChiGiao ?? "", d.TrangThaiGiao ?? "", ngayGiao);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetState();
        }

        private void pnlGridCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
