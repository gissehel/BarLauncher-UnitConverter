using FluentDataAccess;
using BarLauncher.EasyHelper.Core.Service;

namespace BarLauncher.UnitConverter.Lib.Service
{
    public class DataAccessConfigurationService : IDataAccessConfigurationByPath
    {
        public DataAccessConfigurationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        private ISystemService SystemService { get; }

        public string ApplicationDataPath => SystemService.ApplicationDataPath;

        public string DatabaseName => SystemService.ApplicationName;

        public string DatabasePath => SystemService.ApplicationDataPath;
    }
}