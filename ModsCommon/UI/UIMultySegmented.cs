using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public abstract class UIMultySegmented<ValueType> : UISegmented<ValueType>, IUIMultySelector<ValueType>, IUISelector<ValueType>, IAutoLayoutPanel, IReusable, IValueChanger<List<ValueType>>
    {
        private HashSet<int> SelectedIndices { get; set; } = [];

        public List<ValueType> SelectedObjects
        {
            get
            {
                return [.. SelectedIndices.Select((i) => Objects[i])];
            }
            set
            {
                HashSet<int> hashSet = [];
                foreach (ValueType item in value)
                {
                    int num = Objects.FindIndex(delegate (ValueType o)
                    {
                        Func<ValueType, ValueType, bool> isEqualDelegate = IsEqualDelegate;
                        return isEqualDelegate?.Invoke(o, item) ?? (object)o == (object)item || o.Equals(item);
                    });
                    if (num >= 0)
                    {
                        hashSet.Add(num);
                    }
                }
                SetSelected(hashSet, callEvent: false);
            }
        }

        List<ValueType> IValueChanger<List<ValueType>>.Value
        {
            get
            {
                return SelectedObjects;
            }
            set
            {
                SelectedObjects = value;
            }
        }

        string IValueChanger<List<ValueType>>.Format
        {
            set
            {
            }
        }

        public event Action<List<ValueType>> OnSelectObjectsChanged;

        event Action<List<ValueType>> IValueChanger<List<ValueType>>.OnValueChanged
        {
            add
            {
                OnSelectObjectsChanged += value;
            }
            remove
            {
                OnSelectObjectsChanged -= value;
            }
        }

        private void SetSelected(HashSet<int> indices, bool callEvent = true)
        {
            foreach (int selectedIndex in SelectedIndices)
            {
                if (!indices.Contains(selectedIndex))
                {
                    SetSprite(Buttons[selectedIndex], isSelect: false);
                }
            }
            foreach (int index in indices)
            {
                if (!SelectedIndices.Contains(index))
                {
                    SetSprite(Buttons[index], isSelect: true);
                }
            }
            SelectedIndices = [.. indices];
            if (callEvent)
            {
                OnSelectObjectsChanged?.Invoke(SelectedObjects);
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            OnSelectObjectsChanged = null;
        }

        public override void Clear()
        {
            SelectedIndices.Clear();
            base.Clear();
        }

        protected override void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            HashSet<int> hashSet = [.. SelectedIndices];
            int item = Buttons.FindIndex((b) => b == component);
            if (hashSet.Contains(item))
            {
                hashSet.Remove(item);
            }
            else
            {
                hashSet.Add(item);
            }
            SetSelected(hashSet);
        }

        protected override bool IsSelect(int index)
        {
            return SelectedIndices.Contains(index);
        }
    }
}
