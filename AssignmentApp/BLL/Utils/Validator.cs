using System;

namespace AssignmentApp.BLL.Utils
{
    public static class Validator
    {
        public static bool IsDate(string d)
        {
            string[] parts = d.Split('/');
            try
            {
                if ((Convert.ToInt32(parts[0]) >= 1) && (Convert.ToInt32(parts[0]) <= 31) && 
                    (Convert.ToInt32(parts[1]) >= 1) && (Convert.ToInt32(parts[1]) <= 12) && 
                    (Convert.ToInt32(parts[2]) >= 1900))
                    return true;
            }
            catch { }
            return false;
        }

        public static string ConvertDateTime(string d)
        {
            string[] parts = d.Split('/');
            return String.Format("{0}/{1}/{2}", parts[1], parts[0], parts[2]);
        }
    }
}
