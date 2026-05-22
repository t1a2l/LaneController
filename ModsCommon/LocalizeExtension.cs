using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Globalization;
using UnityEngine;

namespace LaneController.ModsCommon
{
    public static class LocalizeExtension
    {
        public static string Separator => " + ";

        public static string Ctrl => CommonLocalize.Key_Control;

        public static string Alt => CommonLocalize.Key_Alt;

        public static string Shift => CommonLocalize.Key_Shift;

        public static string CtrlAlt => Ctrl + Separator + Alt;

        public static string CtrlShift => Ctrl + Separator + Shift;

        public static string AltShift => Alt + Separator + Shift;

        public static string CtrlAltShift => Ctrl + Separator + Alt + Separator + Shift;

        public static string GetLocale(this SavedInputKey savedKey)
        {
            string text = string.Empty;
            if (savedKey.Control)
            {
                text = text + Ctrl + Separator;
            }
            if (savedKey.Alt)
            {
                text = text + Alt + Separator;
            }
            if (savedKey.Shift)
            {
                text = text + Shift + Separator;
            }
            return text + savedKey.GetKeyLocale();
        }

        public static string GetModifiers(bool ctrl = false, bool alt = false, bool shift = false)
        {
            List<string> list = new(3);
            if (ctrl)
            {
                list.Add(Ctrl);
            }
            if (alt)
            {
                list.Add(Alt);
            }
            if (shift)
            {
                list.Add(Shift);
            }
            return string.Join(Separator, [.. list]);
        }

        private static string GetKeyLocale(this SavedInputKey savedKey)
        {
            return savedKey.Key switch
            {
                KeyCode.LeftBracket => "[",
                KeyCode.RightBracket => "]",
                KeyCode.Plus or KeyCode.KeypadPlus => "+",
                KeyCode.Minus or KeyCode.KeypadMinus => "-",
                KeyCode.RightControl or KeyCode.LeftControl => CommonLocalize.Key_Control,
                KeyCode.RightAlt or KeyCode.LeftAlt => CommonLocalize.Key_Alt,
                KeyCode.RightShift or KeyCode.LeftShift => CommonLocalize.Key_Shift,
                KeyCode.Return or KeyCode.KeypadEnter => CommonLocalize.Key_Enter,
                KeyCode.Tab => CommonLocalize.Key_Tab,
                _ => savedKey.Key.GetLocale(),
            };
        }

        public static string GetLocale(this KeyCode key)
        {
            if (!Locale.Exists("KEYNAME", key.ToString()))
            {
                return key.ToString();
            }
            return Locale.Get("KEYNAME", key.ToString());
        }
    }
}
