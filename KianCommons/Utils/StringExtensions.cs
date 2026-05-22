using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework.Math;
using ColossalFramework.Packaging;

namespace LaneController.KianCommons.Utils
{
    internal static class StringExtensions
    {
        public static string RemoveExtension(this string path)
        {
            int length = path.LastIndexOf(".");
            return path.Substring(0, length);
        }

        public static string Remove(this string s, string r)
        {
            return s.Replace(r, "");
        }

        public static string RemoveChar(this string s, char c)
        {
            return s.Remove(c.ToString());
        }

        public static string Remove(this string s, params string[] removes)
        {
            foreach (string r in removes)
            {
                s = s.Remove(r);
            }
            return s;
        }

        public static string RemoveChars(this string s, params char[] chars)
        {
            foreach (char c in chars)
            {
                s = s.RemoveChar(c);
            }
            return s;
        }

        internal static string BIG(string m)
        {
            m = "  " + m + "  ";
            int num = 120;
            string text = mul("*", num);
            string text2 = mul("*", (num - m.Length) / 2);
            return text + "\n" + text2 + m + text2 + "\n" + text;
            static string mul(string s, int i)
            {
                string text3 = "";
                while (i-- > 0)
                {
                    text3 += s;
                }
                return text3;
            }
        }

        internal static string CenterString(this string stringToCenter, int totalLength)
        {
            int totalWidth = (totalLength - stringToCenter.Length) / 2 + stringToCenter.Length;
            return stringToCenter.PadLeft(totalWidth).PadRight(totalLength);
        }

        internal static string ToSTR(this object obj)
        {
            Assertion.AssertStack();
            if (obj == null)
            {
                return "<null>";
            }
            if (obj is string str)
            {
                return str.ToSTR();
            }
            if (obj is InstanceID instanceID)
            {
                return instanceID.ToSTR();
            }
            if (obj is Bezier3 bezier)
            {
                return bezier.ToSTR();
            }
            if (obj is KeyValuePair<InstanceID, InstanceID> map)
            {
                return map.ToSTR();
            }
            if (obj is IDictionary dict)
            {
                return dict.ToSTR();
            }
            if (obj is IEnumerable list)
            {
                return list.ToSTR();
            }
            return obj.ToString();
        }

        internal static string ToSTR(this string str)
        {
            if (str == "")
            {
                return "<empty>";
            }
            if (str == null)
            {
                return "<null>";
            }
            return str;
        }

        internal static string ToSTR(this InstanceID instanceID)
        {
            return $"{instanceID.Type}:{instanceID.Index}";
        }

        internal static string ToSTR(this Bezier3 bezier)
        {
            return $"Bezier[{bezier.a}, {bezier.b}, {bezier.c}, {bezier.d}]";
        }

        internal static string ToSTR(this KeyValuePair<InstanceID, InstanceID> map)
        {
            return "[" + map.Key.ToSTR() + ":" + map.Value.ToSTR() + "]";
        }

        internal static string ToSTR(this IDictionary dict)
        {
            if (dict == null)
            {
                return "<null>";
            }
            List<string> list = [];
            foreach (object key in dict.Keys)
            {
                object obj = dict[key];
                list.Add("(" + key.ToSTR() + " : " + obj.ToSTR() + ")");
            }
            return "{" + list.Join(", ") + " }";
        }

        internal static string ToSTR(this IEnumerable list)
        {
            if (list is Package package)
            {
                return package.ToString();
            }
            if (list == null)
            {
                return "<null>";
            }
            string text = "{ ";
            foreach (object item in list)
            {
                string text2 = item is not KeyValuePair<InstanceID, InstanceID> map ? item?.ToString() ?? "<null>" : map.ToSTR();
                text = text + text2 + ", ";
            }
            text.Remove(text.Length - 2, 2);
            return text + " }";
        }

        internal static string ToSTR<T>(this IEnumerable list, string format)
        {
            if (list == null)
            {
                return "<null>";
            }
            MethodInfo methodInfo = typeof(T).GetMethod("ToString", [typeof(string)]) ?? throw new Exception(typeof(T).Name + ".ToString(string) was not found");
            object[] parameters = [format];
            string text = "{ ";
            foreach (T item in list)
            {
                object arg = methodInfo.Invoke(item, parameters);
                text += $"{arg}, ";
            }
            text.Remove(text.Length - 2, 2);
            return text + " }";
        }

        internal static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split([separator], options);
        }

        internal static string[] SplitLines(this string str, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split("\n", options);
        }

        internal static string Join(this IEnumerable<string> str, string separator)
        {
            return string.Join(separator, [.. str]);
        }

        internal static string JoinLines(this IEnumerable<string> str)
        {
            return str.Join("\n");
        }

        internal static string RemoveEmptyLines(this string str)
        {
            return str.SplitLines(StringSplitOptions.RemoveEmptyEntries).JoinLines();
        }
    }
}
