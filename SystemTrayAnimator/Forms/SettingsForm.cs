using System;
using System.Windows.Forms;
using System.IO;
using SystemTrayAnimator.Settings;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Forms
{
    partial class SettingsForm : Form
    {
        private ApplicationSettings _settings;

        public event EventHandler<EventArgs<ApplicationSettings>> OkClick;

        public SettingsForm(ApplicationSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            InitializeControls(settings);
            UpdateControls();
        }

        private void InitializeControls(ApplicationSettings settings)
        {
            txtDirectoryName.Text = settings.DirectoryName;
            txtFileExtensions.Text = settings.FileExtensions;
            chckIncludeSubdirectories.Checked = settings.IncludeSubdirectories;
            txtInterval.Text = settings.Interval.ToString();
            chkUseDelayFromFirstFrame.Checked = settings.UseDelayFromFirstFrame;
            chkHighDpiSupport.Checked = settings.HighDpiSupport;
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDirectoryName.Text))
            {
                txtDirectoryName.SelectAll();
                txtDirectoryName.Focus();
                MessageBox.Show($"The directory {txtDirectoryName.Text} does not exist.", "Error", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFileExtensions.Text))
            {
                txtFileExtensions.SelectAll();
                txtFileExtensions.Focus();
                MessageBox.Show($"Icon file extensions should be set.", "Error", MessageBoxButtons.OK);
                return;
            }

            if (!int.TryParse(txtInterval.Text, out var interval) || interval <= 0)
            {
                txtInterval.SelectAll();
                txtInterval.Focus();
                MessageBox.Show($"Interval between icons should be an integer and greater than zero.", "Error", MessageBoxButtons.OK);
                return;
            }

            var settings = new ApplicationSettings
            {
                DirectoryName = txtDirectoryName.Text,
                FileExtensions = txtFileExtensions.Text,
                IncludeSubdirectories = chckIncludeSubdirectories.Checked,
                Interval = interval,
                UseDelayFromFirstFrame = chkUseDelayFromFirstFrame.Checked,
                HighDpiSupport = chkHighDpiSupport.Checked,
                IsPaused = _settings.IsPaused
            };
            DialogResult = DialogResult.OK;

            if (!settings.Equals(_settings))
            {
                OkClick?.Invoke(this, new EventArgs<ApplicationSettings>(settings));
            }

            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonBrowseClick(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                SelectedPath = Directory.Exists(txtDirectoryName.Text) ? txtDirectoryName.Text : AssemblyUtils.AssemblyDirectory
            };

            var result = dialog.ShowDialog(this);
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                txtDirectoryName.Text = dialog.SelectedPath;
            }
        }

        private void UseDelayFromFirstFrameCheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void FileExtensionsTextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                ButtonOkClick(sender, e);
            }

            if (e.KeyValue == 27)
            {
                ButtonCancelClick(sender, e);
            }
        }

        private void UpdateControls()
        {
            var isGifExtension = txtFileExtensions.Text.ToLower().Contains("gif");
            chkUseDelayFromFirstFrame.Enabled = isGifExtension;
            txtInterval.Enabled = !chkUseDelayFromFirstFrame.Checked || !isGifExtension;
        }
    }
}
