using AssignmentApp.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucReturn : UserControl
    {
        DataTable dtReturn;         
        DataTable dtInvoiceDetails; 
        DataTable dtCart;        


        public ucReturn()
        {
            InitializeComponent();
        }

        private void cboLoaiGiaoDich_SelectedIndexChanged(object? sender, EventArgs e)
        {

        }

        private void ucReturn_Load(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnReturnSearch.Enabled = true;
            btnReturnRefresh.Enabled = true;
            btnAddToCart.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;

            ResetValues();
            ResetValues1();
            KhoiTaoGioHang();
            Load_DataGridView();

        }

        private void KhoiTaoGioHang()
        {
            dtCart = new DataTable();
            dtCart.Columns.Add("colCurMaSP", typeof(int));
            dtCart.Columns.Add("colCurTenSP", typeof(string));
            dtCart.Columns.Add("colCurSoLuong", typeof(int));
            dtCart.Columns.Add("colCurDonGia", typeof(decimal));
            dtCart.Columns.Add("colCurTinhTrang", typeof(string));
            dtCart.Columns.Add("colCurThanhTien", typeof(decimal), "colCurSoLuong * colCurDonGia");

            dgvCurrentDetails.AutoGenerateColumns = false;

            if (dgvCurrentDetails.Columns.Count >= 6)
            {
                colCurMaSP.DataPropertyName = "colCurMaSP";
                colCurTenSP.DataPropertyName = "colCurTenSP";
                colCurSoLuong.DataPropertyName = "colCurSoLuong";
                colCurDonGia.DataPropertyName = "colCurDonGia";
                colCurTinhTrang.DataPropertyName = "colCurTinhTrang";
                colCurThanhTien.DataPropertyName = "colCurThanhTien";
            }

            dgvCurrentDetails.DataSource = dtCart;
            dgvCurrentDetails.AllowUserToAddRows = false;
        }
        private void ResetValues()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0";
            dtpNgayTra.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            cboLoaiGiaoDich.SelectedIndex = -1;
            lblKhachHang.Text = "Khách hàng: (Trống)";
            lblNhanVien.Text = "Nhân viên: (Trống)";
        }

        private void ResetValues1()
        {
            txtSelMaSP.Text = "";
            txtSelTenSP.Text = "";
            txtSelSoLuong.Text = "";
            txtSelDonGia.Text = "";
            txtSelTinhTrang.Text = "";
            txtSelMaSP.Enabled = false;
            txtSelTenSP.Enabled = false;
            txtSelDonGia.Enabled = false;
        }
        private void Load_DataGridView()
        {
            string sql = @"
                SELECT 
                    th.MaTraHang,
                    th.MaHoaDon,
                    th.TrangThai,
                    th.LoaiGiaoDich,
                    th.TongTienHoan,
                    nd.TenNguoiDung AS NhanVien,
                    th.NgayTra,
                    th.LyDo,
                    kh.TenKhachHang AS KhachHang
                FROM TraHang th
                JOIN NguoiDung  nd ON th.MaNguoiDung  = nd.MaNguoiDung
                JOIN HoaDon     hd ON th.MaHoaDon      = hd.MaHoaDon
                JOIN KhachHang  kh ON hd.MaKhachHang   = kh.MaKhachHang";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtReturn = new DataTable();
            da.Fill(dtReturn);

       
            colMaTraHang.DataPropertyName = "MaTraHang";
            colMaHoaDon.DataPropertyName = "MaHoaDon";
            colTrangThai.DataPropertyName = "TrangThai";
            colLoaiGiaoDich.DataPropertyName = "LoaiGiaoDich";
            colTongTienHoan.DataPropertyName = "TongTienHoan";
            colNhanVien.DataPropertyName = "NhanVien";
            colNgayTra.DataPropertyName = "NgayTra";
            colLyDo.DataPropertyName = "LyDo";

            dgvReturns.AutoGenerateColumns = false;
            dgvReturns.DataSource = dtReturn;
        }
        // ==========================================
        // SỰ KIỆN TAB 1
        // ==========================================

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập mã hóa đơn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập lý do trả hàng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn loại giao dịch!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            // Kiểm tra hóa đơn này đã có phiếu trả chưa
            string sqlCheck = "SELECT MaHoaDon FROM TraHang WHERE MaHoaDon = @mahd";
            SqlCommand cmdCheck = new SqlCommand(sqlCheck, (SqlConnection)DbContext.Conn);
            cmdCheck.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
            SqlDataReader reader = cmdCheck.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                MessageBox.Show("Hóa đơn này đã có phiếu trả rồi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }
            reader.Close();

            string sqlInsert = @"INSERT INTO TraHang
                (MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
                VALUES (@mahd, 1, @lydo, 0, @ngaytra, @trangthai, @loaigd)";

            SqlCommand cmd = new SqlCommand(sqlInsert, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
            cmd.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
            cmd.Parameters.AddWithValue("@ngaytra", dtpNgayTra.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@trangthai", cboTrangThai.Text.Trim());
            cmd.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());
            cmd.ExecuteNonQuery();

            MessageBox.Show("Thêm phiếu trả hàng thành công!\nVào Tab 2 để thêm chi tiết sản phẩm trả.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView();
            ResetValues();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;

        }


        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dtReturn == null || dtReturn.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa phiếu trả này không?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = "DELETE FROM TraHang WHERE MaHoaDon = @mahd";
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
                cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Xóa thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Load_DataGridView();
                ResetValues();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;

        }

        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            btnResetCartForm_Click(sender, e);

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            ResetValues();
            Load_DataGridView();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Nhập mã hóa đơn để tìm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            string sql = @"
                SELECT 
                    th.MaTraHang, th.MaHoaDon, th.TrangThai, th.LoaiGiaoDich,
                    th.TongTienHoan, nd.TenNguoiDung AS NhanVien,
                    kh.TenKhachHang AS KhachHang, th.NgayTra, th.LyDo
                FROM TraHang th
                JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
                JOIN HoaDon    hd ON th.MaHoaDon    = hd.MaHoaDon
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE th.MaHoaDon LIKE @mahd";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            da.SelectCommand.Parameters.AddWithValue("@mahd", "%" + txtMaHoaDon.Text.Trim() + "%");

            DataTable dtSearch = new DataTable();
            da.Fill(dtSearch);

            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phiếu trả phù hợp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

           
            dgvReturns.DataSource = dtSearch;
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues();
            Load_DataGridView();

        }

        private void dgvReturns_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHoaDon.Focus();
                return;
            }

            if (dtReturn == null || dtReturn.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (e.RowIndex < 0) return;

         
            txtMaHoaDon.Text = dgvReturns.CurrentRow.Cells["colMaHoaDon"].Value.ToString();
            txtLyDo.Text = dgvReturns.CurrentRow.Cells["colLyDo"].Value.ToString();
            txtTongTienHoan.Text = dgvReturns.CurrentRow.Cells["colTongTienHoan"].Value.ToString();
            dtpNgayTra.Value = Convert.ToDateTime(dgvReturns.CurrentRow.Cells["colNgayTra"].Value);

       
            string trangThai = dgvReturns.CurrentRow.Cells["colTrangThai"].Value.ToString().Trim();
            string loaiGD = dgvReturns.CurrentRow.Cells["colLoaiGiaoDich"].Value.ToString().Trim();

            cboTrangThai.SelectedIndex = -1;
            for (int i = 0; i < cboTrangThai.Items.Count; i++)
            {
                if (cboTrangThai.Items[i].ToString().Trim() == trangThai)
                { cboTrangThai.SelectedIndex = i; break; }
            }

            cboLoaiGiaoDich.SelectedIndex = -1;
            for (int i = 0; i < cboLoaiGiaoDich.Items.Count; i++)
            {
                if (cboLoaiGiaoDich.Items[i].ToString().Trim() == loaiGD)
                { cboLoaiGiaoDich.SelectedIndex = i; break; }
            }

         
            lblNhanVien.Text = "Nhân viên: " + dtReturn.Rows[e.RowIndex]["NhanVien"].ToString();
            lblKhachHang.Text = "Khách hàng: " + dtReturn.Rows[e.RowIndex]["KhachHang"].ToString();

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            if (txtMaHoaDon.Text.Trim() == "") return;

            string sql = @"
                SELECT nd.TenNguoiDung, kh.TenKhachHang
                FROM   HoaDon    hd
                JOIN   NguoiDung nd ON hd.MaNguoiDung = nd.MaNguoiDung
                JOIN   KhachHang kh ON hd.MaKhachHang  = kh.MaKhachHang
                WHERE  hd.MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblNhanVien.Text = "Nhân viên: " + reader["TenNguoiDung"].ToString();
                lblKhachHang.Text = "Khách hàng: " + reader["TenKhachHang"].ToString();
            }
            else
            {
                lblNhanVien.Text = "Nhân viên: (Không tìm thấy hóa đơn)";
                lblKhachHang.Text = "Khách hàng: (Không tìm thấy hóa đơn)";
            }
            reader.Close();

        }


        private void btnChooseProducts_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập mã hóa đơn ở Tab 1 trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tabSelectionContainer != null && tabSelectionContainer.TabPages.Count >= 2)
            {
                tabSelectionContainer.SelectedIndex = 1;
                pnlReturnTop.Tag = txtMaHoaDon.Text.Trim();
                btnReturnSearch_Click(sender, e);
            }
        }

        // ==========================================
        // SỰ KIỆN TAB 2
        // ==========================================



        private void TinhTongTienHoanTra()
        {
            decimal tongTien = 0;
            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (dtCart.Rows[i]["colCurThanhTien"] != DBNull.Value)
                    tongTien += Convert.ToDecimal(dtCart.Rows[i]["colCurThanhTien"]);
            }
            lblTotalAmount.Text = tongTien.ToString("N0") + " VNĐ";
            txtTongTienHoan.Text = tongTien.ToString("N0") + " đ";
        }


        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtInvoiceDetails == null || dtInvoiceDetails.Rows.Count == 0) return;
            if (e.RowIndex < 0) return;

            DataRowView rowView = (DataRowView)dgvProductsSelection.Rows[e.RowIndex].DataBoundItem;

            txtSelMaSP.Text = rowView["MaSanPham"].ToString();
            txtSelTenSP.Text = rowView["TenSanPham"].ToString();
            txtSelDonGia.Text = rowView["DonGia"].ToString();
            txtSelSoLuong.Text = "";
            txtSelTinhTrang.Text = "";

     
            string tenAnh = rowView["Anh"]?.ToString();
            if (tenAnh != null && tenAnh != "")
            {
                string duongDan1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", tenAnh);
                string duongDan2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", tenAnh);

                if (System.IO.File.Exists(duongDan1)) picAnh.Image = System.Drawing.Image.FromFile(duongDan1);
                else if (System.IO.File.Exists(duongDan2)) picAnh.Image = System.Drawing.Image.FromFile(duongDan2);
                else picAnh.Image = null;
            }
            else
            {
                picAnh.Image = null;
            }

            btnAddToCart.Enabled = true;
            btnRemoveFromCart.Enabled = false;
            txtSelSoLuong.Focus();

        }

        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtCart == null || dtCart.Rows.Count == 0) return;
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvCurrentDetails.Rows[e.RowIndex];
            txtSelMaSP.Text = row.Cells["colCurMaSP"].Value?.ToString();
            txtSelTenSP.Text = row.Cells["colCurTenSP"].Value?.ToString();
            txtSelSoLuong.Text = row.Cells["colCurSoLuong"].Value?.ToString();
            txtSelDonGia.Text = row.Cells["colCurDonGia"].Value?.ToString();
            txtSelTinhTrang.Text = row.Cells["colCurTinhTrang"].Value?.ToString();

            btnAddToCart.Enabled = false;
            btnRemoveFromCart.Enabled = true;

        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào để trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            if (txtSelTinhTrang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập tình trạng hàng trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelTinhTrang.Focus();
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

        
            int soLuongMuaGoc = 0;
            for (int i = 0; i < dgvProductsSelection.Rows.Count; i++)
            {
                if (dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value != null &&
                    Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value) == maSP)
                {
                    soLuongMuaGoc = Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelSoLuongMua"].Value);
                    break;
                }
            }

            if (soLuong > soLuongMuaGoc)
            {
                MessageBox.Show("Số lượng trả không được lớn hơn số lượng đã mua (" + soLuongMuaGoc + ")!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                {
                    MessageBox.Show("Sản phẩm này đã có trong danh sách! Chọn dòng bên phải rồi bấm SỬA.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            decimal donGia = 0;
            decimal.TryParse(txtSelDonGia.Text, out donGia);

            DataRow dongMoi = dtCart.NewRow();
            dongMoi["colCurMaSP"] = maSP;
            dongMoi["colCurTenSP"] = txtSelTenSP.Text.Trim();
            dongMoi["colCurSoLuong"] = soLuong;
            dongMoi["colCurDonGia"] = donGia;
            dongMoi["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
            dtCart.Rows.Add(dongMoi);

            TinhTongTienHoanTra();
            ResetValues1();
            btnAddToCart.Enabled = false;

        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            if (MessageBox.Show("Bạn có muốn bỏ sản phẩm này khỏi danh sách hoàn trả không?",
                "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int maSP = int.Parse(txtSelMaSP.Text);

             
                for (int i = dtCart.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                    {
                        dtCart.Rows[i].Delete();
                        break;
                    }
                }
                dtCart.AcceptChanges();

                TinhTongTienHoanTra();
                ResetValues1();
                btnRemoveFromCart.Enabled = false;
            }

        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {
       
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả để lưu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

         
            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập lý do trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

       
            string tongTienChuoi = txtTongTienHoan.Text.Replace("đ", "").Replace(",", "").Trim();
            decimal tongTien = 0;
            decimal.TryParse(tongTienChuoi, out tongTien);

     
            string sql = @"UPDATE TraHang SET
                LyDo         = @lydo,
                TongTienHoan = @tong,
                NgayTra      = @ngaytra,
                TrangThai    = @trangthai,
                LoaiGiaoDich = @loaigd
                WHERE MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
            cmd.Parameters.AddWithValue("@tong", tongTien);
            cmd.Parameters.AddWithValue("@ngaytra", dtpNgayTra.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@trangthai", cboTrangThai.Text.Trim());
            cmd.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
            cmd.ExecuteNonQuery();

            MessageBox.Show("Lưu thay đổi thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView();
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnReturnSearch_Click(object? sender, EventArgs e)
        {
            string maHDInput = Microsoft.VisualBasic.Interaction.InputBox(
               "Nhập Mã Hóa Đơn cần trả hàng:", "Tìm kiếm hóa đơn", "");

            if (maHDInput.Trim() == "") return;

            string sql = @"
                SELECT cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.Anh
                FROM   ChiTietHoaDon cthd
                JOIN   SanPham       sp ON cthd.MaSanPham = sp.MaSanPham
                WHERE  cthd.MaHoaDon = @mahd";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            da.SelectCommand.Parameters.AddWithValue("@mahd", maHDInput.Trim());

            dtInvoiceDetails = new DataTable();
            da.Fill(dtInvoiceDetails);

            if (dtInvoiceDetails.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn hoặc hóa đơn không có sản phẩm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

       
            colSelMaSP.DataPropertyName = "MaSanPham";
            colSelTenSP.DataPropertyName = "TenSanPham";
            colSelSoLuongMua.DataPropertyName = "SoLuong";
            colSelDaTra.DataPropertyName = "";       
            colSelDonGia.DataPropertyName = "DonGia";

            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = dtInvoiceDetails;

        
            pnlReturnTop.Tag = maHDInput.Trim();

       
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();

            btnLuuCT.Enabled = true;

            MessageBox.Show("Đã tìm thấy hóa đơn " + maHDInput + "! Chọn sản phẩm cần trả ở danh sách bên trái.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReturnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues1();
            if (dtInvoiceDetails != null) dtInvoiceDetails.Rows.Clear();
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();
            btnAddToCart.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            pnlReturnTop.Tag = null;

        }

        private void tabSelectionContainer_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Optional picture load when selecting product detail tab
        }

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            if (dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng hoàn trả đang trống! Không thể lưu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pnlReturnTop.Tag == null)
            {
                MessageBox.Show("Chưa tìm hóa đơn gốc! Nhấn 'Tìm' trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maHoaDonGoc = Convert.ToInt32(pnlReturnTop.Tag);

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlTransaction giaoDich = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
                
                decimal tongTienHoan = 0;
                for (int i = 0; i < dtCart.Rows.Count; i++)
                    tongTienHoan += Convert.ToDecimal(dtCart.Rows[i]["colCurThanhTien"]);

              
                string sqlTraHang = @"
                    INSERT INTO TraHang (MaHoaDon, MaNguoiDung, NgayTra, TongTienHoan, LyDo)
                    VALUES (@mahd, 1, GETDATE(), @tong, N'Khách trả hàng theo yêu cầu');
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmdTraHang = new SqlCommand(sqlTraHang, (SqlConnection)DbContext.Conn, giaoDich);
                cmdTraHang.Parameters.AddWithValue("@mahd", maHoaDonGoc);
                cmdTraHang.Parameters.AddWithValue("@tong", tongTienHoan);
                int maTraHangMoi = Convert.ToInt32(cmdTraHang.ExecuteScalar());

          
                for (int i = 0; i < dtCart.Rows.Count; i++)
                {
                    DataRow dong = dtCart.Rows[i];
                    int maSP = Convert.ToInt32(dong["colCurMaSP"]);
                    int soLuongTra = Convert.ToInt32(dong["colCurSoLuong"]);
                    decimal thanhTien = Convert.ToDecimal(dong["colCurThanhTien"]);
                    string tinhTrang = dong["colCurTinhTrang"].ToString();

                 
                    string sqlChiTiet = @"INSERT INTO ChiTietTraHang (MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang)
                        VALUES (@matrahang, @masp, @soluong, @tienhoan, @tinhtrang)";
                    SqlCommand cmdChiTiet = new SqlCommand(sqlChiTiet, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdChiTiet.Parameters.AddWithValue("@matrahang", maTraHangMoi);
                    cmdChiTiet.Parameters.AddWithValue("@masp", maSP);
                    cmdChiTiet.Parameters.AddWithValue("@soluong", soLuongTra);
                    cmdChiTiet.Parameters.AddWithValue("@tienhoan", thanhTien);
                    cmdChiTiet.Parameters.AddWithValue("@tinhtrang", tinhTrang);
                    cmdChiTiet.ExecuteNonQuery();

            
                    string sqlTonKho = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @masp";
                    SqlCommand cmdTonKho = new SqlCommand(sqlTonKho, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdTonKho.Parameters.AddWithValue("@masp", maSP);
                    int tonKhoTruoc = Convert.ToInt32(cmdTonKho.ExecuteScalar());
                    int tonKhoSau = tonKhoTruoc + soLuongTra;

                    string sqlCapNhatKho = "UPDATE SanPham SET SoLuongTon = @sau WHERE MaSanPham = @masp";
                    SqlCommand cmdCapNhat = new SqlCommand(sqlCapNhatKho, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdCapNhat.Parameters.AddWithValue("@sau", tonKhoSau);
                    cmdCapNhat.Parameters.AddWithValue("@masp", maSP);
                    cmdCapNhat.ExecuteNonQuery();

                    // Ghi lịch sử nhập kho
                    string sqlLichSu = @"INSERT INTO LichSuNhapKho
                        (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai)
                        VALUES (@masp, GETDATE(), @thaydoi, @truoc, @sau, N'Trả hàng', @matrahang, N'Hoàn tất')";
                    SqlCommand cmdLichSu = new SqlCommand(sqlLichSu, (SqlConnection)DbContext.Conn, giaoDich);
                    cmdLichSu.Parameters.AddWithValue("@masp", maSP);
                    cmdLichSu.Parameters.AddWithValue("@thaydoi", soLuongTra);
                    cmdLichSu.Parameters.AddWithValue("@truoc", tonKhoTruoc);
                    cmdLichSu.Parameters.AddWithValue("@sau", tonKhoSau);
                    cmdLichSu.Parameters.AddWithValue("@matrahang", maTraHangMoi);
                    cmdLichSu.ExecuteNonQuery();
                }

           
                giaoDich.Commit();
                MessageBox.Show("Lưu đơn hàng hoàn trả thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnReturnRefresh_Click(sender, e);
                Load_DataGridView();
            }
            catch (Exception ex)
            {
               
                giaoDich.Rollback();
                MessageBox.Show("Có lỗi, hệ thống đã hủy toàn bộ thao tác:\n" + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectProduct_Click(object? sender, EventArgs e)
        {

        }

        // tab 1

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtReturn == null || dtReturn.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập lý do trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            string tongTienChuoi = txtTongTienHoan.Text.Replace("đ", "").Replace(",", "").Trim();
            decimal tongTien = 0;
            decimal.TryParse(tongTienChuoi, out tongTien);

            string sql = @"UPDATE TraHang SET
                LyDo         = @lydo,
                TongTienHoan = @tong,
                NgayTra      = @ngaytra,
                TrangThai    = @trangthai,
                LoaiGiaoDich = @loaigd
                WHERE MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@lydo", txtLyDo.Text.Trim());
            cmd.Parameters.AddWithValue("@tong", tongTien);
            cmd.Parameters.AddWithValue("@ngaytra", dtpNgayTra.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@trangthai", cboTrangThai.Text.Trim());
            cmd.Parameters.AddWithValue("@loaigd", cboLoaiGiaoDich.Text.Trim());
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text.Trim());
            cmd.ExecuteNonQuery();

            MessageBox.Show("Sửa phiếu trả thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Load_DataGridView();
            ResetValues();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            int soLuong;
            if (!int.TryParse(txtSelSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

            // Lấy SL mua gốc để kiểm tra
            int soLuongMuaGoc = 0;
            for (int i = 0; i < dgvProductsSelection.Rows.Count; i++)
            {
                if (dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value != null &&
                    Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelMaSP"].Value) == maSP)
                {
                    soLuongMuaGoc = Convert.ToInt32(dgvProductsSelection.Rows[i].Cells["colSelSoLuongMua"].Value);
                    break;
                }
            }

            if (soLuong > soLuongMuaGoc)
            {
                MessageBox.Show("Số lượng trả vượt quá số lượng gốc (" + soLuongMuaGoc + ")!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật dòng trong dtCart
            for (int i = 0; i < dtCart.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtCart.Rows[i]["colCurMaSP"]) == maSP)
                {
                    dtCart.Rows[i]["colCurSoLuong"] = soLuong;
                    dtCart.Rows[i]["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    break;
                }
            }

            TinhTongTienHoanTra();
            ResetValues1();
            btnRemoveFromCart.Enabled = false;

        }
    }
}
