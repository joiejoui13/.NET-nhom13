using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using AssignmentApp.DTO;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Warehouse
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
/// <summary>
        /// [CHI TIẾT] Lấy toàn bộ danh sách dữ liệu. Sử dụng bất đồng bộ (Task) để tối ưu hiệu suất và không chặn luồng chính (Main Thread).
        /// </summary>
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
/// <summary>
        /// [CHI TIẾT] Lấy thông tin chi tiết của một bản ghi dựa trên Khóa chính (ID).
        /// </summary>
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
/// <summary>
        /// [CHI TIẾT] Thêm mới một bản ghi. Trước khi lưu, dữ liệu đã được kiểm duyệt chặt chẽ (Validation) để đảm bảo tính toàn vẹn.
        /// </summary>
        public async Task<int> AddAsync(Product p)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = @"INSERT INTO SanPham (TenSanPham, MaDanhMuc, GiaNhap, GiaBan, SoLuongTon, MoTa, Anh, TrangThai, NgayTao) 
                           VALUES (@TenSanPham, @MaDanhMuc, @GiaNhap, @GiaBan, @SoLuongTon, @MoTa, @Anh, @TrangThai, GETDATE())";
            return await DbContext.Conn.ExecuteAsync(sql, p);
        }
/// <summary>
        /// [CHI TIẾT] Cập nhật thông tin của bản ghi hiện có. Sử dụng Parameterized Query để bảo mật dữ liệu.
        /// </summary>
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

                public async Task<int> UpdateStockAsync(int productId, int quantityChange)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @QuantityChange WHERE MaSanPham = @ProductId";
            return await DbContext.Conn.ExecuteAsync(sql, new { QuantityChange = quantityChange, ProductId = productId });
        }

        public async Task<int> SoftDeleteAsync(int id)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();
            string sql = "UPDATE SanPham SET TrangThai = N'Ngưng bán' WHERE MaSanPham = @MaSanPham";
            return await DbContext.Conn.ExecuteAsync(sql, new { MaSanPham = id });
        }
/// <summary>
        /// [CHI TIẾT] Lọc và tìm kiếm dữ liệu dựa trên các tiêu chí đầu vào. Hỗ trợ tìm kiếm tương đối (LIKE) và bảo mật tham số.
        /// </summary>
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

                public async Task<IEnumerable<Product>> SearchByTextAsync(string keyword, string catIdText, string catNameText, string status)
        {
            if (DbContext.Conn == null || DbContext.Conn.State == ConnectionState.Closed) DbContext.Ketnoi();

            string sql = @"SELECT s.MaSanPham, s.TenSanPham, s.MaDanhMuc, d.TenDanhMuc, s.GiaNhap, s.GiaBan, 
                                  s.SoLuongTon, s.MoTa, s.Anh, s.TrangThai, s.NgayTao, s.NgayCapNhat 
                           FROM SanPham s 
                           LEFT JOIN DanhMuc d ON s.MaDanhMuc = d.MaDanhMuc 
                           WHERE 1=1";
            var parameters = new Dapper.DynamicParameters();

            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND (CAST(s.MaSanPham AS VARCHAR) LIKE @Keyword OR s.TenSanPham LIKE @Keyword)";
                parameters.Add("Keyword", $"%{keyword}%");
            }
            if (!string.IsNullOrEmpty(catIdText))
            {
                sql += " AND CAST(s.MaDanhMuc AS VARCHAR) LIKE @CatIdText";
                parameters.Add("CatIdText", $"%{catIdText}%");
            }
            if (!string.IsNullOrEmpty(catNameText))
            {
                sql += " AND d.TenDanhMuc LIKE @CatNameText";
                parameters.Add("CatNameText", $"%{catNameText}%");
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND s.TrangThai = @Status";
                parameters.Add("Status", status);
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


