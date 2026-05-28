using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Services.Main
{
    public interface IMainService
    {
        void Logout();
        MenuPermissions GetPermissions(string role);
    }
}
