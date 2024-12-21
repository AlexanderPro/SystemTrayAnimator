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
            settings.DirectoryName = rootElement.XPathSelectElement("//directoryName").Value;
            settings.IncludeSubdirectories = rootElement.XPathSelectElement("//includeSubdirectories").Value.ToLower() == "true";
            settings.PauseFileName = rootElement.XPathSelectElement("//pauseFileName").Value;
            settings.SupportedFileExtensions = rootElement.XPathSelectElement("//supportedFileExtensions").Value;
            settings.Interval = int.Parse(rootElement.XPathSelectElement("//interval").Value);
            return settings;
        }

        private static void Save(string fileName, ApplicationSettings settings)
        {
            var document = new XDocument();
            document.Add(new XElement("systemTrayAnimator",
                                     new XElement("directoryName", settings.DirectoryName),
                                     new XElement("includeSubdirectories", settings.IncludeSubdirectories),
                                     new XElement("pauseFileName", settings.PauseFileName),
                                     new XElement("supportedFileExtensions", settings.SupportedFileExtensions),
                                     new XElement("interval", settings.Interval)));
            FileUtils.Save(fileName, document);
        }
    }
}