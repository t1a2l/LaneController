using System;
using System.Collections.Generic;
using System.Linq;

namespace LaneController.ModsCommon.Utilities
{
    public static class EnumExtension
    {
        public static AttrType GetAttr<AttrType, T>(this T value) where AttrType : Attribute where T : Enum
        {
            return typeof(T).GetField(value.ToString()).GetCustomAttributes(typeof(AttrType), inherit: false).OfType<AttrType>()
                .FirstOrDefault();
        }

        private static Func<T, bool> GetVisibleSelector<T>() where T : Enum
        {
            return (value) => value.IsVisible();
        }

        public static IEnumerable<T> GetEnumValues<T>(Func<T, bool> selector = null) where T : Enum
        {
            return Enum.GetValues(typeof(T)).OfType<T>().Where(selector ?? GetVisibleSelector<T>());
        }

        public static IEnumerable<T> GetEnumValues<T>(this T value) where T : Enum
        {
            return from v in GetEnumValues<T>()
                   where (value.ToInt() & v.ToInt()) != 0
                   select v;
        }

        public static T GetEnum<T>(this List<T> values) where T : Enum
        {
            return values.Aggregate(0, (r, v) => r | v.ToInt()).ToEnum<T>();
        }

        public static bool IsVisible<T>(this T value) where T : Enum
        {
            return value.GetAttr<NotVisibleAttribute, T>() == null;
        }

        public static bool IsItem<T>(this T value) where T : Enum
        {
            return value.GetAttr<NotItemAttribute, T>() == null;
        }

        public static int ToInt<T>(this T value) where T : Enum
        {
            return (int)(object)value;
        }

        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)(object)value;
        }

        public static long ToLong<T>(this T value) where T : Enum
        {
            return (long)(object)value;
        }

        public static T ToEnum<T>(this long value) where T : Enum
        {
            return (T)(object)value;
        }

        public static ulong ToULong<T>(this T value) where T : Enum
        {
            return (ulong)(object)value;
        }

        public static T ToEnum<T>(this ulong value) where T : Enum
        {
            return (T)(object)value;
        }

        public static ToT ToEnum<ToT, FromT>(this FromT item) where ToT : Enum where FromT : Enum
        {
            return (ToT)(object)item;
        }

        public static bool IsSet<T>(this T flags, T flag) where T : Enum
        {
            return (flags.ToInt() & flag.ToInt()) == flag.ToInt();
        }

        public static bool CheckFlags<T>(this T value, T required, T forbidden) where T : Enum, IConvertible
        {
            return (value.ToInt() & (required.ToInt() | forbidden.ToInt())) == required.ToInt();
        }

        public static bool CheckFlags(this NetNode.Flags value, NetNode.Flags required, NetNode.Flags forbidden = NetNode.Flags.None)
        {
            return (value & (required | forbidden)) == required;
        }

        public static bool CheckFlags(this NetSegment.Flags value, NetSegment.Flags required, NetSegment.Flags forbidden = NetSegment.Flags.None)
        {
            return (value & (required | forbidden)) == required;
        }
    }
}
