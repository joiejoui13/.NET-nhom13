using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer).
    /// Đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// Kỹ thuật Dependency Injection (DI) được áp dụng qua Constructor.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            return await _productRepository.GetByIdAsync(id);
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<bool> AddProductAsync(Product p)
        {
            ValidateProduct(p);
            int rowsAffected = await _productRepository.AddAsync(p);
            return rowsAffected > 0;
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<bool> UpdateProductAsync(Product p)
        {
            if (p.MaSanPham <= 0) throw new ArgumentException("Mã sản phẩm không hợp lệ để cập nhật.");
            ValidateProduct(p);
            int rowsAffected = await _productRepository.UpdateAsync(p);
            return rowsAffected > 0;
        }

        public async Task<bool> SoftDeleteProductAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Mã sản phẩm không hợp lệ để xóa.");
            int rowsAffected = await _productRepository.SoftDeleteAsync(id);
            return rowsAffected > 0;
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit)
        {
            return await _productRepository.SearchAsync(idTerm, nameTerm, catId, statusTerm, priceLimit, stockLimit);
        }

        public async Task<DataTable> GetCategoriesForComboBoxAsync()
        {
            return await _productRepository.GetCategoriesForComboBoxAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsByTextAsync(string keyword, string catIdText, string catNameText, string status)
        {
            return await _productRepository.SearchByTextAsync(keyword, catIdText, catNameText, status);
        }

        private void ValidateProduct(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.TenSanPham))
                throw new ArgumentException("Tên sản phẩm không được phép để trống.");

            if (p.MaDanhMuc <= 0)
                throw new ArgumentException("Danh mục sản phẩm không hợp lệ.");

            if (p.GiaNhap < 0)
                throw new ArgumentException("Giá nhập kho phải lớn hơn hoặc bằng 0.");

            if (p.GiaBan <= 0)
                throw new ArgumentException("Giá bán lẻ phải lớn hơn 0.");

            if (p.GiaBan < p.GiaNhap)
                throw new ArgumentException("Giá bán lẻ không được nhỏ hơn giá nhập kho.");

            if (p.SoLuongTon < 0)
                throw new ArgumentException("Số lượng tồn kho phải là số không âm.");
        }
    }
}

