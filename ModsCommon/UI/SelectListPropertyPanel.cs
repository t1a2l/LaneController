using System.Collections.Generic;

namespace LaneController.ModsCommon.UI
{
    public abstract class SelectListPropertyPanel<Type, PanelType> : SelectPropertyPanel<Type, PanelType>, IReusable where PanelType : SelectListPropertyPanel<Type, PanelType>
    {
        private int SelectIndex { get; set; } = -1;

        private List<Type> ObjectsList { get; set; } = [];

        public IEnumerable<Type> Objects => ObjectsList;

        public override Type Value
        {
            get
            {
                if (SelectIndex != -1)
                {
                    return ObjectsList[SelectIndex];
                }
                return default;
            }
            set
            {
                SetSelected(ObjectsList.FindIndex((o) => IsEqual(value, o)));
            }
        }

        private void SetSelected(int index)
        {
            if (index != SelectIndex)
            {
                SelectIndex = index;
                ValueChanged();
            }
        }

        public void Add(Type item)
        {
            ObjectsList.Add(item);
        }

        public void AddRange(IEnumerable<Type> items)
        {
            ObjectsList.AddRange(items);
        }

        public void Clear()
        {
            ObjectsList.Clear();
            Value = default;
        }

        protected abstract bool IsEqual(Type first, Type second);
    }
}
