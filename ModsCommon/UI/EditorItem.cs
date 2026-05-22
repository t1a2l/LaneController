using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class EditorItem : CustomUIPanel
    {
        protected virtual float DefaultHeight => 30f;

        protected virtual int ItemsPadding => 5;

        public virtual bool EnableControl { get; set; } = true;

        private CustomUIPanel Even { get; }

        public virtual bool SupportEven => false;

        public bool IsEven
        {
            get
            {
                return Even.isVisible;
            }
            set
            {
                Even.isVisible = value;
            }
        }

        public EditorItem()
        {
            Even = AddUIComponent<CustomUIPanel>();
            Even.atlas = CommonTextures.Atlas;
            Even.backgroundSprite = CommonTextures.Empty;
            Even.color = new Color32(0, 0, 0, 48);
            IsEven = false;
        }

        public virtual void DeInit()
        {
            IsEven = false;
            EnableControl = true;
        }

        public virtual void Init()
        {
            Init(null);
        }

        protected virtual void Init(float? height)
        {
            size = new Vector2(GetWidth(), height ?? DefaultHeight);
        }

        private float GetWidth()
        {
            if (parent is UIScrollablePanel uIScrollablePanel)
            {
                return uIScrollablePanel.width - uIScrollablePanel.autoLayoutPadding.horizontal - uIScrollablePanel.scrollPadding.horizontal;
            }
            if (parent is UIPanel uIPanel)
            {
                return uIPanel.width - uIPanel.autoLayoutPadding.horizontal;
            }
            return parent.width;
        }

        protected CustomUIButton AddButton(UIComponent parent)
        {
            CustomUIButton customUIButton = parent.AddUIComponent<CustomUIButton>();
            customUIButton.SetDefaultStyle();
            return customUIButton;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            Even?.size = size;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
