using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine;
using System.ComponentModel;


public class CountryData
{
    public string ISO3166Code;
    public string EnglishNameShort;
    public string EnglishNameFull;
    public string KoreanNameShort;
    public string KoreanNameFull;
    public HashSet<string> KoreanNameAdditionals;
    public bool HasUNMembership;
    public bool HasOfficialISO3166Code;
    public HashSet<string> Capital;
    public HashSet<string> CapitalAdditionals;
    public string CapitalComment;

    public void PostInitialize()
    {
        KoreanNames = new HashSet<string> { KoreanNameShort, KoreanNameFull };
        KoreanNames.UnionWith(KoreanNameAdditionals);

        Capitals = new();
        Capitals.UnionWith(Capital);
        Capitals.UnionWith(CapitalAdditionals);
    }

    public HashSet<string> KoreanNames;
    public HashSet<string> Capitals;
}


public class CountryDataOption
{
    public HashSet<string> CountryCodesToExclude = new() { "af_0", "ak", "ck", "nu", "nc", "ll", "os", "ts" };
}


public class CountryDataStorage
{
    public CountryDataOption Option { get; } = new();
    public Dictionary<string, CountryData> CountryData { get; } = new();

    public CountryDataStorage(string rawData)
    {
        Regex splitPattern = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        string[] lines = rawData.Split('\n');
        string[] header = splitPattern.Split(lines[0]);
        FieldInfo[] fields = (from fieldName in header select typeof(CountryData).GetField(fieldName)).ToArray();
        int fieldCount = fields.Length;

        foreach (string line in lines[Range.StartAt(1)])
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var data = new CountryData();
            string[] values = splitPattern.Split(line);

            for (int i = 0; i < fieldCount; i++)
            {
                if (fields[i] == null)
                {
                    continue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(fields[i].FieldType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    fields[i].SetValue(data, converter.ConvertFromInvariantString(values[i]));
                }
                else
                {
                    if (fields[i].FieldType == typeof(HashSet<string>))
                    {
                        fields[i].SetValue(data, new HashSet<string>(values[i].Split(';')));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            data.PostInitialize();
            CountryData.Add(data.ISO3166Code, data);
        }
    }
}
