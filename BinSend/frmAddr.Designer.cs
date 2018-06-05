namespace BinSend
{
    partial class frmAddr
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
            this.cbAddr = new System.Windows.Forms.ComboBox();
            this.gbDel = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.gbGen = new System.Windows.Forms.GroupBox();
            this.cbShortAddr = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cbRandom = new System.Windows.Forms.RadioButton();
            this.cbDeterministic = new System.Windows.Forms.RadioButton();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbLabel = new System.Windows.Forms.TextBox();
            this.lblLabel = new System.Windows.Forms.Label();
            this.cbDML = new System.Windows.Forms.CheckBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.gbDel.SuspendLayout();
            this.gbGen.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAddr
            // 
            this.cbAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAddr.FormattingEnabled = true;
            this.cbAddr.Location = new System.Drawing.Point(6, 23);
            this.cbAddr.Name = "cbAddr";
            this.cbAddr.Size = new System.Drawing.Size(367, 21);
            this.cbAddr.TabIndex = 0;
            // 
            // gbDel
            // 
            this.gbDel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDel.Controls.Add(this.btnDel);
            this.gbDel.Controls.Add(this.cbAddr);
            this.gbDel.Location = new System.Drawing.Point(12, 158);
            this.gbDel.Name = "gbDel";
            this.gbDel.Size = new System.Drawing.Size(460, 58);
            this.gbDel.TabIndex = 1;
            this.gbDel.TabStop = false;
            this.gbDel.Text = "Delete an Address";
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Location = new System.Drawing.Point(379, 22);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "&Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // gbGen
            // 
            this.gbGen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGen.Controls.Add(this.cbDML);
            this.gbGen.Controls.Add(this.lblPassword);
            this.gbGen.Controls.Add(this.lblLabel);
            this.gbGen.Controls.Add(this.tbLabel);
            this.gbGen.Controls.Add(this.tbPassword);
            this.gbGen.Controls.Add(this.cbDeterministic);
            this.gbGen.Controls.Add(this.cbRandom);
            this.gbGen.Controls.Add(this.btnGenerate);
            this.gbGen.Controls.Add(this.cbShortAddr);
            this.gbGen.Location = new System.Drawing.Point(12, 12);
            this.gbGen.Name = "gbGen";
            this.gbGen.Size = new System.Drawing.Size(460, 130);
            this.gbGen.TabIndex = 2;
            this.gbGen.TabStop = false;
            this.gbGen.Text = "Create an Address";
            // 
            // cbShortAddr
            // 
            this.cbShortAddr.AutoSize = true;
            this.cbShortAddr.Location = new System.Drawing.Point(138, 105);
            this.cbShortAddr.Name = "cbShortAddr";
            this.cbShortAddr.Size = new System.Drawing.Size(240, 17);
            this.cbShortAddr.TabIndex = 0;
            this.cbShortAddr.Text = "&Spend extra time to make this address shorter";
            this.cbShortAddr.UseVisualStyleBackColor = true;
            this.cbShortAddr.CheckedChanged += new System.EventHandler(this.cbShortAddr_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(379, 101);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cbRandom
            // 
            this.cbRandom.AutoSize = true;
            this.cbRandom.Checked = true;
            this.cbRandom.Location = new System.Drawing.Point(6, 20);
            this.cbRandom.Name = "cbRandom";
            this.cbRandom.Size = new System.Drawing.Size(106, 17);
            this.cbRandom.TabIndex = 2;
            this.cbRandom.TabStop = true;
            this.cbRandom.Text = "Random Address";
            this.cbRandom.UseVisualStyleBackColor = true;
            this.cbRandom.CheckedChanged += new System.EventHandler(this.cbRandom_CheckedChanged);
            // 
            // cbDeterministic
            // 
            this.cbDeterministic.AutoSize = true;
            this.cbDeterministic.Location = new System.Drawing.Point(6, 46);
            this.cbDeterministic.Name = "cbDeterministic";
            this.cbDeterministic.Size = new System.Drawing.Size(126, 17);
            this.cbDeterministic.TabIndex = 2;
            this.cbDeterministic.Text = "Deterministic Address";
            this.cbDeterministic.UseVisualStyleBackColor = true;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Enabled = false;
            this.tbPassword.Location = new System.Drawing.Point(211, 45);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(188, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tbLabel
            // 
            this.tbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLabel.Location = new System.Drawing.Point(211, 19);
            this.tbLabel.Name = "tbLabel";
            this.tbLabel.Size = new System.Drawing.Size(243, 20);
            this.tbLabel.TabIndex = 4;
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(172, 22);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(33, 13);
            this.lblLabel.TabIndex = 5;
            this.lblLabel.Text = "Label";
            // 
            // cbDML
            // 
            this.cbDML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDML.AutoSize = true;
            this.cbDML.Enabled = false;
            this.cbDML.Location = new System.Drawing.Point(405, 47);
            this.cbDML.Name = "cbDML";
            this.cbDML.Size = new System.Drawing.Size(49, 17);
            this.cbDML.TabIndex = 6;
            this.cbDML.Text = "DML";
            this.cbDML.UseVisualStyleBackColor = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(143, 48);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Passphrase";
            // 
            // frmAddr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 232);
            this.Controls.Add(this.gbGen);
            this.Controls.Add(this.gbDel);
            this.MinimumSize = new System.Drawing.Size(500, 270);
            this.Name = "frmAddr";
            this.Text = "Manage Addresses";
            this.gbDel.ResumeLayout(false);
            this.gbGen.ResumeLayout(false);
            this.gbGen.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAddr;
        private System.Windows.Forms.GroupBox gbDel;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.GroupBox gbGen;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox tbLabel;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.RadioButton cbDeterministic;
        private System.Windows.Forms.RadioButton cbRandom;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox cbShortAddr;
        private System.Windows.Forms.CheckBox cbDML;
        private System.Windows.Forms.Label lblPassword;
    }
}