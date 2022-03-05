using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.UnitConverter.Lib.Core.Service;
using System.Diagnostics;
using System.Reflection;

namespace BarLauncher.UnitConverter.Lib.Service
{
    public class ApplicationInformationService : IApplicationInformationService
    {
        private ISystemService SystemService { get; set; }
        public ApplicationInformationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public string ApplicationName => SystemService.ApplicationName;

        public string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string HomepageUrl => "https://github.com/gissehel/BarLauncher-UnitConverter";

    }
}
