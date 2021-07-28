
namespace American_Auto
{
    partial class ViewRecentJobCards
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewRecentJobCards));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstData = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnGenerateJobCards = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.BtnGenerateJobCards);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 434);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(919, 53);
            this.panel1.TabIndex = 0;
            // 
            // lstData
            // 
            this.lstData.AutoArrange = false;
            this.lstData.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lstData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstData.CheckBoxes = true;
            this.lstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstData.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstData.FullRowSelect = true;
            this.lstData.GridLines = true;
            this.lstData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstData.HideSelection = false;
            this.lstData.HoverSelection = true;
            this.lstData.Location = new System.Drawing.Point(0, 0);
            this.lstData.MultiSelect = false;
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(919, 434);
            this.lstData.TabIndex = 1;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            this.lstData.ItemActivate += new System.EventHandler(this.lstData_ItemActivate);
            this.lstData.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstData_ItemCheck);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Select";
            this.columnHeader9.Width = 65;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Name";
            this.columnHeader10.Width = 138;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Cell No";
            this.columnHeader11.Width = 134;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Address";
            this.columnHeader12.Width = 158;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Reg No";
            this.columnHeader13.Width = 141;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Eng No";
            this.columnHeader1.Width = 152;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ODOMETER";
            this.columnHeader2.Width = 139;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "MODEL/MAKE";
            this.columnHeader3.Width = 168;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Chess No";
            this.columnHeader4.Width = 191;
            // 
            // BtnGenerateJobCards
            // 
            this.BtnGenerateJobCards.Location = new System.Drawing.Point(758, 9);
            this.BtnGenerateJobCards.Name = "BtnGenerateJobCards";
            this.BtnGenerateJobCards.Size = new System.Drawing.Size(148, 35);
            this.BtnGenerateJobCards.TabIndex = 2;
            this.BtnGenerateJobCards.Text = "Generate Job Card";
            this.BtnGenerateJobCards.UseVisualStyleBackColor = true;
            this.BtnGenerateJobCards.Click += new System.EventHandler(this.BtnGenerateJobCards_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(612, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 35);
            this.button2.TabIndex = 3;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ViewRecentJobCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 487);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewRecentJobCards";
            this.Load += new System.EventHandler(this.ViewRecentJobCards_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lstData;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtnGenerateJobCards;
    }
}