using AllGreen.Lib;
using System.Collections.Generic;
using BarLauncher.UnitConverter.Test.AllGreen.Helper;

namespace BarLauncher.UnitConverter.Test.AllGreen.Fixture
{
    public class Bar_launcher_results_fixture : FixtureBase<UnitConverterContext>
    {
        public class Result
        {
            public string Title { get; set; }

            public string SubTitle { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            foreach (var result in Context.ApplicationStarter.BarLauncherContextService.Results)
            {
                yield return new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                };
            }
        }
    }
}