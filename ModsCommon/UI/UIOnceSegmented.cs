using System;
using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public abstract class UIOnceSegmented<ValueType> : UISegmented<ValueType>, IUIOnceSelector<ValueType>, IUISelector<ValueType>, IAutoLayoutPanel, IReusable, IValueChanger<ValueType>
    {
        private int SelectedIndex { get; set; } = -1;

        public ValueType SelectedObject
        {
            get
            {
                if (SelectedIndex < 0)
                {
                    return default;
                }
                return Objects[SelectedIndex];
            }
            set
            {
                SetSelected(Objects.FindIndex(delegate (ValueType o)
                {
                    Func<ValueType, ValueType, bool> isEqualDelegate = IsEqualDelegate;
                    return isEqualDelegate?.Invoke(o, value) ?? (object)o == (object)value || o.Equals(value);
                }), callEvent: false);
            }
        }

        ValueType IValueChanger<ValueType>.Value
        {
            get
            {
                return SelectedObject;
            }
            set
            {
                SelectedObject = value;
            }
        }

        public bool UseWheel { get; set; }

        public bool WheelTip
        {
            set
            {
            }
        }

        string IValueChanger<ValueType>.Format
        {
            set
            {
            }
        }

        public event Action<ValueType> OnSelectObjectChanged;

        event Action<ValueType> IValueChanger<ValueType>.OnValueChanged
        {
            add
            {
                OnSelectObjectChanged += value;
            }
            remove
            {
                OnSelectObjectChanged -= value;
            }
        }

        private void SetSelected(int index, bool callEvent = true)
        {
            if (SelectedIndex == index)
            {
                return;
            }
            if (SelectedIndex != -1)
            {
                SetSprite(Buttons[SelectedIndex], isSelect: false);
            }
            SelectedIndex = index;
            if (SelectedIndex != -1)
            {
                SetSprite(Buttons[SelectedIndex], isSelect: true);
                if (callEvent)
                {
                    OnSelectObjectChanged?.Invoke(SelectedObject);
                }
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            OnSelectObjectChanged = null;
            UseWheel = false;
        }

        public override void Clear()
        {
            SelectedIndex = -1;
            base.Clear();
        }

        protected override void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            SetSelected(Buttons.FindIndex((b) => b == component));
        }

        protected override bool IsSelect(int index)
        {
            return SelectedIndex == index;
        }
    }
}
