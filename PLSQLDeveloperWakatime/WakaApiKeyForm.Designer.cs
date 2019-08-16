namespace WakaTime
{
    partial class WakaApiKeyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WakaApiKeyForm));
            this.lblAPIKey = new System.Windows.Forms.Label();
            this.txtAPIKey = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblAPIKey
            // 
            this.lblAPIKey.AutoSize = true;
            this.lblAPIKey.Location = new System.Drawing.Point(24, 13);
            this.lblAPIKey.Name = "lblAPIKey";
            this.lblAPIKey.Size = new System.Drawing.Size(45, 13);
            this.lblAPIKey.TabIndex = 0;
            this.lblAPIKey.Text = "API Key";
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAPIKey.Location = new System.Drawing.Point(27, 29);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(314, 20);
            this.txtAPIKey.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(266, 161);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.LinkArea = new System.Windows.Forms.LinkArea(6, 9);
            this.lblInstructions.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lblInstructions.Location = new System.Drawing.Point(27, 62);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(229, 17);
            this.lblInstructions.TabIndex = 3;
            this.lblInstructions.TabStop = true;
            this.lblInstructions.Text = "Visit this link to get API Key and paste it here";
            this.lblInstructions.UseCompatibleTextRendering = true;
            this.lblInstructions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblInstructions2_LinkClicked);
            // 
            // WakaApiKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 196);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAPIKey);
            this.Controls.Add(this.lblAPIKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WakaApiKeyForm";
            this.Text = "WakaApiKeyForm";
            this.Load += new System.EventHandler(this.WakaApiKeyForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAPIKey;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel lblInstructions;
    }
}