using Firebase.Database;
using Firebase.Database.Query;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Image = Apitron.PDF.Kit.FlowLayout.Content.Image;
using Apitron.PDF.Kit.FlowLayout;
using System.Globalization;
using System.Threading;

namespace American_Auto
{
    public partial class Dashboad : Form
    {

        private FirebaseClient client;
        public Dashboad()
        {
            client = new FirebaseDatabase().GetFirebaseClient();
            InitializeComponent();
        }
        
        private void Dashboad_Load(object sender, EventArgs e)
        {


        }
        //Create Job Card
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            var child = client.Child("JobCard");
            JobCard jobCard = SetJobCard();

            try
            {
                await child.PostAsync(jobCard);
                ClearInputs();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Check your internet connection", "Error Saving Data");
            }
            if(MessageBox.Show("Would you like to generate a job card?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GenerateJobCard("JobCrd", jobCard);


            }

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
                document.PageHeader.Margin = new Thickness(0, 40, 0, 20);
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
                pageSection.Add(new TextBlock($"{JC_Complaints.Text}")
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
            }, 
            new TextBlock($"DATE: {jobCard.Dates}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"CHESS NO: {jobCard.ChessNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, 
            new TextBlock($"TIME PROMISED: {jobCard.TimePromised}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"ENG NO: {jobCard.EnginNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            },
            new TextBlock($"ADDRESS: {jobCard.Address}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"UNIT/CDA NO: {jobCard.UnitNo}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, 
            new TextBlock($"SERVICE ADVISOR: {jobCard.Service}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"ODOMETER: {jobCard.ODOMETER}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            },
            new TextBlock($"ORDER NO: {jobCard.OrderNo}") 
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }));
            grid.Add(new GridRow(new TextBlock($"MADE/MODEL: {jobCard.Made}")
            {
                Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
            }, new TextBlock($"CELL NO: {jobCard.CellNo}")));
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

        private JobCard SetJobCard()
        {
            return new JobCard
            {
                Address = JC_Address.Text,
                CellNo = JC_CellNo.Text,
                ChessNo = JC_ChessNo.Text,
                Customer = JC_CollectCustomer.Text,
                Dates = JC_Date.Text,
                Driver = JC_Driver.Text,
                EmailAddress = JC_EmailAddress.Text,
                EnginNo = JC_EngNo.Text,
                FaxNo = JC_FaxNo.Text,
                JobNO = JC_JobNo.Text,
                Made = JC_Made.Text,
                Name = JC_Name.Text,
                ODOMETER = JC_Odometer.Text,
                OrderNo = JC_OrderNo.Text,
                RegNo = JC_RegNo.Text,
                Service = JC_Service.Text,
                Complaints = JC_Complaints.Text
            };
        }

        private void ClearInputs()
        {
            JC_Address.Text = string.Empty;
            JC_CellNo.Text = string.Empty;
            JC_ChessNo.Text = string.Empty;
            JC_CollectCustomer.Text = string.Empty;
            JC_Date.Text = string.Empty;
            JC_Driver.Text = string.Empty;
            JC_EmailAddress.Text = string.Empty;
            JC_EngNo.Text = string.Empty;
            JC_FaxNo.Text = string.Empty;
            JC_JobNo.Text = string.Empty;
            JC_Made.Text = string.Empty;
            JC_Name.Text = string.Empty;
            JC_Odometer.Text = string.Empty;
            JC_OrderNo.Text = string.Empty;
            JC_RegNo.Text = string.Empty;
            JC_Service.Text = string.Empty;
            JC_Time_Promised.Text = string.Empty;
            JC_UnitNo.Text = string.Empty;
            JC_Complaints.Text = string.Empty;
        }

        private void BtnGenerateInvoice_Click(object sender, EventArgs e)
        {
            string fileName = $"Ïnvoice ";
            GenerateInvoice(fileName);

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }
        double totalInvoice = 0;
        double totalQuotation = 0;
        private void GenerateInvoice(string _filename)
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




                ResourceManager resourceManager = new ResourceManager();
                resourceManager.RegisterResource(
                    new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("IMG-20210118-WA0010.jpg",
                    Path.Combine(imagesPath, "IMG-20210118-WA0010.jpg"), true)
                    { Interpolate = true });
                
                //resourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("stamp", Path.Combine(imagesPath, "iconfinder_22-hospital_5898980.png"), true) { Interpolate = true });
                document.PageHeader.Margin = new Thickness(0, 40, 0, 20);
                document.PageHeader.Padding = new Thickness(10, 0, 10, 0);
                //document.PageHeader.Height = 120;
                document.PageHeader.LineHeight = 20;
                document.PageHeader.Add(new TextBlock("PAID")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Center,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                    

                });
                document.PageHeader.Add(new Br() { Height = 20 });
                
                

                Section pageSection = new Section() { Padding = new Thickness(20) };

                pageSection.Add(new TextBlock("TAX INVOICE")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                });

