using BarLauncher.UnitConverter.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarLauncher.UnitConverter.Test.AllGreen.Mock
{
    public class ApplicationInformationServiceMock : IApplicationInformationService
    {
        public string ApplicationName => "BarLauncher-UnitConverter";

        public string Version => "1.0.8";

        public string HomepageUrl => "https://github.com/gissehel/BarLauncher-UnitConverter";
    }
}
