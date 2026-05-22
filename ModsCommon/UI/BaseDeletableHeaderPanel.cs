using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseDeletableHeaderPanel<TypeContent> : BaseHeaderPanel<TypeContent> where TypeContent : BaseHeaderContent
    {
        protected CustomUIButton DeleteButton { get; set; }

        public event Action OnDelete;

        public BaseDeletableHeaderPanel()
        {
            AddDeleteButton();
        }

        public virtual void Init(float? height = null, bool isDeletable = true)
        {
            base.Init(height);
            DeleteButton.isVisible = isDeletable;
            SetSize();
        }

        public override void DeInit()
        {
            base.DeInit();
            OnDelete = null;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            SetSize();
        }

        private void SetSize()
        {
            Content.size = new Vector2((DeleteButton.isVisible ? width - DeleteButton.width - 10f : width) - ItemsPadding, height);
            Content.relativePosition = new Vector2(ItemsPadding, 0f);
            DeleteButton.relativePosition = new Vector2(width - DeleteButton.width - 5f, (height - DeleteButton.height) / 2f);
        }

        private void AddDeleteButton()
        {
            DeleteButton = AddUIComponent<CustomUIButton>();
            DeleteButton.zOrder = 0;
            DeleteButton.atlas = CommonTextures.Atlas;
            DeleteButton.normalBgSprite = CommonTextures.CloseButtonNormal;
            DeleteButton.hoveredBgSprite = CommonTextures.CloseButtonHovered;
            DeleteButton.pressedBgSprite = CommonTextures.CloseButtonPressed;
            DeleteButton.size = new Vector2(20f, 20f);
            DeleteButton.eventClick += DeleteClick;
        }

        private void DeleteClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnDelete?.Invoke();
        }
    }
}
