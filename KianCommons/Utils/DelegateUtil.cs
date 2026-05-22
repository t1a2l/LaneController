using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LaneController.KianCommons.Utils
{
    internal static class DelegateUtil
    {
        public static string FullName(MethodBase m)
        {
            return m.DeclaringType.FullName + "::" + m.Name;
        }

        internal static Type[] GetParameterTypes<TDelegate>(bool instance = false) where TDelegate : Delegate
        {
            IEnumerable<Type> source = from p in typeof(TDelegate).GetMethod("Invoke").GetParameters()
                                       select p.ParameterType;
            if (instance)
            {
                source = source.Skip(1);
            }
            return [.. source];
        }

        internal static MethodInfo GetMethod<TDelegate>(this Type type, string name = null, bool throwOnError = false, bool instance = false) where TDelegate : Delegate
        {
            name ??= typeof(TDelegate).Name;
            return type.GetMethod(name, GetParameterTypes<TDelegate>(instance), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty, throwOnError);
        }

        internal static TDelegate CreateClosedDelegate<TDelegate>(object instance, string name = null) where TDelegate : Delegate
        {
            try
            {
                MethodInfo methodInfo = (instance?.GetType())?.GetMethod<TDelegate>(name);
                if (methodInfo == null)
                {
                    return null;
                }
                return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo);
            }
            catch (Exception innerException)
            {
                throw new Exception("CreateClosedDelegate<" + typeof(TDelegate).Name + ">(" + instance.GetType().Name + "," + name + ") failed!", innerException);
            }
        }

        internal static TDelegate CreateDelegate<TDelegate>(Type type, string name = null, bool instance = false, bool throwOnError = false) where TDelegate : Delegate
        {
            try
            {
                object obj;
                if (type == null)
                {
                    obj = null;
                }
                else
                {
                    bool instance2 = instance;
                    obj = type.GetMethod<TDelegate>(name, throwOnError, instance2);
                }
                MethodInfo methodInfo = (MethodInfo)obj;
                if (methodInfo == null)
                {
                    return null;
                }
                return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), methodInfo);
            }
            catch (Exception innerException)
            {
                if (!throwOnError)
                {
                    return null;
                }
                throw new Exception("CreateDelegate<" + typeof(TDelegate).Name + ">(" + instance.GetType().Name + "," + name + ") failed!", innerException);
            }
        }
    }
}
