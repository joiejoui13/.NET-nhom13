using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.BLL.Services.Admin
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IReportService
    {
        Task<decimal> GetRevenueAsync(DateTime start, DateTime end);
        Task<int> GetOrderCountAsync(DateTime start, DateTime end);
        Task<int> GetTotalProductsSoldAsync(DateTime start, DateTime end);
        Task<IEnumerable<SalesReportRow>> GetSalesReportAsync(DateTime start, DateTime end);
        Task<IEnumerable<RevenueTrendRow>> GetRevenueTrendAsync(DateTime start, DateTime end, string period);
        Task<IEnumerable<TopProductRow>> GetTopProductsAsync(DateTime start, DateTime end, int topN = 5);
        Task<IEnumerable<OrderStatusRow>> GetOrderStatusDistributionAsync(DateTime start, DateTime end);
    }
}
