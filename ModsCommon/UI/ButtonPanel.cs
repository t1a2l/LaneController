using System;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class ButtonPanel : EditorItem, IReusable
    {
        bool IReusable.InCache { get; set; }

        protected CustomUIButton Button { get; set; }

        private float Height => 20f;

        public string Text
        {
            get
            {
                return Button.text;
            }
            set
            {
                Button.text = value;
            }
        }

        public override bool EnableControl
        {
            get
            {
                return Button.isEnabled;
            }
            set
            {
                Button.isEnabled = value;
            }
        }

        public override bool SupportEven => true;

        public event Action OnButtonClick;

        public ButtonPanel()
        {
            Button = AddButton(this);
            Button.textScale = 0.8f;
            Button.textPadding = new RectOffset(0, 0, 3, 0);
            Button.isEnabled = EnableControl;
            Button.eventClick += ButtonClick;
        }

        protected override void Init(float? height)
        {
            base.Init(height);
            SetSize();
        }

        public override void DeInit()
        {
            base.DeInit();
            Text = string.Empty;
            OnButtonClick = null;
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnButtonClick?.Invoke();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            SetSize();
        }

        protected virtual void SetSize()
        {
            Button.size = new Vector2(width - ItemsPadding * 2, Height);
            Button.relativePosition = new Vector3(ItemsPadding, (height - Height) / 2f);
        }
    }
}
