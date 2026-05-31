using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer).
    /// Đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// Kỹ thuật Dependency Injection (DI) được áp dụng qua Constructor.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly DAL.Repositories.Sales.ICustomerRepository _customerRepository;

        public CustomerService(DAL.Repositories.Sales.ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int maKhachHang)
        {
            return await _customerRepository.GetByIdAsync(maKhachHang);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string tenKhachHang, string soDienThoai)
        {
            return await _customerRepository.SearchAsync(tenKhachHang, soDienThoai);
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.TenKhachHang))
                throw new System.ArgumentException("Tên khách hàng không được để trống!");

            customer.NgayTao = System.DateTime.Now;
            if (string.IsNullOrEmpty(customer.TrangThai))
                customer.TrangThai = "Hoạt động";

            return await _customerRepository.AddAsync(customer);
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.TenKhachHang))
                throw new System.ArgumentException("Tên khách hàng không được để trống!");

            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<int> SoftDeleteCustomerAsync(int maKhachHang)
        {
            return await _customerRepository.UpdateStatusAsync(maKhachHang, "Đã xóa");
        }
    }
}
