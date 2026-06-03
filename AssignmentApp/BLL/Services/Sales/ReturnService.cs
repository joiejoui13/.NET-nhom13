using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _repository;
        private readonly IProductRepository _productRepository;

        public ReturnService(IReturnRepository repository, IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Return>> GetAllReturnsAsync()
        {
            return _repository.GetAllReturnsAsync();
        }

        public Task<IEnumerable<Return>> SearchReturnsAsync(string maHD, string khach, string nhanVien, string lydo, string trangThai, string loaiGD, decimal? tongTien, DateTime? ngayTra)
        {
            return _repository.SearchReturnsAsync(maHD, khach, nhanVien, lydo, trangThai, loaiGD, tongTien, ngayTra);
        }

        public Task<IEnumerable<ReturnInvoiceProduct>> GetInvoiceProductsAsync(int maHoaDon)
        {
            return _repository.GetInvoiceProductsAsync(maHoaDon);
        }

        public Task<IEnumerable<ReturnDetail>> GetReturnDetailsAsync(int maTraHang)
        {
            return _repository.GetReturnDetailsAsync(maTraHang);
        }

        public Task<string?> GetCustomerNameByInvoiceAsync(int maHoaDon)
        {
            return _repository.GetCustomerNameByInvoiceAsync(maHoaDon);
        }

        public Task<int> CreateReturnAsync(Return r)
        {
            // Business logic validation can go here
            if (r.MaHoaDon <= 0) throw new ArgumentException("Mã hóa đơn không hợp lệ.");
            return _repository.CreateReturnAsync(r);
        }

        public Task<bool> UpdateReturnAsync(Return r)
        {
            if (r.MaTraHang <= 0) throw new ArgumentException("Mã phiếu trả không hợp lệ.");
            return _repository.UpdateReturnAsync(r);
        }

        public Task<bool> DeleteReturnTransactionAsync(int maTraHang)
        {
            return _repository.DeleteReturnTransactionAsync(maTraHang);
        }

                public async Task<bool> SaveReturnDetailsTransactionAsync(int maTraHang, List<ReturnDetail> details, decimal tongTienHoanThucTe, string loaiGiaoDich)
        {
            if (maTraHang <= 0) throw new ArgumentException("Mã phiếu trả không hợp lệ.");
            
            bool result = await _repository.SaveReturnDetailsTransactionAsync(maTraHang, details, tongTienHoanThucTe, loaiGiaoDich);
            
            if (result)
            {
                // Inventory logic has been successfully migrated to DAL to ensure Database Transaction Atomicity
            }
            
            return result;
        }
    }
}

