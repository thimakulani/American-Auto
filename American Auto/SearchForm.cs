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
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
            
        }
        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(InputSearch.Text))
            {
                try 
                {
                    //  lstData.Clear();
                    var client = new FirebaseDatabase().GetFirebaseClient();
                    var child = await client.Child("JobCard")
                        .OnceAsync<JobCard>();

                    Items.Clear();
                    lstData.Items.Clear();
                    foreach (var item in child)
                    {

                        if (item.Object.RegNo.Contains(InputSearch.Text))
                        {
                            ListViewItem listViewItem = new ListViewItem();
                            listViewItem.SubItems.Add(item.Object.Name);
                            listViewItem.SubItems.Add(item.Object.CellNo);
                            listViewItem.SubItems.Add(item.Object.Address);
                            listViewItem.SubItems.Add(item.Object.RegNo);
                            listViewItem.SubItems.Add(item.Object.EnginNo);
                            listViewItem.SubItems.Add(item.Object.ODOMETER);
                            listViewItem.SubItems.Add(item.Object.Made);
                            listViewItem.SubItems.Add(item.Object.ChessNo);
                            lstData.Items.Add(listViewItem);
                            Items.Add(item.Object);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                } 
            }
        }
        private List<JobCard> Items = new List<JobCard>();
        private async void SearchForm_Load(object sender, EventArgs e)
        {

            /* ColumnHeader header = new ColumnHeader();
             header.Text = "gkgag";
             header.Width = 100;
             header.TextAlign = HorizontalAlignment.Center;*/
            // lstData.View = View.Details;
            try
            {
                //  lstData.Clear();
                Items.Clear();
                var client = new FirebaseDatabase().GetFirebaseClient();
                var child = await client.Child("JobCard")
                    .OnceAsync<JobCard>();
                foreach (var item in child)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.SubItems.Add(item.Object.Name);
                    listViewItem.SubItems.Add(item.Object.CellNo);
                    listViewItem.SubItems.Add(item.Object.Address);
                    listViewItem.SubItems.Add(item.Object.RegNo);
                    listViewItem.SubItems.Add(item.Object.EnginNo);
                    listViewItem.SubItems.Add(item.Object.ODOMETER);
                    listViewItem.SubItems.Add(item.Object.Made);
                    listViewItem.SubItems.Add(item.Object.ChessNo);
                    lstData.Items.Add(listViewItem);
                    Items.Add(item.Object);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
        public event EventHandler<GetSearchResults> ResultsHandler;
        public class GetSearchResults : EventArgs
        {
            public ListViewItem Results { get; set; }
        }
        private void lstData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            if (MessageBox.Show($"You have selected {lstData.Items[e.Index].SubItems[1].Text} with Reg No {lstData.Items[e.Index].SubItems[4].Text}", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResultsHandler.Invoke(this, new GetSearchResults { Results = lstData.Items[e.Index] });
            }

        }

        private void lstData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
        }

        private void lstData_ItemActivate(object sender, EventArgs e)
        {
            
        }
    }
}
