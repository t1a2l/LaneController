using ColossalFramework.Math;

namespace LaneController.KianCommons.Serialization
{
    public struct Bezier3XML
    {
        public Vector3XML A;

        public Vector3XML B;

        public Vector3XML C;

        public Vector3XML D;

        public static implicit operator Bezier3(Bezier3XML v)
        {
            return new Bezier3(v.A, v.B, v.C, v.D);
        }

        public static implicit operator Bezier3XML(Bezier3 v)
        {
            return new Bezier3XML
            {
                A = v.a,
                B = v.b,
                C = v.c,
                D = v.d
            };
        }
    }
}
