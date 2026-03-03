using System;
using System.Linq;

namespace Jan.Core
{
    public static class GlobalsUtils
    {
        public static string[] GetNames(Type type)
        {
            return type
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null))
                .ToArray();
        }
    }
}