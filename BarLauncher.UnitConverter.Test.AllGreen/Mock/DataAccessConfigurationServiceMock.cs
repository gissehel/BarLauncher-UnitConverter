using FluentDataAccess;

namespace BarLauncher.UnitConverter.Test.AllGreen.Mock
{
    public class DataAccessConfigurationServiceMock : IDataAccessConfigurationByPath
    {
        public string ApplicationDataPath { get; set; }

        public string DatabaseName => "BarLauncher-UnitConverter";

        public string DatabasePath => ".";
    }
}