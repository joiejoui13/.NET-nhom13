using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddProductAsync(Product p)
        {
            ValidateProduct(p);
            int rowsAffected = await _productRepository.AddAsync(p);
            return rowsAffected > 0;
        }

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

        public async Task<IEnumerable<Product>> SearchProductsAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit)
        {
            return await _productRepository.SearchAsync(idTerm, nameTerm, catId, statusTerm, priceLimit, stockLimit);
        }

        public async Task<DataTable> GetCategoriesForComboBoxAsync()
        {
            return await _productRepository.GetCategoriesForComboBoxAsync();
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
