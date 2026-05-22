using ColossalFramework.Math;
using UnityEngine;

namespace LaneController.Util
{
    public static class BezierUtil
    {
        public static float middleT1 = 0.1f;

        public static float middleT2 = 0.9f;

        public static Bezier3 Add(this Bezier3 lhs, Bezier3 rhs)
        {
            return new Bezier3(lhs.a + rhs.a, lhs.b + rhs.b, lhs.c + rhs.c, lhs.d + rhs.d);
        }

        public static bool EqualsTo(this Bezier3 lhs, Bezier3 rhs)
        {
            if (lhs.a == rhs.a && lhs.b == rhs.b && lhs.c == rhs.c)
            {
                return lhs.d == rhs.d;
            }
            return false;
        }

        public static Bezier3 MathLine(Vector3 startDir, Vector3 endDir, Bezier3 basic, float shift)
        {
            Vector3 start = CalcShift(basic.a, startDir, ShiftAtOffset(shift, 0f));
            Vector3 point = CalcShift(basic, middleT1, ShiftAtOffset(shift, middleT1 / 2f));
            Vector3 point2 = CalcShift(basic, middleT2, ShiftAtOffset(shift, 0.5f + (middleT2 / 2f)));
            Vector3 end = CalcShift(basic.d, -endDir, ShiftAtOffset(shift, 1f));
            return CalcPerfict(start, end, point, point2, middleT1, middleT2);
        }

        public static float ShiftAtOffset(float shift, float t)
        {
            return shift * (t - 0.5f);
        }

        public static Vector3 CalcShift(Bezier3 basic, float t, float shift)
        {
            Vector3 pos = basic.Position(t);
            Vector3 dir = basic.Tangent(t);
            return CalcShift(pos, dir, shift);
        }

        public static Vector3 Turn90(this Vector3 v, bool isClockWise)
        {
            if (!isClockWise)
            {
                return new Vector3(0f - v.z, v.y, v.x);
            }
            return new Vector3(v.z, v.y, 0f - v.x);
        }

        public static Vector3 CalcShift(Vector3 pos, Vector3 dir, float shift)
        {
            return pos + (dir.Turn90(isClockWise: true).normalized * shift);
        }

        public static Bezier3 Shift(this Bezier3 bezier, float shift, float vshift)
        {
            float magnitude = (bezier.d - bezier.a).magnitude;
            Vector3 vector = bezier.b - bezier.a;
            bezier.a = CalcShift(bezier.a, vector, shift);
            bezier.a.y += vshift;
            Vector3 vector2 = bezier.c - bezier.d;
            bezier.d = CalcShift(bezier.d, -vector2, shift);
            bezier.d.y += vshift;
            float magnitude2 = (bezier.d - bezier.a).magnitude;
            float num = magnitude2 / magnitude;
            bezier.b = bezier.a + (vector * num);
            bezier.c = bezier.d + (vector2 * num);
            return bezier;
        }

        public static Bezier3 CalcPerfict(Vector3 start, Vector3 end, Vector3 point1, Vector3 point2, float t1, float t2)
        {
            CalcCoef(t1, out var a, out var b, out var c, out var d);
            CalcCoef(t2, out var a2, out var b2, out var c2, out var d2);
            Vector3 u = CalcU(start, end, point1, a, d);
            Vector3 u2 = CalcU(start, end, point2, a2, d2);
            CalcMiddlePoints(b, c, u, b2, c2, u2, out var middle, out var middle2);
            return new Bezier3
            {
                a = start,
                b = middle,
                c = middle2,
                d = end
            };
        }

        public static void CalcCoef(float t, out float a, out float b, out float c, out float d)
        {
            float num = 1f - t;
            a = num * num * num;
            b = 3f * t * num * num;
            c = 3f * t * t * num;
            d = t * t * t;
        }

        public static float CalcU(float start, float end, float point, float a, float d)
        {
            return point - (a * start) - (d * end);
        }

        public static void CalcMiddlePoints(float b1, float c1, float u1, float b2, float c2, float u2, out float m1, out float m2)
        {
            m2 = (u2 - (b2 / b1 * u1)) / (c2 - (b2 / b1 * c1));
            m1 = (u1 - (c1 * m2)) / b1;
        }

        public static Vector3 CalcU(Vector3 start, Vector3 end, Vector3 point, float a, float d)
        {
            return new Vector3(CalcU(start.x, end.x, point.x, a, d), CalcU(start.y, end.y, point.y, a, d), CalcU(start.z, end.z, point.z, a, d));
        }

        public static void CalcMiddlePoints(float b1, float c1, Vector3 u1, float b2, float c2, Vector3 u2, out Vector3 middle1, out Vector3 middle2)
        {
            CalcMiddlePoints(b1, c1, u1.x, b2, c2, u2.x, out middle1.x, out middle2.x);
            CalcMiddlePoints(b1, c1, u1.y, b2, c2, u2.y, out middle1.y, out middle2.y);
            CalcMiddlePoints(b1, c1, u1.z, b2, c2, u2.z, out middle1.z, out middle2.z);
        }
    }
}
