﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using Wox.Plugin;

namespace Wox.UnitConverter
{
    public class UnitConverterPlugin : IPlugin
    {
        private IUnitService<ScalarFloat, float> UnitService { get; set; }

        public void Init(PluginInitContext context)
        {
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            UnitService = new UnitService<ScalarFloat, float>(constantProvider);
        }

        private Tuple<string, string> Separate(string value, string pattern)
        {
            var patternIndex = value.IndexOf(pattern);
            if (patternIndex < 0)
            {
                return null;
            }
            var left = value.Substring(0, patternIndex);
            var right = value.Substring(patternIndex + pattern.Length, value.Length - (patternIndex + pattern.Length));
            return new Tuple<string, string>(left, right);
        }

        public List<Result> Query(Query query)
        {
            string title = query.Search;
            string output = null;

            try
            {
                var search = query.Search;
                var unitString = search;
                string targetUnitString = null;
                string targetExtraUnitString = null;
                var arrowFields = Separate(search, "->");
                if (arrowFields != null)
                {
                    unitString = arrowFields.Item1;
                    targetUnitString = arrowFields.Item2;
                }

                if (targetUnitString != null)
                {
                    var doubleComaFields = Separate(targetUnitString, ":");
                    if (doubleComaFields != null)
                    {
                        targetUnitString = doubleComaFields.Item1;
                        targetExtraUnitString = doubleComaFields.Item2;
                    }
                }
                var unit = UnitService.Parse(unitString);
                string inputUnit = unit.UnitElement.Name;
                string outputUnit = null;

                if (targetUnitString != null)
                {
                    var targetUnit = UnitService.Parse(targetUnitString);
                    UnitValue<ScalarFloat, float> targetExtraUnit = null;
                    if (targetExtraUnitString != null)
                    {
                        targetExtraUnit = UnitService.Parse(targetExtraUnitString);
                    }
                    var result = UnitService.Convert(UnitService.Divide(unit, targetUnit));
                    string resultAsString = null;
                    string targetUnitAsString = null;

                    if (result.UnitElement.GetDimension().QuantityCount == 0)
                    {
                        resultAsString = result.Value.Value.ToString();
                    }
                    else
                    {
                        if (targetExtraUnit != null)
                        {
                            result = UnitService.Convert(result, targetExtraUnit);
                        }
                        outputUnit = result.UnitElement.Name;
                        resultAsString = string.Format("({0})", result.AsString);
                    }

                    if (targetUnit.Value.Value == targetUnit.Value.GetNeutral().Value)
                    {
                        targetUnitAsString = targetUnit.UnitElement.AsString;
                    }
                    else
                    {
                        targetUnitAsString = string.Format("({0})", targetUnit.AsString);
                    }
                    if (targetUnit.UnitElement.GetDimension().QuantityCount != 0)
                    {
                        if (outputUnit == null)
                        {
                            outputUnit = targetUnit.UnitElement.Name;
                        }
                        else
                        {
                            outputUnit = outputUnit + " x " + targetUnit.UnitElement.Name;
                        }
                    }
                    output = string.Format("{0} {1}", resultAsString, targetUnitAsString);
                }
                else
                {
                    var result = UnitService.Convert(unit);
                    if (result.UnitElement.GetDimension().QuantityCount == 0 && result.Value.Value == 1)
                    {
                        output = null;
                    }
                    else
                    {
                        output = result.AsString;
                        if (result.UnitElement.GetDimension().QuantityCount != 0)
                        {
                            outputUnit = result.UnitElement.Name;
                        }
                    }
                }
                if (outputUnit != null)
                {
                    title = string.Format("{0} ( {1} -> {2} )", title, inputUnit, outputUnit);
                }
                else
                {
                    title = string.Format("{0} ( {1} -> )", title, inputUnit);
                }
            }
            catch (Exception ex)
            {
                title = "Exception !";
                output = ex.Message;
            }

            if (output == null)
            {
                return null;
            }

            return new List<Result>
            {
                new Result
                {
                    Title = title,
                    SubTitle = output,
                    IcoPath = "Unitlib-64.png",
                }
            };
        }
    }
}