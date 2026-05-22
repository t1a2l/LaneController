using System;
using System.Reflection;

namespace LaneController.KianCommons.Utils
{
    internal static class ReflectionExtension
    {
        private const BindingFlags ALL = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        internal static MethodInfo GetMethod(this Type type, string name, bool throwOnError = true)
        {
            return ReflectionHelpers.GetMethod(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty, throwOnError);
        }

        internal static MethodInfo GetMethod(this Type type, string name, BindingFlags binding = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty, bool throwOnError = true)
        {
            return ReflectionHelpers.GetMethod(type, name, binding, throwOnError);
        }

        internal static MethodInfo GetMethod(this Type type, string name, Type[] types, BindingFlags binding = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty, bool throwOnError = true)
        {
            return ReflectionHelpers.GetMethod(type, name, binding, types, throwOnError);
        }

        internal static void SetValue<TStruct>(this FieldInfo fieldInfo, ref TStruct obj, object val) where TStruct : struct
        {
            object obj2 = obj;
            fieldInfo.SetValue(obj2, val);
            obj = (TStruct)obj2;
        }

        internal static void SetValue<TStruct>(this PropertyInfo propertyInfo, ref TStruct obj, object value, object[] index) where TStruct : struct
        {
            object obj2 = obj;
            propertyInfo.SetValue(obj2, value, index);
            obj = (TStruct)obj2;
        }
    }
}
