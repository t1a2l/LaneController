using System;
using System.ComponentModel;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class UITextField<TypeValue> : CustomUITextField, IValueChanger<TypeValue>, IReusable
    {
        private TypeValue _value;

        private string _format;

        private static string DefaultFormat => "{0}";

        bool IReusable.InCache { get; set; }

        private bool InProcess { get; set; }

        public bool UseReset { get; set; } = true;

        public TypeValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                ValueChanged(value, callEvent: false);
            }
        }

        public string Format
        {
            private get
            {
                if (string.IsNullOrEmpty(_format))
                {
                    return DefaultFormat;
                }
                return _format;
            }
            set
            {
                _format = value;
                RefreshText();
            }
        }

        public event Action<TypeValue> OnValueChanged;

        public event Action OnResetValue;

        public void SimulateEnterValue(TypeValue value)
        {
            ValueChanged(value);
        }

        protected override void OnKeyUp(UIKeyEventParameter p)
        {
            base.OnKeyUp(p);
            if (containsMouse && !containsFocus && UseReset && p.keycode == KeyCode.Delete)
            {
                ResetValue();
            }
        }

        public virtual void ResetValue()
        {
            OnResetValue?.Invoke();
        }

        protected virtual void ValueChanged(TypeValue value, bool callEvent = true)
        {
            if (!InProcess)
            {
                InProcess = true;
                _value = value;
                if (callEvent)
                {
                    OnValueChanged?.Invoke(_value);
                }
                RefreshText();
                InProcess = false;
            }
        }

        protected void RefreshText()
        {
            text = hasFocus ? GetString(Value) : FormatString(Value);
        }

        public virtual void DeInit()
        {
            OnValueChanged = null;
            Unfocus();
            _value = default;
            _format = null;
            m_Text = string.Empty;
        }

        protected string FormatString(TypeValue value)
        {
            return string.Format(Format, GetString(value));
        }

        protected virtual string GetString(TypeValue value)
        {
            return value?.ToString() ?? string.Empty;
        }

        protected override void OnGotFocus(UIFocusEventParameter p)
        {
            RefreshText();
            base.OnGotFocus(p);
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            RefreshText();
        }

        protected override void OnSubmit()
        {
            bool flag = hasFocus;
            base.OnSubmit();
            if (!flag && text == GetString(Value))
            {
                RefreshText();
                return;
            }
            TypeValue value = default;
            try
            {
                if (typeof(TypeValue) == typeof(string))
                {
                    value = (TypeValue)(object)text;
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    value = (TypeValue)TypeDescriptor.GetConverter(typeof(TypeValue)).ConvertFromString(text);
                }
            }
            catch
            {
            }
            ValueChanged(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator TypeValue(UITextField<TypeValue> field)
        {
            return field.Value;
        }

        public void SetDefaultStyle()
        {
            atlas = CommonTextures.Atlas;
            normalBgSprite = CommonTextures.FieldNormal;
            hoveredBgSprite = CommonTextures.FieldHovered;
            focusedBgSprite = CommonTextures.FieldNormal;
            disabledBgSprite = CommonTextures.FieldDisabled;
            selectionSprite = CommonTextures.Empty;
            allowFloats = true;
            isInteractive = true;
            enabled = true;
            readOnly = false;
            builtinKeyNavigation = true;
            cursorWidth = 1;
            cursorBlinkTime = 0.45f;
            selectOnFocus = true;
            textScale = 0.7f;
            verticalAlignment = UIVerticalAlignment.Middle;
            padding = new RectOffset(0, 0, 6, 0);
        }
    }
}
