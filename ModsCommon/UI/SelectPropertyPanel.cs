using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class SelectPropertyPanel<Type, PanelType> : EditorPropertyPanel, IReusable where PanelType : SelectPropertyPanel<Type, PanelType>
    {
        bool IReusable.InCache { get; set; }

        protected abstract string NotSet { get; }

        protected SelectPropertyButton Selector { get; set; }

        protected CustomUIButton Button { get; set; }

        protected abstract float Width { get; }

        public abstract Type Value { get; set; }

        public bool Selected
        {
            get
            {
                return Selector.state == UIButton.ButtonState.Focused;
            }
            set
            {
                Selector.state = value ? UIButton.ButtonState.Focused : UIButton.ButtonState.Normal;
            }
        }

        public event Action<Type> OnValueChanged;

        public event Action<PanelType> OnSelect;

        public event Action<PanelType> OnEnter;

        public event Action<PanelType> OnLeave;

        public SelectPropertyPanel()
        {
            AddSelector();
        }

        private void AddSelector()
        {
            Selector = Content.AddUIComponent<SelectPropertyButton>();
            Selector.text = NotSet;
            Selector.atlas = CommonTextures.Atlas;
            Selector.normalBgSprite = CommonTextures.FieldNormal;
            Selector.hoveredBgSprite = CommonTextures.FieldHovered;
            Selector.disabledBgSprite = CommonTextures.FieldDisabled;
            Selector.focusedBgSprite = CommonTextures.FieldFocused;
            Selector.isInteractive = false;
            Selector.enabled = true;
            Selector.autoSize = false;
            Selector.textHorizontalAlignment = UIHorizontalAlignment.Left;
            Selector.textVerticalAlignment = UIVerticalAlignment.Middle;
            Selector.height = 20f;
            Selector.width = Width;
            Selector.textScale = 0.6f;
            Selector.textPadding = new RectOffset(8, 0, 4, 0);
            Button = Selector.AddUIComponent<CustomUIButton>();
            Button.atlas = TextureHelper.InGameAtlas;
            Button.text = string.Empty;
            Button.size = Selector.size;
            Button.relativePosition = new Vector3(0f, 0f);
            Button.textVerticalAlignment = UIVerticalAlignment.Middle;
            Button.textHorizontalAlignment = UIHorizontalAlignment.Left;
            Button.normalFgSprite = "IconDownArrow";
            Button.hoveredFgSprite = "IconDownArrowHovered";
            Button.pressedFgSprite = "IconDownArrowPressed";
            Button.focusedFgSprite = "IconDownArrow";
            Button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            Button.horizontalAlignment = UIHorizontalAlignment.Right;
            Button.verticalAlignment = UIVerticalAlignment.Middle;
            Button.textScale = 0.8f;
            Button.eventClick += ButtonClick;
            Button.eventMouseEnter += ButtonMouseEnter;
            Button.eventMouseLeave += ButtonMouseLeave;
            Button.eventIsEnabledChanged += ButtonIsEnabledChanged;
        }

        public override void DeInit()
        {
            base.DeInit();
            OnSelect = null;
            OnEnter = null;
            OnLeave = null;
            OnValueChanged = null;
        }

        public new void Focus()
        {
            Button.Focus();
        }

        private void ButtonIsEnabledChanged(UIComponent component, bool value)
        {
            Button.normalFgSprite = value ? "IconDownArrow" : "Empty";
        }

        protected void ValueChanged()
        {
            OnValueChanged?.Invoke(Value);
            SelectPropertyButton selector = Selector;
            Type value = Value;
            selector.text = (value?.ToString()) ?? NotSet;
        }

        protected virtual void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnSelect?.Invoke((PanelType)this);
        }

        protected virtual void ButtonMouseEnter(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnEnter?.Invoke((PanelType)this);
        }

        protected virtual void ButtonMouseLeave(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnLeave?.Invoke((PanelType)this);
        }
    }
}
