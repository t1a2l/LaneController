using System;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseHeaderDropDown<TypeItem> : BaseHeaderPopupButton<CustomUIListBox>
    {
        protected TypeItem[] Items { get; set; } = [];

        public float ListWidth { get; set; }

        public float MinListWidth { get; set; }

        public event Action<TypeItem> OnSelect;

        public void Init(TypeItem[] items)
        {
            Items = items;
        }

        protected override void OnPopupOpened()
        {
            base.OnPopupOpened();
            Popup.atlas = CommonTextures.Atlas;
            Popup.normalBgSprite = CommonTextures.FieldHovered;
            Popup.itemHeight = 20;
            Popup.itemHover = CommonTextures.FieldNormal;
            Popup.itemHighlight = CommonTextures.FieldFocused;
            Popup.textScale = 0.7f;
            Popup.color = Color.white;
            Popup.itemTextColor = Color.black;
            Popup.itemPadding = new RectOffset(5, 5, 5, 0);
            Popup.items = [.. Items.Select((i) => GetItemLabel(i))];
            float popupWidth = GetPopupWidth();
            int num = Popup.items.Length * Popup.itemHeight + Popup.listPadding.vertical;
            Popup.size = new Vector2(popupWidth, num);
            Popup.eventSelectedIndexChanged += PopupSelectedIndexChanged;
        }

        public float GetPopupWidth()
        {
            if (ListWidth == 0f)
            {
                float num = 0f;
                float pixelRatio = PixelsToUnits();
                for (int i = 0; i < Popup.items.Length; i++)
                {
                    using UIFontRenderer uIFontRenderer = Popup.font.ObtainRenderer();
                    uIFontRenderer.wordWrap = false;
                    uIFontRenderer.pixelRatio = pixelRatio;
                    uIFontRenderer.textScale = Popup.textScale;
                    uIFontRenderer.characterSpacing = Popup.characterSpacing;
                    uIFontRenderer.multiLine = false;
                    uIFontRenderer.textAlign = UIHorizontalAlignment.Left;
                    uIFontRenderer.processMarkup = Popup.processMarkup;
                    uIFontRenderer.colorizeSprites = Popup.colorizeSprites;
                    uIFontRenderer.overrideMarkupColors = false;
                    Vector2 vector = uIFontRenderer.MeasureString(Popup.items[i]);
                    if (vector.x > num)
                    {
                        num = vector.x;
                    }
                }
                num += Popup.listPadding.horizontal + Popup.itemPadding.horizontal;
                return Mathf.Max(num, MinListWidth);
            }
            return Mathf.Max(ListWidth, MinListWidth);
        }

        private void PopupSelectedIndexChanged(UIComponent component, int index)
        {
            OnSelect?.Invoke(Items[index]);
            ClosePopup();
        }

        protected abstract string GetItemLabel(TypeItem item);
    }
}
