using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Sales
{
    public interface IReturnService
    {
        Task<IEnumerable<Return>> GetAllReturnsAsync();
        Task<IEnumerable<Return>> SearchReturnsAsync(string maHD, string khach, string nhanVien, string lydo, string trangThai, string loaiGD, decimal? tongTien, DateTime? ngayTra);
        Task<IEnumerable<ReturnInvoiceProduct>> GetInvoiceProductsAsync(int maHoaDon);
        Task<IEnumerable<ReturnDetail>> GetReturnDetailsAsync(int maTraHang);
        Task<string?> GetCustomerNameByInvoiceAsync(int maHoaDon);
        
        Task<int> CreateReturnAsync(Return r);
        Task<bool> UpdateReturnAsync(Return r);
        Task<bool> DeleteReturnTransactionAsync(int maTraHang);
        
        Task<bool> SaveReturnDetailsTransactionAsync(int maTraHang, List<ReturnDetail> details, decimal tongTienHoanThucTe, string loaiGiaoDich);
    }
}
