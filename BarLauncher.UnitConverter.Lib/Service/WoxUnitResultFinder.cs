﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.UnitConverter.Lib.Core.Service;

namespace BarLauncher.UnitConverter.Lib.Service
{
    public class BarLauncherUnitResultFinder : BarLauncherResultFinder
    {
        public IUnitConversionService UnitConversionService { get; }
        public ISystemService SystemService { get; }

        public IApplicationInformationService ApplicationInformationService { get; }

        public BarLauncherUnitResultFinder(IBarLauncherContextService barLauncherContextService, IUnitConversionService unitConversionService, ISystemService systemService, IApplicationInformationService applicationInformationService)
            : base(barLauncherContextService)
        {
            UnitConversionService = unitConversionService;
            SystemService = systemService;
            ApplicationInformationService = applicationInformationService;
        }

        public override void Init()
        {
            UnitConversionService.Init();
            InitCommands();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public IEnumerable<BarLauncherResult> CreateNewUnit(string search)
        {
            var result = UnitConversionService.PrepareNewUnitCreation(search);

            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, result.Item3);
            }
        }

        public IEnumerable<BarLauncherResult> CreateNewPrefix(string search)
        {
            var result = UnitConversionService.PrepareNewPrefixCreation(search);

            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, result.Item3);
            }
        }

        private void InitCommands()
        {
            AddCommand("convert", "convert <UNIT> [ -> <UNIT> [ : <UNIT> ]]", "Convert a value to another unit (express in a third unit)", ConvertCommand);
            AddCommand("search", "search <unit|prefix> [PATTERN]", "search info a unit or a prefix", null);
            AddCommand("help", "help", "Get help on this extension (web)", HelpCommand);
            AddCommand("export", "export <FILENAME>", "Export unit and prefix definitions to the file", ExportCommand);
            AddCommand("import", "import <FILENAME>", "Import unit and prefix definitions from the file", ImportCommand);
            AddCommand("version", "version", "{0} version {1}".FormatWith(ApplicationInformationService.ApplicationName, ApplicationInformationService.Version), () => { });

            AddDefaultCommand(DefaultCommand);

            AddCommand("unit", "search unit [PATTERN]", "search a unit", SearchUnitCommand, "search");
            AddCommand("prefix", "search prefix [PATTERN]", "search a prefix", SearchPrefixCommand, "search");
        }

        public IEnumerable<BarLauncherResult> DefaultCommand(BarLauncherQuery query, int position)
        {
            var search = query.GetAllSearchTermsStarting(position);

            if (search.Contains("="))
            {
                return CreateNewUnit(search);
            }
            else if (search.Contains("~"))
            {
                return CreateNewPrefix(search);
            }
            else
            {
                return ConvertUnit(search);
            }
        }

        public IEnumerable<BarLauncherResult> ExportCommand(BarLauncherQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (string.IsNullOrEmpty(filename))
            {
                yield return GetEmptyCommandResult("export", CommandInfos);
            }
            else
            {
                yield return GetActionResult("export {0}".FormatWith(filename), "Export unit and prefix definitions to filename [{0}]".FormatWith(filename), () => Export(filename));
            }
        }

        private void Export(string filename)
        {
            var fullFilename = GetImportExportFullFilename(filename);
            UnitConversionService.ExportTo(fullFilename);
            SystemService.OpenUrl(SystemService.ApplicationDataPath);
        }

        private IEnumerable<BarLauncherResult> ImportCommand(BarLauncherQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (string.IsNullOrEmpty(filename))
            {
                yield return GetEmptyCommandResult("import", CommandInfos);
            }
            else
            {
                var fullFilename = GetImportExportFullFilename(filename);
                if (UnitConversionService.CanImportFrom(fullFilename))
                {
                    yield return GetActionResult("import {0}".FormatWith(filename), "Import unit and prefix definitions from filename [{0}]".FormatWith(filename), () => Import(fullFilename));
                }
                else
                {
                    yield return new BarLauncherResult { Title = "import {0}".FormatWith(filename), SubTitle = "Import unit and prefix definitions from filename [{0}] (No file found!)".FormatWith(filename) };
                }
            }
        }

        private void Import(string fullFilename)
        {
            if (UnitConversionService.CanImportFrom(fullFilename))
            {
                UnitConversionService.ImportFrom(fullFilename);
            }
        }

        private string GetImportExportFullFilename(string filename) => Path.IsPathRooted(filename) ? filename : Path.Combine(SystemService.ApplicationDataPath, filename);

        public IEnumerable<BarLauncherResult> ConvertUnit(string search)
        {
            var result = UnitConversionService.Convert(search);
            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, () => { });
            }
        }

        private IEnumerable<BarLauncherResult> ConvertCommand(BarLauncherQuery query, int position) => ConvertUnit(query.GetAllSearchTermsStarting(position));

        private void HelpCommand() => SystemService.OpenUrl("https://github.com/gissehel/BarLauncher-UnitConverter");

        private IEnumerable<BarLauncherResult> SearchPrefixCommand(BarLauncherQuery query, int position)
        {
            var unitPrefixes = UnitConversionService.GetUnitPrefixes();
            var patterns = query.SearchTerms.Skip(position);
            var patternFull = query.GetAllSearchTermsStarting(position);
            var hasResult = false;
            foreach (var unitPrefix in unitPrefixes)
            {
                var searchField = "{0} {1} {2}".FormatWith(unitPrefix.Name, unitPrefix.Symbol, unitPrefix.Namespace);
                bool match = true;
                foreach (var pattern in patterns)
                {
                    match = match && searchField.MatchPattern(pattern);
                }
                if (match && unitPrefix.Symbol.Length > 0)
                {
                    var convertSI = UnitConversionService.Convert(unitPrefix.Symbol + "m");
                    var title = "{0} ({1})".FormatWith(unitPrefix.Name, unitPrefix.Symbol);
                    var subtitle = "{2}m -> {0} (namespace: {1})".FormatWith(convertSI.Item2, unitPrefix.Namespace, unitPrefix.Symbol);
                    if (patternFull == unitPrefix.Name)
                    {
                        subtitle = subtitle + " - convert";
                        hasResult = true;
                        yield return GetCompletionResultFinal(title, subtitle, () => "convert {0}m".FormatWith(unitPrefix.Symbol));
                    }
                    else
                    {
                        hasResult = true;
                        yield return GetCompletionResultFinal(title, subtitle, () => "search prefix {0}".FormatWith(unitPrefix.Name));
                    }
                }
            }
            if (!hasResult)
            {
                yield return GetEmptyCommandResult("prefix", GetCommandInfos("search"));
            }
        }

        private IEnumerable<BarLauncherResult> SearchUnitCommand(BarLauncherQuery query, int position)
        {
            var unitBaseNames = UnitConversionService.GetUnitBaseNames();
            var patterns = query.SearchTerms.Skip(position);
            var patternFull = query.GetAllSearchTermsStarting(position);
            var hasResult = false;
            foreach (var unitBaseName in unitBaseNames)
            {
                var searchField = "{0} {1} {2}".FormatWith(unitBaseName.Name, unitBaseName.Symbol, unitBaseName.Namespace);
                bool match = true;
                foreach (var pattern in patterns)
                {
                    match = match && searchField.MatchPattern(pattern);
                }
                if (match && unitBaseName.Symbol.Length > 0)
                {
                    var convertSI = UnitConversionService.Convert(unitBaseName.Symbol);
                    var title = "{0} ({1})".FormatWith(unitBaseName.Name, unitBaseName.Symbol);
                    var subtitle = "{0} (namespace: {1})".FormatWith(convertSI.Item2, unitBaseName.Namespace);
                    if (patternFull == unitBaseName.Name)
                    {
                        subtitle = subtitle + " - convert";
                        hasResult = true;
                        yield return GetCompletionResultFinal(title, subtitle, () => "convert {0}".FormatWith(unitBaseName.Symbol));
                    }
                    else
                    {
                        hasResult = true;
                        yield return GetCompletionResultFinal(title, subtitle, () => "search unit {0}".FormatWith(unitBaseName.Name));
                    }
                }
            }
            if (!hasResult)
            {
                yield return GetEmptyCommandResult("unit", GetCommandInfos("search"));
            }
        }
    }
}