using System;
using System.Windows.Forms;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using System.IO;
using Apitron.PDF.Kit.FlowLayout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace American_Auto
{
    public partial class ViewRecentJobCards : Form
    {
        public ViewRecentJobCards()
        {
            InitializeComponent();

        }
        List<JobCard> Items = new List<JobCard>();
        List<string> Keys = new List<string>();
        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                //  lstData.Clear();
                var client = new FirebaseDatabase().GetFirebaseClient();
                var child = await client.Child("JobCard")
                    .OnceAsync<JobCard>();
                Items.Clear();
                Keys.Clear();
                lstData.Items.Clear();
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
                    Keys.Add(item.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void lstData_ItemActivate(object sender, EventArgs e)
        {
            
        }
        private void GenerateJobCard(string _filename, JobCard jobCard)
        {
            string filename = $"{_filename}" + DateTime.Now.ToString("dddd, dd_MMM_yyyy HH_mm_ss tt") + ".pdf";

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                string imagesPath = @"";
                FlowDocument document = new FlowDocument()
                {
                    Margin = new Thickness(10)
                }; 
                //   document.StyleManager.RegisterStyle("gridrow.tableHeader", new Style() { Background = RgbColors.Transparent });
                document.StyleManager.RegisterStyle("gridrow.centerAlignedCells > *,gridrow > *.centerAlignedCell", new Style() { Align = Align.Center, Margin = new Thickness(0) });
                document.StyleManager.RegisterStyle("gridrow > *.leftAlignedCell", new Style() { Align = Align.Left, Margin = new Thickness(2, 0, 0, 0) });
                document.StyleManager.RegisterStyle("gridrow > *", new Style() { Align = Align.Right, Margin = new Thickness(0, 0, 2, 0) });
                document.StyleManager.RegisterStyle(".footer", new Style()
                {
                    Align = Align.Right,
                    VerticalAlign =
                    VerticalAlign.Bottom
                });



                ResourceManager resourceManager = new ResourceManager();
                resourceManager.RegisterResource(
                    new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("IMG-20210118-WA0010.jpg",
                    Path.Combine(imagesPath, "IMG-20210118-WA0010.jpg"), true)
                    { Interpolate = true });

                //resourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("stamp", Path.Combine(imagesPath, "iconfinder_22-hospital_5898980.png"), true) { Interpolate = true });
                document.PageHeader.Margin = new Thickness(0, 20, 0, 20);
                document.PageHeader.Padding = new Thickness(10, 0, 10, 0);
                //document.PageHeader.Height = 120;
                document.PageHeader.LineHeight = 20;
                //document.PageHeader.Add(new TextBlock("PAID")
                //{
                //    Display = Display.InlineBlock,
                //    Align = Align.Center,
                //    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 16)


                //});
                //document.PageHeader.Add(new Br());
                //document.PageHeader.Add(new TextBlock("TAX INVOICE")
                //{
                //    Display = Display.InlineBlock,
                //    Align = Align.Right,
                //    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 16)

                //});

                //document.PageHeader.Add(new TextBlock($"Date: {DateTime.Now.ToLongDateString()}")
                //{
                //    Display = Display.InlineBlock,
                //    Align = Align.Right,

                //});
                //document.PageHeader.Add(new TextBlock($"Invoice No: 12345678")
                //{
                //    Display = Display.InlineBlock,
                //    Align = Align.Right,

                //});
                //document.PageHeader.Add(new TextBlock($"Salesperson: PersonName")
                //{
                //    Display = Display.InlineBlock,
                //    Align = Align.Right,

                //});
                //// var fileStream = File.Create(DateTime.Now.ToString("dddd, dd/MMM/yyyy HH:mm:ss tt" + ".pdf"));
                //document.PageHeader.Add(new Br() { Height = 20});
                ////document.PageHeader.Add(new Image("IMG-20210118-WA0010.jpg") { Height = 50, Width = 50, VerticalAlign = VerticalAlign.Middle });



                Section pageSection = new Section() { Padding = new Thickness(20) };






                //table header
                pageSection.Add(CreateTables(jobCard));
                pageSection.Add(new Br { Height = 20 });
                pageSection.Add(new TextBlock("COMPLAINTS")
                {
                    Align = Align.Center,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });
                pageSection.Add(new Br { Height = 20 });
                pageSection.Add(new TextBlock($"{jobCard.Complaints}")
                {
                    Align = Align.Center,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });




                document.Add(pageSection);


                Section footer = new Section() { Padding = new Thickness(20) };
                footer.AddItems(CreateInfoSubsections(new string[] { "DIRECTOR/S MATEMANA" }));
                footer.AddItems(CreateInfoSubsections(new string[] { "SITULINGA MACHENICAL WORKSHOP(PTY) LTD/TA " +
                    "VELLY'S AMERICAN STYLE MOTOR" }));
                footer.AddItems(CreateInfoSubsections(new string[] { "REG NO: 2013/179692/07, I.T.9520249177" }));
                footer.AddItems(CreateInfoSubsections(new string[] { "BANK: STANDARD BANK, ACC NO:332549100, AC TYPE: CHEQUE" }));
                document.PageFooter.Add(footer);

                document.Write(fileStream, resourceManager, new PageBoundary(Boundaries.A4));
                fileStream.Close();
                document.Clear();
                PdfPreviewer pdfPreview = new PdfPreviewer(filename);
                pdfPreview.ShowDialog();
            }
        }
        private IList<Section> CreateInfoSubsections(string[] info)
        {
            List<Section> sections = new List<Section>();
            double width = 100 / info.Length;
            for (int i = 0; i < info.Length; i++)
            {
                Section ss = new Section()
                {
                    Width = Length.FromPercentage(width),
                    Display = Display.InlineBlock,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                };

                ss.Add(new TextBlock(info[i]));
                ss.Add(new Br());
                sections.Add(ss);
            }
            return sections;
        }
        private ContentElement CreateTables(JobCard jobCard)
        {
            Grid grid = new Grid(Length.Auto, Length.Auto);

            grid.Add(new GridRow(new TextBlock("American Style Auto".ToUpper()) 
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, 
            new TextBlock("JOB CARD")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"REG NO: {jobCard.RegNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"DATE: {jobCard.Dates}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"CHESS NO: {jobCard.ChessNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"TIME PROMISED: {jobCard.TimePromised}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"ENG NO: {jobCard.EnginNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"ADDRESS: {jobCard.Address}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"UNIT/CDA NO: {jobCard.UnitNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"SERVICE ADVISOR: {jobCard.Service}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"ODOMETER: {jobCard.ODOMETER}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"ORDER NO: {jobCard.OrderNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"MADE/MODEL: {jobCard.Made}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"CELL NO: {jobCard.CellNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"COLLECT CUSTOMER: {jobCard.Customer}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"FAX NO: {jobCard.FaxNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"DRIVER: {jobCard.Driver}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"EMAIL ADDRESS: {jobCard.Driver}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));

            return grid;
        }
        int index = -1;
        private void lstData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            index = e.Index;
        }


        private void ViewRecentJobCards_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (index != -1)
            {
                try
                {
                    var client = new FirebaseDatabase().GetFirebaseClient();
                    await client.Child($"JobCard/{Keys[index]}")
                        .DeleteAsync();
                    await LoadData();
                    index = -1;
                }
                catch (Exception)
                {
                    MessageBox.Show("Network error");
                }
            }
        }

        private void BtnGenerateJobCards_Click(object sender, EventArgs e)
        {
            if(index != -1)
            {
                GenerateJobCard("Invoice", Items[index]);
                index = -1;
            }
            
        }
    }
}
