using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly Dictionary<Type, List<PropertyInfo>> Map = new Dictionary<Type, List<PropertyInfo>>();

        public static List<PropertyInfo> GetProperties(this Object obj)
        {
            List<PropertyInfo> props;
            Type objType = obj.GetType();
            if (!Map.TryGetValue(objType, out props))
            {
                props = obj.GetType().GetProperties().ToList();
                Map.Add(objType, props);
            }

            return props;
        }

        //todo: make recursive
        public static bool AllPropertiesNull(this Object obj)
        {
            return obj.GetProperties().TrueForAll(x => IsNull(x.PropertyType, x.GetValue(obj, null)));
        }

        public static bool AllStringPropertiesNullOrEmpty(this Object obj)
        {
            return
                obj.GetProperties()
                    .Where(x => x.PropertyType == typeof (string))
                    .ToList()
                    .TrueForAll(x => string.IsNullOrWhiteSpace((string) x.GetValue(obj, null)));
        }

        public static void TrimAllNotNullStringProperties(this Object obj)
        {
            List<PropertyInfo> stringProps = obj.GetProperties().Where(x => x.PropertyType == typeof (string)).ToList();

            foreach (PropertyInfo prop in stringProps)
            {
                var value = (string) prop.GetValue(obj, null);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.Trim();
                    prop.SetValue(obj, value, null);
                }
            }
        }

        public static void TrimAllNotNullStringPropertiesAndSetNullToEmpty(this Object obj)
        {
            List<PropertyInfo> stringProps = obj.GetProperties().Where(x => x.PropertyType == typeof (string)).ToList();

            foreach (PropertyInfo prop in stringProps)
            {
                var value = (string) prop.GetValue(obj, null);
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = string.Empty;
                    prop.SetValue(obj, value, null);
                }
                else
                {
                    value = value.Trim();
                    prop.SetValue(obj, value, null);
                }
            }
        }

        private static bool IsNull(Type propType, object value)
        {
            if (propType == typeof (string)) return string.IsNullOrWhiteSpace(value as string);
            return value == null;
        }
    }
}