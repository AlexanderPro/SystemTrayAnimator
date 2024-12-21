using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemTrayAnimator.Settings;

namespace SystemTrayAnimator.Forms
{
    public partial class SettingsForm : Form
    {
        public ApplicationSettings Settings { get; private set; }

        public SettingsForm(ApplicationSettings settings)
        {
            Settings = settings;
            InitializeComponent();
        }
    }
}
