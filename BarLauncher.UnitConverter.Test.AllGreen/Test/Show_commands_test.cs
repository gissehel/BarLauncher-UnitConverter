using AllGreen.Lib;
using BarLauncher.UnitConverter.Test.AllGreen.Fixture;
using BarLauncher.UnitConverter.Test.AllGreen.Helper;

namespace BarLauncher.UnitConverter.Test.AllGreen.Test
{
    public class Show_commands_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_test>()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert <UNIT> [ -> <UNIT> [ : <UNIT> ]]", "Convert a value to another unit (express in a third unit)")
            .Check("search <unit|prefix> [PATTERN]", "search info a unit or a prefix")
            .Check("help", "Get help on this extension (web)")
            .Check("export <FILENAME>", "Export unit and prefix definitions to the file")
            .Check("import <FILENAME>", "Import unit and prefix definitions from the file")
            .Check("version", "BarLauncher-UnitConverter version 1.0.8")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit p"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("help", "Get help on this extension (web)")
            .Check("export <FILENAME>", "Export unit and prefix definitions to the file")
            .Check("import <FILENAME>", "Import unit and prefix definitions from the file")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit e"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert <UNIT> [ -> <UNIT> [ : <UNIT> ]]", "Convert a value to another unit (express in a third unit)")
            .Check("search <unit|prefix> [PATTERN]", "search info a unit or a prefix")
            .Check("help", "Get help on this extension (web)")
            .Check("export <FILENAME>", "Export unit and prefix definitions to the file")
            .Check("version", "BarLauncher-UnitConverter version 1.0.8")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit n"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert <UNIT> [ -> <UNIT> [ : <UNIT> ]]", "Convert a value to another unit (express in a third unit)")
            .Check("version", "BarLauncher-UnitConverter version 1.0.8")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit nm"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("nm ( nanometre -> metre )", "1E-09 m")
            .EndUsing()

            .EndTest();
    }
}