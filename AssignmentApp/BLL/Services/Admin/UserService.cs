using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO;
using AssignmentApp.BLL.Utils;

namespace AssignmentApp.BLL.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

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

        public async Task<IEnumerable<User>> SearchUsersAsync(string idTerm, string nameTerm, string phoneTerm, string emailTerm, string roleTerm, string statusTerm)
        {
            return await _userRepository.SearchAsync(idTerm, nameTerm, phoneTerm, emailTerm, roleTerm, statusTerm);
        }
    }
}
