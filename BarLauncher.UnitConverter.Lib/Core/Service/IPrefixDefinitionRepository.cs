using System.Collections.Generic;
using BarLauncher.UnitConverter.Lib.DomainModel;

namespace BarLauncher.UnitConverter.Lib.Core.Service
{
    public interface IPrefixDefinitionRepository
    {
        void Init();

        void AddPrefixDefinition(PrefixDefinition prefixDefinition);

        IEnumerable<PrefixDefinition> GetPrefixDefinitions();
    }
}