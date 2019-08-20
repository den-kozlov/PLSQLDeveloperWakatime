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
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.lblProxy = new System.Windows.Forms.Label();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.cbbLogLevel = new System.Windows.Forms.ComboBox();
            this.dlgPythonLocation = new System.Windows.Forms.OpenFileDialog();
            this.txtPythonLocation = new System.Windows.Forms.TextBox();
            this.btnBrowseForPython = new System.Windows.Forms.Button();
            this.lblPythonBinary = new System.Windows.Forms.Label();
            this.cbPythonAutolocate = new System.Windows.Forms.CheckBox();
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
            this.txtAPIKey.Size = new System.Drawing.Size(305, 20);
            this.txtAPIKey.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(257, 264);
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
            // txtProxy
            // 
            this.txtProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxy.Location = new System.Drawing.Point(27, 106);
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.Size = new System.Drawing.Size(305, 20);
            this.txtProxy.TabIndex = 4;
            // 
            // lblProxy
            // 
            this.lblProxy.AutoSize = true;
            this.lblProxy.Location = new System.Drawing.Point(27, 87);
            this.lblProxy.Name = "lblProxy";
            this.lblProxy.Size = new System.Drawing.Size(216, 13);
            this.lblProxy.TabIndex = 5;
            this.lblProxy.Text = "Proxy (http[s]://[user:password@]server:port";
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(27, 144);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(70, 13);
            this.lblLogLevel.TabIndex = 6;
            this.lblLogLevel.Text = "Logging level";
            // 
            // cbbLogLevel
            // 
            this.cbbLogLevel.FormattingEnabled = true;
            this.cbbLogLevel.Location = new System.Drawing.Point(27, 160);
            this.cbbLogLevel.Name = "cbbLogLevel";
            this.cbbLogLevel.Size = new System.Drawing.Size(138, 21);
            this.cbbLogLevel.TabIndex = 7;
            // 
            // dlgPythonLocation
            // 
            this.dlgPythonLocation.Filter = "Python binary|pythonw.exe";
            this.dlgPythonLocation.Title = "Define Python Location";
            // 
            // txtPythonLocation
            // 
            this.txtPythonLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPythonLocation.Location = new System.Drawing.Point(27, 233);
            this.txtPythonLocation.Name = "txtPythonLocation";
            this.txtPythonLocation.Size = new System.Drawing.Size(249, 20);
            this.txtPythonLocation.TabIndex = 8;
            // 
            // btnBrowseForPython
            // 
            this.btnBrowseForPython.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseForPython.Location = new System.Drawing.Point(281, 233);
            this.btnBrowseForPython.Name = "btnBrowseForPython";
            this.btnBrowseForPython.Size = new System.Drawing.Size(51, 21);
            this.btnBrowseForPython.TabIndex = 9;
            this.btnBrowseForPython.Text = "Browse";
            this.btnBrowseForPython.UseVisualStyleBackColor = true;
            this.btnBrowseForPython.Click += new System.EventHandler(this.BtnBrowseForPython_Click);
            // 
            // lblPythonBinary
            // 
            this.lblPythonBinary.AutoSize = true;
            this.lblPythonBinary.Location = new System.Drawing.Point(27, 195);
            this.lblPythonBinary.Name = "lblPythonBinary";
            this.lblPythonBinary.Size = new System.Drawing.Size(179, 13);
            this.lblPythonBinary.TabIndex = 10;
            this.lblPythonBinary.Text = "Python binaries location (version 3.x)";
            // 
            // cbPythonAutolocate
            // 
            this.cbPythonAutolocate.AutoSize = true;
            this.cbPythonAutolocate.Location = new System.Drawing.Point(30, 210);
            this.cbPythonAutolocate.Name = "cbPythonAutolocate";
            this.cbPythonAutolocate.Size = new System.Drawing.Size(123, 17);
            this.cbPythonAutolocate.TabIndex = 11;
            this.cbPythonAutolocate.Text = "Locate automatically";
            this.cbPythonAutolocate.UseVisualStyleBackColor = true;
            this.cbPythonAutolocate.CheckedChanged += new System.EventHandler(this.CbPythonAutolocate_CheckedChanged);
            // 
            // WakaApiKeyForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 299);
            this.Controls.Add(this.cbPythonAutolocate);
            this.Controls.Add(this.lblPythonBinary);
            this.Controls.Add(this.btnBrowseForPython);
            this.Controls.Add(this.txtPythonLocation);
            this.Controls.Add(this.cbbLogLevel);
            this.Controls.Add(this.lblLogLevel);
            this.Controls.Add(this.lblProxy);
            this.Controls.Add(this.txtProxy);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAPIKey);
            this.Controls.Add(this.lblAPIKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(369, 338);
            this.Name = "WakaApiKeyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WakaApiKeyForm";
            this.Load += new System.EventHandler(this.WakaApiKeyForm_Load);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.WakaApiKeyForm_Validating);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAPIKey;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel lblInstructions;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.Label lblProxy;
        private System.Windows.Forms.Label lblLogLevel;
        private System.Windows.Forms.ComboBox cbbLogLevel;
        private System.Windows.Forms.OpenFileDialog dlgPythonLocation;
        private System.Windows.Forms.TextBox txtPythonLocation;
        private System.Windows.Forms.Button btnBrowseForPython;
        private System.Windows.Forms.Label lblPythonBinary;
        private System.Windows.Forms.CheckBox cbPythonAutolocate;
    }
}