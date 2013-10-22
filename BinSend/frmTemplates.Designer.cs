namespace BinSend
{
    partial class frmTemplates
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
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lbTemplates = new System.Windows.Forms.ListBox();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 217);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(49, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(67, 217);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(49, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(122, 217);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(49, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lbTemplates
            // 
            this.lbTemplates.FormattingEnabled = true;
            this.lbTemplates.Location = new System.Drawing.Point(12, 25);
            this.lbTemplates.Name = "lbTemplates";
            this.lbTemplates.Size = new System.Drawing.Size(159, 186);
            this.lbTemplates.TabIndex = 0;
            this.lbTemplates.SelectedIndexChanged += new System.EventHandler(this.lbTemplates_SelectedIndexChanged);
            this.lbTemplates.DoubleClick += new System.EventHandler(this.lbTemplates_DoubleClick);
            // 
            // gbPreview
            // 
            this.gbPreview.Controls.Add(this.lblContent);
            this.gbPreview.Controls.Add(this.lblSubject);
            this.gbPreview.Controls.Add(this.lblEncoding);
            this.gbPreview.Controls.Add(this.lblTo);
            this.gbPreview.Controls.Add(this.lblFrom);
            this.gbPreview.Location = new System.Drawing.Point(177, 12);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new System.Drawing.Size(284, 228);
            this.gbPreview.TabIndex = 4;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "Preview";
            // 
            // lblContent
            // 
            this.lblContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblContent.Location = new System.Drawing.Point(6, 91);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(272, 134);
            this.lblContent.TabIndex = 4;
            this.lblContent.Text = "Content:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(6, 61);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 3;
            this.lblSubject.Text = "Subject:";
            // 
            // lblEncoding
            // 
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.Location = new System.Drawing.Point(6, 46);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(55, 13);
            this.lblEncoding.TabIndex = 2;
            this.lblEncoding.Text = "Encoding:";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(6, 31);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 13);
            this.lblTo.TabIndex = 1;
            this.lblTo.Text = "To:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(6, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Double click an entry to apply it";
            // 
            // frmTemplates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 247);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.lbTemplates);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTemplates";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Templates";
            this.gbPreview.ResumeLayout(false);
            this.gbPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListBox lbTemplates;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label label1;
    }
}