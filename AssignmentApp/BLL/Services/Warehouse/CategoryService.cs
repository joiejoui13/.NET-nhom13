using System;
using System.Collections.Generic;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
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
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
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
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            int rows = await _categoryRepository.DeleteAsync(id);
            return rows > 0;
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string idTerm, string nameTerm, string descTerm, string statusTerm)
        {
            return await _categoryRepository.SearchAsync(idTerm, nameTerm, descTerm, statusTerm);
        }
    }
}
