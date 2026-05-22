using UnityEngine;

namespace LaneController.Util
{
    public static class Colors
    {
        public static Color White => new(1f, 1f, 1f, 1f);

        public static Color OrangeWeb => new(1f, 0.65f, 0f, 1f);

        public static Color NeonBlue => new(0.27f, 0.4f, 1f, 1f);

        public static Color SteelPink => new(0.8f, 0.2f, 0.8f, 1f);

        public static Color YellowGreen => new(0.6f, 0.8f, 0.2f, 1f);

        public static Color GameBlue => new(0f, 0.71f, 1f, 1f);

        public static Color GameGreen => new(0.37f, 0.65f, 0f, 1f);

        public static Color Add(Color a, float b)
        {
            return new Color(a.r + b, a.g + b, a.b + b, a.a + b);
        }
    }
}
