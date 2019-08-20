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
            cbbLogLevel.SelectedIndex = cbbLogLevel.Items.IndexOf(_wakaTimeConfigFile.logLevel.ToString());
            try
            {
                txtAPIKey.Text = _wakaTimeConfigFile.ApiKey;
                txtProxy.Text = _wakaTimeConfigFile.Proxy;
                cbPythonAutolocate.Checked = !String.IsNullOrEmpty(_wakaTimeConfigFile.PythonBinaryLocation);
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
                        Logger.Debug("LogLevel set to {0}", cbbLogLevel.SelectedItem.ToString());
                        _wakaTimeConfigFile.logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), cbbLogLevel.SelectedItem.ToString());
                    }
                    else
                        _wakaTimeConfigFile.logLevel = LogLevel.None;
                    _wakaTimeConfigFile.PythonBinaryLocation = cbPythonAutolocate.Checked ? "" : txtPythonLocation.Text;
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

        private void UpdatePythonBinariesControlls()
        {
            if (cbPythonAutolocate.Checked)
            {
                txtPythonLocation.Enabled = false;
                txtPythonLocation.Text = Dependencies.AutoDetectPythonLocation();
            }
            else
            {
                txtPythonLocation.Enabled = true;
                txtPythonLocation.Text = Dependencies.PythonUserDefinedLocation;
            }
            btnBrowseForPython.Enabled = txtPythonLocation.Enabled;
        }
        private void WakaApiKeyForm_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void CbPythonAutolocate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnBrowseForPython_Click(object sender, EventArgs e)
        {
            dlgPythonLocation.FileName = Dependencies.PythonUserDefinedLocation;
            if (dlgPythonLocation.ShowDialog() == DialogResult.OK)
            {
                txtPythonLocation.Text = dlgPythonLocation.FileName;
                Dependencies.PythonUserDefinedLocation = dlgPythonLocation.FileName;
            }
        }
    }
}
