using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class ListPropertyPanel<Type, UISelector> : EditorPropertyPanel, IReusable where UISelector : UIComponent, IUISelector<Type>
    {
        bool IReusable.InCache { get; set; }

        public UISelector Selector { get; protected set; }

        protected virtual float DropDownWidth => 230f;

        protected virtual bool AllowNull => true;

        public string NullText { get; set; } = string.Empty;

        public event Action<bool> OnDropDownStateChange;

        public ListPropertyPanel()
        {
            AddSelector();
            Selector.IsEqualDelegate = IsEqual;
        }

        protected virtual void AddSelector()
        {
            Selector = Content.AddUIComponent<UISelector>();
            Selector.SetDefaultStyle(new Vector2(DropDownWidth, 20f));
            Selector.eventSizeChanged += SelectorSizeChanged;
            if (Selector is UIDropDown uIDropDown)
            {
                uIDropDown.eventDropdownOpen += DropDownOpen;
                uIDropDown.eventDropdownClose += DropDownClose;
            }
        }

        private void SelectorSizeChanged(UIComponent component, Vector2 value)
        {
            Refresh();
        }

        private void DropDownOpen(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            dropdown.triggerButton.isInteractive = false;
            OnDropDownStateChange?.Invoke(obj: true);
        }

        private void DropDownClose(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            dropdown.triggerButton.isInteractive = true;
            OnDropDownStateChange?.Invoke(obj: false);
        }

        protected override void Init(float? height)
        {
            base.Init(height);
            Selector.Clear();
            if (AllowNull)
            {
                Selector.AddItem(default, NullText ?? string.Empty);
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            OnDropDownStateChange = null;
            Selector.Clear();
        }

        public void Add(Type item)
        {
            Selector.AddItem(item);
        }

        public void AddRange(IEnumerable<Type> items)
        {
            foreach (Type item in items)
            {
                Selector.AddItem(item);
            }
        }

        protected abstract bool IsEqual(Type first, Type second);
    }
}
