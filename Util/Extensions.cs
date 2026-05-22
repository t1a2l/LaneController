using System;
using ColossalFramework.Math;
using UnityEngine;

namespace LaneController.Util
{
    internal static class Extensions
    {
        public static bool IsDefault(this Bezier3 bezier)
        {
            if (bezier.a == default && bezier.b == default && bezier.c == default)
            {
                return bezier.d == default;
            }
            return false;
        }

        public static ref Vector3 ControlPoint(this ref Bezier3 bezier, int i)
        {
            switch (i)
            {
                case 0: return ref bezier.a;
                case 1: return ref bezier.b;
                case 2: return ref bezier.c;
                case 3: return ref bezier.d;
                default: throw new IndexOutOfRangeException("i=" + i);
            }
        }
    }
}
