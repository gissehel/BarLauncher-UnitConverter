using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.ScriptResult;
using System.IO;
using System.Text;
using BarLauncher.EasyHelper;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.Engine.Service;

namespace BarLauncher.UnitConverter.Test.AllGreen.Helper
{
    public class UnitConverterContext : IContext<UnitConverterContext>
    {
        public ITestScript<UnitConverterContext> TestScript { get; set; }
        public ITestScriptResult<UnitConverterContext> TestScriptResult { get; set; }

        public ApplicationStarter ApplicationStarter { get; set; }


        private IOutputService _jsonOutputService = new JsonOutputService();
        private IOutputService JsonOutputService => _jsonOutputService;

        private IOutputService _textOutputService = new TextOutputService();
        private IOutputService TextOutputService => _textOutputService;

        public void OnTestStart()
        {
            ApplicationStarter = new ApplicationStarter();
            ApplicationStarter.Init(TestScript.Name);
        }

        public void OnTestStop()
        {
            var testScriptResult = TestScriptResult as TestScriptResult<UnitConverterContext>;
            JsonOutputService.Output(testScriptResult, ApplicationStarter.TestPath, testScriptResult.TestScript.Name);
            TextOutputService.Output(testScriptResult, ApplicationStarter.TestPath, testScriptResult.TestScript.Name);
        }
    }
}