using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;
using AssignmentApp.GUI.Forms;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucOrderManagement : UserControl
    {
        private OrderService _orderService;

        public ucOrderManagement()
        {
            InitializeComponent();
        }

        private async void ucOrderManagement_Load(object sender, EventArgs e)
        {
         
        }

        private async System.Threading.Tasks.Task LoadData()
        {
         
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
    
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
          
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
      
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}
