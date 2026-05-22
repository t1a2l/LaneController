using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class UISegmented<ValueType> : UIAutoLayoutPanel, IReusable
    {
        private bool _autoButtonSize = true;

        private float _buttonWidth = 50f;

        private float _textScale = 0.8f;

        bool IReusable.InCache { get; set; }

        public Func<ValueType, ValueType, bool> IsEqualDelegate { get; set; }

        protected List<ValueType> Objects { get; } = [];

        protected List<CustomUIButton> Buttons { get; } = [];

        protected Dictionary<CustomUIButton, bool> Clickable { get; } = [];

        protected virtual int TextPadding
        {
            get
            {
                if (!AutoButtonSize)
                {
                    return (int)Mathf.Clamp((_buttonWidth - 20f) / 2f, 0f, 8f);
                }
                return 8;
            }
        }

        public bool AutoButtonSize
        {
            get
            {
                return _autoButtonSize;
            }
            set
            {
                if (value != _autoButtonSize)
                {
                    _autoButtonSize = value;
                    SetButtonsWidth();
                }
            }
        }

        public float ButtonWidth
        {
            get
            {
                return _buttonWidth;
            }
            set
            {
                if (value != _buttonWidth)
                {
                    _buttonWidth = value;
                    SetButtonsWidth();
                }
            }
        }

        public float TextScale
        {
            get
            {
                return _textScale;
            }
            set
            {
                if (value != _textScale)
                {
                    _textScale = value;
                    SetButtonsWidth();
                }
            }
        }

        public UISegmented()
        {
            autoLayoutDirection = LayoutDirection.Horizontal;
            autoFitChildrenHorizontally = true;
            autoFitChildrenVertically = true;
        }

        public void AddItem(ValueType item, string label = null)
        {
            AddItem(item, label, null, null, clickable: true, null);
        }

        public void AddItem(ValueType item, string label = null, UITextureAtlas iconAtlas = null, string iconSprite = null, bool clickable = true, float? width = null)
        {
            Objects.Add(item);
            MultyAtlasUIButton multyAtlasUIButton = AddUIComponent<MultyAtlasUIButton>();
            multyAtlasUIButton.atlas = CommonTextures.Atlas;
            multyAtlasUIButton.textHorizontalAlignment = UIHorizontalAlignment.Center;
            if (iconAtlas != null && !string.IsNullOrEmpty(iconSprite))
            {
                multyAtlasUIButton.AtlasForeground = iconAtlas;
                multyAtlasUIButton.normalFgSprite = iconSprite;
                multyAtlasUIButton.tooltip = label ?? item.ToString();
                multyAtlasUIButton.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            }
            else
            {
                multyAtlasUIButton.text = label ?? item.ToString();
            }
            UpdateButton(multyAtlasUIButton, width);
            if (clickable)
            {
                multyAtlasUIButton.eventClick += ButtonClick;
            }
            CustomUIButton customUIButton = Buttons.LastOrDefault();
            Buttons.Add(multyAtlasUIButton);
            Clickable.Add(multyAtlasUIButton, clickable);
            SetSprite(multyAtlasUIButton, isSelect: false);
            if (customUIButton != null)
            {
                SetSprite(customUIButton, IsSelect(Buttons.Count - 2));
            }
        }

        private void SetButtonsWidth()
        {
            StopLayout();
            foreach (CustomUIButton button in Buttons)
            {
                UpdateButton(button);
            }
            StartLayout();
        }

        private void UpdateButton(CustomUIButton button, float? width = null)
        {
            button.textPadding = new RectOffset(TextPadding, TextPadding, 4, 0);
            button.textScale = TextScale;
            if (AutoButtonSize)
            {
                button.autoSize = true;
                button.autoSize = false;
            }
            else if (width.HasValue)
            {
                button.width = width.Value;
            }
            else
            {
                button.width = ButtonWidth;
            }
            button.height = 20f;
        }

        protected void SetSprite(CustomUIButton button, bool isSelect)
        {
            int index = Buttons.IndexOf(button);
            string text = Suffix(index);
            if (isSelect)
            {
                string text2 = button.disabledBgSprite = CommonTextures.FieldFocused + text;
                string text4 = button.pressedBgSprite = text2;
                string normalBgSprite = button.hoveredBgSprite = text4;
                button.normalBgSprite = normalBgSprite;
                button.disabledColor = new Color32(192, 192, 192, byte.MaxValue);
            }
            else if (!Clickable[button])
            {
                string text2 = button.disabledBgSprite = CommonTextures.FieldDisabled + text;
                string text4 = button.pressedBgSprite = text2;
                string normalBgSprite = button.hoveredBgSprite = text4;
                button.normalBgSprite = normalBgSprite;
                button.disabledColor = Color.white;
            }
            else
            {
                button.normalBgSprite = CommonTextures.FieldNormal + text;
                string normalBgSprite = button.pressedBgSprite = CommonTextures.FieldHovered + text;
                button.hoveredBgSprite = normalBgSprite;
                button.disabledBgSprite = CommonTextures.FieldDisabled + text;
                button.disabledColor = Color.white;
            }
        }

        private string Suffix(int index)
        {
            if (index == 0)
            {
                if (Buttons.Count != 1)
                {
                    return "Left";
                }
                return string.Empty;
            }
            if (index != Buttons.Count - 1)
            {
                return "Middle";
            }
            return "Right";
        }

        protected abstract void ButtonClick(UIComponent component, UIMouseEventParameter eventParam);

        protected abstract bool IsSelect(int index);

        public virtual void DeInit()
        {
            Clear();
            _autoButtonSize = true;
            _buttonWidth = 50f;
            _textScale = 0.8f;
        }

        public virtual void Clear()
        {
            Objects.Clear();
            foreach (CustomUIButton button in Buttons)
            {
                ComponentPool.Free(button);
            }
            Buttons.Clear();
            Clickable.Clear();
        }

        public void SetDefaultStyle(Vector2? size = null)
        {
        }
    }
}
