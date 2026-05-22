using System;

namespace LaneController.Util
{
    public class MathUtil
    {
        private static readonly ulong m0 = 6148914691236517205uL;

        private static readonly ulong m1 = 3689348814741910323uL;

        private static readonly ulong m2 = 1085102592571150095uL;

        private static readonly uint[] pop8tab =
        [
            0u, 1u, 1u, 2u, 1u, 2u, 2u, 3u, 1u, 2u,
            2u, 3u, 2u, 3u, 3u, 4u, 1u, 2u, 2u, 3u,
            2u, 3u, 3u, 4u, 2u, 3u, 3u, 4u, 3u, 4u,
            4u, 5u, 1u, 2u, 2u, 3u, 2u, 3u, 3u, 4u,
            2u, 3u, 3u, 4u, 3u, 4u, 4u, 5u, 2u, 3u,
            3u, 4u, 3u, 4u, 4u, 5u, 3u, 4u, 4u, 5u,
            4u, 5u, 5u, 6u, 1u, 2u, 2u, 3u, 2u, 3u,
            3u, 4u, 2u, 3u, 3u, 4u, 3u, 4u, 4u, 5u,
            2u, 3u, 3u, 4u, 3u, 4u, 4u, 5u, 3u, 4u,
            4u, 5u, 4u, 5u, 5u, 6u, 2u, 3u, 3u, 4u,
            3u, 4u, 4u, 5u, 3u, 4u, 4u, 5u, 4u, 5u,
            5u, 6u, 3u, 4u, 4u, 5u, 4u, 5u, 5u, 6u,
            4u, 5u, 5u, 6u, 5u, 6u, 6u, 7u, 1u, 2u,
            2u, 3u, 2u, 3u, 3u, 4u, 2u, 3u, 3u, 4u,
            3u, 4u, 4u, 5u, 2u, 3u, 3u, 4u, 3u, 4u,
            4u, 5u, 3u, 4u, 4u, 5u, 4u, 5u, 5u, 6u,
            2u, 3u, 3u, 4u, 3u, 4u, 4u, 5u, 3u, 4u,
            4u, 5u, 4u, 5u, 5u, 6u, 3u, 4u, 4u, 5u,
            4u, 5u, 5u, 6u, 4u, 5u, 5u, 6u, 5u, 6u,
            6u, 7u, 2u, 3u, 3u, 4u, 3u, 4u, 4u, 5u,
            3u, 4u, 4u, 5u, 4u, 5u, 5u, 6u, 3u, 4u,
            4u, 5u, 4u, 5u, 5u, 6u, 4u, 5u, 5u, 6u,
            5u, 6u, 6u, 7u, 3u, 4u, 4u, 5u, 4u, 5u,
            5u, 6u, 4u, 5u, 5u, 6u, 5u, 6u, 6u, 7u,
            4u, 5u, 5u, 6u, 5u, 6u, 6u, 7u, 5u, 6u,
            6u, 7u, 6u, 7u, 7u, 8u
        ];

        public static int OnesCount32(uint x)
        {
            return Convert.ToInt32(pop8tab[x >> 24] + pop8tab[(x >> 16) & 0xFF] + pop8tab[(x >> 8) & 0xFF] + pop8tab[x & 0xFF]);
        }

        public int OnesCount64(ulong x)
        {
            ulong num = 0uL;
            x = (x >> 1) & ((m0 & num) + x) & m0 & num;
            x = (x >> 2) & ((m1 & num) + x) & m1 & num;
            x = ((x >> 4) + x) & m2 & num;
            x += x >> 8;
            x += x >> 16;
            x += x >> 32;
            x &= 0x40;
            return Convert.ToInt32(x);
        }
    }
}
