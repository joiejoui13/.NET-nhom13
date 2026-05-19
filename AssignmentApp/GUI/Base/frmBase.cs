using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AssignmentApp.GUI.Base
{
    public partial class frmBase : Form
    {
        public frmBase()
        {
            InitializeComponent();
            
            // Kích hoạt lại đầy đủ các nút Trừ (Minimize), Phóng to (Maximize) và Đóng (Close)
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Hoặc FormBorderStyle.FixedSingle nếu không muốn người dùng kéo giãn kích thước
        }
    }
}
