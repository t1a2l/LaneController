using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using LaneController.KianCommons.Math;

namespace LaneController.KianCommons.Utils
{
    internal static class EnumExtensions
    {
        internal static T Max<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Max();
        }

        internal static void SetBit(this ref byte b, int idx)
        {
            b |= (byte)(1 << idx);
        }

        internal static void ClearBit(this ref byte b, int idx)
        {
            b &= (byte)~(1 << idx);
        }

        internal static bool GetBit(this byte b, int idx)
        {
            return (b & (byte)(1 << idx)) != 0;
        }

        internal static void SetBit(this ref byte b, int idx, bool value)
        {
            if (value)
            {
                b.SetBit(idx);
            }
            else
            {
                b.ClearBit(idx);
            }
        }

        public static int CountOnes(int value)
        {
            int num = 0;
            while (value != 0)
            {
                value >>= 1;
                num++;
            }
            return num;
        }

        public static int CountOnes(long value)
        {
            int num = 0;
            while (value != 0L)
            {
                value >>= 1;
                num++;
            }
            return num;
        }

        public static int CountOnes(uint value)
        {
            int num = 0;
            while (value != 0)
            {
                value >>= 1;
                num++;
            }
            return num;
        }

        public static int CountOnes(ulong value)
        {
            int num = 0;
            while (value != 0L)
            {
                value >>= 1;
                num++;
            }
            return num;
        }

