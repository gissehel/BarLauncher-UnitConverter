using AllGreen.Lib;
using BarLauncher.UnitConverter.Test.AllGreen.Fixture;
using BarLauncher.UnitConverter.Test.AllGreen.Helper;

namespace BarLauncher.UnitConverter.Test.AllGreen.Test
{
    public class Get_help_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_test>()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit el"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("help", "Get help on this extension (web)")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .Check("https://github.com/gissehel/BarLauncher-UnitConverter")
            .EndUsing()

            .EndTest();
    }
}