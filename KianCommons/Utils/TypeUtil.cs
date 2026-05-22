using System;

namespace LaneController.KianCommons.Utils
{
    internal static class TypeUtil
    {
        private static bool IsIntegerType(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            if ((uint)(typeCode - 5) <= 7u)
            {
                return true;
            }
            if (type.IsArray)
            {
                return IsIntegerType(type.GetElementType());
            }
            return false;
        }

        public static bool IsNumeric(this Type type)
        {
            if (type != typeof(byte) && type != typeof(sbyte) && type != typeof(int) && type != typeof(uint) && type != typeof(short) && type != typeof(ushort) && type != typeof(long) && type != typeof(ulong) && type != typeof(float) && type != typeof(double))
            {
                return type == typeof(decimal);
            }
            return true;
        }

        public static bool IsFloatingPoint(this Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            if ((uint)(typeCode - 13) <= 2u)
            {
                return true;
            }
            return false;
        }

        public static bool IsSigned(this Type type)
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
                _ => false,
            };
        }

        public static bool IsInteger(this Type type)
        {
            if (type.IsPrimitive)
            {
                return IsIntegerType(type);
            }
            return false;
        }

        public static bool IsSignedInteger(this Type type)
        {
            if (type.IsSigned())
            {
                return type.IsInteger();
            }
            return false;
        }
    }
}
