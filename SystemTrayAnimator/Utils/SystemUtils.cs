using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace SystemTrayAnimator.Utils
{
    static class SystemUtils
    {
        public static void RunAs(string fileName, string arguments, bool showWindow, string workinDirectory = null)
        {
            var fullFileNames = GetFullPaths(fileName);
            if (fullFileNames.Any())
            {
                var fullFileName = fullFileNames[0];
                var process = new Process();
                process.StartInfo.FileName = fullFileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = !string.IsNullOrEmpty(workinDirectory) ? workinDirectory : Path.GetDirectoryName(fullFileName);
                if (!showWindow)
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                }
                process.Start();
            }
        }

        public static string GetDefaultBrowserModuleName()
        {
            var browserName = "iexplore.exe";
            using var userChoiceKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice");
            if (userChoiceKey == null)
            {
                return browserName;
            }

            var progIdValue = userChoiceKey.GetValue("Progid");
            if (progIdValue == null)
            {
                return browserName;
            }

            var progId = progIdValue.ToString();
            var path = progId + "\\shell\\open\\command";
            using var pathKey = Registry.ClassesRoot.OpenSubKey(path);
            if (pathKey == null)
            {
                return browserName;
            }

            try
            {
                path = pathKey.GetValue(null).ToString().ToLower().Replace("\"", "");
                const string exeSuffix = ".exe";
                if (!path.EndsWith(exeSuffix))
                {
                    path = path.Substring(0, path.LastIndexOf(exeSuffix, StringComparison.Ordinal) + exeSuffix.Length);
                }
                return path;
            }
            catch
            {
                return browserName;
            }
        }

        private static List<string> GetFullPaths(string fileName)
        {
            if (File.Exists(fileName))
            {
                return new List<string> { Path.GetFullPath(fileName) };
            }

            var fullPaths = new List<string>();
            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    fullPaths.Add(fullPath);
                }
            }
            return fullPaths;
        }
    }
}