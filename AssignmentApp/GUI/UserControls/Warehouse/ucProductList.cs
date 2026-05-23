using System;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : UserControl
    {
        public ucProductList()
        {
            InitializeComponent();
        }

        private void ucProductList_Load(object sender, EventArgs e)
        {
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                
                // Cập nhật thông tin chi tiết sang tab 2
                lblProductDetailName.Text = row.Cells["colTenSanPham"].Value?.ToString() ?? "";
                lblProductDetailPrice.Text = $"Giá bán: {row.Cells["colGiaBan"].Value?.ToString() ?? "0"} VNĐ";
                lblProductDetailStock.Text = $"Số lượng tồn: {row.Cells["colSoLuongTon"].Value?.ToString() ?? "0"}";
                
                lblProductDetailDesc.Text = $"Mã sản phẩm: {row.Cells["colMaSanPham"].Value?.ToString()}\n" +
                                            $"Danh mục: {row.Cells["colMaDanhMuc"].Value?.ToString()}\n" +
                                            $"Giá nhập: {row.Cells["colGiaNhap"].Value?.ToString()} VNĐ\n" +
                                            $"Trạng thái: {row.Cells["colTrangThai"].Value?.ToString()}";

                // Cũng cập nhật ngược lại form nhập liệu bên trái để dễ chỉnh sửa
                txtMaSanPham.Text = row.Cells["colMaSanPham"].Value?.ToString() ?? "";
                txtTenSanPham.Text = row.Cells["colTenSanPham"].Value?.ToString() ?? "";
                txtGiaNhap.Text = row.Cells["colGiaNhap"].Value?.ToString() ?? "";
                txtGiaBan.Text = row.Cells["colGiaBan"].Value?.ToString() ?? "";
                txtSoLuongTon.Text = row.Cells["colSoLuongTon"].Value?.ToString() ?? "";
                
                string categoryId = row.Cells["colMaDanhMuc"].Value?.ToString() ?? "";
                cboDanhMuc.Text = categoryId;

                string status = row.Cells["colTrangThai"].Value?.ToString() ?? "";
                cboTrangThai.Text = status;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(ofd.FileName);
                        picProductImage.Image = img;
                        picProductDetailImage.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
