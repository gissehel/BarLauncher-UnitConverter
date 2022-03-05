using BarLauncher.EasyHelper.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BarLauncher.UnitConverter.Lib.Service
{
    public class SystemServiceWithMigration : SystemService
    {
        private string GetDatabaseName(string applicationDataPath, string applicationName) => Path.Combine(applicationDataPath, applicationName + ".sqlite");
        public SystemServiceWithMigration(string applicationName) : base(applicationName)
        {
        }
        public SystemServiceWithMigration(string applicationName, params string[] oldApplicationNames) : base(applicationName)
        {
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
