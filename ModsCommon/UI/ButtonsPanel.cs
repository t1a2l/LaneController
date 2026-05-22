using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class ButtonsPanel : EditorItem, IReusable
    {
        bool IReusable.InCache { get; set; }

        protected List<CustomUIButton> Buttons { get; } = [];

        public int Count => Buttons.Count;

        private float Space => 10f;

        private float Height => 20f;

        public override bool SupportEven => true;

        public override bool EnableControl
        {
            get
            {
                return base.EnableControl;
            }
            set
            {
                base.EnableControl = value;
                foreach (CustomUIButton button in Buttons)
                {
                    button.isEnabled = value;
                }
            }
        }

        public CustomUIButton this[int index] => Buttons[index];

        public event Action<int> OnButtonClick;

        protected override void Init(float? height)
        {
            base.Init(height);
            SetSize();
        }

        public override void DeInit()
        {
            foreach (CustomUIButton button in Buttons)
            {
                button.parent.RemoveUIComponent(button);
                Destroy(button);
            }
            OnButtonClick = null;
            Buttons.Clear();
            base.DeInit();
        }

        public int AddButton(string text)
        {
            CustomUIButton customUIButton = AddButton(this);
            customUIButton.text = text;
            customUIButton.textScale = 0.8f;
            customUIButton.textPadding = new RectOffset(0, 0, 3, 0);
            customUIButton.isEnabled = EnableControl;
            customUIButton.eventClick += ButtonClick;
            Buttons.Add(customUIButton);
            return Count - 1;
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (component is CustomUIButton item)
            {
                int num = Buttons.IndexOf(item);
                if (num != -1)
                {
                    OnButtonClick?.Invoke(num);
                }
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            SetSize();
        }

        private void SetSize()
        {
            float num = (width - Space * (Count - 1) - ItemsPadding * 2) / Count;
            for (int i = 0; i < Count; i++)
            {
                Buttons[i].size = new Vector2(num, Height);
                Buttons[i].relativePosition = new Vector2((num + Space) * i + ItemsPadding, (height - Height) / 2f);
            }
        }
    }
}
