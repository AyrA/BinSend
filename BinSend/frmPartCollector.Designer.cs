namespace BinSend
{
    partial class frmPartCollector
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
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.lvParts = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chPart = new System.Windows.Forms.ColumnHeader();
            this.chHash = new System.Windows.Forms.ColumnHeader();
            this.chSize = new System.Windows.Forms.ColumnHeader();
            this.chValid = new System.Windows.Forms.ColumnHeader();
            this.gbParts = new System.Windows.Forms.GroupBox();
            this.btnAssemble = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDelPart = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gbFiles.SuspendLayout();
            this.gbParts.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbFiles
            // 
            this.lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(3, 16);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(206, 329);
            this.lbFiles.TabIndex = 0;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // gbFiles
            // 
            this.gbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbFiles.Controls.Add(this.lbFiles);
            this.gbFiles.Location = new System.Drawing.Point(12, 38);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(212, 349);
            this.gbFiles.TabIndex = 0;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            // 
            // lvParts
            // 
            this.lvParts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chPart,
            this.chHash,
            this.chSize,
            this.chValid});
            this.lvParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvParts.FullRowSelect = true;
            this.lvParts.Location = new System.Drawing.Point(3, 16);
            this.lvParts.Name = "lvParts";
            this.lvParts.Size = new System.Drawing.Size(626, 330);
            this.lvParts.TabIndex = 0;
            this.lvParts.UseCompatibleStateImageBehavior = false;
            this.lvParts.View = System.Windows.Forms.View.Details;
            this.lvParts.DoubleClick += new System.EventHandler(this.lvParts_DoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "File name";
            this.chName.Width = 110;
            // 
            // chPart
            // 
            this.chPart.Text = "Part";
            // 
            // chHash
            // 
            this.chHash.Text = "Hash";
            this.chHash.Width = 301;
            // 
            // chSize
            // 
            this.chSize.Text = "Size (KB)";
            // 
            // chValid
            // 
            this.chValid.Text = "Valid";
            // 
            // gbParts
            // 
            this.gbParts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbParts.Controls.Add(this.lvParts);
            this.gbParts.Location = new System.Drawing.Point(230, 38);
            this.gbParts.Name = "gbParts";
            this.gbParts.Size = new System.Drawing.Size(632, 349);
            this.gbParts.TabIndex = 1;
            this.gbParts.TabStop = false;
            this.gbParts.Text = "Parts of selected File";
            // 
            // btnAssemble
            // 
            this.btnAssemble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAssemble.Location = new System.Drawing.Point(12, 393);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(103, 23);
            this.btnAssemble.TabIndex = 2;
            this.btnAssemble.Text = "&Assemble File";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(121, 393);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(103, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete File";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDelPart
            // 
            this.btnDelPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelPart.Location = new System.Drawing.Point(787, 393);
            this.btnDelPart.Name = "btnDelPart";
            this.btnDelPart.Size = new System.Drawing.Size(75, 23);
            this.btnDelPart.TabIndex = 6;
            this.btnDelPart.Text = "D&elete Part";
            this.btnDelPart.UseVisualStyleBackColor = true;
            this.btnDelPart.Click += new System.EventHandler(this.btnDelPart_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(706, 393);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 5;
            this.btnPreview.Text = "&Preview Part";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRescan.Location = new System.Drawing.Point(233, 393);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(180, 23);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan for new Parts and Files";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(682, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Use this window to decode content, sent with the BinSend DEFAULT template or a co" +
                "mpatible scheme";
            // 
            // frmPartCollector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 427);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnDelPart);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAssemble);
            this.Controls.Add(this.gbParts);
            this.Controls.Add(this.gbFiles);
            this.Name = "frmPartCollector";
            this.Text = "Part collector";
            this.gbFiles.ResumeLayout(false);
            this.gbParts.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.ListView lvParts;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chPart;
        private System.Windows.Forms.GroupBox gbParts;
        private System.Windows.Forms.ColumnHeader chHash;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chValid;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDelPart;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Label label1;
    }
}