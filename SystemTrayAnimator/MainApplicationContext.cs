using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using SystemTrayAnimator.Settings;
using SystemTrayAnimator.Utils;
using SystemTrayAnimator.Forms;

namespace SystemTrayAnimator
{
    class MainApplicationContext : ApplicationContext
    {
        private readonly object _lockObject;
        private int _frameIndex;
        private ApplicationSettings _settings;
        private AccurateTimer _timer;
        private SystemTrayMenu _systemTrayMenu;
        private FileSystemWatcher _watcher;
        private FrameList _frames;
        private AboutForm _aboutForm;
        private SettingsForm _settingsForm;

        public MainApplicationContext()
        {
            try
            {
                _settings = ApplicationSettingsFile.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read the settings.{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _lockObject = new object();
            _frameIndex = 0;
            _frames = new FrameList();
            _timer = new AccurateTimer(ShowFrame);

            if (!Directory.Exists(_settings.DirectoryName))
            {
                Directory.CreateDirectory(_settings.DirectoryName);
            }

            ReadDirectory();

            _systemTrayMenu = new SystemTrayMenu();
            _systemTrayMenu.MenuItemAutoStartClick += MenuItemAutoStartClick;
            _systemTrayMenu.MenuItemPauseClick += MenuItemPauseClick;
            _systemTrayMenu.MenuItemSettingsClick += MenuItemSettingsClick;
            _systemTrayMenu.MenuItemAboutClick += MenuItemAboutClick;
            _systemTrayMenu.MenuItemExitClick += MenuItemExitClick;
            _systemTrayMenu.Build();
            _systemTrayMenu.CheckMenuItemAutoStart(AutoStarter.IsEnabled(AssemblyUtils.AssemblyProductName, AssemblyUtils.AssemblyLocation));

            ShowFrame();

            _watcher = new FileSystemWatcher();
            _watcher.Path = _settings.DirectoryName;
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = _settings.IncludeSubdirectories;
            _watcher.Filter = _settings.SupportedFileExtensions;
            _watcher.Changed += WatcherOnChanged;

            var pauseFileName = Path.Combine(_settings.DirectoryName, _settings.PauseFileName);
            if (File.Exists(pauseFileName))
            {
                _settings.IsPaused = true;
                _systemTrayMenu.CheckMenuItemPause(true);
            }
            else
            {
                _timer.Start(_settings.Interval);
            }
            _watcher.EnableRaisingEvents = true;
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            ReadDirectory();
        }

        private void ReadDirectory()
        {
            var directoryName = string.Empty;
            var pauseFileName = string.Empty;
            var supportedFileExtensions = string.Empty;
            var includeSubdirectories = false;
            var isPaused = false;
            lock (_lockObject)
            {
                directoryName = _settings.DirectoryName;
                pauseFileName = Path.Combine(_settings.DirectoryName, _settings.PauseFileName);
                supportedFileExtensions = _settings.SupportedFileExtensions;
                includeSubdirectories = _settings.IncludeSubdirectories;
                isPaused = _settings.IsPaused;
            }

            if (isPaused || !Directory.Exists(directoryName))
            {
                return;
            }

            if (File.Exists(pauseFileName))
            {
                lock (_lockObject)
                {
                    _timer.Stop();
                    _settings.IsPaused = true;
                    _systemTrayMenu.CheckMenuItemPause(true);
                }
                return;
            }

            //var fileExtensions = supportedFileExtensions.ToLower().Split(';', ',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
            var fileNames = Directory.EnumerateFiles(directoryName, supportedFileExtensions, includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).OrderBy(x => x).ToArray();
            var frames = new FrameList(fileNames);
            lock (_lockObject)
            {
                if (!_frames.Equals(frames))
                {
                    ClearFrames(_frames);
                    _frames = frames;
                    _frameIndex = 0;
                }
                else
                {
                    ClearFrames(frames);
                }
            }
        }

        private void ShowFrame()
        {
            lock(_lockObject)
            {
                if (_frames.Count == 0)
                {
                    _systemTrayMenu.Icon.Icon = Properties.Resources.SystemTrayAnimator;
                }
                else if (_frames.Count == 1)
                {
                    _systemTrayMenu.Icon.Icon = _frames[0].Icon;
                }
                else
                {
                    _systemTrayMenu.Icon.Icon = _frames[_frameIndex].Icon;
                    _frameIndex++;
                    if (_frameIndex >= _frames.Count)
                    {
                        _frameIndex = 0;
                    }
                }
            }
        }

        private void ClearFrames(FrameList frames)
        {
            foreach (var frame in frames)
            {
                frame.Dispose();
            }
            frames.Clear();
        }

        private void MenuItemAutoStartClick(object sender, EventArgs e)
        {
            var keyName = AssemblyUtils.AssemblyProductName;
            var assemblyLocation = AssemblyUtils.AssemblyLocation;
            var autoStartEnabled = AutoStarter.IsEnabled(keyName, assemblyLocation);
            if (autoStartEnabled)
            {
                AutoStarter.Disable(keyName);
            }
            else
            {
                AutoStarter.Enable(keyName, assemblyLocation);
            }
            ((ToolStripMenuItem)sender).Checked = !autoStartEnabled;
        }

        private void MenuItemPauseClick(object sender, EventArgs e)
        {
            lock (_lockObject)
            {
                if (_settings.IsPaused)
                {
                    var pauseFileName = Path.Combine(_settings.DirectoryName, _settings.PauseFileName);
                    if (File.Exists(pauseFileName))
                    {
                        File.Delete(pauseFileName);
                    }
                    _timer.Stop();
                    _settings.IsPaused = !_settings.IsPaused;
                    ((ToolStripMenuItem)sender).Checked = _settings.IsPaused;
                }
                else
                {
                    _timer.Start(_settings.Interval);
                    _settings.IsPaused = !_settings.IsPaused;
                    ((ToolStripMenuItem)sender).Checked = _settings.IsPaused;
                }
            }
        }

        private void MenuItemAboutClick(object sender, EventArgs e)
        {
            if (_aboutForm == null || _aboutForm.IsDisposed || !_aboutForm.IsHandleCreated)
            {
                _aboutForm = new AboutForm();
            }
            _aboutForm.Show();
            _aboutForm.Activate();
        }

        private void MenuItemSettingsClick(object sender, EventArgs e)
        {
            if (_settingsForm == null || _settingsForm.IsDisposed || !_settingsForm.IsHandleCreated)
            {
                _settingsForm = new SettingsForm(_settings);
                if (_settingsForm.DialogResult == DialogResult.OK)
                {
                    _settings = _settingsForm.Settings;
                    ApplicationSettingsFile.Save(_settingsForm.Settings);
                }                
            }

            _settingsForm.Show();
            _settingsForm.Activate();
        }

        private void MenuItemExitClick(object sender, EventArgs e)
        {
            _timer?.Stop();
            _systemTrayMenu?.Dispose();
            Application.Exit();
        }
    }
}