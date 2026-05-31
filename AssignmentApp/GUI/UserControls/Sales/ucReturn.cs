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
 
        DataTable dtCart;
        DataTable dtInvoiceDetails;

        // Các biến lưu trạng thái của Phiếu trả đang chọn ở Tab 1
        private int currentMaTraHang = -1;
        private int currentMaHoaDon = -1;
        private string currentTrangThai = "";

        public ucReturn()
        {
            InitializeComponent();

            // Extracted from Designer
            cboTrangThai.Items.AddRange(new object[] { "Hoàn tất", "", "Chờ xử lý", "", "Đã hủy" });
            cboLoaiGiaoDich.Items.AddRange(new object[] { "Trả hàng", "", "Đổi 1:1" });
        }

        private void cboLoaiGiaoDich_SelectedIndexChanged(object? sender, EventArgs e)
        {

        }

        private void ucReturn_Load(object sender, EventArgs e)
        {
            btnReturnSearch.Enabled = true;
            btnReturnRefresh.Enabled = true;
            btnAddToCart.Enabled = false;
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            ResetValues1();
            KhoiTaoGioHangTamtinh();
            Load_DataGridView();
        }


        private void KhoiTaoGioHangTamtinh()
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
                dgvCurrentDetails.Columns[0].DataPropertyName = "colCurMaSP";
                dgvCurrentDetails.Columns[1].DataPropertyName = "colCurTenSP";
                dgvCurrentDetails.Columns[2].DataPropertyName = "colCurSoLuong";
                dgvCurrentDetails.Columns[3].DataPropertyName = "colCurDonGia";
                dgvCurrentDetails.Columns[4].DataPropertyName = "colCurTinhTrang";
                dgvCurrentDetails.Columns[5].DataPropertyName = "colCurThanhTien";
            }

            dgvCurrentDetails.DataSource = dtCart;
            dgvCurrentDetails.AllowUserToAddRows = false;
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


        DataTable dtReturn;
        private void Load_DataGridView()
        {
            string sql = @"
        SELECT 
            th.MaTraHang,
            th.MaHoaDon,
            th.TrangThai,
            th.LoaiGiaoDich,
            th.TongTienHoan,
            nd.TenNguoiDung AS MaNguoiDung,
            th.NgayTra,
            th.LyDo,
            nd.TenNguoiDung AS NhanVien,
            kh.TenKhachHang AS KhachHang
        FROM TraHang th
        JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
        JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
        JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang";

            if (DbContext.Conn.State == ConnectionState.Closed)
                DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtReturn = new DataTable();
            da.Fill(dtReturn);

            dgvReturns.AutoGenerateColumns = false;
            dgvReturns.DataSource = null;   // reset trước
            dgvReturns.DataSource = dtReturn;


            if (dgvReturns.Columns["NhanVien"] != null)
                dgvReturns.Columns["NhanVien"].Visible = false;
            if (dgvReturns.Columns["KhachHang"] != null)
                dgvReturns.Columns["KhachHang"].Visible = false;
        }

        // ==========================================
        // SỰ KIỆN TAB 1
        // ==========================================
        private void ResetValues()
        {
            txtMaHoaDon.Text = "";
            txtLyDo.Text = "";
            txtTongTienHoan.Text = "0";
            txtTongTienHoan.Enabled = false; // Ngăn không cho tự nhập
            dtpNgayTra.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = -1;
            cboLoaiGiaoDich.SelectedIndex = -1;

            lblKhachHang.Text = "Khách hàng: (Trống)";
            lblNhanVien.Text = "Nhân viên: (Trống)";
            
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            // Reset biến toàn cục
            currentMaTraHang = -1;
            currentMaHoaDon = -1;
            currentTrangThai = "";
        }
        private void btnAdd_Click(object? sender, EventArgs e)
        {
            // Bước 1: Xóa trắng form
            ResetValues();
            
            // Bước 2: Thiết lập trạng thái nút bấm
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            // Bước 3: Đưa trỏ chuột vào vị trí sẵn sàng nhập
            txtMaHoaDon.Enabled = true; // Cho phép nhập mã hóa đơn
            txtMaHoaDon.Focus();
            
            cboTrangThai.Text = "Chờ xử lý";
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dtReturn.Rows.Count == 0 || txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn hủy phiếu trả này?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (!int.TryParse(txtMaHoaDon.Text.Trim(), out int maHoaDon)) return;

                if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

                string sql = "UPDATE TraHang SET TrangThai = N'Đã hủy' WHERE MaHoaDon = @MaHoaDon";
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                
                try { cmd.ExecuteNonQuery(); } catch { }
                
                Load_DataGridView();
                ResetValues();
                
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                txtMaHoaDon.Enabled = false;
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            // Bước 1: Kiểm tra dữ liệu
            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Phải nhập mã hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out int maHoaDon))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }
            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Phải nhập lý do!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }
            if (cboTrangThai.Text.Trim() == "")
            {
                MessageBox.Show("Phải chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }
            if (cboLoaiGiaoDich.Text.Trim() == "")
            {
                MessageBox.Show("Phải chọn loại giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiGiaoDich.Focus();
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            // Kiểm tra mã hóa đơn có tồn tại trong bảng HoaDon không
            string sqlCheckHD = "SELECT MaHoaDon FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            SqlCommand cmdCheckHD = new SqlCommand(sqlCheckHD, (SqlConnection)DbContext.Conn);
            cmdCheckHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
            SqlDataReader readerHD = cmdCheckHD.ExecuteReader();
            if (!readerHD.HasRows)
            {
                readerHD.Close();
                MessageBox.Show("Mã hóa đơn này không tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }
            readerHD.Close();

            // Bước 2: Xử lý tiền
            string tongTienStr = txtTongTienHoan.Text.Trim();
            if (tongTienStr == "") tongTienStr = "0";
            tongTienStr = tongTienStr.Replace(" đ", "").Replace("đ", "").Replace(",", "").Replace(".", "");
            if (!decimal.TryParse(tongTienStr, out decimal tongTien)) tongTien = 0;

            // Bước 3: Tạo SQL và Thực thi
            string sql = @"INSERT INTO TraHang (MaHoaDon, MaNguoiDung, LyDo, TongTienHoan, NgayTra, TrangThai, LoaiGiaoDich)
                           VALUES (@MaHoaDon, @MaNguoiDung, @LyDo, @TongTienHoan, @NgayTra, @TrangThai, @LoaiGiaoDich)";
            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
            cmd.Parameters.AddWithValue("@MaNguoiDung", 1); // Theo logic cũ
            cmd.Parameters.AddWithValue("@LyDo", txtLyDo.Text.Trim());
            cmd.Parameters.AddWithValue("@TongTienHoan", tongTien);
            cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);
            cmd.Parameters.AddWithValue("@TrangThai", cboTrangThai.Text.Trim());
            cmd.Parameters.AddWithValue("@LoaiGiaoDich", cboLoaiGiaoDich.Text.Trim());

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm phiếu trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Load_DataGridView();
            ResetValues();

            // Bước 4: Reset nút
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtMaHoaDon.Enabled = false;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            ResetValues();
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaHoaDon.Enabled = false;
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            txtMaHoaDon.Enabled = true;

            if (txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Hãy nhập mã hóa đơn để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }

            string sql = $@"
            SELECT 
              th.MaTraHang,
              th.MaHoaDon,
              th.TrangThai,
              th.LoaiGiaoDich,
              th.TongTienHoan,
              nd.TenNguoiDung AS NhanVien,
              kh.TenKhachHang AS KhachHang,
              th.NgayTra,
              th.LyDo
              FROM TraHang th
              JOIN NguoiDung nd ON th.MaNguoiDung = nd.MaNguoiDung
              JOIN HoaDon hd ON th.MaHoaDon = hd.MaHoaDon
              JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
              WHERE th.MaHoaDon LIKE '%{txtMaHoaDon.Text.Trim()}%'";

            DataTable dtSearch = DbContext.GetDataToTable(sql);

            if (dtSearch.Rows.Count == 0)
                MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Tìm thấy {dtSearch.Rows.Count} phiếu trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dgvReturns.DataSource = dtSearch;
            ResetValues();
            
            btnCancel.Enabled = true;
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            Load_DataGridView();
            ResetValues();
            
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            txtMaHoaDon.Enabled = false;
        }

        private void dgvReturns_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (btnAdd.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHoaDon.Focus();
                return;
            }
            if (dgvReturns.Rows.Count == 0 || e.RowIndex < 0)
            {
                return;
            }

            txtMaHoaDon.Text = dgvReturns.CurrentRow.Cells["colMaHoaDon"].Value.ToString();
            txtLyDo.Text = dgvReturns.CurrentRow.Cells["colLyDo"].Value.ToString();
            txtTongTienHoan.Text = dgvReturns.CurrentRow.Cells["colTongTienHoan"].Value.ToString();
            dtpNgayTra.Value = Convert.ToDateTime(dgvReturns.CurrentRow.Cells["colNgayTra"].Value);

            string trangThai = dgvReturns.CurrentRow.Cells["colTrangThai"].Value.ToString().Trim();
            string loaiGD = dgvReturns.CurrentRow.Cells["colLoaiGiaoDich"].Value.ToString().Trim();

            // Lưu vào biến toàn cục
            if (int.TryParse(dgvReturns.CurrentRow.Cells["colMaTraHang"].Value?.ToString(), out int mt))
                currentMaTraHang = mt;
            if (int.TryParse(txtMaHoaDon.Text, out int mh))
                currentMaHoaDon = mh;
            currentTrangThai = trangThai;

            cboTrangThai.Text = trangThai;
            cboLoaiGiaoDich.Text = loaiGD;

            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && dtReturn != null && rowIndex < dtReturn.Rows.Count)
            {
                lblNhanVien.Text = "Nhân viên: " + dtReturn.Rows[rowIndex]["NhanVien"].ToString();
                lblKhachHang.Text = "Khách hàng: " + dtReturn.Rows[rowIndex]["KhachHang"].ToString();
            }

            if (trangThai == "Đã hoàn thành" || trangThai == "Đã hủy")
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            
            btnCancel.Enabled = true;
            txtMaHoaDon.Enabled = false;
        }

        private void txtMaHoaDon_Leave(object sender, EventArgs e)
        {
            string sql = @"
            SELECT 
              nd.TenNguoiDung,
               kh.TenKhachHang
                FROM HoaDon hd
                JOIN NguoiDung nd ON hd.MaNguoiDung = nd.MaNguoiDung
                JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang
                WHERE hd.MaHoaDon = @mahd";

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            cmd.Parameters.AddWithValue("@mahd", txtMaHoaDon.Text);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblNhanVien.Text = "Nhân viên: " + reader["TenNguoiDung"].ToString();
                lblKhachHang.Text = "Khách hàng: " + reader["TenKhachHang"].ToString();
            }

            reader.Close();
        }

        private void btnChooseProducts_Click(object? sender, EventArgs e)
        {

        }

        // ==========================================
        // SỰ KIỆN TAB 2
        // ==========================================

        private void TinhTongTienHoanTra()
        {
            decimal total = 0;
            foreach (DataRow row in dtCart.Rows)
            {
                if (row["colCurThanhTien"] != DBNull.Value)
                {
                    total += Convert.ToDecimal(row["colCurThanhTien"]);
                }
            }

            lblTotalAmount.Text = total.ToString("N0") + " VNĐ";
        }

  
        private void dgvProductsSelection_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtInvoiceDetails == null || dtInvoiceDetails.Rows.Count == 0) return;

            if (e.RowIndex >= 0)
            {
                DataRowView rowView = (DataRowView)dgvProductsSelection.Rows[e.RowIndex].DataBoundItem;

           
                txtSelMaSP.Text = rowView["MaSanPham"].ToString();
                txtSelTenSP.Text = rowView["TenSanPham"].ToString();
                txtSelDonGia.Text = rowView["DonGia"].ToString();

                txtSelSoLuong.Text = "";
                txtSelTinhTrang.Text = "";


                string tenFileAnh = rowView["Anh"]?.ToString();

                if (!string.IsNullOrEmpty(tenFileAnh))
                {
                    string path1 = System.IO.Path.Combine(Application.StartupPath, "GUI", "Resources", tenFileAnh);
                    string path2 = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\GUI\Resources", tenFileAnh);


                    if (System.IO.File.Exists(path1))
                    {
                    
                        picAnh.Image = System.Drawing.Image.FromFile(path1);
                    }
                    else if (System.IO.File.Exists(path2))
                    {
                        picAnh.Image = System.Drawing.Image.FromFile(path2);
                    }
                    else
                    {
                        picAnh.Image = null;
                    }
                }
                else
                {
                    picAnh.Image = null;
                }

                btnAddToCart.Enabled = true;
                btnEdit.Enabled = false;
                btnRemoveFromCart.Enabled = false;
                txtSelSoLuong.Focus();
            }
        }
        private void dgvCurrentDetails_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dtCart.Rows.Count == 0) return;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCurrentDetails.Rows[e.RowIndex];

                txtSelMaSP.Text = row.Cells["colCurMaSP"].Value?.ToString();
                txtSelTenSP.Text = row.Cells["colCurTenSP"].Value?.ToString();
                txtSelSoLuong.Text = row.Cells["colCurSoLuong"].Value?.ToString();
                txtSelDonGia.Text = row.Cells["colCurDonGia"].Value?.ToString();
                txtSelTinhTrang.Text = row.Cells["colCurTinhTrang"].Value?.ToString();

                btnAddToCart.Enabled = false;
                btnEdit.Enabled = true;
                btnRemoveFromCart.Enabled = true;
            }
        }

        private void btnAddToCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào để trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSelSoLuong.Text.Trim().Length == 0 || !int.TryParse(txtSelSoLuong.Text.Trim(), out int slTra) || slTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelSoLuong.Focus();
                return;
            }

            if (txtSelTinhTrang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tình trạng hàng trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSelTinhTrang.Focus();
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

 
            int slMuaGoc = 0;
            foreach (DataGridViewRow r in dgvProductsSelection.Rows)
            {
                if (r.Cells[0].Value != null && Convert.ToInt32(r.Cells[0].Value) == maSP)
                {
                    slMuaGoc = Convert.ToInt32(r.Cells[2].Value);
                    break;
                }
            }

            if (slTra > slMuaGoc)
            {
                MessageBox.Show($"Số lượng trả không được lớn hơn số lượng đã mua ({slMuaGoc})!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        
            foreach (DataRow row in dtCart.Rows)
            {
                if (Convert.ToInt32(row["colCurMaSP"]) == maSP)
                {
                    MessageBox.Show("Sản phẩm này đã nằm trong danh sách trả! Nếu muốn thay đổi số lượng, hãy chọn dòng bên phải và bấm SỬA.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

        
            DataRow newRow = dtCart.NewRow();
            newRow["colCurMaSP"] = maSP;
            newRow["colCurTenSP"] = txtSelTenSP.Text;
            newRow["colCurSoLuong"] = slTra;
            decimal donGia = 0;
            decimal.TryParse(txtSelDonGia.Text, out donGia);
            newRow["colCurDonGia"] = donGia;
            newRow["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
            dtCart.Rows.Add(newRow);

          
            TinhTongTienHoanTra();
            ResetValues1();
            btnAddToCart.Enabled = false;
        }

        private void btnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            if (MessageBox.Show("Bạn có muốn bỏ sản phẩm này khỏi danh sách hoàn trả không?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
                btnEdit.Enabled = false;
                btnRemoveFromCart.Enabled = false;
            }
        }

        private void btnResetCartForm_Click(object? sender, EventArgs e)
        {

        }

        private void btnReturnSearch_Click(object? sender, EventArgs e)
        {
            string maHDInput = Microsoft.VisualBasic.Interaction.InputBox("Nhập Mã Hóa Đơn cần trả hàng:", "Tìm kiếm hóa đơn", "");
            if (string.IsNullOrEmpty(maHDInput.Trim())) return;

            string sql = $@"SELECT cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.MoTa, sp.Anh 
                    FROM ChiTietHoaDon cthd
                    INNER JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                    WHERE cthd.MaHoaDon = {maHDInput.Trim()}";

            if (DbContext.Conn.State == ConnectionState.Closed)
            {
                DbContext.Ketnoi();
            }

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtInvoiceDetails = new DataTable();
            da.Fill(dtInvoiceDetails);

            if (dtInvoiceDetails.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn hoặc hóa đơn không có sản phẩm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

  
            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = dtInvoiceDetails;

     
            pnlReturnTop.Tag = maHDInput.Trim();

  
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();

            btnLuuCT.Enabled = true;
            MessageBox.Show($"Đã tìm thấy hóa đơn {maHDInput}! Hãy chọn sản phẩm cần trả ở danh sách bên trái.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReturnRefresh_Click(object? sender, EventArgs e)
        {
            ResetValues1();
            if (dtInvoiceDetails != null) dtInvoiceDetails.Rows.Clear();
            dtCart.Rows.Clear();
            TinhTongTienHoanTra();

            btnAddToCart.Enabled = false;
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
            btnLuuCT.Enabled = false;
            pnlReturnTop.Tag = null;
        }

        private void LoadInvoiceDetails(int maHoaDon)
        {
            string sql = $@"SELECT cthd.MaSanPham, sp.TenSanPham, cthd.SoLuong, cthd.DonGia, sp.MoTa, sp.Anh 
                    FROM ChiTietHoaDon cthd
                    INNER JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                    WHERE cthd.MaHoaDon = {maHoaDon}";

            if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            SqlDataAdapter da = new SqlDataAdapter(sql, (SqlConnection)DbContext.Conn);
            dtInvoiceDetails = new DataTable();
            da.Fill(dtInvoiceDetails);

            dgvProductsSelection.AutoGenerateColumns = false;
            dgvProductsSelection.DataSource = dtInvoiceDetails;
            pnlReturnTop.Tag = maHoaDon.ToString();
        }

        private void LoadReturnDetails(int maTraHang)
        {
            if (dtCart == null) KhoiTaoGioHangTamtinh();
            dtCart.Rows.Clear();

            string sql = $@"SELECT ctth.MaSanPham, sp.TenSanPham, ctth.SoLuong, sp.GiaBan AS DonGia, ctth.TinhTrang, ctth.TienHoan 
                    FROM ChiTietTraHang ctth
                    INNER JOIN SanPham sp ON ctth.MaSanPham = sp.MaSanPham
                    WHERE ctth.MaTraHang = {maTraHang}";

            if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DataRow newRow = dtCart.NewRow();
                newRow["colCurMaSP"] = reader["MaSanPham"];
                newRow["colCurTenSP"] = reader["TenSanPham"];
                newRow["colCurSoLuong"] = reader["SoLuong"];
                newRow["colCurDonGia"] = reader["DonGia"];
                newRow["colCurTinhTrang"] = reader["TinhTrang"];
                newRow["colCurThanhTien"] = reader["TienHoan"];
                dtCart.Rows.Add(newRow);
            }
            reader.Close();
            TinhTongTienHoanTra();
        }

        private void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabChonSanPham)
            {
                if (currentMaTraHang == -1)
                {
                    MessageBox.Show("Vui lòng chọn một phiếu trả hàng ở tab Thông tin phiếu trả trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabMain.SelectedTab = tabPhieuTra;
                    return;
                }

                LoadInvoiceDetails(currentMaHoaDon);
                LoadReturnDetails(currentMaTraHang);

                if (currentTrangThai == "Đã hoàn thành" || currentTrangThai == "Đã hủy")
                {
                    btnAddToCart.Enabled = false;
                    btnRemoveFromCart.Enabled = false;
                    btnSuaCT.Enabled = false;
                    btnLuuCT.Enabled = false;
                    MessageBox.Show($"Phiếu trả này đang ở trạng thái '{currentTrangThai}', bạn không thể chỉnh sửa chi tiết.", "Chỉ xem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnAddToCart.Enabled = true;
                    btnRemoveFromCart.Enabled = true;
                    btnSuaCT.Enabled = true;
                    btnLuuCT.Enabled = true;
                }
            }
        }

        private void btnBackToReceipt_Click(object? sender, EventArgs e)
        {
            if (currentMaTraHang == -1)
            {
                MessageBox.Show("Chưa chọn phiếu trả hàng nào để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            SqlTransaction transaction = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
                // 1. Tính tổng tiền
                decimal tongTienHoan = 0;
                foreach (DataRow r in dtCart.Rows)
                {
                    tongTienHoan += Convert.ToDecimal(r["colCurThanhTien"]);
                }

                // 2. Cập nhật tổng tiền vào TraHang
                string sqlUpdateTraHang = $"UPDATE TraHang SET TongTienHoan = @tongTien WHERE MaTraHang = {currentMaTraHang}";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdateTraHang, (SqlConnection)DbContext.Conn, transaction);
                cmdUpdate.Parameters.AddWithValue("@tongTien", tongTienHoan);
                cmdUpdate.ExecuteNonQuery();

                // 3. Xóa chi tiết cũ
                string sqlDeleteChiTiet = $"DELETE FROM ChiTietTraHang WHERE MaTraHang = {currentMaTraHang}";
                SqlCommand cmdDelete = new SqlCommand(sqlDeleteChiTiet, (SqlConnection)DbContext.Conn, transaction);
                cmdDelete.ExecuteNonQuery();

                // 4. Thêm chi tiết mới
                foreach (DataRow row in dtCart.Rows)
                {
                    int maSP = Convert.ToInt32(row["colCurMaSP"]);
                    int soLuongTra = Convert.ToInt32(row["colCurSoLuong"]);
                    decimal thanhTienDong = Convert.ToDecimal(row["colCurThanhTien"]);
                    string tinhTrang = row["colCurTinhTrang"].ToString();

                    string sqlInsertChiTiet = $@"INSERT INTO ChiTietTraHang (MaTraHang, MaSanPham, SoLuong, TienHoan, TinhTrang)
                                         VALUES ({currentMaTraHang}, {maSP}, {soLuongTra}, @thanhTienDong, N'{tinhTrang}')";
                    SqlCommand cmdChiTiet = new SqlCommand(sqlInsertChiTiet, (SqlConnection)DbContext.Conn, transaction);
                    cmdChiTiet.Parameters.AddWithValue("@thanhTienDong", thanhTienDong);
                    cmdChiTiet.ExecuteNonQuery();
                }

                transaction.Commit();
                MessageBox.Show("Cập nhật chi tiết phiếu trả hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Trở về tab 1 và load lại dữ liệu
                Load_DataGridView();
                tabSelectionContainer.SelectedTab = tabPhieuTra;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Có lỗi xảy ra trong quá trình lưu dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSelectProduct_Click(object? sender, EventArgs e)
        {

        }

        private void dgvReturns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPhieuTra_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            txtMaHoaDon.Enabled = false;
            Load_DataGridView();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra chọn dữ liệu
            if (dtReturn == null || dtReturn.Rows.Count == 0 || txtMaHoaDon.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn phiếu trả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!int.TryParse(txtMaHoaDon.Text.Trim(), out int maHoaDon)) return;

            if (txtLyDo.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập lý do trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLyDo.Focus();
                return;
            }

            if (cboTrangThai.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return;
            }

            if (currentMaTraHang == -1)
            {
                MessageBox.Show("Vui lòng chọn phiếu trả cụ thể từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            // Bước 2: Xử lý tiền
            string tongTienStr = txtTongTienHoan.Text.Trim();
            if (tongTienStr == "") tongTienStr = "0";
            tongTienStr = tongTienStr.Replace(" đ", "").Replace("đ", "").Replace(",", "").Replace(".", "");
            if (!decimal.TryParse(tongTienStr, out decimal tongTien)) tongTien = 0;

            SqlTransaction transaction = ((SqlConnection)DbContext.Conn).BeginTransaction();

            try
            {
                // Bước 3: Tạo SQL UPDATE
                string sql = @"UPDATE TraHang SET
                               LyDo = @LyDo,
                               TongTienHoan = @TongTienHoan,
                               NgayTra = @NgayTra,
                               TrangThai = @TrangThai,
                               LoaiGiaoDich = @LoaiGiaoDich
                               WHERE MaTraHang = @MaTraHang";

                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)DbContext.Conn, transaction);
                cmd.Parameters.AddWithValue("@LyDo", txtLyDo.Text.Trim());
                cmd.Parameters.AddWithValue("@TongTienHoan", tongTien);
                cmd.Parameters.AddWithValue("@NgayTra", dtpNgayTra.Value);
                cmd.Parameters.AddWithValue("@TrangThai", cboTrangThai.Text.Trim());
                cmd.Parameters.AddWithValue("@LoaiGiaoDich", cboLoaiGiaoDich.Text.Trim());
                cmd.Parameters.AddWithValue("@MaTraHang", currentMaTraHang);
                cmd.ExecuteNonQuery();

                // Bước 4: Kiểm tra cập nhật Tồn Kho nếu trạng thái chuyển sang Đã hoàn thành
                if (cboTrangThai.Text.Trim() == "Đã hoàn thành" && currentTrangThai != "Đã hoàn thành")
                {
                    string sqlCT = $"SELECT MaSanPham, SoLuong FROM ChiTietTraHang WHERE MaTraHang = {currentMaTraHang}";
                    SqlCommand cmdCT = new SqlCommand(sqlCT, (SqlConnection)DbContext.Conn, transaction);
                    SqlDataReader readerCT = cmdCT.ExecuteReader();
                    DataTable dtCT = new DataTable();
                    dtCT.Load(readerCT);
                    readerCT.Close();

                    foreach (DataRow row in dtCT.Rows)
                    {
                        int maSP = Convert.ToInt32(row["MaSanPham"]);
                        int soLuongTra = Convert.ToInt32(row["SoLuong"]);

                        string sqlTonKho = $"SELECT SoLuongTon FROM SanPham WHERE MaSanPham = {maSP}";
                        SqlCommand cmdTonKho = new SqlCommand(sqlTonKho, (SqlConnection)DbContext.Conn, transaction);
                        int tonKhoHienTai = Convert.ToInt32(cmdTonKho.ExecuteScalar());
                        int tonKhoSau = tonKhoHienTai + soLuongTra;

                        string sqlUpdateKho = $"UPDATE SanPham SET SoLuongTon = {tonKhoSau} WHERE MaSanPham = {maSP}";
                        SqlCommand cmdUpdateKho = new SqlCommand(sqlUpdateKho, (SqlConnection)DbContext.Conn, transaction);
                        cmdUpdateKho.ExecuteNonQuery();

                        string sqlInsertLichSu = $@"INSERT INTO LichSuNhapKho 
                                            (MaSanPham, Thoigian, ThayDoi, SoLuongTruoc, SoLuongSau, LoaiGiaoDich, MaThamChieu, TrangThai)
                                            VALUES ({maSP}, GETDATE(), {soLuongTra}, {tonKhoHienTai}, {tonKhoSau}, N'Trả hàng', {currentMaTraHang}, N'Hoàn tất')";
                        SqlCommand cmdLichSu = new SqlCommand(sqlInsertLichSu, (SqlConnection)DbContext.Conn, transaction);
                        cmdLichSu.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                MessageBox.Show("Cập nhật phiếu trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Load_DataGridView();
            ResetValues();

            // Bước 4: Khóa nút
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            txtMaHoaDon.Enabled = false;
        }

        private void txtMaHoaDon_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProductsSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductsSelection_CellClick(sender, e);
        }

        private void tabChonSanPham_Click(object sender, EventArgs e)
        {
        }

        private void dgvCurrentDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCurrentDetails_CellClick(sender, e);
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (txtSelMaSP.Text.Trim() == "") return;

            if (txtSelSoLuong.Text.Trim().Length == 0 || !int.TryParse(txtSelSoLuong.Text.Trim(), out int slTra) || slTra <= 0)
            {
                MessageBox.Show("Số lượng trả phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP = int.Parse(txtSelMaSP.Text);

     
            int slMuaGoc = 0;
            foreach (DataGridViewRow r in dgvProductsSelection.Rows)
            {
                if (Convert.ToInt32(r.Cells["colMaSP"].Value) == maSP)
                {
                    slMuaGoc = Convert.ToInt32(r.Cells["colSoLuong"].Value);
                    break;
                }
            }

            if (slTra > slMuaGoc)
            {
                MessageBox.Show($"Số lượng trả vượt quá số lượng gốc ({slMuaGoc})!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

      
            foreach (DataRow row in dtCart.Rows)
            {
                if (Convert.ToInt32(row["colCurMaSP"]) == maSP)
                {
                    row["colCurSoLuong"] = slTra;
                    row["colCurTinhTrang"] = txtSelTinhTrang.Text.Trim();
                    break;
                }
            }

            TinhTongTienHoanTra();
            ResetValues();
            btnEdit.Enabled = false;
            btnRemoveFromCart.Enabled = false;
        }

        private void btnBoquaCT_Click(object sender, EventArgs e)
        {
            btnReturnRefresh_Click(sender, e);
        }

        private void pnlReturnTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabProductDetail_Click(object sender, EventArgs e)
        {

        }
    }
}
