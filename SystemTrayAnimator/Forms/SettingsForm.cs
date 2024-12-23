using System;
using System.Windows.Forms;
using System.IO;
using SystemTrayAnimator.Settings;

namespace SystemTrayAnimator.Forms
{
    partial class SettingsForm : Form
    {
        public ApplicationSettings Settings { get; private set; }

        public SettingsForm(ApplicationSettings settings)
        {
            Settings = settings;
            InitializeComponent();
            InitializeControls(settings);
        }

        private void InitializeControls(ApplicationSettings settings)
        {
            txtDirectoryName.Text = settings.DirectoryName;
            txtFileExtensions.Text = settings.SupportedFileExtensions;
            chckIncludeSubdirectories.Checked = settings.IncludeSubdirectories;
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDirectoryName.Text))
            {
                txtDirectoryName.SelectAll();
                txtDirectoryName.Focus();
                return;
            }

            if (!int.TryParse(txtFileExtensions.Text, out var left))
            {
                txtFileExtensions.SelectAll();
                txtFileExtensions.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
    }
}
