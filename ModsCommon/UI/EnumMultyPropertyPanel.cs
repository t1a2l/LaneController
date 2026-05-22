using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;

namespace LaneController.ModsCommon.UI
{
    public abstract class EnumMultyPropertyPanel<EnumType, UISelector> : ListMultyPropertyPanel<EnumType, UISelector> where EnumType : Enum where UISelector : UIComponent, IUIMultySelector<EnumType>
    {
        protected override bool AllowNull => false;

        public EnumType SelectedObject
        {
            get
            {
                return Selector.SelectedObjects.GetEnum();
            }
            set
            {
                Selector.SelectedObjects = [.. value.GetEnumValues()];
            }
        }

        public event Action<EnumType> OnSelectObjectChanged;

        public override void Init()
        {
            Init(null);
        }

        public void Init(Func<EnumType, bool> selector)
        {
            Init((float?)null);
            FillItems(selector);
        }

        protected virtual void FillItems(Func<EnumType, bool> selector)
        {
            Selector.StopLayout();
            foreach (EnumType enumValue in EnumExtension.GetEnumValues<EnumType>())
            {
                if (selector == null || selector(enumValue))
                {
                    Selector.AddItem(enumValue, GetDescription(enumValue));
                }
            }
            Selector.StartLayout();
        }

        public virtual void Clear()
        {
            Selector.StopLayout();
            Selector.Clear();
            Selector.StartLayout();
        }

        protected abstract string GetDescription(EnumType value);

        protected override void SelectorValueChanged(List<EnumType> value)
        {
            base.SelectorValueChanged(value);
            OnSelectObjectChanged?.Invoke(value.GetEnum());
        }
    }
}
