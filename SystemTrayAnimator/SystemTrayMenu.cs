using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator
{
    class SystemTrayMenu : IDisposable
    {
        private readonly ContextMenuStrip _systemTrayMenu;
        private readonly ToolStripMenuItem _menuItemAutoStart;
        private readonly ToolStripMenuItem _menuItemPause;
        private readonly ToolStripMenuItem _menuItemSettings;
        private readonly ToolStripMenuItem _menuItemAbout;
        private readonly ToolStripMenuItem _menuItemExit;
        private readonly ToolStripSeparator _menuItemSeparator1;
        private readonly ToolStripSeparator _menuItemSeparator2;
        private readonly NotifyIcon _icon;
        private bool _isBuilt;

        public event EventHandler MenuItemAutoStartClick;
        public event EventHandler MenuItemPauseClick;
        public event EventHandler MenuItemSettingsClick;
        public event EventHandler MenuItemAboutClick;
        public event EventHandler MenuItemExitClick;

        public NotifyIcon Icon => _icon;

        public SystemTrayMenu()
        {
            _menuItemAutoStart = new ToolStripMenuItem();
            _menuItemPause = new ToolStripMenuItem();
            _menuItemSettings = new ToolStripMenuItem();
            _menuItemAbout = new ToolStripMenuItem();
            _menuItemExit = new ToolStripMenuItem();
            _menuItemSeparator1 = new ToolStripSeparator();
            _menuItemSeparator2 = new ToolStripSeparator();
            var components = new Container();
            _systemTrayMenu = new ContextMenuStrip(components);
            _icon = new NotifyIcon(components);
            _isBuilt = false;
        }

        public void Build()
        {
            if (_isBuilt)
            {
                return;
            }

            _menuItemAutoStart.Name = "miAutoStart";
            _menuItemAutoStart.Size = new Size(175, 22);
            _menuItemAutoStart.Text = "Auto start program";
            _menuItemAutoStart.Click += (sender, e) => MenuItemAutoStartClick?.Invoke(sender, e);

            _menuItemPause.Name = "miPause";
            _menuItemPause.Size = new Size(175, 22);
            _menuItemPause.Text = "Pause";
            _menuItemPause.Click += (sender, e) => MenuItemPauseClick?.Invoke(sender, e);

            _menuItemSettings.Name = "miSettings";
            _menuItemSettings.Size = new Size(175, 22);
            _menuItemSettings.Text = "Settings...";
            _menuItemSettings.Click += (sender, e) => MenuItemSettingsClick?.Invoke(sender, e);

            _menuItemAbout.Name = "miAbout";
            _menuItemAbout.Size = new Size(175, 22);
            _menuItemAbout.Text = "About";
            _menuItemAbout.Click += (sender, e) => MenuItemAboutClick?.Invoke(sender, e);

            _menuItemExit.Name = "miExit";
            _menuItemExit.Size = new Size(175, 22);
            _menuItemExit.Text = "Exit";
            _menuItemExit.Click += (sender, e) => MenuItemExitClick?.Invoke(sender, e);

            _menuItemSeparator1.Name = "miSeparator1";
            _menuItemSeparator1.Size = new Size(172, 6);

            _menuItemSeparator2.Name = "miSeparator2";
            _menuItemSeparator2.Size = new Size(172, 6);

            _systemTrayMenu.Items.AddRange(new ToolStripItem[] { _menuItemAutoStart, _menuItemSeparator1, _menuItemPause, _menuItemSettings, _menuItemAbout, _menuItemSeparator2, _menuItemExit });

            _systemTrayMenu.Name = AssemblyUtils.AssemblyTitle;
            _systemTrayMenu.Size = new Size(176, 80);

            _icon.ContextMenuStrip = _systemTrayMenu;
            _icon.Text = AssemblyUtils.AssemblyTitle;
            _icon.Visible = true;
            _icon.DoubleClick += (sender, e) => MenuItemSettingsClick?.Invoke(sender, e);

            _isBuilt = true;
        }

        public void CheckMenuItemAutoStart(bool check)
        {
            _menuItemAutoStart.Checked = check;
        }

        public void CheckMenuItemPause(bool check)
        {
            _menuItemPause.Checked = check;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _menuItemAutoStart?.Dispose();
                _menuItemPause?.Dispose();
                _menuItemSettings?.Dispose();
                _menuItemAbout?.Dispose();
                _menuItemExit?.Dispose();
                _menuItemSeparator1?.Dispose();
                _menuItemSeparator2?.Dispose();
                _systemTrayMenu?.Dispose();
                _icon.Visible = false;
                _icon.Dispose();
            }
        }

        ~SystemTrayMenu()
        {
            Dispose(false);
        }
    }
}