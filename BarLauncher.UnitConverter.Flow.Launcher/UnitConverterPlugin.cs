using FluentDataAccess;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.UnitConverter.Lib.Core.Service;
using BarLauncher.UnitConverter.Lib.Service;
using BarLauncher.EasyHelper.Flow.Launcher;

namespace BarLauncher.UnitConverter
{
    public class UnitConverterPlugin : FlowLauncherPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            ISystemService systemService = new SystemService("BarLauncher-UnitConverter");
            ISystemService systemServiceWithMigration = new SystemServiceWithMigration(systemService, "Wox.UnitConverter");
            IDataAccessConfigurationByPath dataAccessConfigurationService = new DataAccessConfigurationService(systemServiceWithMigration);
            IDataAccessService dataAccessService = DataAccessSQLite.GetService(dataAccessConfigurationService);
            IPrefixDefinitionRepository prefixDefinitionRepository = new PrefixDefinitionRepository(dataAccessService);
            IUnitDefinitionRepository unitDefinitionRepository = new UnitDefinitionRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorService();
            IFileReaderService fileReaderService = new FileReaderService();
            IApplicationInformationService applicationInformationService = new ApplicationInformationService(systemServiceWithMigration);
            IUnitConversionService unitConversionService = new UnitConversionService(unitService, dataAccessService, prefixDefinitionRepository, unitDefinitionRepository, fileGeneratorService, fileReaderService);

            return new BarLauncherUnitResultFinder(BarLauncherContextService, unitConversionService, systemServiceWithMigration, applicationInformationService);
        }
    }
}