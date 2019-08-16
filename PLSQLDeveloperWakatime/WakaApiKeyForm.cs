using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace WakaTime
{
    public partial class WakaApiKeyForm : Form
    {
        private readonly WakaTimeConfigFile _wakaTimeConfigFile;

        public WakaApiKeyForm()
        {
            InitializeComponent();

            _wakaTimeConfigFile = new WakaTimeConfigFile();
        }

        private void WakaApiKeyForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtAPIKey.Text = _wakaTimeConfigFile.ApiKey;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LblInstructions2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://wakatime.com/settings/account");
            Process.Start(sInfo);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Guid apiKey;
                var parse = Guid.TryParse(txtAPIKey.Text.Trim(), out apiKey);
                if (parse)
                {
                    _wakaTimeConfigFile.ApiKey = apiKey.ToString();
                    _wakaTimeConfigFile.Save();
                    WakaTime.ApiKey = apiKey.ToString();
                }
                else
                {
                    MessageBox.Show("Please enter valid Api Key.");
                    DialogResult = DialogResult.None; // do not close dialog box
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
