using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace American_Auto
{
    public partial class PdfPreviewer : Form
    {
        private string filename;

        public PdfPreviewer(string filename)
        {
            InitializeComponent();
            this.filename = filename;
            var pdfContent = new MemoryStream(System.IO.File.ReadAllBytes(filename));
            pdfContent.Position = 0;

            pdfView.Document = new Apitron.PDF.Rasterizer.Document(pdfContent);

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            pdfView.Document.Dispose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                
                string folderName = folderBrowserDialog1.SelectedPath;
                string sourcePath = "";
                string sourceFile = System.IO.Path.Combine(sourcePath, filename);
                string destFile = System.IO.Path.Combine(folderName, filename);
                System.IO.Directory.CreateDirectory(folderName);
                System.IO.File.Copy(sourceFile, destFile, true);

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        filename = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(folderName, filename);
                        System.IO.File.Copy(s, destFile, true);
                        File.SetAttributes(destFile, FileAttributes.Normal);
                    }
                    MessageBox.Show("Your document has been saved successfully");
                }
                else
                {
                    MessageBox.Show("Saved!");
                }
            }
        }
    }
}
