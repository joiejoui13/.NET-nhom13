using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    public class ProductRepository : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, 
                                  s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao, s.NgayCapNhat 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc
                           ORDER BY s.NgayTao DESC";
            return await DbContext.Conn.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, 
                                  s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao, s.NgayCapNhat 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE s.MaSanPham = @MaSanPham";
            return await DbContext.Conn.QuerySingleOrDefaultAsync<Product>(sql, new { MaSanPham = id });
        }

        public async Task<int> AddAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO SanPham (TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) 
                           VALUES (@TenSanPham, @MaDanhMuc, @GiaNhap, @GiaBan, @SoLuongTon, @MoTa, @Anh, @TrangThai, GETDATE())";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> UpdateAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"UPDATE SanPham 
                           SET TenSanPham = @TenSanPham, 
                               MaDanhMuc = @MaDanhMuc, 
                               GiaNhap = @GiaNhap, 
                               GiaBan = @GiaBan, 
                               SoLuongTon = @SoLuongTon, 
                               MoTa = @MoTa, 
                               Anh = @Anh, 
                               TrangThai = @TrangThai, 
                               NgayCapNhat = GETDATE() 
                           WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }

        public async Task<int> SoftDeleteAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE SanPham SET TrangThai = N'Ngưng bán' WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaSanPham = id });
        }

        public async Task<IEnumerable<Product>> SearchAsync(string idTerm, string nameTerm, int catId, string statusTerm, double priceLimit, int stockLimit)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, 
                                  s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao, s.NgayCapNhat 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(idTerm))
            {
                sql += " AND s.MaSanPham LIKE @IdTerm";
                parameters.Add("IdTerm", $"%{idTerm}%");
            }
            if (!string.IsNullOrEmpty(nameTerm))
            {
                sql += " AND (s.TenSanPham LIKE @NameTerm OR s.MoTa LIKE @NameTerm)";
                parameters.Add("NameTerm", $"%{nameTerm}%");
            }
            if (catId > 0)
            {
                sql += " AND s.MaDanhMuc = @CatId";
                parameters.Add("CatId", catId);
            }
            if (!string.IsNullOrEmpty(statusTerm))
            {
                sql += " AND s.TrangThai = @StatusTerm";
                parameters.Add("StatusTerm", statusTerm);
            }
            if (priceLimit > 0)
            {
                sql += " AND s.GiaBan <= @PriceLimit";
                parameters.Add("PriceLimit", priceLimit);
            }
            if (stockLimit > 0)
            {
                sql += " AND s.SoLuongTon <= @StockLimit";
                parameters.Add("StockLimit", stockLimit);
            }

            sql += " ORDER BY s.NgayTao DESC";

            return await DbContext.Conn.QueryAsync<Product>(sql, parameters);
        }

        public async Task<DataTable> GetCategoriesForComboBoxAsync()
        {
            return await Task.Run(() => 
            {
                string sql = "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY TenDanhMuc ASC";
                return DbContext.GetDataToTable(sql);
            });
        }
    }
}
