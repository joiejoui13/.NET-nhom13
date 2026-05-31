using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            // Kiểm duyệt đầu vào
            if (string.IsNullOrWhiteSpace(category.TenDanhMuc))
                throw new Exception("Tên danh mục không được để trống.");
            if (string.IsNullOrWhiteSpace(category.MoTa))
                throw new Exception("Mô tả không được để trống.");
            if (string.IsNullOrWhiteSpace(category.TrangThai))
                throw new Exception("Trạng thái không được để trống.");

            // Kiểm tra trùng lặp Tên danh mục
            var existingCategory = await _categoryRepository.GetByNameAsync(category.TenDanhMuc);
            if (existingCategory != null)
                throw new Exception("Tên danh mục đã tồn tại, vui lòng chọn tên khác.");

            category.NgayTao = DateTime.Now;

            int rows = await _categoryRepository.AddAsync(category);
            return rows > 0;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.TenDanhMuc))
                throw new Exception("Tên danh mục không được để trống.");
            if (string.IsNullOrWhiteSpace(category.MoTa))
                throw new Exception("Mô tả không được để trống.");
            if (string.IsNullOrWhiteSpace(category.TrangThai))
                throw new Exception("Trạng thái không được để trống.");

            category.NgayCapNhat = DateTime.Now;

            int rows = await _categoryRepository.UpdateAsync(category);
            return rows > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            int rows = await _categoryRepository.DeleteAsync(id);
            return rows > 0;
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string idTerm, string nameTerm, string descTerm, string statusTerm)
        {
            return await _categoryRepository.SearchAsync(idTerm, nameTerm, descTerm, statusTerm);
        }
    }
}
