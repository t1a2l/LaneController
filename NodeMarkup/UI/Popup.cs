using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.NodeMarkup.UI
{
    public abstract class Popup<ObjectType, EntityPanel> : CustomUIPanel, IReusable where ObjectType : class where EntityPanel : PopupEntity<ObjectType>
    {
        private ObjectType _selectedObject;

        private int _startIndex;

        bool IReusable.InCache { get; set; }

        protected virtual float DefaultEntityHeight => 20f;

        private CustomUIScrollbar ScrollBar { get; set; }

        private List<ObjectType> RawValues { get; set; }

        private List<ObjectType> Values { get; set; }

        private List<EntityPanel> Entities { get; set; } = [];

        private Func<ObjectType, bool> Selector { get; set; }

        public ObjectType SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                if (value != _selectedObject)
                {
                    _selectedObject = value;
                    int num = Values.IndexOf(_selectedObject);
                    if (num >= 0)
                    {
                        StartIndex = num;
                        SetValues();
                    }
                }
            }
        }

        private int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                if (value != _startIndex)
                {
                    _startIndex = Mathf.Clamp(value, 0, Values.Count - VisibleCount);
                    SetValues();
                }
            }
        }

        protected virtual int VisibleCount => Mathf.Min(Values.Count, MaxVisibleItems, Mathf.FloorToInt(maximumSize.y / EntityHeight));

        protected bool ShowScroll => Values.Count > Entities.Count;

        public float EntityHeight { get; set; }

        public int MaxVisibleItems { get; set; }

        public string ItemHover { get; set; }

        public string ItemSelected { get; set; }

        public event Action<ObjectType> OnSelectedChanged;

        public Popup()
        {
            clipChildren = true;
            GameObject gameObject = Instantiate(ModsCommon.UI.UIHelper.ScrollBar.gameObject);
            AttachUIComponent(gameObject);
            ScrollBar = gameObject.GetComponent<CustomUIScrollbar>();
            ScrollBar.eventValueChanged += ScrollBarValueChanged;
            EntityHeight = DefaultEntityHeight;
        }

        public void Init(IEnumerable<ObjectType> values, Func<ObjectType, bool> selector = null)
        {
            Selector = selector;
            RawValues = [.. values];
            Refresh();
        }

        public virtual void DeInit()
        {
            OnSelectedChanged = null;
            RawValues = null;
            Values = null;
            Selector = null;
            _startIndex = 0;
            EntityHeight = DefaultEntityHeight;
        }

        protected virtual bool Filter(ObjectType value)
        {
            if (Selector != null)
            {
                return Selector(value);
            }
            return true;
        }

        protected virtual void Refresh()
        {
            Values = RawValues == null ? [] : [.. RawValues.Where(Filter)];
            int visibleCount = VisibleCount;
            height = GetHeight();
            if (Entities.Count < visibleCount)
            {
                for (int i = Entities.Count; i < visibleCount; i++)
                {
                    EntityPanel val = AddUIComponent<EntityPanel>();
                    val.atlas = atlas;
                    val.hoveredBgSprite = ItemHover;
                    val.focusedBgSprite = ItemSelected;
                    val.OnSelected += ObjectSelected;
                    Entities.Add(val);
                }
            }
            else if (Entities.Count > visibleCount)
            {
                for (int j = visibleCount; j < Entities.Count; j++)
                {
                    Entities[j].OnSelected -= ObjectSelected;
                    ComponentPool.Free(Entities[j]);
                }
                Entities.RemoveRange(visibleCount, Entities.Count - visibleCount);
            }
            ScrollBar.minValue = 0f;
            ScrollBar.maxValue = Values.Count - visibleCount;
            RefreshItems();
            SetValues();
        }

        private void ObjectSelected(ObjectType value)
        {
            _selectedObject = value;
            OnSelectedChanged?.Invoke(value);
        }

        protected virtual void SetValues()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                if (StartIndex + i < Values.Count)
                {
                    ObjectType val = Values[StartIndex + i];
                    Entities[i].SetObject(val);
                    Entities[i].Selected = val == SelectedObject;
                }
                else
                {
                    Entities[i].SetObject(null);
                    Entities[i].Selected = false;
                }
            }
            ScrollBar.value = StartIndex;
        }

        protected virtual void RefreshItems()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                EntityPanel val = Entities[i];
                val.size = GetEntitySize(i);
                val.relativePosition = GetEntityPosition(i);
            }
            ScrollBar.isVisible = ShowScroll;
            ScrollBar.height = GetScrollHeight();
            ScrollBar.relativePosition = GetScrollPosition();
        }

        protected virtual float GetHeight()
        {
            return VisibleCount * EntityHeight;
        }

        protected virtual Vector2 GetEntitySize(int index)
        {
            return new Vector2(width - (ShowScroll ? ScrollBar.width : 0f), EntityHeight);
        }

        protected virtual Vector2 GetEntityPosition(int index)
        {
            return new Vector2(0f, EntityHeight * index);
        }

        protected virtual float GetScrollHeight()
        {
            return VisibleCount * EntityHeight;
        }

        protected virtual Vector2 GetScrollPosition()
        {
            return new Vector2(width - ScrollBar.width, 0f);
        }

        protected override void OnMouseWheel(UIMouseEventParameter p)
        {
            p.Use();
            StartIndex += (!(p.wheelDelta > 0f) ? 1 : -1) * (!Utility.OnlyShiftIsPressed ? 1 : 10);
        }

        private void ScrollBarValueChanged(UIComponent component, float value)
        {
            StartIndex = (int)value;
        }
    }
}
