using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Settings
{
    static class ApplicationSettingsFile
    {
        public static ApplicationSettings Read()
        {
            if (GetCurrentDirectoryFile().Exists)
            {
                return ReadFromCurrentDirectoryFile();
            }
            else if (GetProfileFile().Exists)
            {
                return ReadFromProfileFile();
            }
            else
            {
                var settings = new ApplicationSettings();
                SaveToProfileFile(settings);
                return settings;
            }
        }

        public static void Save(ApplicationSettings settings)
        {
            if (GetCurrentDirectoryFile().Exists)
            {
                SaveToCurrentDirectoryFile(settings);
            }
            else
            {
                SaveToProfileFile(settings);
            }
        }

        private static FileInfo GetCurrentDirectoryFile()
        {
            var fileName = Path.Combine(AssemblyUtils.AssemblyDirectory, $"{AssemblyUtils.AssemblyTitle}.xml");
            return new FileInfo(fileName);
        }

        private static FileInfo GetProfileFile()
        {
            var directoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyUtils.AssemblyTitle, AssemblyUtils.AssemblyProductVersion);
            var fileName = Path.Combine(directoryName, $"{AssemblyUtils.AssemblyTitle}.xml");
            return new FileInfo(fileName);
        }

        private static ApplicationSettings ReadFromProfileFile()
        {
            var fileInfo = GetProfileFile();
            using var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Read(stream);
        }

        private static ApplicationSettings ReadFromCurrentDirectoryFile()
        {
            var fileInfo = GetCurrentDirectoryFile();
            using var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Read(stream);
        }

        private static void SaveToProfileFile(ApplicationSettings settings)
        {
            var fileInfo = GetProfileFile();
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            Save(fileInfo.FullName, settings);
        }

        private static void SaveToCurrentDirectoryFile(ApplicationSettings settings)
        {
            var fileInfo = GetCurrentDirectoryFile();
            Save(fileInfo.FullName, settings);
        }

        private static ApplicationSettings Read(Stream stream)
        {
            var settings = new ApplicationSettings();
            var document = XDocument.Load(stream);
            var rootElement = document.XPathSelectElement("/systemTrayAnimator");
            settings.IconDirectoryName = rootElement.XPathSelectElement("/iconDirectoryName").Value;
            settings.PauseFileName = rootElement.XPathSelectElement("/pauseFileName").Value;
            settings.ListFileName = rootElement.XPathSelectElement("/listFileName").Value;
            settings.IntervalBetweenFrames = int.Parse(rootElement.XPathSelectElement("/intervalBetweenFrames").Value);
            settings.IntervalForShowOneFrame = int.Parse(rootElement.XPathSelectElement("/intervalForShowOneFrame").Value);
            return settings;
        }

        private static void Save(string fileName, ApplicationSettings settings)
        {
            var document = new XDocument();
            document.Add(new XElement("systemTrayAnimator",
                                     new XAttribute("iconDirectoryName", settings.IconDirectoryName),
                                     new XAttribute("pauseFileName", settings.PauseFileName),
                                     new XAttribute("listFileName", settings.ListFileName),
                                     new XAttribute("intervalBetweenFrames", settings.IntervalBetweenFrames.ToString()),
                                     new XAttribute("intervalForShowOneFrame", ((int)settings.IntervalForShowOneFrame).ToString())));
            FileUtils.Save(fileName, document);
        }
    }
}