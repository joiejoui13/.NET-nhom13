using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucDelivery : Base.ucBase
    {
        private DeliveryService _deliveryService;

        public ucDelivery()
        {     
            InitializeComponent();
        }

        private async void ucDelivery_Load(object sender, EventArgs e)
        {
         
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
          
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void dgvDeliveries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void cbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
