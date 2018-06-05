namespace BinSend
{
    partial class frmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbFromAddr = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbToAddr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudChunk = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudTTL = new System.Windows.Forms.NumericUpDown();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnManageAdd = new System.Windows.Forms.Button();
            this.btnAddressBook = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudChunk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // cbFromAddr
            // 
            this.cbFromAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFromAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFromAddr.FormattingEnabled = true;
            this.cbFromAddr.Location = new System.Drawing.Point(61, 6);
            this.cbFromAddr.Name = "cbFromAddr";
            this.cbFromAddr.Size = new System.Drawing.Size(458, 21);
            this.cbFromAddr.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            // 
            // tbToAddr
            // 
            this.tbToAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbToAddr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbToAddr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbToAddr.Location = new System.Drawing.Point(61, 33);
            this.tbToAddr.Name = "tbToAddr";
            this.tbToAddr.Size = new System.Drawing.Size(458, 20);
            this.tbToAddr.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Subject";
            // 
            // tbSubject
            // 
            this.tbSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubject.Location = new System.Drawing.Point(61, 59);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(511, 20);
            this.tbSubject.TabIndex = 7;
            // 
            // tbBody
            // 
            this.tbBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBody.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBody.Location = new System.Drawing.Point(61, 85);
            this.tbBody.Multiline = true;
            this.tbBody.Name = "tbBody";
            this.tbBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBody.Size = new System.Drawing.Size(511, 282);
            this.tbBody.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Body";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 375);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Chunk size (KB)";
            // 
            // nudChunk
            // 
            this.nudChunk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudChunk.Location = new System.Drawing.Point(146, 373);
            this.nudChunk.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudChunk.Name = "nudChunk";
            this.nudChunk.Size = new System.Drawing.Size(69, 20);
            this.nudChunk.TabIndex = 12;
            this.nudChunk.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(236, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "TTL (h)";
            // 
            // nudTTL
            // 
            this.nudTTL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudTTL.Location = new System.Drawing.Point(284, 373);
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
            this.nudTTL.Size = new System.Drawing.Size(69, 20);
            this.nudTTL.TabIndex = 14;
            this.nudTTL.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(525, 400);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(47, 23);
            this.btnSelectFile.TabIndex = 18;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // tbFile
            // 
            this.tbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFile.Location = new System.Drawing.Point(61, 401);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(458, 20);
            this.tbFile.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 375);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Options";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 404);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "File";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(61, -394);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(455, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // btnManageAdd
            // 
            this.btnManageAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageAdd.Location = new System.Drawing.Point(525, 4);
            this.btnManageAdd.Name = "btnManageAdd";
            this.btnManageAdd.Size = new System.Drawing.Size(47, 23);
            this.btnManageAdd.TabIndex = 2;
            this.btnManageAdd.Text = "...";
            this.btnManageAdd.UseVisualStyleBackColor = true;
            // 
            // btnAddressBook
            // 
            this.btnAddressBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddressBook.Location = new System.Drawing.Point(525, 33);
            this.btnAddressBook.Name = "btnAddressBook";
            this.btnAddressBook.Size = new System.Drawing.Size(47, 23);
            this.btnAddressBook.TabIndex = 5;
            this.btnAddressBook.Text = "...";
            this.btnAddressBook.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(525, 427);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(47, 23);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // btnTemplate
            // 
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTemplate.Location = new System.Drawing.Point(359, 373);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(213, 23);
            this.btnTemplate.TabIndex = 15;
            this.btnTemplate.Text = "Select and Manage Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            // 
            // OFD
            // 
            this.OFD.Filter = "All Files|*.*";
            this.OFD.Title = "Select file to send";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.btnAddressBook);
            this.Controls.Add(this.btnManageAdd);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudTTL);
            this.Controls.Add(this.nudChunk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbBody);
            this.Controls.Add(this.tbSubject);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbToAddr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.cbFromAddr);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "frmMain";
            this.Text = "BinSend";
            ((System.ComponentModel.ISupportInitialize)(this.nudChunk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFromAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbToAddr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudChunk;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudTTL;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button btnManageAdd;
        private System.Windows.Forms.Button btnAddressBook;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.OpenFileDialog OFD;
    }
}

