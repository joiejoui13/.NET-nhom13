using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.DAL.Repositories.Admin
{
    /// <summary>
    /// Interface (Giao diện) định nghĩa các hợp đồng (contract) thao tác với CSDL.
    /// Áp dụng mẫu thiết kế Repository Pattern.
    /// </summary>
    public interface IReportRepository
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
