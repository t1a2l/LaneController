using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class TabStrip<TabType> : CustomUIPanel where TabType : Tab
    {
        public Action<int> SelectedTabChanged;

        private int _selectedTab;

        public int SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                if (value != _selectedTab)
                {
                    _selectedTab = value;
                    SelectedTabChanged?.Invoke(_selectedTab);
                }
            }
        }

        protected List<TabType> Tabs { get; } = [];

        private bool ArrangeInProgress { get; set; }

        public TabStrip()
        {
            clipChildren = true;
        }

        public override void Update()
        {
            base.Update();
            for (int i = 0; i < Tabs.Count; i++)
            {
                TabType val = Tabs[i];
                if (i == SelectedTab)
                {
                    val.state = UIButton.ButtonState.Focused;
                }
                else if (!val.Hovered)
                {
                    val.state = UIButton.ButtonState.Normal;
                }
            }
        }

        public void AddTab(string name, float textScale = 0.85f)
        {
            AddTabImpl(name, textScale);
        }

        protected TabType AddTabImpl(string name, float textScale = 0.85f)
        {
            TabType val = AddUIComponent<TabType>();
            val.text = name;
            val.textPadding = new RectOffset(5, 5, 2, 2);
            val.textScale = textScale;
            val.verticalAlignment = UIVerticalAlignment.Middle;
            SetStyle(val);
            ArrangeTabs();
            return val;
        }

        public void ArrangeTabs()
        {
            TabType[] array = [.. Tabs.Where((t) => t.isVisible)];
            if (array.Any() && !ArrangeInProgress)
            {
                ArrangeInProgress = true;
                TabType[] array2 = array;
                foreach (TabType val in array2)
                {
                    val.autoSize = true;
                    val.autoSize = false;
                    val.textHorizontalAlignment = UIHorizontalAlignment.Center;
                }
                List<List<TabType>> tabRows = FillTabRows(array);
                ArrangeTabRows(tabRows);
                PlaceTabRows(tabRows);
                FitChildrenVertically();
                ArrangeInProgress = false;
            }
        }

        private List<List<TabType>> FillTabRows(TabType[] tabs)
        {
            float num = tabs.Sum((t) => t.width);
            int num2 = (int)(num / width) + 1;
            int num3 = tabs.Length / num2;
            int num4 = tabs.Length - num3 * num2;
            List<List<TabType>> list = [];
            for (int num5 = 0; num5 < num2; num5++)
            {
                List<TabType> list2 = [];
                list.Add(list2);
                int num6 = num5 * num3 + Math.Min(num5, num4);
                int num7 = num6 + num3 + (num5 < num4 ? 1 : 0);
                for (int num8 = num6; num8 < num7; num8++)
                {
                    list2.Add(tabs[num8]);
                }
            }
            return list;
        }

        private void ArrangeTabRows(List<List<TabType>> tabRows)
        {
            for (int i = 0; i < tabRows.Count; i++)
            {
                List<TabType> list = tabRows[i];
                float num = 0f;
                for (int j = 0; j < list.Count; j++)
                {
                    if (num + list[j].width > width)
                    {
                        TabType[] array = [.. list.Skip(j == 0 ? j + 1 : j)];
                        if (array.Any())
                        {
                            if (i == tabRows.Count - 1)
                            {
                                tabRows.Add([]);
                            }
                            tabRows[i + 1].InsertRange(0, array);
                            TabType[] array2 = array;
                            foreach (TabType item in array2)
                            {
                                list.Remove(item);
                            }
                        }
                        break;
                    }
                    num += list[j].width;
                }
            }
        }

        private void PlaceTabRows(List<List<TabType>> tabRows)
        {
            float num = 0f;
            for (int i = 0; i < tabRows.Count; i++)
            {
                List<TabType> list = tabRows[i];
                float num2 = list.Sum((t) => t.width);
                float num3 = list.Max((t) => t.height);
                if (i < tabRows.Count - 1)
                {
                    num3 += 4f;
                }
                float num4 = (width - num2) / list.Count;
                float num5 = 0f;
                for (int num6 = 0; num6 < list.Count; num6++)
                {
                    TabType val = list[num6];
                    val.width = num6 < list.Count - 1 ? Mathf.Floor(val.width + num4) : width - num5;
                    val.height = num3;
                    val.relativePosition = new Vector2(num5, num);
                    num5 += val.width;
                }
                num += num3 - 4f;
            }
        }

        protected override void OnComponentAdded(UIComponent child)
        {
            base.OnComponentAdded(child);
            if (child is TabType val)
            {
                val.eventClick += TabClick;
                val.eventIsEnabledChanged += TabButtonIsEnabledChanged;
                val.eventVisibilityChanged += TabButtonVisibilityChanged;
                Tabs.Add(val);
            }
        }

        protected override void OnComponentRemoved(UIComponent child)
        {
            base.OnComponentRemoved(child);
            if (child is TabType val)
            {
                val.eventClick -= TabClick;
                val.eventIsEnabledChanged -= TabButtonIsEnabledChanged;
                val.eventVisibilityChanged -= TabButtonVisibilityChanged;
                Tabs.Remove(val);
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            ArrangeTabs();
        }

        private void TabClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (component is TabType item)
            {
                SelectedTab = Tabs.IndexOf(item);
            }
        }

        private void TabButtonIsEnabledChanged(UIComponent component, bool value)
        {
            if (!component.isEnabled)
            {
                TabType val = component as TabType;
                val.disabledColor = val.state == UIButton.ButtonState.Focused ? val.focusedColor : val.color;
            }
        }

        private void TabButtonVisibilityChanged(UIComponent component, bool value)
        {
            ArrangeTabs();
        }

        protected virtual void SetStyle(TabType tabButton)
        {
            tabButton.atlas = CommonTextures.Atlas;
            tabButton.normalBgSprite = CommonTextures.TabNormal;
            tabButton.focusedBgSprite = CommonTextures.TabFocused;
            tabButton.hoveredBgSprite = CommonTextures.TabHover;
        }
    }
    public class TabStrip : TabStrip<Tab>
    {
    }
}
