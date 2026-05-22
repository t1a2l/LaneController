using System;
using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public abstract class ListOncePropertyPanel<Type, UISelector> : ListPropertyPanel<Type, UISelector>, IReusable where UISelector : UIComponent, IUIOnceSelector<Type>
    {
        public Type SelectedObject
        {
            get
            {
                return Selector.SelectedObject;
            }
            set
            {
                Selector.SelectedObject = value;
            }
        }

        public bool UseWheel
        {
            get
            {
                return Selector.UseWheel;
            }
            set
            {
                Selector.UseWheel = value;
            }
        }

        public bool WheelTip
        {
            set
            {
                Selector.WheelTip = value;
            }
        }

        public event Action<Type> OnSelectObjectChanged;

        protected override void AddSelector()
        {
            base.AddSelector();
            Selector.OnSelectObjectChanged += SelectorValueChanged;
        }

        protected virtual void SelectorValueChanged(Type value)
        {
            OnSelectObjectChanged?.Invoke(value);
        }

        public override void DeInit()
        {
            OnSelectObjectChanged = null;
            UseWheel = false;
            WheelTip = false;
            base.DeInit();
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {SelectedObject}";
        }
    }
}
