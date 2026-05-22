using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class EditorPropertyPanel : EditorItem
    {
        protected class ContentPanel : CustomUIPanel
        {
            public ContentPanel()
            {
                autoLayoutDirection = LayoutDirection.Horizontal;
                autoFitChildrenHorizontally = true;
                autoLayoutPadding = new RectOffset(5, 0, 0, 0);
                name = "Content";
            }

            protected override void OnSizeChanged()
            {
                base.OnSizeChanged();
                Refresh();
            }

            public void Refresh()
            {
                autoLayout = true;
                autoLayout = false;
                foreach (UIComponent component in components)
                {
                    component.relativePosition = new Vector2(component.relativePosition.x, (height - component.height) / 2f);
                }
            }
        }

        private CustomUILabel Label { get; set; }

        protected ContentPanel Content { get; set; }

        public string Text
        {
            get
            {
                return Label.text;
            }
            set
            {
                Label.text = value;
            }
        }

        public override bool EnableControl
        {
            get
            {
                return Content.isEnabled;
            }
            set
            {
                Content.isEnabled = value;
            }
        }

        public override bool SupportEven => true;

        public EditorPropertyPanel()
        {
            Label = AddUIComponent<CustomUILabel>();
            Label.textScale = 0.75f;
            Label.autoSize = false;
            Label.autoHeight = true;
            Label.wordWrap = true;
            Label.padding = new RectOffset(0, 0, 2, 0);
            Label.disabledTextColor = new Color32(160, 160, 160, byte.MaxValue);
            Label.name = "Label";
            Label.eventTextChanged += delegate
            {
                SetLabel();
            };
            Content = AddUIComponent<ContentPanel>();
        }

        protected override void Init(float? height)
        {
            base.Init(height);
            Refresh();
        }

        public override void DeInit()
        {
            base.DeInit();
            Text = string.Empty;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            Refresh(refreshContent: false);
        }

        protected void Refresh(bool refreshContent = true)
        {
            if (refreshContent)
            {
                Content.Refresh();
            }
            Content.height = height;
            Content.relativePosition = new Vector2(width - Content.width - ItemsPadding, 0f);
            SetLabel();
        }

        private void SetLabel()
        {
            Label.width = width - Content.width - ItemsPadding * 2;
            Label.MakePixelPerfect(recursive: false);
            Label.relativePosition = new Vector2(5f, (height - Label.height) / 2f);
        }

        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            if (isVisible)
            {
                Refresh();
            }
        }
    }
}
