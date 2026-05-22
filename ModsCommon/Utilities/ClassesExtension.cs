using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LaneController.ModsCommon.Utilities
{
    public static class ClassesExtension
    {
        private delegate TSource Plus<TSource>(TSource x, TSource y);

        private delegate TSource Div<TSource>(TSource x, float count);

        private static readonly Plus<Vector3> VectorPlus = (Plus<Vector3>)Delegate.CreateDelegate(typeof(Plus<Vector3>), typeof(Vector3).GetMethod("op_Addition", BindingFlags.Static | BindingFlags.Public));

        private static readonly Div<Vector3> VectorDiv = (Div<Vector3>)Delegate.CreateDelegate(typeof(Div<Vector3>), typeof(Vector3).GetMethod("op_Division", BindingFlags.Static | BindingFlags.Public));

        public static string Unique(this Guid guid)
        {
            return guid.ToString().Substring(0, 8);
        }

        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> values)
        {
            foreach (T value in values)
            {
                hashSet.Add(value);
            }
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> values)
        {
            HashSet<T> hashSet = [.. values];
            return hashSet;
        }

        public static float AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float defaultValue)
        {
            return source.Select(selector).AverageOrDefault(defaultValue);
        }

        public static Vector3 AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector3> selector, Vector3 defaultValue)
        {
            return source.Select(selector).AverageOrDefault(defaultValue);
        }

        public static float AverageOrDefault(this IEnumerable<float> source, float defaultValue)
        {
            return source.AverageOrDefault(0f, PlusFloat, DivFloat, defaultValue);
        }

        public static Vector3 AverageOrDefault(this IEnumerable<Vector3> source, Vector3 defaultValue)
        {
            return source.AverageOrDefault(Vector3.zero, VectorPlus, VectorDiv, defaultValue);
        }

        private static TSource AverageOrDefault<TSource>(this IEnumerable<TSource> source, TSource startValue, Plus<TSource> plus, Div<TSource> div, TSource defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            TSource x = startValue;
            long num = 0L;
            foreach (TSource item in source)
            {
                x = plus(x, item);
                num = checked(num + 1);
            }
            if (num <= 0)
            {
                return defaultValue;
            }
            return div(x, num);
        }

        private static float PlusFloat(float x, float y)
        {
            return x + y;
        }

        private static float DivFloat(float x, float count)
        {
            return x / count;
        }

        public static string GetRegionLocale(string locale)
        {
            return locale.Substring(0, 2).ToLower() switch
            {
                "cs" => "cs-CZ",
                "da" => "da-DK",
                "de" => "de-DE",
                "en" => "en-US",
                "es" => "es-ES",
                "fi" => "fi-FI",
                "fr" => "fr-FR",
                "hu" => "hu-HU",
                "id" => "id-ID",
                "it" => "it-IT",
                "ja" => "ja-JP",
                "ko" => "ko-KR",
                "mr" => "mr-IN",
                "nl" => "nl-NL",
                "pl" => "pl-PL",
                "pt" => "pt-PT",
                "ro" => "ro-RO",
                "ru" => "ru-RU",
                "tr" => "tr-TR",
                "zh" => "zh-CN",
                _ => locale.Length == 2 ? locale + "-" + locale.ToLower() : locale,
            };
        }
    }
}
