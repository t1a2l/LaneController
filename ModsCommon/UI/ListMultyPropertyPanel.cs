using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public abstract class ListMultyPropertyPanel<Type, UISelector> : ListPropertyPanel<Type, UISelector>, IReusable where UISelector : UIComponent, IUIMultySelector<Type>
    {
        public List<Type> SelectedObjects
        {
            get
            {
                return Selector.SelectedObjects;
            }
            set
            {
                Selector.SelectedObjects = value;
            }
        }

        public event Action<List<Type>> OnSelectObjectsChanged;

        protected override void AddSelector()
        {
            base.AddSelector();
            Selector.OnSelectObjectsChanged += SelectorValueChanged;
        }

        protected virtual void SelectorValueChanged(List<Type> value)
        {
            OnSelectObjectsChanged?.Invoke(value);
        }

        public override void DeInit()
        {
            OnSelectObjectsChanged = null;
            base.DeInit();
        }

        public override string ToString()
        {
            return base.ToString() + ": " + string.Join(",", [.. SelectedObjects.Select((i) => i.ToString())]);
        }
    }
}
