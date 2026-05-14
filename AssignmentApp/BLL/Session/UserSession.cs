using AssignmentApp.DTO;

namespace AssignmentApp.BLL.Session
{
    public static class UserSession
    {
        public static User CurrentUser { get; set; }
        public static System.DateTime LoginTime { get; set; }

        public static void ClearSession()
        {
            CurrentUser = null;
        }
    }
}
