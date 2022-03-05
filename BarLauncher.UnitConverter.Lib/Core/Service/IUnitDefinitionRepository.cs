using System.Collections.Generic;
using BarLauncher.UnitConverter.Lib.DomainModel;

namespace BarLauncher.UnitConverter.Lib.Core.Service
{
    public interface IUnitDefinitionRepository
    {
        void Init();

        void AddUnitDefinition(UnitDefinition unitDefinition);

        IEnumerable<UnitDefinition> GetUnitDefinitions();
    }
}