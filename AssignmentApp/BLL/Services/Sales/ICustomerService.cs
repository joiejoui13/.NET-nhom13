using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int maKhachHang);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string tenKhachHang, string soDienThoai);
        Task<int> AddCustomerAsync(Customer customer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> SoftDeleteCustomerAsync(int maKhachHang);
    }
}
