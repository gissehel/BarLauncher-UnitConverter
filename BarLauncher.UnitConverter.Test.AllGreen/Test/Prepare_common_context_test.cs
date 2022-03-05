using AllGreen.Lib;
using BarLauncher.UnitConverter.Test.AllGreen.Fixture;
using BarLauncher.UnitConverter.Test.AllGreen.Helper;

namespace BarLauncher.UnitConverter.Test.AllGreen.Test
{
    public class Prepare_common_context_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Bar_launcher_bar_fixture>()

            .DoAction(f => f.Start_the_bar())
            .DoAction(f => f.Display_bar_launcher())
            .DoCheck(f => f.The_current_query_is(), "")

            .EndUsing()

            .EndTest();
    }
}