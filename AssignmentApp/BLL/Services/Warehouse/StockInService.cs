using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.BLL.Services.Warehouse
{
    public class StockInService : IStockInService
    {
        private readonly IStockInRepository _stockInRepository;

        public StockInService(IStockInRepository stockInRepository)
        {
            _stockInRepository = stockInRepository;
        }

        public async Task<IEnumerable<StockInReceipt>> GetAllReceiptsAsync()
        {
            return await _stockInRepository.GetAllReceiptsAsync();
        }

        public async Task<StockInReceipt> GetReceiptByIdAsync(int id)
        {
            return await _stockInRepository.GetReceiptByIdAsync(id);
        }

        public async Task<IEnumerable<StockInDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            return await _stockInRepository.GetReceiptDetailsAsync(receiptId);
        }

        public async Task<IEnumerable<StockInReceipt>> SearchReceiptsAsync(string receiptIdStr, string userIdStr, string status, string dateStr)
        {
            int? receiptId = null;
            if (int.TryParse(receiptIdStr, out int idVal)) receiptId = idVal;

            int? userId = null;
            if (int.TryParse(userIdStr, out int userVal)) userId = userVal;

            if (receiptId == null && userId == null && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(dateStr))
            {
                throw new Exception("Cần tối thiểu 1 thông tin để bộ lọc có thể hoạt động!");
            }

            return await _stockInRepository.SearchReceiptsAsync(receiptId, userId, status, dateStr);
        }

        public async Task<int> SaveReceiptAsync(StockInReceipt receipt, List<StockInDetailModel> details, bool isAddingNew)
        {
            if (receipt.MaNguoiDung <= 0)
            {
                throw new Exception("Mã nhân viên lập phiếu không hợp lệ!");
            }

            if (receipt.TrangThai == "Đã hoàn thành" && (details == null || details.Count == 0))
            {
                throw new Exception("Không thể lưu phiếu RỖNG (không có sản phẩm) ở trạng thái 'Đã hoàn thành'! Vui lòng thêm sản phẩm vào giỏ hàng.");
            }

            if (!isAddingNew)
            {
                var currentReceipt = await _stockInRepository.GetReceiptByIdAsync(receipt.MaPhieuNhap);
                if (currentReceipt != null && (currentReceipt.TrangThai == "Đã hoàn thành" || currentReceipt.TrangThai == "Đã hủy"))
                {
                    throw new Exception($"Đơn đã {currentReceipt.TrangThai.ToLower()}, hệ thống đã khóa lại, không thể sửa đổi để bảo toàn toàn vẹn dữ liệu!");
                }
            }

            double total = 0;
            foreach (var d in details)
            {
                total += d.ThanhTien;
            }
            receipt.TongTien = total;

            return await _stockInRepository.SaveReceiptWithTransactionAsync(receipt, details, isAddingNew);
        }

        public async Task<bool> CancelReceiptAsync(int receiptId)
        {
            return await _stockInRepository.CancelReceiptWithTransactionAsync(receiptId);
        }
    }
}
