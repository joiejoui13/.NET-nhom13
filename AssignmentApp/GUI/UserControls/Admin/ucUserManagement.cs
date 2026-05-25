using System;
using System.Data;
using System.Windows.Forms;

namespace AssignmentApp.GUI.UserControls.Admin
{
    public partial class ucUserManagement : UserControl
    {
        public ucUserManagement()
        {
            InitializeComponent();
        }
        DataTable tblND;
        private void ucUserManagement_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT manhanvien, tennhanvien, maphongban, diachi, dienthoai, ngaysinh, gioitinh, anh, luong, lamviec FROM tblNhanvien";
            tblND = BLL.Services.Admin.EmployeeService.GetDataToTable(sql);
            dgvUsers.DataSource = tblND;
            dgvUsers.Columns[0].HeaderText = "Mã nhân viên";
            dgvUsers.Columns[1].HeaderText = "Tên nhân viên";
            dgvUsers.Columns[2].HeaderText = "Mã phòng ban";
            dgvUsers.Columns[3].HeaderText = "Địa chỉ";
            dgvUsers.Columns[4].HeaderText = "Điện thoại";
            dgvUsers.Columns[5].HeaderText = "Ngày sinh";
            dgvUsers.Columns[6].HeaderText = "Giới tính";
            dgvUsers.Columns[7].HeaderText = "Ảnh";
            dgvUsers.Columns[8].HeaderText = "Lương";
            dgvUsers.Columns[9].HeaderText = "Làm việc";
            dgvUsers.Columns[0].Width = 100;
            dgvUsers.Columns[1].Width = 150;
            dgvUsers.Columns[2].Width = 150;
            dgvUsers.Columns[3].Width = 150;
            dgvUsers.Columns[4].Width = 150;
            dgvUsers.Columns[5].Width = 150;
            dgvUsers.Columns[6].Width = 150;
            dgvUsers.Columns[7].Width = 150;
            dgvUsers.Columns[8].Width = 150;
            dgvUsers.Columns[9].Width = 150;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
