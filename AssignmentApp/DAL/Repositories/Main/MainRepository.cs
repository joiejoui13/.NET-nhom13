using System;
using System.Data;
using System.Threading.Tasks;
using AssignmentApp.DAL.Core;
using Dapper;

namespace AssignmentApp.DAL.Repositories.Main
{
    public class MainRepository : IMainRepository
    {
        // Tầng DAL của Main: Chuẩn bị sẵn cấu trúc 3 lớp.
        // Hiện tại các nghiệp vụ của Main (Logout, Phân quyền Menu) chưa cần gọi xuống Database.
        // File này sẵn sàng để load các cấu hình, thông báo hệ thống từ DB sau này.
    }
}
