using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class ColorPropertyPanel : EditorPropertyPanel, IReusable
    {
        bool IReusable.InCache { get; set; }

        private bool InProcess { get; set; }

        private ByteUITextField R { get; set; }

        private ByteUITextField G { get; set; }

        private ByteUITextField B { get; set; }

        private ByteUITextField A { get; set; }

        private UIColorField ColorSample { get; set; }

        protected UIColorPicker Popup { get; private set; }

        private UISlider Opacity { get; set; }

        public bool WheelTip
        {
            set
            {
                R.MouseTips = value;
                G.MouseTips = value;
                B.MouseTips = value;
                A.MouseTips = value;
            }
        }

        public Color32 Value
        {
            get
            {
                return new Color32(R, G, B, A);
            }
            set
            {
                ValueChanged(value, callEvent: false, OnChangedValue);
            }
        }

        public event Action<Color32> OnValueChanged;

        public ColorPropertyPanel()
        {
            R = AddField("R");
            G = AddField("G");
            B = AddField("B");
            A = AddField("A");
            AddColorSample();
        }

        protected void ValueChanged(Color32 color, bool callEvent = true, Action<Color32> action = null)
        {
            if (!InProcess)
            {
                InProcess = true;
                action(color);
                if (callEvent)
                {
                    OnValueChanged?.Invoke(Value);
                }
                InProcess = false;
            }
        }

        private void FieldChanged(byte value)
        {
            ValueChanged(Value, callEvent: true, OnChangedField);
        }

        private void SelectedColorChanged(UIComponent component, Color value)
        {
            Color32 color = value;
            color.a = A;
            ValueChanged(color, callEvent: true, OnChangedSelected);
        }

        private void OpacityChanged(UIComponent component, float value)
        {
            Color32 value2 = Value;
            value2.a = (byte)value;
            ValueChanged(value2, callEvent: true, OnChangedOpacity);
        }

        protected void OnChangedValue(Color32 color)
        {
            SetFields(color);
            SetSample(color);
            SetOpacity(color);
        }

        private void OnChangedField(Color32 color)
        {
            SetSample(color);
            SetOpacity(color);
        }

        private void OnChangedSelected(Color32 color)
        {
            SetFields(color);
            SetOpacity(color);
        }

        private void OnChangedOpacity(Color32 color)
        {
            SetFields(color);
            SetSample(color);
        }

        private void SetFields(Color32 color)
        {
            R.Value = color.r;
            G.Value = color.g;
            B.Value = color.b;
            A.Value = color.a;
        }

        private void SetSample(Color32 color)
        {
            color.a = byte.MaxValue;
            ColorSample?.selectedColor = color;
            Popup?.color = color;
        }

        private void SetOpacity(Color32 color)
        {
            if (Opacity != null)
            {
                Opacity.value = color.a;
                color.a = byte.MaxValue;
                Opacity.Find<UISlicedSprite>("color").color = color;
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            WheelTip = false;
            OnValueChanged = null;
        }

        private ByteUITextField AddField(string name)
        {
            CustomUILabel customUILabel = Content.AddUIComponent<CustomUILabel>();
            customUILabel.text = name;
            customUILabel.textScale = 0.7f;
            customUILabel.padding = new RectOffset(0, 0, 2, 0);
            ByteUITextField byteUITextField = Content.AddUIComponent<ByteUITextField>();
            byteUITextField.SetDefaultStyle();
            byteUITextField.MinValue = 0;
            byteUITextField.MaxValue = byte.MaxValue;
            byteUITextField.CheckMax = true;
            byteUITextField.CheckMin = true;
            byteUITextField.UseWheel = true;
            byteUITextField.WheelStep = 10;
            byteUITextField.width = 30f;
            byteUITextField.OnValueChanged += FieldChanged;
            return byteUITextField;
        }

        private void AddColorSample()
        {
            UIComponent uIComponent = UITemplateManager.Get("LineTemplate");
            if (uIComponent is not null)
            {
                CustomUIPanel customUIPanel = Content.AddUIComponent<CustomUIPanel>();
                customUIPanel.atlas = CommonTextures.Atlas;
                customUIPanel.backgroundSprite = CommonTextures.ColorPickerBoard;
                ColorSample = Instantiate(uIComponent.Find<UIColorField>("LineColor").gameObject).GetComponent<UIColorField>();
                customUIPanel.AttachUIComponent(ColorSample.gameObject);
                UIColorField colorSample = ColorSample;
                Vector2 vector = customUIPanel.size = new Vector2(20f, 20f);
                colorSample.size = vector;
                ColorSample.relativePosition = new Vector2(0f, 0f);
                ColorSample.anchor = UIAnchorStyle.None;
                ColorSample.atlas = CommonTextures.Atlas;
                ColorSample.normalBgSprite = CommonTextures.ColorPickerNormal;
                ColorSample.normalFgSprite = CommonTextures.ColorPickerColor;
                ColorSample.hoveredBgSprite = CommonTextures.ColorPickerHovered;
                ColorSample.hoveredFgSprite = CommonTextures.ColorPickerColor;
                ColorSample.disabledBgSprite = CommonTextures.ColorPickerDisabled;
                ColorSample.disabledFgSprite = CommonTextures.ColorPickerColor;
                ColorSample.eventSelectedColorChanged += SelectedColorChanged;
                ColorSample.eventColorPickerOpen += ColorPickerOpen;
                ColorSample.eventColorPickerClose += ColorPickerClose;
            }
        }

        protected virtual void ColorPickerOpen(UIColorField dropdown, UIColorPicker popup, ref bool overridden)
        {
            dropdown.triggerButton.isInteractive = false;
            Popup = popup;
            Popup.component.size += new Vector2(31f, 31f);
            Popup.component.relativePosition -= new Vector3(dropdown.width + 31f, Math.Max(Popup.component.absolutePosition.y - dropdown.absolutePosition.y, 0f));
            if (Popup.component is UIPanel uIPanel)
            {
                uIPanel.atlas = TextureHelper.InGameAtlas;
                uIPanel.backgroundSprite = "ButtonWhite";
                uIPanel.color = new Color32(201, 211, 216, byte.MaxValue);
            }
            Opacity = AddOpacitySlider(popup.component);
            SetOpacity(Value);
        }

        private void ColorPickerClose(UIColorField dropdown, UIColorPicker popup, ref bool overridden)
        {
            dropdown.triggerButton.isInteractive = true;
            Popup = null;
            Opacity = null;
        }

        private UISlider AddOpacitySlider(UIComponent parent)
        {
            UISlider uISlider = parent.AddUIComponent<UISlider>();
            uISlider.atlas = TextureHelper.InGameAtlas;
            uISlider.size = new Vector2(18f, 200f);
            uISlider.relativePosition = new Vector3(254f, 12f);
            uISlider.orientation = UIOrientation.Vertical;
            uISlider.minValue = 0f;
            uISlider.maxValue = 255f;
            uISlider.stepSize = 1f;
            uISlider.eventValueChanged += OpacityChanged;
            UISlicedSprite uISlicedSprite = uISlider.AddUIComponent<UISlicedSprite>();
            uISlicedSprite.atlas = CommonTextures.Atlas;
            uISlicedSprite.spriteName = CommonTextures.OpacitySliderBoard;
            uISlicedSprite.relativePosition = Vector2.zero;
            uISlicedSprite.size = uISlider.size;
            uISlicedSprite.fillDirection = UIFillDirection.Vertical;
            UISlicedSprite uISlicedSprite2 = uISlider.AddUIComponent<UISlicedSprite>();
            uISlicedSprite2.name = "color";
            uISlicedSprite2.atlas = CommonTextures.Atlas;
            uISlicedSprite2.spriteName = CommonTextures.OpacitySliderColor;
            uISlicedSprite2.relativePosition = Vector2.zero;
            uISlicedSprite2.size = uISlider.size;
            uISlicedSprite2.fillDirection = UIFillDirection.Vertical;
            UISlicedSprite uISlicedSprite3 = uISlider.AddUIComponent<UISlicedSprite>();
            uISlicedSprite3.relativePosition = Vector2.zero;
            uISlicedSprite3.fillDirection = UIFillDirection.Horizontal;
            uISlicedSprite3.size = new Vector2(29f, 7f);
            uISlicedSprite3.spriteName = "ScrollbarThumb";
            uISlider.thumbObject = uISlicedSprite3;
            return uISlider;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Value}";
        }

        public static implicit operator Color32(ColorPropertyPanel property)
        {
            return property.Value;
        }
    }
}
