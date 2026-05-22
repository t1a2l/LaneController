using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseAdvancedScrollablePanel<TypeContent> : CustomUIPanel, IAutoLayoutPanel where TypeContent : UIAutoLayoutScrollablePanel
    {
        public TypeContent Content { get; private set; }

        private bool InProgress { get; set; }

        public BaseAdvancedScrollablePanel()
        {
            clipChildren = true;
            Content = AddUIComponent<TypeContent>();
            Content.autoLayoutDirection = LayoutDirection.Vertical;
            Content.scrollWheelDirection = UIOrientation.Vertical;
            Content.builtinKeyNavigation = true;
            Content.clipChildren = true;
            Content.autoLayoutPadding = new RectOffset(0, 0, 0, 0);
            Content.autoReset = false;
            this.AddScrollbar(Content);
            Content.eventSizeChanged += ContentSizeChanged;
            Content.verticalScrollbar.eventVisibilityChanged += ScrollbarVisibilityChanged;
        }

        private void ContentSizeChanged(UIComponent component, Vector2 value)
        {
            foreach (UIComponent component2 in Content.components)
            {
                component2.width = Content.width - Content.autoLayoutPadding.horizontal;
            }
        }

        private void ScrollbarVisibilityChanged(UIComponent component, bool value)
        {
            if (!InProgress || value)
            {
                InProgress = true;
                SetContentSize();
                InProgress = false;
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            SetContentSize();
        }

        private void SetContentSize()
        {
            Content.size = size - new Vector2(Content.verticalScrollbar.isVisible ? Content.verticalScrollbar.width : 0f, 0f);
        }

        public void StopLayout()
        {
            Content.StopLayout();
        }

        public void StartLayout(bool layoutNow = true)
        {
            Content.StartLayout(layoutNow);
        }
    }
}