                pageSection.Add(new TextBlock($"Date: {DateTime.Now.ToLongDateString()}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });
                pageSection.Add(new TextBlock($"Invoice No: {IVC_InvoiceNo.Text}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });
                pageSection.Add(new TextBlock($"Salesperson: {INC_Salesperson.Text}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });
                // var fileStream = File.Create(DateTime.Now.ToString("dddd, dd/MMM/yyyy HH:mm:ss tt" + ".pdf"));
                pageSection.Add(new Br() { Height = 0 });
                pageSection.Add(new Image("IMG-20210118-WA0010.jpg") { Height = 75, Width = 75, VerticalAlign = VerticalAlign.Middle });
                pageSection.Add(new Br() { Height = 20 });



                pageSection.AddItems(CreateInfoSubsections(new string[] { "VAT:4120273927" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "157 Blaauwberg" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Ladanna" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Polokwane" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "0699" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "CK: 2013/179692/07" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Tel: 015 293 02157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Cell: 067 110 2157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Fax: 086 661 2157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Email: amaricanstylemotors@gmail.com" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "RFM Maintanance" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "First National Bank: 62538434721" }));


                pageSection.Add(new Br() { Padding = new Thickness(0, 20, 0, 20) });
                pageSection.Add(new TextBlock("Bill To:")
                {
                    Display = Display.InlineBlock,
                    
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 10)

                });
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{IVC_Name.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{IVC_CustomerAddress.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{IVC_CustomerAddress2.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"VAT: " }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"Tel: {IVC_CustomerCell.Text}" }));


                pageSection.AddItems(CreateInfoSubsections(new string[] { $"Reg no: {IVC_RegNo.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"Make: {IVC_Make}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"Eng No: {IVC_Eng.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"KM: {IVC_KM.Text}" }));



                
                //table header
                pageSection.Add(CreateInvoiceTable());
                pageSection.Add(new Br { Height = 20 });
                try
                {

                    double vat = totalInvoice * 0.15;

                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Subtotal R: {totalInvoice}" }));
                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"VAT: R {vat}" }));
                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Total: R{totalInvoice + vat}" }));
                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Balance: R{IVC_Balance.Text}" }));

                }
                catch
                {

                }

                
                pageSection.AddItems(CreateInfoSubsections(new string[] { "For the love of your car"}));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Thank you for choosing us"}));

                document.Add(pageSection);

                Section footer = new Section() { Padding = new Thickness(20) };
                footer.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Invoice #{IVC_InvoiceNo.Text}" }));

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

                };

                ss.Add(new TextBlock(info[i]) 
                {
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                });
                ss.Add(new Br());
                sections.Add(ss);
            }
            return sections;
        }
        private Grid CreateInvoiceTable()
        {
            Grid requestsGrid = new Grid(Length.FromPercentage(10), Length.Auto, Length.Auto, Length.Auto, Length.Auto, Length.Auto) 
            { 
                
            };

            requestsGrid.Add(new GridRow(
                new TextBlock("Qty")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Item")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Description")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Unit Price")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                },
                new TextBlock("TAX%")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                },
                new TextBlock("Total")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                })
                {
                    Class = "tableHeader centerAlignedCells"
                });


            for (int i = 0; i < lstInvoiceData.Items.Count; i++)
            {

                requestsGrid.Add(new GridRow(
                new TextBlock(lstInvoiceData.Items[i].SubItems[0].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstInvoiceData.Items[i].SubItems[1].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstInvoiceData.Items[i].SubItems[2].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstInvoiceData.Items[i].SubItems[3].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstInvoiceData.Items[i].SubItems[4].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),

                },
                new TextBlock(lstInvoiceData.Items[i].SubItems[5].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                })
                {
                    Class = "tableHeader centerAlignedCells"
                });
            }




            return requestsGrid;
        }
        private IList<Section> CreateInfoSubsectionsAlighnRight(string[] info)
        {
            List<Section> sections = new List<Section>();
            double width = 100 / info.Length;
            for (int i = 0; i < info.Length; i++)
            {
                Section ss = new Section()
                {
                    Width = Length.FromPercentage(width),
                    Display = Display.InlineBlock,
                    Align = Align.Right
                };

                ss.Add(new TextBlock(info[i])
                {
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 10)
                });
                ss.Add(new Br());
                sections.Add(ss);
            }
            return sections;
        }

