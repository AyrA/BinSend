namespace BinSend
{
    partial class frmRead
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
            this.lvFragments = new System.Windows.Forms.ListView();
            this.chPart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAssemble = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(12, 12);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(242, 446);
            this.lbFiles.TabIndex = 0;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // lvFragments
            // 
            this.lvFragments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFragments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPart,
            this.chHash});
            this.lvFragments.FullRowSelect = true;
            this.lvFragments.Location = new System.Drawing.Point(260, 12);
            this.lvFragments.Name = "lvFragments";
            this.lvFragments.Size = new System.Drawing.Size(504, 446);
            this.lvFragments.TabIndex = 1;
            this.lvFragments.UseCompatibleStateImageBehavior = false;
            this.lvFragments.View = System.Windows.Forms.View.Details;
            // 
            // chPart
            // 
            this.chPart.Text = "Part";
            // 
            // chHash
            // 
            this.chHash.Text = "Hash";
            // 
            // btnAssemble
            // 
            this.btnAssemble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssemble.Location = new System.Drawing.Point(689, 472);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(75, 23);
            this.btnAssemble.TabIndex = 3;
            this.btnAssemble.Text = "&Assemble";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(608, 472);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // SFD
            // 
            this.SFD.Title = "Assemble File";
            // 
            // frmRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 502);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAssemble);
            this.Controls.Add(this.lvFragments);
            this.Controls.Add(this.lbFiles);
            this.Name = "frmRead";
            this.Text = "Read Files";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.ListView lvFragments;
        private System.Windows.Forms.ColumnHeader chPart;
        private System.Windows.Forms.ColumnHeader chHash;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.SaveFileDialog SFD;
    }
}