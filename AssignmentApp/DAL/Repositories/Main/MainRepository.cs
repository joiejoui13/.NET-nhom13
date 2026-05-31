using System;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Main
{
    /// <summary>
    /// Class thao tác trực tiếp với CSDL (Tầng DAL - Data Access Layer).
    /// Áp dụng Pattern Repository và thư viện Micro-ORM Dapper để tối ưu hóa hiệu năng truy vấn.
    /// Mọi câu lệnh SQL đều dùng Parameterized Query để chống SQL Injection.
    /// </summary>
    public class MainRepository : IMainRepository
    {
        // Tầng DAL của Main: Chuẩn bị sẵn cấu trúc 3 lớp.
        // Hiện tại các nghiệp vụ của Main (Logout, Phân quyền Menu) chưa cần gọi xuống Database.
        // File này sẵn sàng để load các cấu hình, thông báo hệ thống từ DB sau này.
    }
}
