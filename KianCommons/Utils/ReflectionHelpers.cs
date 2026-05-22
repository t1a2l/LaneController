using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneController.KianCommons.Utils
{
    internal static class ReflectionHelpers
    {
        public const BindingFlags COPYABLE = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public const BindingFlags ALL = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        public const BindingFlags ALL_Declared = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        internal static string ThisMethod
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return CurrentMethod(2);
            }
        }

        internal static Version Version(this Assembly asm)
        {
            return asm.GetName().Version;
        }

        internal static Version VersionOf(this Type t)
        {
            return t.Assembly.GetName().Version;
        }

        internal static Version VersionOf(this object obj)
        {
            return obj.GetType().VersionOf();
        }

        internal static string Name(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        public static string FullName(this MethodBase m)
        {
            return m.DeclaringType.FullName + "." + m.Name;
        }

        internal static T ShalowClone<T>(this T source) where T : class, new()
        {
            T val = new();
            CopyProperties<T>(val, source);
            return val;
        }

        internal static void CopyProperties(object target, object origin)
        {
            Type type = target.GetType();
            Type type2 = origin.GetType();
            Assertion.Assert(type == type2 || type.IsSubclassOf(type2));
            FieldInfo[] fields = origin.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                object value = fieldInfo.GetValue(origin);
                fieldInfo.SetValue(target, value);
            }
        }

        internal static void CopyProperties<T>(object target, object origin)
        {
            Assertion.Assert(target is T, "target is T");
            Assertion.Assert(origin is T, "origin is T");
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                object value = fieldInfo.GetValue(origin);
                fieldInfo.SetValue(target, value);
            }
        }

        internal static void CopyPropertiesForced<T>(object target, object origin)
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                string name = fieldInfo.Name;
                FieldInfo field = origin.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
                FieldInfo field2 = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
                if (field != null && field2 != null)
                {
                    try
                    {
                        object obj = field.GetValue(origin);
                        field2.SetValue(target, obj);
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal static void SetAllDeclaredFieldsToNull(object instance)
        {
            Type type = instance.GetType();
            FieldInfo[] allFields = type.GetAllFields(declaredOnly: true);
            FieldInfo[] array = allFields;
            foreach (FieldInfo fieldInfo in array)
            {
                if (fieldInfo.FieldType.IsClass)
                {
                    _ = Log.VERBOSE;
                    fieldInfo.SetValue(instance, null);
                }
            }
        }

        internal static void SetAllDeclaredFieldsToNull(this UIComponent c)
        {
            SetAllDeclaredFieldsToNull((object)c);
        }

        internal static string GetPrettyFunctionName(MethodInfo m)
        {
            string name = m.Name;
            string[] array = name.Split(["g__", "|"], StringSplitOptions.RemoveEmptyEntries);
            if (array.Length == 3)
            {
                return array[1];
            }
            return name;
        }

        internal static T GetAttribute<T>(this ICustomAttributeProvider memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.GetAttributes<T>().FirstOrDefault();
        }

        internal static T[] GetAttributes<T>(this ICustomAttributeProvider memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit) as T[];
        }

        internal static bool HasAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.HasAttribute(typeof(T), inherit);
        }

        internal static bool HasAttribute(this MemberInfo member, Type t, bool inherit = true)
        {
            object[] customAttributes = member.GetCustomAttributes(t, inherit);
            if (customAttributes != null)
            {
                return customAttributes.Length != 0;
            }
            return false;
        }

        internal static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this object obj, bool inherit = true) where T : Attribute
        {
            return from _field in obj.GetType().GetFields()
                   where _field.HasAttribute<T>(inherit)
                   select _field;
        }

        internal static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return from _field in type.GetFields()
                   where _field.HasAttribute<T>(inherit)
                   select _field;
        }

        internal static IEnumerable<MemberInfo> GetMembersWithAttribute<T>(this object obj, bool inherit = true) where T : Attribute
        {
            return obj.GetType().GetMembersWithAttribute<T>(inherit);
        }

        internal static IEnumerable<MemberInfo> GetMembersWithAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return from _member in type.GetMembers()
                   where _member.HasAttribute<T>(inherit)
                   select _member;
        }

        internal static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this object obj, bool inherit = true) where T : Attribute
        {
            return from _property in obj.GetType().GetProperties()
                   where _property.HasAttribute<T>(inherit)
                   select _property;
        }

        internal static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return from _property in type.GetProperties()
                   where _property.HasAttribute<T>(inherit)
                   select _property;
        }

        internal static object GetFieldValue(object target, string fieldName)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (fieldName == null)
            {
                throw new ArgumentNullException("fieldName");
            }
            Type type = target.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty) ?? throw new Exception($"{type}.{fieldName} not found");
            return fieldInfo.GetValue(target);
        }

        internal static object GetFieldValue<T>(string fieldName)
        {
            return GetFieldValue(typeof(T), fieldName);
        }

        internal static object GetFieldValue(Type type, string fieldName)
        {
            if (type == null)
            {
                throw new ArgumentNullException("target");
            }
            if (fieldName == null)
            {
                throw new ArgumentNullException("fieldName");
            }
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty) ?? throw new Exception($"{type}.{fieldName} not found");
            return fieldInfo.GetValue(null);
        }

        internal static void SetFieldValue<T>(string fieldName, object value)
        {
            SetFieldValue(typeof(T), fieldName, value);
        }

        internal static void SetFieldValue(Type targetType, string fieldName, object value)
        {
            FieldInfo field = GetField(targetType, fieldName);
            field.SetValue(null, value);
        }

        internal static void SetFieldValue(object target, string fieldName, object value)
        {
            FieldInfo field = GetField(target.GetType(), fieldName);
            field.SetValue(target, value);
        }

        internal static FieldInfo GetField(object target, string fieldName, bool throwOnError = true)
        {
            return GetField(target.GetType(), fieldName, throwOnError);
        }

        internal static FieldInfo GetField<T>(string fieldName, bool throwOnError = true)
        {
            return GetField(typeof(T), fieldName, throwOnError);
        }

        internal static FieldInfo GetField(Type declaringType, string fieldName, bool throwOnError = true)
        {
            FieldInfo field = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if (field == null && throwOnError)
            {
                throw new Exception($"{declaringType}.{fieldName} not found");
            }
            return field;
        }

        internal static MethodInfo GetMethod(Type type, string method, bool throwOnError = true)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method2 = type.GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if (throwOnError && method2 == null)
            {
                throw new Exception("Method not found: " + type.Name + "." + method);
            }
            return method2;
        }

        internal static MethodInfo GetMethod(Type type, string name, BindingFlags bindingFlags, Type[] types, bool throwOnError = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method = type.GetMethod(name, bindingFlags, null, types, null);
            if (throwOnError && method == null)
            {
                throw new Exception($"failed to retrieve method {type}.{name}({types.ToSTR()} bindingFlags:{bindingFlags.ToSTR()})");
            }
            return method;
        }

        internal static MethodInfo GetMethod(Type type, string name, BindingFlags bindingFlags, bool throwOnError = true)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method = type.GetMethod(name, bindingFlags);
            if (throwOnError && method == null)
            {
                throw new Exception($"failed to retrieve method {type}.{name} bindingFlags:{bindingFlags.ToSTR()}");
            }
            return method;
        }

        internal static string ToSTR(this BindingFlags flags)
        {
            if (flags != (BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                return flags.ToString();
            }
            return "ALL";
        }

        internal static object InvokeMethod<T>(string method)
        {
            return GetMethod(typeof(T), method)?.Invoke(null, null);
        }

        internal static object InvokeMethod<T>(string method, params object[] args)
        {
            return GetMethod(typeof(T), method)?.Invoke(null, args);
        }

        internal static object InvokeMethod(Type type, string method)
        {
            return GetMethod(type, method)?.Invoke(null, null);
        }

        internal static object InvokeMethod(string qualifiedType, string method)
        {
            Type type = Type.GetType(qualifiedType, throwOnError: true);
            return InvokeMethod(type, method);
        }

        internal static object InvokeMethod(object instance, string method)
        {
            Type type = instance.GetType();
            return GetMethod(type, method)?.Invoke(instance, null);
        }

        internal static object InvokeMethod(object instance, string method, params object[] args)
        {
            Type type = instance.GetType();
            return GetMethod(type, method)?.Invoke(instance, args);
        }

        internal static object InvokeSetter(object instance, string propertyName, object value)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName) ?? throw new Exception($"{type}.{propertyName} not found");
            return propertyInfo.GetSetMethod().Invoke(instance, [value]);
        }

        internal static System.Reflection.EventInfo GetEvent(Type type, string eventName, bool throwOnError = true)
        {
            System.Reflection.EventInfo eventInfo = type.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if (eventInfo == null && throwOnError)
            {
                throw new Exception($"could not find {eventName} in {type}");
            }
            return eventInfo;
        }

        internal static T EventToDelegate<T>(object instance, string eventName) where T : Delegate
        {
            return (T)instance.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).GetValue(instance);
        }

        internal static T EventToDelegate<T>(Type type, string eventName) where T : Delegate
        {
            return (T)type.GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).GetValue(null);
        }

        internal static void InvokeEvent(object instance, string eventName, bool verbose = false)
        {
            Delegate[] eventDelegates = GetEventDelegates(instance, eventName);
            if (verbose)
            {
                Log.Info("Executing event `" + instance.GetType().FullName + "." + eventName + "` ...");
            }
            ExecuteDelegates(eventDelegates, verbose);
        }

        internal static void InvokeEvent(Type type, string eventName, bool verbose = false)
        {
            Delegate[] eventDelegates = GetEventDelegates(type, eventName);
            if (verbose)
            {
                Log.Info("Executing event `" + type.FullName + "." + eventName + "` ...");
            }
            ExecuteDelegates(eventDelegates, verbose);
        }

        internal static Delegate[] GetEventDelegates(Type type, string eventName)
        {
            MulticastDelegate multicastDelegate = (MulticastDelegate)type.GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).GetValue(null);
            return multicastDelegate.GetInvocationList();
        }

        internal static Delegate[] GetEventDelegates(object instance, string eventName)
        {
            MulticastDelegate multicastDelegate = (MulticastDelegate)instance.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).GetValue(instance);
            return multicastDelegate.GetInvocationList();
        }

        internal static void ExecuteDelegates(Delegate[] delegates, bool verbose = false)
        {
            if (delegates == null)
            {
                throw new ArgumentNullException("delegates");
            }
            Stopwatch stopwatch = new();
            foreach (Delegate obj in delegates)
            {
                if (obj is not null)
                {
                    if (verbose)
                    {
                        Log.Info($"Executing {obj.Target}:{obj.Method.Name} ...");
                        stopwatch.Reset();
                        stopwatch.Start();
                    }
                    obj.Method.Invoke(obj.Target, null);
                    if (verbose)
                    {
                        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        Log.Info($"Done executing {obj.Target}:{obj.Method.Name}! duration={elapsedMilliseconds:#,0}ms");
                    }
                }
            }
        }

        private static string JoinArgs(object[] args)
        {
            if (args.IsNullorEmpty())
            {
                return "";
            }
            return args.Select((a) => a.ToSTR()).Join(", ");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string CurrentMethod(int i = 1, params object[] args)
        {
            MethodBase method = new StackFrame(i).GetMethod();
            return method.DeclaringType.Name + "." + method.Name + "(" + JoinArgs(args) + ")";
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string CurrentMethodFull(int i = 1, params object[] args)
        {
            MethodBase method = new StackFrame(i).GetMethod();
            if (args.IsNullorEmpty())
            {
                string text = (from p in method.GetParameters()
                               select p.ParameterType.Name + " " + p.Name).Join(" ,");
                return method.FullName() + "(" + text + ")";
            }
            return method.FullName() + "(" + JoinArgs(args) + ")";
        }

        internal static void LogCalled(params object[] args)
        {
            Log.Info(CurrentMethod(2, args) + " called.");
        }

        internal static void LogSucceeded()
        {
            Log.Info(CurrentMethod(2) + " succeeded!");
        }
    }
}
