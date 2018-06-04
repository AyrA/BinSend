namespace BinSend
{
    partial class frmBinSend
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBinSend));
            this.btnFile = new System.Windows.Forms.Button();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.nudKB = new System.Windows.Forms.NumericUpDown();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.cbFrom = new System.Windows.Forms.ComboBox();
            this.cbTo = new System.Windows.Forms.ComboBox();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.nudTTL = new System.Windows.Forms.NumericUpDown();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnEncoding = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudKB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.Location = new System.Drawing.Point(406, 492);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 12;
            this.btnFile.Text = "&Send file...";
            this.TT.SetToolTip(this.btnFile, "Selects a file and sends it");
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // tbSubject
            // 
            this.tbSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubject.Location = new System.Drawing.Point(12, 64);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(542, 20);
            this.tbSubject.TabIndex = 2;
            this.tbSubject.Text = "Subject";
            this.TT.SetToolTip(this.tbSubject, "Message subject");
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbText.Location = new System.Drawing.Point(12, 119);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbText.Size = new System.Drawing.Size(542, 325);
            this.tbText.TabIndex = 4;
            this.tbText.Text = resources.GetString("tbText.Text");
            this.TT.SetToolTip(this.tbText, "Message content with variables:\r\n{0}=File name\r\n{1}=Current part ID\r\n{2}=Number o" +
                    "f parts\r\n{3}=encoding scheme\r\n{4}=Length of {6}\r\n{5}=list of all hashes\r\n{6}=enc" +
                    "oded attachment");
            // 
            // nudKB
            // 
            this.nudKB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudKB.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudKB.Location = new System.Drawing.Point(274, 494);
            this.nudKB.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.nudKB.Name = "nudKB";
            this.nudKB.Size = new System.Drawing.Size(63, 20);
            this.nudKB.TabIndex = 10;
            this.TT.SetToolTip(this.nudKB, "Number of KB in each part. Set to 0 for 1 part transfer");
            this.nudKB.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // OFD
            // 
            this.OFD.Filter = "All Files|*.*";
            this.OFD.Title = "Select File to send";
            // 
            // pbStatus
            // 
            this.pbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbStatus.Location = new System.Drawing.Point(12, 492);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(194, 23);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbStatus.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 476);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status: Not sending";
            // 
            // cbFormat
            // 
            this.cbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(12, 452);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(512, 21);
            this.cbFormat.TabIndex = 5;
            this.TT.SetToolTip(this.cbFormat, "Encoding scheme.\r\nNote: yEnc is currently buggy and will fail to decode.");
            // 
            // btnGetFile
            // 
            this.btnGetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetFile.Location = new System.Drawing.Point(487, 492);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(67, 23);
            this.btnGetFile.TabIndex = 13;
            this.btnGetFile.Text = "&GetFiles";
            this.TT.SetToolTip(this.btnGetFile, "assemble files from inbox");
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // cbFrom
            // 
            this.cbFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFrom.FormattingEnabled = true;
            this.cbFrom.Location = new System.Drawing.Point(12, 12);
            this.cbFrom.Name = "cbFrom";
            this.cbFrom.Size = new System.Drawing.Size(542, 21);
            this.cbFrom.TabIndex = 0;
            this.TT.SetToolTip(this.cbFrom, "Sending identity");
            // 
            // cbTo
            // 
            this.cbTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbTo.FormattingEnabled = true;
            this.cbTo.Location = new System.Drawing.Point(12, 39);
            this.cbTo.Name = "cbTo";
            this.cbTo.Size = new System.Drawing.Size(542, 21);
            this.cbTo.TabIndex = 1;
            this.TT.SetToolTip(this.cbTo, "To address");
            this.cbTo.Leave += new System.EventHandler(this.cbTo_Leave);
            // 
            // nudStart
            // 
            this.nudStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudStart.Location = new System.Drawing.Point(212, 494);
            this.nudStart.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.nudStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(56, 20);
            this.nudStart.TabIndex = 9;
            this.TT.SetToolTip(this.nudStart, "Part Number to start with (in case the previous attempt made trouble)");
            this.nudStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudTTL
            // 
            this.nudTTL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudTTL.Location = new System.Drawing.Point(343, 494);
            this.nudTTL.Maximum = new decimal(new int[] {
            672,
            0,
            0,
            0});
            this.nudTTL.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTTL.Name = "nudTTL";
            this.nudTTL.Size = new System.Drawing.Size(57, 20);
            this.nudTTL.TabIndex = 11;
            this.TT.SetToolTip(this.nudTTL, "Time to live (in Hours) for messages.\r\nShorter causes faster sending.");
            this.nudTTL.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(12, 90);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnTemplate.TabIndex = 3;
            this.btnTemplate.Text = "Templates";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnEncoding
            // 
            this.btnEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncoding.Location = new System.Drawing.Point(530, 450);
            this.btnEncoding.Name = "btnEncoding";
            this.btnEncoding.Size = new System.Drawing.Size(24, 23);
            this.btnEncoding.TabIndex = 6;
            this.btnEncoding.Text = "?";
            this.btnEncoding.UseVisualStyleBackColor = true;
            this.btnEncoding.Click += new System.EventHandler(this.btnEncoding_Click);
            // 
            // frmBinSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 526);
            this.Controls.Add(this.btnEncoding);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.cbTo);
            this.Controls.Add(this.cbFrom);
            this.Controls.Add(this.cbFormat);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.nudStart);
            this.Controls.Add(this.nudTTL);
            this.Controls.Add(this.nudKB);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.tbSubject);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.btnFile);
            this.Name = "frmBinSend";
            this.Text = "Binary Send";
            ((System.ComponentModel.ISupportInitialize)(this.nudKB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.NumericUpDown nudKB;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.ToolTip TT;
        private System.Windows.Forms.ComboBox cbFrom;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnEncoding;
        private System.Windows.Forms.ComboBox cbTo;
        private System.Windows.Forms.NumericUpDown nudStart;
        private System.Windows.Forms.NumericUpDown nudTTL;
    }
}