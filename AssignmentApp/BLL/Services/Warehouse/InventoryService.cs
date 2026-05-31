using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Warehouse;
using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Warehouse
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
        public async Task<IEnumerable<InventoryLog>> GetAllLogsAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
        public async Task<InventoryLog> GetLogByIdAsync(int id)
        {
            return await _inventoryRepository.GetByIdAsync(id);
        }

        public async Task<int> GetProductStockAsync(int productId)
        {
            return await _inventoryRepository.GetProductStockAsync(productId);
        }
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<bool> AddLogAsync(InventoryLog log)
        {
            // Kiểm duyệt đầu vào (validation) đã được thực hiện phần lớn ở GUI (để tiện báo lỗi các ô),
            // nhưng BLL sẽ kiểm tra Tồn kho âm.
            
            int currentStock = await _inventoryRepository.GetProductStockAsync(log.MaSanPham);
            
            log.SoLuongTruoc = currentStock;
            log.SoLuongSau = currentStock + log.ThayDoi;

            if (log.SoLuongSau < 0)
                throw new Exception("Tồn kho sau khi điều chỉnh không thể nhỏ hơn 0! Vui lòng xem lại số lượng thay đổi.");

            log.Thoigian = DateTime.Now;
            if (string.IsNullOrEmpty(log.TrangThai)) log.TrangThai = "Đang hoạt động";

            return await _inventoryRepository.AddWithTransactionAsync(log);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
        public async Task<bool> UpdateLogAsync(InventoryLog newLog)
        {
            var oldLog = await _inventoryRepository.GetByIdAsync(newLog.MaLichSu);
            if (oldLog == null)
                throw new Exception("Không tìm thấy bản ghi lịch sử cũ để sửa!");

            if (oldLog.TrangThai == "Đã khóa" || oldLog.TrangThai == "Đã hủy")
                throw new Exception("Bản ghi lịch sử này đã bị chốt (khóa hoặc hủy), không thể chỉnh sửa!");

            // 1. Tính tồn kho nếu thu hồi (revert) thay đổi cũ
            int currentStockOld = await _inventoryRepository.GetProductStockAsync(oldLog.MaSanPham);
            int stockOldReverted = currentStockOld - oldLog.ThayDoi;

            if (stockOldReverted < 0)
                throw new Exception("Không thể sửa! Nếu hủy lệnh cũ thì sản phẩm bị âm tồn kho (có thể đã xuất bán).");

            // 2. Lấy tồn hiện tại của sản phẩm mới (nếu sửa mã sản phẩm)
            int currentStockNew = 0;
            if (newLog.MaSanPham == oldLog.MaSanPham)
            {
                currentStockNew = stockOldReverted;
            }
            else
            {
                currentStockNew = await _inventoryRepository.GetProductStockAsync(newLog.MaSanPham);
            }

            int finalStockNew = currentStockNew + newLog.ThayDoi;
            if (finalStockNew < 0)
                throw new Exception("Không thể sửa vì thay đổi mới làm tồn kho của sản phẩm bị âm!");

            newLog.SoLuongTruoc = currentStockNew;
            newLog.SoLuongSau = finalStockNew;

            return await _inventoryRepository.UpdateWithTransactionAsync(newLog, oldLog);
        }
/// <summary>
        /// [CHI TIẾT] Xóa bản ghi khỏi cơ sở dữ liệu dựa vào Khóa chính. Hành động này sẽ thay đổi trạng thái hoặc xóa vĩnh viễn (tùy nghiệp vụ).
        /// </summary>
        public async Task<bool> DeleteLogAsync(int id)
        {
            var oldLog = await _inventoryRepository.GetByIdAsync(id);
            if (oldLog == null)
                throw new Exception("Không tìm thấy bản ghi cần xóa!");

            if (oldLog.TrangThai == "Đã khóa" || oldLog.TrangThai == "Đã hủy")
                throw new Exception("Bản ghi này đã bị khóa hệ thống hoặc bị hủy từ trước, không thể tác động!");

            int currentStock = await _inventoryRepository.GetProductStockAsync(oldLog.MaSanPham);
            int stockReverted = currentStock - oldLog.ThayDoi;

            if (stockReverted < 0)
                throw new Exception("Không thể xóa bản ghi vì việc thu hồi sẽ làm Tồn kho của sản phẩm rớt xuống dưới 0 (Âm kho)!");

            return await _inventoryRepository.DeleteWithTransactionAsync(oldLog);
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
        public async Task<IEnumerable<InventoryLog>> SearchLogsAsync(string idTerm, string refTerm, string productTerm, string typeTerm, string statusTerm)
        {
            return await _inventoryRepository.SearchAsync(idTerm, refTerm, productTerm, typeTerm, statusTerm);
        }

        public async Task<System.Data.DataTable> GetProductsForComboBoxAsync()
        {
            return await _inventoryRepository.GetProductsForComboBoxAsync();
        }
    }
}
