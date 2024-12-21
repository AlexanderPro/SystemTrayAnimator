using System;
using System.Windows.Forms;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Forms
{
    partial class AboutForm : Form
    {
        private const string URL = "https://github.com/AlexanderPro/SystemTrayAnimator";

        public AboutForm()
        {
            InitializeComponent();
            Text = $"About {AssemblyUtils.AssemblyProductName}";
            lblProductName.Text = $"{AssemblyUtils.AssemblyProductName} v{AssemblyUtils.AssemblyProductVersion}";
            lblCopyright.Text = $"{AssemblyUtils.AssemblyCopyright} {AssemblyUtils.AssemblyCompany}";
            linkUrl.Text = URL;
        }

        private void CloseClick(object sender, EventArgs e) => Close();

        private void LinkClick(object sender, EventArgs e) => SystemUtils.RunAs(SystemUtils.GetDefaultBrowserModuleName(), URL, true);

        private void KeyDownClick(object sender, KeyEventArgs e) => CloseClick(sender, e);
    }
}
