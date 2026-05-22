using System.Linq;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class AutoSizeAdvancedScrollablePanel : BaseAdvancedScrollablePanel<AutoSizeAdvancedScrollablePanel.AutoSizeScrollablePanel>
    {
        public class AutoSizeScrollablePanel : UIAutoLayoutScrollablePanel
        {
            protected override void OnComponentAdded(UIComponent child)
            {
                base.OnComponentAdded(child);
                FitContentChildren();
                child.eventVisibilityChanged += OnChildVisibilityChanged;
                child.eventSizeChanged += OnChildSizeChanged;
            }

            protected override void OnComponentRemoved(UIComponent child)
            {
                base.OnComponentRemoved(child);
                FitContentChildren();
                child.eventVisibilityChanged -= OnChildVisibilityChanged;
                child.eventSizeChanged -= OnChildSizeChanged;
            }

            private void OnChildVisibilityChanged(UIComponent component, bool value)
            {
                FitContentChildren();
            }

            private void OnChildSizeChanged(UIComponent component, Vector2 value)
            {
                FitContentChildren();
            }

            private void FitContentChildren()
            {
                if (!autoLayout || !(parent != null))
                {
                    return;
                }
                float num = 0f;
                foreach (UIComponent component in components)
                {
                    if (component.isVisibleSelf)
                    {
                        num = Mathf.Max(num, component.relativePosition.y + component.height);
                    }
                }
                if (components.Any())
                {
                    num += autoLayoutPadding.bottom;
                }
                if (num < height)
                {
                    height = num;
                    parent.height = num;
                }
                else
                {
                    parent.height = num;
                    height = num;
                }
                verticalScrollbar.isVisible = Mathf.CeilToInt(verticalScrollbar.scrollSize) < Mathf.CeilToInt(verticalScrollbar.maxValue - verticalScrollbar.minValue);
            }

            public override void StartLayout(bool layoutNow = true)
            {
                base.StartLayout(layoutNow);
                if (layoutNow)
                {
                    FitContentChildren();
                }
            }
        }

        public Vector2 MaxSize
        {
            get
            {
                return maximumSize;
            }
            set
            {
                maximumSize = value;
                Content.maximumSize = value;
            }
        }

        public AutoSizeAdvancedScrollablePanel()
        {
            Content.verticalScrollbar.autoHide = false;
        }
    }
}
