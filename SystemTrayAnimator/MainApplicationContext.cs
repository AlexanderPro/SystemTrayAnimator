using System;
using System.Linq;
using System.IO;
using System.Drawing;
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
        private Icon _applicationIcon;
        private ApplicationSettings _settings;
        private AccurateTimer _timer;
        private SystemTrayMenu _systemTrayMenu;
        private FileSystemWatcher _watcher;
        private FrameList _frames;
        private AboutForm _aboutForm;
        private SettingsForm _settingsForm;

        public bool InitializationError { get; }

        public MainApplicationContext()
        {
            try
            {
                try
                {
                    _settings = ApplicationSettingsFile.Read();
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to read the settings.", e);
                }

                if (!Directory.Exists(_settings.DirectoryName))
                {
                    throw new Exception($"The directory {_settings.DirectoryName ?? string.Empty} does not exist.");
                }

                _lockObject = new object();
                _frameIndex = 0;
                _frames = new FrameList();
                _timer = new AccurateTimer(ShowFrame);
                _applicationIcon = Properties.Resources.SystemTrayAnimator;

                // Enable support of high DPI
                if (_settings.HighDpiSupport)
                {
                    SystemUtils.EnableHighDpiSupport();
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
                _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes;
                _watcher.IncludeSubdirectories = _settings.IncludeSubdirectories;
                _watcher.Filter = _settings.FileExtensions;
                _watcher.Created += (sender, e) => { ReadDirectory(); };
                _watcher.Changed += (sender, e) => { ReadDirectory(); };
                _watcher.Renamed += (sender, e) => { ReadDirectory(); };
                _watcher.Deleted += (sender, e) => { ReadDirectory(); };
                _watcher.EnableRaisingEvents = true;
                _timer.Start(_settings.Interval);
            }
            catch (Exception ex)
            {
                InitializationError = true;
                _systemTrayMenu?.Dispose();
                MessageBox.Show($"{ex.Message} {ex.InnerException?.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadDirectory()
        {
            var directoryName = string.Empty;
            var fileExtensions = string.Empty;
            var includeSubdirectories = false;
            lock (_lockObject)
            {
                directoryName = _settings.DirectoryName;
                fileExtensions = _settings.FileExtensions;
                includeSubdirectories = _settings.IncludeSubdirectories;
            }

            if (!Directory.Exists(directoryName))
            {
                return;
            }

            var fileNames = Directory
                .EnumerateFiles(directoryName, fileExtensions, includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .OrderBy(x => x)
                .ToArray();
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
            lock (_lockObject)
            {
                if (_frames.Count == 0)
                {
                    _systemTrayMenu.Icon.Icon = _applicationIcon;
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
                    _settings.IsPaused = !_settings.IsPaused;
                    _timer.Start(_settings.Interval);
                    ((ToolStripMenuItem)sender).Checked = _settings.IsPaused;
                }
                else
                {
                    _settings.IsPaused = !_settings.IsPaused;
                    _timer.Stop();
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
                _settingsForm.OkClick += (sender, e) =>
                {
                    lock (_lockObject)
                    {
                        _settings = e.Entity;
                        ApplicationSettingsFile.Save(_settings);
                        _watcher.Path = _settings.DirectoryName;
                        _watcher.IncludeSubdirectories = _settings.IncludeSubdirectories;
                        _watcher.Filter = _settings.FileExtensions;
                        if (!_settings.IsPaused)
                        {
                            _timer.Stop();
                        }
                    }
                    ReadDirectory();
                    lock (_lockObject)
                    {
                        if (!_settings.IsPaused)
                        {
                            _timer.Start(_settings.Interval);
                        }
                    }
                };
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