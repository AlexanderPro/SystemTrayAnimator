using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using SystemTrayAnimator.Settings;

namespace SystemTrayAnimator
{
    class MainApplicationContext : ApplicationContext
    {
        private readonly object _lockObject;
        private ApplicationSettings _settings;
        private AccurateTimer _timer;
        private SystemTrayMenu _systemTrayMenu;
        private FileSystemWatcher _watcher;
        private FrameList _frames;

        public MainApplicationContext()
        {
            try
            {
                _settings = ApplicationSettingsFile.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read the settings.{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            _lockObject = new object();
            ReadDirectory();
            _watcher = new FileSystemWatcher();
            _watcher.Path = _settings.DirectoryName;
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Filter = _settings.SupportedFileExtensions;
            _watcher.Changed += WatcherOnChanged;
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
            var isPaused = false;
            lock (_lockObject)
            {
                directoryName = _settings.DirectoryName;
                pauseFileName = _settings.PauseFileName;
                supportedFileExtensions = _settings.SupportedFileExtensions;
                isPaused = _settings.IsPaused;
            }

            if (isPaused)
            {
                return;
            }

            if (File.Exists(pauseFileName))
            {
                lock (_lockObject)
                {
                    _settings.IsPaused = true;
                    _systemTrayMenu.CheckMenuItemPause(true);
                }
                return;
            }

            if (_frames != null)
            {
                foreach (var frame in _frames)
                {
                    frame.Dispose();
                }
                _frames.Clear();
            }

            var fileExtensions = supportedFileExtensions.ToLower().Split(';', ',');
            var fileNames = Directory.GetFiles(directoryName).Where(x => supportedFileExtensions.Contains(Path.GetExtension(x.ToLower()))).OrderBy(x => x).ToArray();
            lock (_lockObject)
            {
                _frames = new FrameList(fileNames);
            }
        }
    }
}
