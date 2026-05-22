using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;

namespace LaneController.ModsCommon.UI
{
    public abstract class EnumOncePropertyPanel<EnumType, UISelector> : ListOncePropertyPanel<EnumType, UISelector> where EnumType : Enum where UISelector : UIComponent, IUIOnceSelector<EnumType>
    {
        protected override bool AllowNull => false;

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
    }
}
