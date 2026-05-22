using System;
using System.Windows.Forms;
using AssignmentApp.BLL.Services.Sales;
using AssignmentApp.DAL.Repositories.Sales;
using AssignmentApp.DTO;

namespace AssignmentApp.GUI.UserControls.Sales
{
    public partial class ucCustomer : UserControl
    {
        private CustomerService _customerService;

        public ucCustomer()
        {
            InitializeComponent();
        }

        private async void ucCustomer_Load(object sender, EventArgs e)
        {
           
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
