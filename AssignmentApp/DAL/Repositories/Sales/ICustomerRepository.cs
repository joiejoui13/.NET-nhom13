using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.DAL.Repositories.Sales
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int maKhachHang);
        Task<IEnumerable<Customer>> SearchAsync(string tenKhachHang, string soDienThoai);
        Task<int> AddAsync(Customer customer);
        Task<int> UpdateAsync(Customer customer);
        Task<int> UpdateStatusAsync(int maKhachHang, string trangThai);
    }
}
