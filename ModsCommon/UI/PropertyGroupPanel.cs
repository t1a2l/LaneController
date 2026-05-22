using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class PropertyGroupPanel : UIAutoLayoutPanel, IReusable
    {
        private static Color32 NormalColor { get; } = new Color32(82, 101, 117, byte.MaxValue);

        bool IReusable.InCache { get; set; }

        protected virtual Color32 Color => NormalColor;

        public PropertyGroupPanel()
        {
            atlas = TextureHelper.InGameAtlas;
            backgroundSprite = "ButtonWhite";
            color = Color;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = new RectOffset(0, 0, 0, 0);
            autoFitChildrenVertically = true;
        }

        public virtual void Init(float? width = null)
        {
            if (width.HasValue)
            {
                base.width = width.Value;
            }
            else if (parent is UIScrollablePanel uIScrollablePanel)
            {
                base.width = uIScrollablePanel.width - uIScrollablePanel.autoLayoutPadding.horizontal - uIScrollablePanel.scrollPadding.horizontal;
            }
            else if (parent is UIPanel uIPanel)
            {
                base.width = uIPanel.width - uIPanel.autoLayoutPadding.horizontal;
            }
            else
            {
                base.width = parent.width;
            }
        }

        public virtual void DeInit()
        {
            StopLayout();
            UIComponent[] array = [.. components];
            UIComponent[] array2 = array;
            foreach (UIComponent component in array2)
            {
                ComponentPool.Free(component);
            }
            isVisible = true;
            StartLayout(layoutNow: false);
        }

        protected override void OnSizeChanged()
        {
            StopLayout();
            foreach (UIComponent component in components)
            {
                component.width = width - autoLayoutPadding.horizontal;
            }
            StartLayout();
            base.OnSizeChanged();
        }

        protected override void OnComponentAdded(UIComponent child)
        {
            base.OnComponentAdded(child);
            if (child is EditorItem { SupportEven: not false } editorItem)
            {
                editorItem.eventVisibilityChanged += ItemVisibilityChanged;
                SetEven();
            }
        }

        protected override void OnComponentRemoved(UIComponent child)
        {
            base.OnComponentRemoved(child);
            if (child is EditorItem { SupportEven: not false } editorItem)
            {
                editorItem.eventVisibilityChanged -= ItemVisibilityChanged;
                SetEven();
            }
        }

        private void ItemVisibilityChanged(UIComponent component, bool value)
        {
            SetEven();
        }

        public void SetEven()
        {
            if (autoLayout)
            {
                EditorItem[] array = [.. (from c in components.OfType<EditorItem>()
                                      where c.SupportEven && c.isVisible
                                      select c)];
                bool flag = array.Length > 1;
                EditorItem[] array2 = array;
                foreach (EditorItem editorItem in array2)
                {
                    editorItem.IsEven = flag;
                    flag = !flag;
                }
            }
        }

        public override void StartLayout(bool layoutNow = true)
        {
            base.StartLayout(layoutNow);
            SetEven();
        }
    }
}
