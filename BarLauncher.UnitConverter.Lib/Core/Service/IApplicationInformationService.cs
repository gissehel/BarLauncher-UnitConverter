using System;
using System.Collections.Generic;
using System.Text;

namespace BarLauncher.UnitConverter.Lib.Core.Service
{
    public interface IApplicationInformationService
    {
        string ApplicationName { get; }

        string Version { get; }

        string HomepageUrl { get; }
    }
}
