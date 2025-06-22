using System;
using System.Data;
using System.Windows.Forms;
using OrderSearchApp;

namespace OrderSearchApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;
            dgvOrderDetail.AllowUserToAddRows = false;
            dgvOrderDetail.ReadOnly = true; // 預設唯讀
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string orderID = txtOrderID.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();

            // 禁止全空搜尋
            if (string.IsNullOrEmpty(orderID) && string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Please enter at least Order ID or Customer Name.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // OrderID 只允許和客戶名稱一起查
            if (!string.IsNullOrEmpty(orderID) && string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("When entering Order ID, you must also provide Customer Name.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = Database.SearchOrder(orderID, customerName);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No matching order found.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvOrderDetail.DataSource = null;
            }
            else
            {
                dgvOrderDetail.DataSource = dt;
                dgvOrderDetail.AllowUserToAddRows = false;
                dgvOrderDetail.ReadOnly = true; // <-- 設唯讀
                dgvOrderDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                if (dgvOrderDetail.Columns.Contains("Customer Name"))
                    dgvOrderDetail.Columns["Customer Name"].Width = 120;
                if (dgvOrderDetail.Columns.Contains("Unit"))
                    dgvOrderDetail.Columns["Unit"].Width = 80;
                if (dgvOrderDetail.Columns.Contains("Total"))
                    dgvOrderDetail.Columns["Total"].Width = 80;
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOrderID.Text = "";
            txtCustomerName.Text = "";
            dgvOrderDetail.DataSource = null;
        }
    }
}
