using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;
using System.IO;

namespace AssignmentApp.GUI.UserControls.Warehouse
{
    public partial class ucProductList : Base.ucBase
    {
        private ProductRepository _repo = new ProductRepository();
        private string _imageFolderPath = Path.Combine(Application.StartupPath, "Images", "Products");

        public ucProductList()
        {
            InitializeComponent();
            this.Load += UcProductList_Load;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            btnChooseImage.Click += BtnChooseImage_Click;

            // Đảm bảo thư mục lưu ảnh tồn tại
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        private async void UcProductList_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var products = await _repo.GetAllAsync();
                dgvProducts.DataSource = products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null)
            {
                var row = dgvProducts.CurrentRow;
                string tenFileAnh = row.Cells["Anh"].Value?.ToString();

                HienThiAnhSanPham(tenFileAnh);
            }
        }

        private void HienThiAnhSanPham(string tenFileAnh)
        {
            if (string.IsNullOrEmpty(tenFileAnh))
            {
                picProduct.Image = null;
                return;
            }

            string imagePath = Path.Combine(_imageFolderPath, tenFileAnh);
            if (File.Exists(imagePath))
            {
                try
                {
                    // Tránh lỗi lock file khi mở ảnh
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        picProduct.Image = Image.FromStream(fs);
                    }
                }
                catch
                {
                    picProduct.Image = null;
                }
            }
            else
            {
                picProduct.Image = null;
            }
        }

        private async void BtnChooseImage_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHang = dgvProducts.CurrentRow.Cells["MaHang"].Value?.ToString();
            if (string.IsNullOrEmpty(maHang)) return;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh sản phẩm";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.webp;*.bmp;*.gif";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourceFilePath = ofd.FileName;
                        string extension = Path.GetExtension(sourceFilePath);
                        
                        // Tạo tên file mới tránh trùng lặp: MaHang + Extension
                        string newFileName = maHang + "_" + DateTime.Now.Ticks + extension;
                        string destFilePath = Path.Combine(_imageFolderPath, newFileName);

                        // Copy file vào thư mục dự án
                        File.Copy(sourceFilePath, destFilePath, true);

                        // Cập nhật Database
                        await _repo.UpdateImageAsync(maHang, newFileName);

                        // Tải lại dữ liệu lên DataGridView và cập nhật PictureBox
                        await LoadDataAsync();
                        
                        MessageBox.Show("Cập nhật ảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
