using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaneController.KianCommons.Utils
{
    internal static class ListExtensions
    {
        internal static List<T> Clone1<T>(this IEnumerable<T> orig) where T : ICloneable
        {
            return [.. orig.Select((item) => (T)item.Clone())];
        }

        internal static Dictionary<TKey, TValue> ShallowClone<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            if (dict == null)
            {
                return null;
            }
            return new Dictionary<TKey, TValue>(dict);
        }

        internal static bool IsNullorEmpty<T>(this ICollection<T> a)
        {
            if (a != null)
            {
                return a.Count == 0;
            }
            return true;
        }

        internal static bool IsNullorEmpty<T>(this IEnumerable<T> a)
        {
            if (a != null)
            {
                return !a.Any();
            }
            return true;
        }

        internal static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> a) where T : class
        {
            return a ?? Enumerable.Empty<T>();
        }

        internal static IEnumerable<T?> EmptyIfNull<T>(this IEnumerable<T?> a) where T : struct
        {
            return a ?? Enumerable.Empty<T?>();
        }

        internal static bool AllEqual<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            comparer = comparer ?? EqualityComparer<T>.Default;
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    if (!comparer.Equals(enumerator.Current, current))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal static bool AllEqual<T, T2>(this IEnumerable<T> source, Func<T, T2> selector, IEqualityComparer<T2> comparer = null)
        {
            return source.Select(selector).AllEqual();
        }

        internal static int FindIndex<T>(this IEnumerable<T> e, Func<T, bool> predicate)
        {
            int num = 0;
            foreach (T item in e)
            {
                if (predicate(item))
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        internal static void DropElement<T>(ref T[] array, int i)
        {
            int num = array.Length;
            T[] array2 = new T[num - 1];
            int j = 0;
            int num2 = 0;
            for (; j < num; j++)
            {
                if (j != i)
                {
                    array2[num2] = array[j];
                    num2++;
                }
            }
            array = array2;
        }

        internal static T[] RemoveAt<T>(this T[] array, int index)
        {
            List<T> list = [.. array];
            list.RemoveAt(index);
            return [.. list];
        }

        internal static bool ContainsRef<T>(this IEnumerable<T> list, T element) where T : class
        {
            foreach (T item in list)
            {
                if (item == element)
                {
                    return true;
                }
            }
            return false;
        }

        internal static void AppendElement<T>(ref T[] array, T element)
        {
            Array.Resize(ref array, array.Length + 1);
            array.Last() = element;
        }

        internal static T[] AppendOrCreate<T>(this T[] array, T element)
        {
            if (array == null)
            {
                array = [element];
            }
            else
            {
                AppendElement(ref array, element);
            }
            return array;
        }

        internal static void ReplaceElement<T>(this T[] array, T oldVal, T newVal)
        {
            int num = ((IList)array).IndexOf(oldVal);
            if (num >= 0)
            {
                array[num] = newVal;
            }
        }

        internal static void ReplaceElement(this Array array, object oldVal, object newVal)
        {
            int num = ((IList)array).IndexOf(oldVal);
            if (num >= 0)
            {
                array.SetValue(newVal, num);
            }
        }

        internal static ref T Last<T>(this T[] array)
        {
            return ref array[array.Length - 1];
        }

        internal static void Swap<T>(this IList<T> list, int i1, int i2)
        {
            T value = list[i1];
            list[i1] = list[i2];
            list[i2] = value;
        }

        internal static TItem MinBy<TItem, TBy>(this IEnumerable<TItem> items, Func<TItem, TBy> selector) where TBy : IComparable
        {
            if (items == null)
            {
                return default;
            }
            TItem result = default;
            TBy val = default;
            bool flag = true;
            foreach (TItem item in items)
            {
                TBy val2 = selector(item);
                if (flag || val2.CompareTo(val) < 0)
                {
                    flag = false;
                    result = item;
                    val = val2;
                }
            }
            return result;
        }

        internal static TItem MaxBy<TItem, TBy>(this IEnumerable<TItem> items, Func<TItem, TBy> selector) where TBy : IComparable
        {
            if (items == null)
            {
                return default;
            }
            TItem result = default;
            TBy val = default;
            bool flag = true;
            foreach (TItem item in items)
            {
                TBy val2 = selector(item);
                if (flag || val2.CompareTo(val) > 0)
                {
                    flag = false;
                    result = item;
                    val = val2;
                }
            }
            return result;
        }

        internal static TValue GetorDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.TryGetValue(key, out var value))
            {
                return default;
            }
            return value;
        }

        internal static TValue GetorSet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.TryGetValue(key, out var value))
            {
                return dict[key] = value;
            }
            return value;
        }

        internal static void AddRange<T>(this HashSet<T> hashset, IEnumerable<T> elements)
        {
            foreach (T element in elements)
            {
                hashset.Add(element);
            }
        }

        internal static bool InRange(this IList list, int i)
        {
            if (0 <= i)
            {
                return i < list.Count;
            }
            return false;
        }

        internal static bool InRange<T>(this IList<T> list, int i)
        {
            if (0 <= i)
            {
                return i < list.Count;
            }
            return false;
        }
    }
}
