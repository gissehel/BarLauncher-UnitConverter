using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BarLauncher.UnitConverter.Lib.Service
{
    public class SystemServiceWithMigration : ISystemService
    {
        private ISystemService SystemService { get; set; }

        public string ApplicationName => SystemService.ApplicationName;

        public string ApplicationDataPath => SystemService.ApplicationDataPath;

        public void OpenUrl(string url) => SystemService.OpenUrl(url);

        public void StartCommandLine(string command, string arguments) => SystemService.StartCommandLine(command, arguments);

        public void CopyTextToClipboard(string text) => SystemService.CopyTextToClipboard(text);

        public void OpenDirectory(string directory) => SystemService.OpenDirectory(directory);

        private string GetDatabaseName(string applicationDataPath, string applicationName) => Path.Combine(applicationDataPath, applicationName + ".sqlite");

        public SystemServiceWithMigration(ISystemService systemService, params string[] oldApplicationNames)
        {
            SystemService = systemService;

            var currentDatabaseName = GetDatabaseName(ApplicationDataPath, ApplicationName);

            if (!File.Exists(currentDatabaseName))
            {
                foreach (var oldApplicationName in oldApplicationNames.AsEnumerable().Reverse())
                {
                    string oldDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), oldApplicationName);

                    var oldDatabaseName = GetDatabaseName(oldDataPath, oldApplicationName);
                    if (File.Exists(oldDatabaseName))
                    {
                        File.Move(oldDatabaseName, currentDatabaseName);
                        return;
                    }
                }
            }
        }
    }
}
