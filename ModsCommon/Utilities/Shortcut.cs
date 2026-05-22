using System;
using ColossalFramework;
using UnityEngine;

namespace LaneController.ModsCommon.Utilities
{
    public class Shortcut
    {
        public virtual string Label => LabelKey;

        protected string LabelKey { get; }

        public SavedInputKey InputKey { get; }

        private Action Action { get; }

        public bool CanRepeat { get; set; }

        public bool IgnoreModifiers { get; set; }

        public bool NotSet => InputKey.Key == KeyCode.None;

        protected float LastPress { get; private set; }

        protected int LastPressFrame { get; private set; }

        protected bool DelayIsOut
        {
            get
            {
                if (Time.time - LastPress > 0.15f)
                {
                    return LastPressFrame != Time.frameCount;
                }
                return false;
            }
        }

        public bool IsPressed
        {
            get
            {
                InputKey value = InputKey.value;
                KeyCode key = (KeyCode)((int)value & 0xFFFFFFF);
                if (!DelayIsOut || (CanRepeat ? !Input.GetKey(key) : !Input.GetKeyUp(key)))
                {
                    return false;
                }
                if (!IgnoreModifiers)
                {
                    if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) != (((int)value & 0x40000000) != 0))
                    {
                        return false;
                    }
                    if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) != (((int)value & 0x20000000) != 0))
                    {
                        return false;
                    }
                    if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr)) != (((int)value & 0x10000000) != 0))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public Shortcut(string fileName, string name, string labelKey, InputKey key, Action action = null)
        {
            LabelKey = labelKey;
            InputKey = new SavedInputKey(name, fileName, key, autoUpdate: true);
            Action = action;
        }

        public Shortcut(SavedInputKey savedInputKey, string labelKey, Action action = null)
        {
            LabelKey = labelKey;
            InputKey = savedInputKey;
            Action = action;
        }

        public virtual bool Press(Event e)
        {
            if ((CanRepeat ? e.type == EventType.KeyDown : e.type == EventType.KeyUp) && IsPressed)
            {
                Press();
                e.Use();
                LastPress = Time.time;
                LastPressFrame = Time.frameCount;
                return true;
            }
            return false;
        }

        public void Press()
        {
            Action?.Invoke();
        }

        public override string ToString()
        {
            return InputKey.GetLocale();
        }

        public static implicit operator string(Shortcut shortcut)
        {
            return shortcut.ToString();
        }
    }
}
