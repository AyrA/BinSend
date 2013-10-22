namespace BinSend
{
    partial class frmPreview
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
            this.tbPreview = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbPreview
            // 
            this.tbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPreview.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPreview.Location = new System.Drawing.Point(0, 0);
            this.tbPreview.Multiline = true;
            this.tbPreview.Name = "tbPreview";
            this.tbPreview.ReadOnly = true;
            this.tbPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbPreview.Size = new System.Drawing.Size(615, 391);
            this.tbPreview.TabIndex = 0;
            // 
            // frmPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 391);
            this.Controls.Add(this.tbPreview);
            this.Name = "frmPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Part Preview (first 10 KB)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPreview;
    }
}