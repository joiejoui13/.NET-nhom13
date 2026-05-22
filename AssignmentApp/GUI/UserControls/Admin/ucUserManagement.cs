using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucUserManagement : UserControl
    {
        // Định nghĩa các sự kiện (events) để Form chính có thể lắng nghe
        public event EventHandler AddNewRequested;
        public event EventHandler<string> EditRequested; // Truyền vào ID của người dùng cần sửa

        public ucUserManagement()
        {
            InitializeComponent();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            // Gửi tín hiệu ra ngoài báo rằng nút Thêm mới đã được bấm
            AddNewRequested?.Invoke(this, EventArgs.Empty);
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có click vào 1 dòng hợp lệ không
            if (e.RowIndex >= 0)
            {
                // DEMO tạm thời: Nhấn vào bất kỳ đâu trên dòng cũng gọi EditRequested
                EditRequested?.Invoke(this, "ID_USER_MAU");
            }
        }
    }
}
