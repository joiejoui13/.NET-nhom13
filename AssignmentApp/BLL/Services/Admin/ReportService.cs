using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentApp.DAL.Repositories.Admin;
using AssignmentApp.DTO.Models;

namespace AssignmentApp.BLL.Services.Admin
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        private void ValidateDateRange(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }
        }

        public async Task<decimal> GetRevenueAsync(DateTime start, DateTime end)
        {
            ValidateDateRange(start, end);
            return await _reportRepository.GetRevenueAsync(start, end);
        }

        public async Task<int> GetOrderCountAsync(DateTime start, DateTime end)
        {
            ValidateDateRange(start, end);
            return await _reportRepository.GetOrderCountAsync(start, end);
        }

        public async Task<int> GetTotalProductsSoldAsync(DateTime start, DateTime end)
        {
            ValidateDateRange(start, end);
            return await _reportRepository.GetTotalProductsSoldAsync(start, end);
        }

        public async Task<IEnumerable<SalesReportRow>> GetSalesReportAsync(DateTime start, DateTime end)
        {
            ValidateDateRange(start, end);
            return await _reportRepository.GetSalesReportAsync(start, end);
        }

        public async Task<IEnumerable<RevenueTrendRow>> GetRevenueTrendAsync(DateTime start, DateTime end, string period)
        {
            ValidateDateRange(start, end);
            if (string.IsNullOrEmpty(period))
            {
                period = "Tháng";
            }
            return await _reportRepository.GetRevenueTrendAsync(start, end, period);
        }

        public async Task<IEnumerable<TopProductRow>> GetTopProductsAsync(DateTime start, DateTime end, int topN = 5)
        {
            ValidateDateRange(start, end);
            if (topN <= 0) topN = 5;
            return await _reportRepository.GetTopProductsAsync(start, end, topN);
        }

        public async Task<IEnumerable<OrderStatusRow>> GetOrderStatusDistributionAsync(DateTime start, DateTime end)
        {
            ValidateDateRange(start, end);
            return await _reportRepository.GetOrderStatusDistributionAsync(start, end);
        }
    }
}
