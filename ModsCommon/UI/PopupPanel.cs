using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class PopupPanel : CustomUIPanel
    {
        private float _width = 250f;

        protected virtual Color32 Background => Color.black;

        public CustomUIScrollablePanel Content { get; private set; }

        private float Padding => 2f;

        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                Refresh();
            }
        }

        public Vector2 MaxSize
        {
            get
            {
                return Content.maximumSize;
            }
            set
            {
                Content.maximumSize = value;
            }
        }

        public PopupPanel()
        {
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            color = Background;
            atlas = CommonTextures.Atlas;
            backgroundSprite = CommonTextures.FieldHovered;
            AddPanel();
        }

        private void AddPanel()
        {
            Content = AddUIComponent<CustomUIScrollablePanel>();
            Content.autoLayout = true;
            Content.autoLayoutDirection = LayoutDirection.Vertical;
            Content.autoLayoutPadding = new RectOffset(0, 0, 0, 0);
            Content.clipChildren = true;
            Content.builtinKeyNavigation = true;
            Content.scrollWheelDirection = UIOrientation.Vertical;
            Content.maximumSize = new Vector2(500f, 500f);
            Content.relativePosition = new Vector2(Padding, Padding);
            this.AddScrollbar(Content);
        }

        public void Refresh()
        {
            Content.FitChildrenVertically();
            Content.width = Content.verticalScrollbar.isVisible ? Width - Content.verticalScrollbar.width : Width;
            ContentSizeChanged();
        }

        private void ContentSizeChanged(UIComponent component = null, Vector2 value = default)
        {
            if (!(Content != null))
            {
                return;
            }
            size = new Vector2(Width + Padding * 2f, Content.height + Padding * 2f);
            foreach (UIComponent component2 in Content.components)
            {
                component2.width = Content.width;
            }
        }
    }
}