        private void BtnAddToQuote_Click(object sender, EventArgs e)
        {
            CreateItem createItem = new CreateItem();
            createItem.ItemsHandler += CreateItem_ItemsHandler;
            createItem.ShowDialog();
        }
        private void CreateItem_ItemsHandler(object sender, CreateItem.GetItemsEventHandler e)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            //addItems.Add(e.Add_Items);
            lstData.Items.Add(new ListViewItem(e.Add_Items));
            double tot = 0;
            if (lstData.Items.Count > 0)
            {
                for (int i = 0; i < lstData.Items.Count; i++)
                {
                    tot += (double.Parse(lstData.Items[i].SubItems[3].Text, System.Globalization.CultureInfo.CurrentCulture));
                }
            }
            lblTotalQuote.Text = $"TOTAL: R" + tot.ToString("0.##");
            totalQuotation = tot;

        }

        private void BtnQuoteRemove_Click(object sender, EventArgs e)
        {

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;

            // lstData.Items.Remove(lstData.CheckedItems.);
            ListView.CheckedListViewItemCollection selected = new ListView.CheckedListViewItemCollection(lstData);


            selected = lstData.CheckedItems;
            foreach (var itms in selected)
            {

                lstData.Items.Remove((ListViewItem)itms);
            }
            double tot = 0;
            if (lstData.Items.Count > 0)
            {
                for (int i = 0; i < lstData.Items.Count; i++)
                {
                    tot += (double.Parse(lstData.Items[i].SubItems[3].Text, System.Globalization.CultureInfo.CurrentCulture));
                }
            }
            lblTotalQuote.Text = $"TOTAL: R" + tot.ToString("0.##");
            totalQuotation = tot;

        }
        private void lstData_ItemCheck(object sender, ItemCheckEventArgs e)
        {


        }

        private void AddToInvoice_Click(object sender, EventArgs e)
        {
            CreateItem add_invoice_items = new CreateItem();
            add_invoice_items.ItemsHandler += Add_invoice_items_ItemsHandler;
            add_invoice_items.ShowDialog();
        }

        private void Add_invoice_items_ItemsHandler(object sender, CreateItem.GetItemsEventHandler e)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            lstInvoiceData.Items.Add(new ListViewItem(e.Add_Items));
            double tot = 0;
            if (lstInvoiceData.Items.Count > 0)
            {
                for (int i = 0; i < lstInvoiceData.Items.Count; i++)
                {
                    tot += (double.Parse(lstInvoiceData.Items[i].SubItems[3].Text, System.Globalization.CultureInfo.CurrentCulture));
                }
            }
            lblTotalInvoice.Text = $"TOTAL: R" + tot.ToString("0.##");
            totalInvoice = tot;

        }

        private void BtnRemoveInvoiceItem_Click(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            ListView.CheckedListViewItemCollection selected = new ListView.CheckedListViewItemCollection(lstInvoiceData);


            selected = lstInvoiceData.CheckedItems;
            foreach (var itms in selected)
            {

                lstInvoiceData.Items.Remove((ListViewItem)itms);
            }
            double tot = 0;
            if(lstInvoiceData.Items.Count > 0)
            {
                for (int i = 0; i < lstInvoiceData.Items.Count; i++)
                {
                    tot += (double.Parse(lstInvoiceData.Items[i].SubItems[3].Text, System.Globalization.CultureInfo.CurrentCulture));
                }
            }
            lblTotalInvoice.Text = $"TOTAL: R" + tot.ToString("0.##");
            totalInvoice = tot;

        }

        private void BtnGenerateQuotation_Click(object sender, EventArgs e)
        {
            GenerateQuotation("Quotation");
        }
        private void GenerateQuotation(string _filename)
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




                ResourceManager resourceManager = new ResourceManager();
                resourceManager.RegisterResource(
                    new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("IMG-20210118-WA0010.jpg",
                    Path.Combine(imagesPath, "IMG-20210118-WA0010.jpg"), true)
                    { Interpolate = true });

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
                document.PageHeader.Add(new Br() { Height = 15});
                document.PageHeader.Add(new TextBlock("Quote")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 12)

                });




                Section pageSection = new Section() { Padding = new Thickness(20) };

                pageSection.Add(new TextBlock($"Date: {DateTime.Now.ToLongDateString()}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                });
                pageSection.Add(new TextBlock($"Quote No: {QT_QuoteNo.Text}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                });
                pageSection.Add(new TextBlock($"Salesperson: {QuotationSalesPerson.Text}")
                {
                    Display = Display.InlineBlock,
                    Align = Align.Right,
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                });
                // var fileStream = File.Create(DateTime.Now.ToString("dddd, dd/MMM/yyyy HH:mm:ss tt" + ".pdf"));
                pageSection.Add(new Br() { Height = 0 });
                pageSection.Add(new Image("IMG-20210118-WA0010.jpg") { Height = 70, Width = 70, VerticalAlign = VerticalAlign.Middle });
                pageSection.Add(new Br() { Height = 20 });



                pageSection.AddItems(CreateInfoSubsections(new string[] { "VAT:4120273927" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "157 Blaauwberg" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Ladanna" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Polokwane" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "0699" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "CK: 2013/179692/07" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Tel: 015 293 02157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Cell: 067 110 2157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Fax: 086 661 2157" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Email: amaricanstylemotors@gmail.com" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "RFM Maintanance" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { "First National Bank: 62538434721" }));


                pageSection.Add(new Br() { Padding = new Thickness(0, 20, 0, 20) });
                pageSection.Add(new TextBlock("Bill To:")
                {
                    Display = Display.InlineBlock,

                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.HelveticaBold, 12)

                });
                pageSection.Add(new Br() { Height = 20});

                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{QT_Name.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"Cell: {QT_CustomerCell.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{QT_ClientAddress.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"{QT_ClientAddress2.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"VAT: " }));

                pageSection.Add(new Br() { Height = 20 });

                pageSection.AddItems(CreateInfoSubsections(new string[] { $"MAKE: {QT_Make.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"REG: {QT_RegNo.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"VIN: {QT_Vin.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"ENG: {QT_Eng.Text}" }));
                pageSection.AddItems(CreateInfoSubsections(new string[] { $"KM: {QT_KM.Text}KM" }));

                pageSection.AddItems(CreateInfoSubsections(new string[] { "\n" }));

                pageSection.Add(new Br() { Height = 20 });

                  

                //table header
                pageSection.Add(CreateQuotationTable());
                pageSection.Add(new Br { Height = 20 });
                try
                {

                    double vat = totalQuotation * 0.15;

                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Subtotal: R{totalQuotation.ToString("0.##")}" }));
                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"VAT: R{vat.ToString("0.##")}" }));
                    pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Total: R{(totalQuotation + vat).ToString("0.##")}" }));
                    //pageSection.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Paid: {totalQuotation}" }));

                }
                catch
                {

                }
                pageSection.Add(new Br() { Height = 20 });
                pageSection.AddItems(CreateInfoSubsections(new string[] { "Please contact us for more information about payment options" }));
                document.Add(pageSection);

                Section footer = new Section() { Padding = new Thickness(20) };
                footer.AddItems(CreateInfoSubsectionsAlighnRight(new string[] { $"Quote #{QT_QuoteNo.Text}" }));
                
                document.PageFooter.Add(footer);


                document.Write(fileStream, resourceManager, new PageBoundary(Boundaries.A4));
                fileStream.Close();
                document.Clear();
                PdfPreviewer pdfPreview = new PdfPreviewer(filename);
                pdfPreview.ShowDialog();
            }

        }

        private Grid CreateQuotationTable()
        {
            Grid requestsGrid = new Grid(Length.FromPercentage(10), Length.Auto, Length.Auto, Length.Auto, Length.Auto, Length.Auto)
            {

            };

            requestsGrid.Add(new GridRow(
                new TextBlock("Qty")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Item")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Description")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock("Unit Price")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                },
                new TextBlock("TAX%")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                },
                new TextBlock("Total")
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)

                })
            {
                Class = "tableHeader centerAlignedCells"
            });


            for (int i = 0; i < lstData.Items.Count; i++)
            {

                requestsGrid.Add(new GridRow(
                new TextBlock(lstData.Items[i].SubItems[0].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstData.Items[i].SubItems[1].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstData.Items[i].SubItems[2].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstData.Items[i].SubItems[3].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstData.Items[i].SubItems[4].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                },
                new TextBlock(lstData.Items[i].SubItems[5].Text)
                {
                    Padding = new Thickness(3, 3, 3, 3),
                    Font = new Apitron.PDF.Kit.Styles.Text.Font(Apitron.PDF.Kit.FixedLayout.Resources.Fonts.StandardFonts.TimesRoman, 10)
                })
                {
                    Class = "tableHeader centerAlignedCells"
                });
            }




            return requestsGrid;
        }

        SearchForm searchFormQuotes;
        private void BtnSearchQuote_Click(object sender, EventArgs e)
        {
            searchFormQuotes = new SearchForm();
            searchFormQuotes.ResultsHandler += SearchFormQuotes_ResultsHandler;
            searchFormQuotes.ShowDialog();
        }

        private void SearchFormQuotes_ResultsHandler(object sender, SearchForm.GetSearchResults e)
        {
            QT_Name.Text = e.Results.SubItems[1].Text;
            QT_CustomerCell.Text = e.Results.SubItems[2].Text;
            QT_ClientAddress.Text = e.Results.SubItems[3].Text;
            QT_RegNo.Text = e.Results.SubItems[4].Text;
            QT_Eng.Text = e.Results.SubItems[5].Text;
            QT_ClientAddress2.Text = e.Results.SubItems[0].Text;
            QT_Make.Text = e.Results.SubItems[6].Text;
            searchFormQuotes.Close();
        }

        private void lstInvoiceData_ControlAdded(object sender, ControlEventArgs e)
        {
            
        }
        SearchForm searchInvoiceForm;
        private void BtnInvoiceSearch_Click(object sender, EventArgs e)
        {
            searchInvoiceForm = new SearchForm();
            searchInvoiceForm.ResultsHandler += SearchInvoiceForm_ResultsHandler;
            searchInvoiceForm.ShowDialog();
        }

        private void SearchInvoiceForm_ResultsHandler(object sender, SearchForm.GetSearchResults e)
        {
            IVC_Name.Text = e.Results.SubItems[1].Text;
            IVC_CustomerCell.Text = e.Results.SubItems[2].Text;
            IVC_CustomerAddress.Text = e.Results.SubItems[3].Text;
            IVC_RegNo.Text = e.Results.SubItems[4].Text;
            IVC_Eng.Text = e.Results.SubItems[5].Text;
            IVC_CustomerAddress2.Text = e.Results.SubItems[0].Text;
            IVC_Make.Text = e.Results.SubItems[6].Text;
            searchInvoiceForm.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void BtnRecentJobCards_Click(object sender, EventArgs e)
        {
            ViewRecentJobCards viewRecentJobCards = new ViewRecentJobCards();
            viewRecentJobCards.ShowDialog();
        }

        private void Dashboad_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private void IVC_Balance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') >= 0))
            {
                e.Handled = true;
            }
        }
    }
    public class JobCard
    {
        public string Id { get; set; }
        public string Complaints { get; set; }
        public string RegNo { get; set; }
        public string ChessNo { get; set; }
        public string EnginNo { get; set; }
        public string ODOMETER { get; set; }
        public string JobNO { get; set; }
        public string Made { get; set; }
        public string Customer { get; set; }
        public string Driver { get; set; }
        public string Dates { get; set; }
        public string TimePromised { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Service { get; set; }
        public string OrderNo { get; set; }
        public string CellNo { get; set; }
        public string FaxNo { get; set; }
        public string EmailAddress { get; set; }
        public string UnitNo { get; set; }



    }
    public class FirebaseDatabase
    {
        public FirebaseClient GetFirebaseClient()
        {
            return new FirebaseClient("https://votemate-19127.firebaseio.com/");
        }
    }
        
}
