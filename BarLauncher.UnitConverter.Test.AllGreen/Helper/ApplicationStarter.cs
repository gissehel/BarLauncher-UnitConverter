using FluentDataAccess;
using System;
using System.IO;
using System.Reflection;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using BarLauncher.UnitConverter.Lib.Core.Service;
using BarLauncher.UnitConverter.Lib.Service;
using BarLauncher.UnitConverter.Test.AllGreen.Mock;

namespace BarLauncher.UnitConverter.Test.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public BarLauncherContextServiceMock BarLauncherContextService { get; set; }

        public QueryServiceMock QueryService { get; set; }

        public IBarLauncherResultFinder BarLauncherUnitResultFinder { get; set; }

        public SystemServiceMock SystemService { get; set; }

        private string TestName { get; set; }

        private string testPath = null;
        public string TestPath => testPath ?? (testPath = GetApplicationDataPath());

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            BarLauncherContextServiceMock barLauncherContextService = new BarLauncherContextServiceMock(queryService);
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            SystemServiceMock systemService = new SystemServiceMock();
            IDataAccessConfigurationByPath dataAccessConfigurationService = new DataAccessConfigurationService(systemService);
            IDataAccessService dataAccessService = DataAccessSQLite.GetService(dataAccessConfigurationService);
            IPrefixDefinitionRepository prefixDefinitionRepository = new PrefixDefinitionRepository(dataAccessService);
            IUnitDefinitionRepository unitDefinitionRepository = new UnitDefinitionRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorServiceMock();
            IFileReaderService fileReaderService = new FileReaderServiceMock();
            IApplicationInformationService applicationInformationService = new ApplicationInformationServiceMock();
            IUnitConversionService unitConversionService = new UnitConversionService(unitService, dataAccessService, prefixDefinitionRepository, unitDefinitionRepository, fileGeneratorService, fileReaderService);

            BarLauncherUnitResultFinder barLauncherUnitResultFinder = new BarLauncherUnitResultFinder(barLauncherContextService, unitConversionService, systemService, applicationInformationService);

            systemService.ApplicationDataPath = TestPath;

            dataAccessService.Init();
            barLauncherUnitResultFinder.Init();

            BarLauncherContextService = barLauncherContextService;
            QueryService = queryService;
            BarLauncherUnitResultFinder = barLauncherUnitResultFinder;
            SystemService = systemService;

            BarLauncherContextService.AddQueryFetcher("unit", BarLauncherUnitResultFinder);
        }

        public void Start()
        {
            // Make any initialisation that should occur AFTER instanciation.
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.CodeBase;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), "AG_{0:yyyyMMdd-HHmmss-fff}_{1}".FormatWith(DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}