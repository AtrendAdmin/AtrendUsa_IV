using System;
using System.Collections.Generic;
using System.Text;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Extensions
{
    public static class StringExtensions
    {
        public static int? IntParseOrDefault(this string source, int defaultValue)
        {
            if (source == null) return defaultValue;
            int.TryParse(source, out defaultValue);
            return defaultValue;
        }

        public static bool InvariantEquals(this String source, string toCompare)
        {
            return source.Equals(toCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static TimeSpan? ToTimeSpan(this String source)
        {
            if (string.IsNullOrWhiteSpace(source)) return null;
            TimeSpan timeSpanToReturn;
            bool parseSuccess = TimeSpan.TryParse(source, out timeSpanToReturn);
            return parseSuccess ? (TimeSpan?) timeSpanToReturn : null;
        }

        public static TimeSpan? ToTimeSpanFromAmPm(this String source)
        {
            if (string.IsNullOrWhiteSpace(source)) return null;
            DateTime dateTimeToReturn;
            bool parseSuccess = DateTime.TryParse(source, out dateTimeToReturn);
            return parseSuccess ? (TimeSpan?) dateTimeToReturn.TimeOfDay : null;
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, string delim)
        {
            if (collection == null)
            {
                return "";
            }

            var result = new StringBuilder();
            foreach (T value in collection)
            {
                result.Append(value);
                result.Append(delim);
            }
            if (result.Length > 0)
            {
                result.Length -= delim.Length;
            }
            return result.ToString();
        }
    }
}