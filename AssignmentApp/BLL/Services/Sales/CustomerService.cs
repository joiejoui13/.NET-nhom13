using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepo;

        public CustomerService(CustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.GetAllCustomersAsync();
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.MaKhachHang))
            {
                customer.MaKhachHang = GenerateCustomerId();
            }

            customer.NgayTao = DateTime.Now;
            
            // Set default points for new customer if not provided
            if (customer.DiemTichLuy < 0)
                customer.DiemTichLuy = 0;

            return await _customerRepo.AddCustomerAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer.DiemTichLuy < 0)
                customer.DiemTichLuy = 0;

            return await _customerRepo.UpdateCustomerAsync(customer);
        }

        private string GenerateCustomerId()
        {
            // Simple ID generation for now.
            // In a real app, this should check DB for max ID or use a UUID/GUID.
            return "KH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
