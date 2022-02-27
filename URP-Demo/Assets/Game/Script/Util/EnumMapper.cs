using System;
using UnityEngine;

namespace Demo.Util
{
    public static class EnumMapper
    {
        public static T GetEnumType<T>(string name) where T : Enum
        {
            Array enumNames = Enum.GetValues(typeof(T));
            foreach (var enumName in enumNames)
            {
                if (enumName.ToString().ToLower() == name.ToLower())
                {
                    return (T)enumName;
                }
            }
            Debug.LogError("Error : Invalid enum name");
            return default;
        }
    }
}