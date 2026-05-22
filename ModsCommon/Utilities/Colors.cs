using System;
using UnityEngine;

namespace LaneController.ModsCommon.Utilities
{
    public static class Colors
    {
        public enum Overlay
        {
            Red,
            Green,
            Blue,
            Orange,
            Purple,
            Lime,
            SkyBlue,
            Yellow,
            Pink,
            Turquoise
        }

        private const byte Alpha = 224;

        public static Color32 White { get; } = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

        public static Color32 White192 { get; } = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 192);

        public static Color32 White128 { get; } = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 128);

        public static Color32 White64 { get; } = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 64);

        public static Color32 Green { get; } = new Color32(0, 200, 81, byte.MaxValue);

        public static Color32 Red { get; } = new Color32(byte.MaxValue, 68, 68, byte.MaxValue);

        public static Color32 Blue { get; } = new Color32(0, 180, byte.MaxValue, byte.MaxValue);

        public static Color32 Orange { get; } = new Color32(byte.MaxValue, 136, 0, byte.MaxValue);

        public static Color32 Yellow { get; } = new Color32(byte.MaxValue, 187, 51, byte.MaxValue);

        public static Color32 Gray224 { get; } = new Color32(224, 224, 224, byte.MaxValue);

        public static Color32 Gray192 { get; } = new Color32(192, 192, 192, byte.MaxValue);

        public static Color32 Gray128 { get; } = new Color32(128, 128, 128, byte.MaxValue);

        public static Color32 Gray64 { get; } = new Color32(64, 64, 64, byte.MaxValue);

        public static Color32 Purple { get; } = new Color32(148, 87, byte.MaxValue, byte.MaxValue);

        public static Color32 Hover { get; } = new Color32(217, 251, byte.MaxValue, byte.MaxValue);

        public static Color32 Error { get; } = new Color32(253, 77, 60, byte.MaxValue);

        public static Color32 Warning { get; } = new Color32(253, 150, 62, byte.MaxValue);

        public static Color32[] OverlayColors { get; } =
        [
            new(218, 33, 40, 224),
            new(72, 184, 94, 224),
            new(0, 120, 191, 224),
            new(245, 130, 32, 224),
            new(142, 71, 155, 224),
            new(180, 212, 69, 224),
            new(0, 193, 243, 224),
            new(byte.MaxValue, 198, 26, 224),
            new(230, 106, 192, 224),
            new(53, 201, 159, 224)
        ];

        public static Color32 GetOverlayColor(int index, byte alpha = 224, byte hue = byte.MaxValue)
        {
            Color32 color = OverlayColors[index % OverlayColors.Length];
            color.a = alpha;
            if (hue != byte.MaxValue)
            {
                return color.SetHue(hue);
            }
            return color;
        }

        public static Color32 GetOverlayColor(Overlay index, byte alpha = 224, byte hue = byte.MaxValue)
        {
            return GetOverlayColor((int)index, alpha, hue);
        }

        public static Color32 SetHue(this Color32 color, byte hue)
        {
            return new Color32(SetHue(color.r, hue), SetHue(color.g, hue), SetHue(color.b, hue), color.a);
        }

        private static byte SetHue(byte value, byte hue)
        {
            return (byte)(255f - (255 - value) / 255f * hue);
        }

        public static Color32 SetAlpha(this Color32 color, byte alpha)
        {
            return new Color32(color.r, color.g, color.b, alpha);
        }

        public static Color32 GetStyleIconColor(this Color32 color)
        {
            float num = 255f / Math.Max(Math.Max(color.r, color.g), color.b);
            Color32 color2 = new((byte)(color.r * num), (byte)(color.g * num), (byte)(color.b * num), byte.MaxValue);
            if (!(color2 == Color.black))
            {
                return color2;
            }
            return Color.white;
        }

        public static Vector4 ToX3Vector(this Color32 c)
        {
            return ((Color)c).ToX3Vector();
        }

        public static Vector4 ToX3Vector(this Color c)
        {
            return new Vector4(ColorChange(c.r), ColorChange(c.g), ColorChange(c.b), Mathf.Pow(c.a, 2f));
        }

        private static float ColorChange(float c)
        {
            return Mathf.Pow(c, 4f);
        }

        public static string AddColor(this string text, Color32 color)
        {
            return $"<color #{color.r:X2}{color.g:X2}{color.b:X2}>{text}</color>";
        }

        public static string AddInfoColor(this string text)
        {
            return "<color #87D3FF>" + text + "</color>";
        }

        public static string AddErrorColor(this string text)
        {
            return "<color #FF7E00>" + text + "</color>";
        }

        public static string AddActionColor(this string text)
        {
            return "<color #5CE66E>" + text + "</color>";
        }

        public static string AddWarningColor(this string text)
        {
            return "<color #FFD119>" + text + "</color>";
        }

        public static string AddInfoColor(this Shortcut shortcut)
        {
            return shortcut.ToString().AddInfoColor();
        }

        public static string AddErrorColor(this Shortcut shortcut)
        {
            return shortcut.ToString().AddErrorColor();
        }

        public static string AddActionColor(this Shortcut shortcut)
        {
            return shortcut.ToString().AddActionColor();
        }

        public static string AddWarningColor(this Shortcut shortcut)
        {
            return shortcut.ToString().AddWarningColor();
        }
    }
}
