using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class UIDropDown<ValueType> : CustomUIDropDown, IUIOnceSelector<ValueType>, IUISelector<ValueType>, IAutoLayoutPanel, IReusable
    {
        bool IReusable.InCache { get; set; }

        public Func<ValueType, ValueType, bool> IsEqualDelegate { get; set; }

        private List<ValueType> Objects { get; } = [];

        public ValueType SelectedObject
        {
            get
            {
                if (selectedIndex < 0)
                {
                    return default;
                }
                return Objects[selectedIndex];
            }
            set
            {
                selectedIndex = Objects.FindIndex(delegate (ValueType o)
                {
                    Func<ValueType, ValueType, bool> isEqualDelegate = IsEqualDelegate;
                    return isEqualDelegate?.Invoke(o, value) ?? (object)o == (object)value || (o?.Equals(value) ?? false);
                });
            }
        }

        public bool CanWheel { get; set; }

        public bool UseWheel { get; set; }

        public bool WheelTip
        {
            set
            {
                tooltip = value ? CommonLocalize.ListPanel_ScrollWheel : string.Empty;
            }
        }

        public bool UseScrollBar { get; set; }

        public event Action<ValueType> OnSelectObjectChanged;

        public UIDropDown()
        {
            eventSelectedIndexChanged += IndexChanged;
        }

        protected virtual void IndexChanged(UIComponent component, int value)
        {
            OnSelectObjectChanged?.Invoke(SelectedObject);
        }

        public void AddItem(ValueType item, string label = null)
        {
            Objects.Add(item);
            AddItem(label ?? item.ToString());
        }

        public void Clear()
        {
            selectedIndex = -1;
            Objects.Clear();
            items = [];
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            base.OnMouseMove(p);
            CanWheel = true;
        }

        protected override void OnMouseLeave(UIMouseEventParameter p)
        {
            base.OnMouseLeave(p);
            CanWheel = false;
        }

        protected sealed override void OnMouseWheel(UIMouseEventParameter p)
        {
            m_TooltipShowing = true;
            tooltipBox.Hide();
            if (UseWheel && (CanWheel || Time.realtimeSinceStartup - m_HoveringStartTime >= 1f))
            {
                if (p.wheelDelta > 0f && selectedIndex > 0)
                {
                    selectedIndex--;
                }
                else if (p.wheelDelta < 0f && selectedIndex < Objects.Count - 1)
                {
                    selectedIndex++;
                }
                p.Use();
            }
        }

        public void StopLayout()
        {
        }

        public void StartLayout(bool layoutNow = true)
        {
        }

        void IReusable.DeInit()
        {
            Clear();
            UseWheel = false;
            WheelTip = false;
            UseScrollBar = false;
        }

        public void SetDefaultStyle(Vector2? size = null)
        {
            this.DefaultStyle(size);
            if (UseScrollBar)
            {
                listScrollbar = UIHelper.ScrollBar;
            }
        }
    }
}
