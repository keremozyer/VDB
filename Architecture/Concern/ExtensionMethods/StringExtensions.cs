using Newtonsoft.Json;
using System;
using System.Linq;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class StringExtensions
    {
        public static T DeserializeJSON<T>(this string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static bool IsAllDigit(this string version, params string[] excludedStrings)
        {
            if (String.IsNullOrWhiteSpace(version)) return false;

            string excludedCharactersRemoved = version;
            foreach (string excludedString in excludedStrings ?? Array.Empty<string>())
            {
                excludedCharactersRemoved = excludedCharactersRemoved.Replace(excludedString, String.Empty);
            }
            return excludedCharactersRemoved.All(char.IsDigit);
        }
    }
}
