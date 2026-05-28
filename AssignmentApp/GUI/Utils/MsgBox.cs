using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AssignmentApp.GUI.Utils
{
    // Lớp tiện ích đóng gói Guna2MessageDialog nguyên bản của thư viện Guna
    public static class MsgBox
    {
        public static DialogResult Show(Form parent, string message, string title = "Thông báo", 
            MessageDialogButtons buttons = MessageDialogButtons.OK, 
            MessageDialogIcon icon = MessageDialogIcon.Information)
        {
            Guna2MessageDialog dialog = new Guna2MessageDialog
            {
                Parent = parent,
                Text = message,
                Caption = title,
                Buttons = buttons,
                Icon = icon,
                Style = MessageDialogStyle.Light // Giữ nguyên Style Light hiện đại, bo tròn và đổ bóng mặc định của Guna
            };
            
            return dialog.Show();
        }
    }
}
