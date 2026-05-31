using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;

namespace AssignmentApp.BLL.Services.Admin
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer).
    /// Đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// Kỹ thuật Dependency Injection (DI) được áp dụng qua Constructor.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<bool> AddUserAsync(User user)
        {
            // Kiểm duyệt
            if (string.IsNullOrWhiteSpace(user.TenNguoiDung))
                throw new Exception("Tên người dùng không được để trống.");
            if (string.IsNullOrWhiteSpace(user.MatKhau))
                throw new Exception("Mật khẩu không được để trống.");

            // Băm mật khẩu trước khi lưu xuống Database
            user.MatKhau = PasswordHasher.HashPassword(user.MatKhau);

            // Gán ngày tạo
            user.NgayTao = DateTime.Now;

            int rows = await _userRepository.AddAsync(user);
            return rows > 0;
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<bool> UpdateUserAsync(User user, bool updatePassword)
        {
            if (string.IsNullOrWhiteSpace(user.TenNguoiDung))
                throw new Exception("Tên người dùng không được để trống.");

            if (updatePassword)
            {
                if (string.IsNullOrWhiteSpace(user.MatKhau))
                    throw new Exception("Mật khẩu mới không được để trống.");
                
                // Băm mật khẩu mới
                user.MatKhau = PasswordHasher.HashPassword(user.MatKhau);
            }
            else
            {
                // Nếu không cập nhật mật khẩu, ta cần giữ nguyên mật khẩu cũ trong DB.
                // Để làm điều này an toàn nhất, ta truy xuất mật khẩu hiện tại từ DB (nếu DTO MatKhau đang rỗng)
                var existingUser = await _userRepository.GetByIdAsync(user.MaNguoiDung);
                if (existingUser != null)
                {
                    user.MatKhau = existingUser.MatKhau;
                }
            }

            int rows = await _userRepository.UpdateAsync(user);
            return rows > 0;
        }

        public async Task<bool> LockUserAsync(int id)
        {
            int rows = await _userRepository.DeleteAsync(id); // DeleteAsync thực chất là UPDATE TrangThai = 'Khóa'
            return rows > 0;
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<User>> SearchUsersAsync(string idTerm, string nameTerm, string phoneTerm, string emailTerm, string roleTerm, string statusTerm)
        {
            return await _userRepository.SearchAsync(idTerm, nameTerm, phoneTerm, emailTerm, roleTerm, statusTerm);
        }
    }
}
