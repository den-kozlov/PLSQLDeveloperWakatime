using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace WakaTime
{
    public partial class WakaApiKeyForm : Form
    {
        internal event EventHandler ConfigSaved;

        private readonly ConfigFile _wakaTimeConfigFile;

        public WakaApiKeyForm()
        {
            InitializeComponent();

            _wakaTimeConfigFile = new ConfigFile();
        }

        private void WakaApiKeyForm_Load(object sender, EventArgs e)
        {
            cbbLogLevel.Items.Clear();
            foreach (var ll in Enum.GetNames(typeof(LogLevel)))
            {
                cbbLogLevel.Items.Add(ll);
            }
            cbbLogLevel.SelectedItem = _wakaTimeConfigFile.logLevel.ToString();
            try
            {
                txtAPIKey.Text = _wakaTimeConfigFile.ApiKey;
                txtProxy.Text = _wakaTimeConfigFile.Proxy;
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
                    _wakaTimeConfigFile.Proxy = txtProxy.Text;
                    if (cbbLogLevel.SelectedIndex > -1)
                    {
                        _wakaTimeConfigFile.logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), cbbLogLevel.SelectedItem.ToString());
                    }
                    else
                        _wakaTimeConfigFile.logLevel = LogLevel.None;
                    _wakaTimeConfigFile.Save();
                    ConfigSaved?.Invoke(this, EventArgs.Empty);
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

        private void WakaApiKeyForm_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
