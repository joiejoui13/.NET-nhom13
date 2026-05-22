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
    public partial class ucPromotion : UserControl
    {
        // Định nghĩa các sự kiện (events) để Form chính có thể lắng nghe
        public event EventHandler AddNewRequested;
        public event EventHandler<string> EditRequested; // Truyền vào ID của Khuyến mãi cần sửa

        public ucPromotion()
        {
            InitializeComponent();
        }

        private void btnAddNewPromotion_Click(object sender, EventArgs e)
        {
            // Gửi tín hiệu ra ngoài báo rằng nút Thêm mới đã được bấm
            // Form chính sẽ bắt sự kiện này để ẩn ucPromotion và hiện Form Thêm mới lên
            AddNewRequested?.Invoke(this, EventArgs.Empty);
        }

        private void dgvPromotion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có click vào 1 dòng hợp lệ không (không phải header)
            if (e.RowIndex >= 0)
            {
                // TODO: Cấu hình DataGridView có một cột nút bấm Sửa tên là "colEdit"
                // if (dgvPromotion.Columns[e.ColumnIndex].Name == "colEdit")
                // {
                //      string promotionId = dgvPromotion.Rows[e.RowIndex].Cells["colId"].Value.ToString();
                //      EditRequested?.Invoke(this, promotionId);
                // }
                
                // DEMO tạm thời: Nhấn vào bất kỳ đâu trên dòng cũng gọi EditRequested
                EditRequested?.Invoke(this, "ID_GIÁ_TRỊ_MẪU");
            }
        }
    }
}
