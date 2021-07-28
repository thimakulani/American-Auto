using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace American_Auto
{
    public partial class CreateItem : Form
    {
        public CreateItem()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        public event EventHandler<GetItemsEventHandler> ItemsHandler;
        public class GetItemsEventHandler : EventArgs
        {
            public string[] Add_Items { get; set; }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //AddItems addItems = new AddItems()
            //{
            //    Description = txtDescription.Text,
            //    Item = txtItem.Text,
            //    QTY = txtQTY.Text,
            //    Vat = txtVat.Text,
            //    UnitPrice = txtUnitPrice.Text
            //};
            if (string.IsNullOrEmpty(txtUnitPrice.Text))
            {
                MessageBox.Show("Please provide price");
                return;
            }
            string[] items = {
                txtQTY.Text,
                txtItem.Text,
                txtDescription.Text,
                txtUnitPrice.Text,
                txtVat.Text,
                txtUnitPrice.Text

            };
            ItemsHandler.Invoke(this, new GetItemsEventHandler { Add_Items = items });
            Close();
            //Dispose();
        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if((e.KeyChar == '.')&&((sender as TextBox).Text.IndexOf('.') >= 0))
            {
                e.Handled = true;
            }
        }
    }
    public class AddItems
    {
        public string QTY { get; set; }
        public string Item { get; set; }
        public string UnitPrice { get; set; }
        public string Description { get; set; }
        public string Vat { get; set; }
    }
}
