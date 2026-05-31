using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.BLL.Services.Warehouse
{
    /// <summary>
    /// Class xử lý các nghiệp vụ (Business Logic Layer).
    /// Đứng giữa giao diện và cơ sở dữ liệu để kiểm tra, làm sạch dữ liệu trước khi lưu.
    /// Kỹ thuật Dependency Injection (DI) được áp dụng qua Constructor.
    /// </summary>
    public class StockInService : IStockInService
    {
        private readonly IStockInRepository _stockInRepository;

        public StockInService(IStockInRepository stockInRepository)
        {
            _stockInRepository = stockInRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<StockInReceipt>> GetAllReceiptsAsync()
        {
            return await _stockInRepository.GetAllReceiptsAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<StockInReceipt> GetReceiptByIdAsync(int id)
        {
            return await _stockInRepository.GetReceiptByIdAsync(id);
        }

        public async Task<IEnumerable<StockInDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            return await _stockInRepository.GetReceiptDetailsAsync(receiptId);
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
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