        internal static T GetMaxEnumValue<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Max();
        }

        internal static int GetEnumCount<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        [Conditional("DEBUG")]
        private static void CheckEnumWithFlags<T>() where T : struct, Enum, IConvertible
        {
        }

        [Conditional("DEBUG")]
        private static void CheckEnumWithFlags(Type type)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type '" + type.FullName + "' is not an enum");
            }
            if (!Attribute.IsDefined(type, typeof(FlagsAttribute)))
            {
                throw new ArgumentException("Type '" + type.FullName + "' doesn't have the 'Flags' attribute");
            }
            if (!Enum.GetUnderlyingType(type).IsInteger())
            {
                throw new Exception("Type '" + type.FullName + "' is not integer based enum.");
            }
        }

        public static void SetFlagsRef<T>(this ref T value, T flags, bool on) where T : struct, IConvertible
        {
            value = value.SetFlags(flags, on);
        }

        internal static bool IsFlagSet(this NetInfo.Direction value, NetInfo.Direction flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this NetInfo.LaneType value, NetInfo.LaneType flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this VehicleInfo.VehicleType value, VehicleInfo.VehicleType flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this NetNode.Flags value, NetNode.Flags flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this NetNode.FlagsLong value, NetNode.FlagsLong flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this NetSegment.Flags value, NetSegment.Flags flag)
        {
            return (value & flag) != 0;
        }

        internal static bool IsFlagSet(this NetLane.Flags value, NetLane.Flags flag)
        {
            return (value & flag) != 0;
        }

        internal static bool CheckFlags(this NetInfo.Direction value, NetInfo.Direction required, NetInfo.Direction forbidden = NetInfo.Direction.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this NetInfo.LaneType value, NetInfo.LaneType required, NetInfo.LaneType forbidden = NetInfo.LaneType.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this VehicleInfo.VehicleType value, VehicleInfo.VehicleType required, VehicleInfo.VehicleType forbidden = VehicleInfo.VehicleType.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this NetNode.Flags value, NetNode.Flags required, NetNode.Flags forbidden = NetNode.Flags.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this NetNode.FlagsLong value, NetNode.FlagsLong required, NetNode.FlagsLong forbidden = NetNode.FlagsLong.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this NetSegment.Flags value, NetSegment.Flags required, NetSegment.Flags forbidden = NetSegment.Flags.None)
        {
            return (value & (required | forbidden)) == required;
        }

        internal static bool CheckFlags(this NetLane.Flags value, NetLane.Flags required, NetLane.Flags forbidden = NetLane.Flags.None)
        {
            return (value & (required | forbidden)) == required;
        }

        public static bool CheckFlags<T>(this T value, T required, T forbidden) where T : struct, Enum, IConvertible
        {
            long num = value.ToInt64();
            long num2 = required.ToInt64();
            long num3 = forbidden.ToInt64();
            return (num & (num2 | num3)) == num2;
        }

        public static ulong ToUInt64(this IConvertible value)
        {
            Type type = value.GetType();
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            if (type.IsSigned())
            {
                return (ulong)value.ToInt64(CultureInfo.InvariantCulture);
            }
            return value.ToUInt64(CultureInfo.InvariantCulture);
        }

        public static long ToInt64(this IConvertible value)
        {
            Type type = value.GetType();
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            if (type.IsSigned())
            {
                return value.ToInt64(CultureInfo.InvariantCulture);
            }
            return (long)value.ToUInt64(CultureInfo.InvariantCulture);
        }

        public static IEnumerable<IConvertible> GetValuesSorted(Type enumType)
        {
            MemberInfo[] members = enumType.GetMembers();
            foreach (MemberInfo memberInfo in members)
            {
                if (memberInfo is FieldInfo fieldInfo && fieldInfo.FieldType == enumType)
                {
                    IConvertible convertible = null;
                    try
                    {
                        convertible = Enum.Parse(enumType, memberInfo.Name) as IConvertible;
                    }
                    catch
                    {
                    }
                    if (convertible != null)
                    {
                        yield return convertible;
                    }
                }
            }
        }

        public static IEnumerable<T> GetValuesSorted<T>() where T : struct, Enum, IConvertible
        {
            return GetValuesSorted(typeof(T)).Cast<T>();
        }

        public static IEnumerable<IConvertible> GetPow2Values(Type enumType)
        {
            return from val in GetValuesSorted(enumType)
                   where MathUtil.IsPow2(val)
                   select val;
        }

        public static IEnumerable<T> GetPow2Values<T>() where T : struct, Enum, IConvertible
        {
            return from val in GetValuesSorted<T>()
                   where MathUtil.IsPow2(val)
                   select val;
        }

        public static IEnumerable<T> ExtractPow2Flags<T>(this T flags) where T : struct, Enum, IConvertible
        {
            return from flag in GetPow2Values<T>()
                   where flags.IsFlagSet(flag)
                   select flag;
        }

        public static IEnumerable<IConvertible> ExtractPow2Flags(this IConvertible flags)
        {
            return from flag in GetPow2Values(flags.GetType())
                   where flags.IsFlagSet(flag)
                   select flag;
        }

        public static bool IsFlagSet(this IConvertible flags, IConvertible flag)
        {
            long num = flags.ToInt64();
            long num2 = flag.ToInt64();
            return (num & num2) != 0;
        }

        public static IEnumerable<int> GetPow2ValuesI32(Type enumType)
        {
            IEnumerable<int> source = Enum.GetValues(enumType).Cast<int>();
            return source.Where((v) => MathUtil.IsPow2(v));
        }

        public static MemberInfo GetEnumMemberInfo(this Type enumType, object value)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            string name = Enum.GetName(enumType, value);
            return name == null
                ? throw new Exception($"{enumType.GetType().Name}:{value} not found")
                : enumType.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).FirstOrDefault() ?? throw new Exception(enumType.GetType().Name + "." + name + " not found");
        }

        public static MemberInfo GetEnumMemberInfo(this Enum value)
        {
            return value.GetType().GetEnumMemberInfo(value);
        }

        public static T[] GetEnumMemberAttributes<T>(Type enumType, object value) where T : Attribute
        {
            return enumType.GetEnumMemberInfo(value).GetAttributes<T>();
        }

        public static T[] GetEnumMemberAttributes<T>(this Enum value) where T : Attribute
        {
            return GetEnumMemberAttributes<T>(value.GetType(), value);
        }

        public static T[] GetEnumValues<T>() where T : struct, Enum, IConvertible
        {
            return Enum.GetValues(typeof(T)) as T[];
        }
    }
}
