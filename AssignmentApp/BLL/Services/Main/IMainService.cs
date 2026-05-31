using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Main
{
    /// <summary>
    /// Interface định nghĩa các nghiệp vụ (Business Logic Layer).
    /// Giao diện (GUI) sẽ gọi đến interface này thay vì gọi trực tiếp xuống Database.
    /// </summary>
    public interface IMainService
    {
        void Logout();
        MenuPermissions GetPermissions(string role);
    }
}
