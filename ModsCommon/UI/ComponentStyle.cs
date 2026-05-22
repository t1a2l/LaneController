using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public static class ComponentStyle
    {
        public static void DefaultStyle(this UIButton button)
        {
            button.normalBgSprite = "ButtonMenu";
            button.hoveredTextColor = new Color32(7, 132, byte.MaxValue, byte.MaxValue);
            button.pressedTextColor = new Color32(30, 30, 44, byte.MaxValue);
            button.disabledTextColor = new Color32(7, 7, 7, byte.MaxValue);
            button.horizontalAlignment = UIHorizontalAlignment.Center;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
        }

        public static void CustomStyle(this UIButton button)
        {
            button.normalBgSprite = "TextFieldPanel";
            button.color = new Color32(224, 224, 224, byte.MaxValue);
            button.hoveredColor = new Color32(192, 192, 192, byte.MaxValue);
            button.pressedColor = new Color32(7, 132, byte.MaxValue, byte.MaxValue);
            button.textColor = Color.white;
            button.hoveredTextColor = Color.white;
            button.pressedTextColor = Color.white;
            button.horizontalAlignment = UIHorizontalAlignment.Center;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
        }

        public static void DefaultStyle(this CustomUIDropDown dropDown, Vector2? size = null)
        {
            dropDown.AtlasBackground = CommonTextures.Atlas;
            dropDown.normalBgSprite = CommonTextures.FieldNormal;
            dropDown.hoveredBgSprite = CommonTextures.FieldHovered;
            dropDown.disabledBgSprite = CommonTextures.FieldDisabled;
            dropDown.AtlasForeground = TextureHelper.InGameAtlas;
            dropDown.normalFgSprite = "IconDownArrow";
            dropDown.hoveredFgSprite = "IconDownArrowHovered";
            dropDown.focusedFgSprite = "IconDownArrow";
            dropDown.disabledFgSprite = "IconDownArrowDisabled";
            dropDown.atlas = CommonTextures.Atlas;
            dropDown.listBackground = CommonTextures.FieldHovered;
            dropDown.itemHover = CommonTextures.FieldNormal;
            dropDown.itemHighlight = CommonTextures.FieldFocused;
            dropDown.itemHeight = 20;
            dropDown.listHeight = 700;
            dropDown.listPosition = UIDropDown.PopupListPosition.Below;
            dropDown.clampListToScreen = true;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.itemPadding = new RectOffset(14, 0, 5, 0);
            dropDown.popupColor = Color.white;
            dropDown.popupTextColor = Color.black;
            dropDown.textScale = 0.7f;
            dropDown.textFieldPadding = new RectOffset(8, 0, 6, 0);
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Right;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.triggerButton = dropDown;
            dropDown.size = size ?? new Vector2(230f, 20f);
        }

        public static void DefaultSettingsStyle(this UIDropDown dropDown, Vector2? size = null)
        {
            dropDown.atlas = TextureHelper.InGameAtlas;
            dropDown.listBackground = "OptionsDropboxListbox";
            dropDown.itemHeight = 24;
            dropDown.itemHover = "ListItemHover";
            dropDown.itemHighlight = "ListItemHighlight";
            dropDown.normalBgSprite = "OptionsDropbox";
            dropDown.hoveredBgSprite = "OptionsDropboxHovered";
            dropDown.focusedBgSprite = "OptionsDropboxFocused";
            dropDown.autoListWidth = true;
            dropDown.listHeight = 700;
            dropDown.listPosition = UIDropDown.PopupListPosition.Below;
            dropDown.clampListToScreen = false;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.popupColor = Color.white;
            dropDown.popupTextColor = new Color32(170, 170, 170, byte.MaxValue);
            dropDown.textScale = 1f;
            dropDown.textFieldPadding = new RectOffset(14, 40, 7, 0);
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.itemPadding = new RectOffset(14, 14, 4, 0);
            dropDown.triggerButton = dropDown;
            dropDown.size = size ?? new Vector2(400f, 31f);
        }

        public static void CustomSettingsStyle(this CustomUIDropDown dropDown, Vector2? size = null)
        {
            dropDown.AtlasBackground = CommonTextures.Atlas;
            dropDown.normalBgSprite = CommonTextures.FieldNormal;
            dropDown.hoveredBgSprite = CommonTextures.FieldNormal;
            dropDown.AtlasForeground = TextureHelper.InGameAtlas;
            dropDown.normalFgSprite = "IconDownArrow";
            dropDown.hoveredFgSprite = "IconDownArrowHovered";
            dropDown.focusedFgSprite = "IconDownArrow";
            dropDown.disabledFgSprite = "IconDownArrowDisabled";
            dropDown.atlas = CommonTextures.Atlas;
            dropDown.listBackground = CommonTextures.FieldHovered;
            dropDown.itemHover = CommonTextures.FieldNormal;
            dropDown.itemHighlight = CommonTextures.FieldFocused;
            dropDown.itemHeight = 24;
            dropDown.listHeight = 700;
            dropDown.autoListWidth = true;
            dropDown.listPosition = UIDropDown.PopupListPosition.Below;
            dropDown.itemPadding = new RectOffset(14, 14, 4, 0);
            dropDown.clampListToScreen = false;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.color = new Color32(101, 101, 101, byte.MaxValue);
            dropDown.HoveredBgColor = new Color32(172, 172, 172, byte.MaxValue);
            dropDown.NormalFgColor = Color.black;
            dropDown.HoveredFgColor = new Color32(64, 64, 64, byte.MaxValue);
            dropDown.popupColor = new Color32(101, 101, 101, byte.MaxValue);
            dropDown.popupTextColor = Color.white;
            dropDown.textScale = 1f;
            dropDown.textFieldPadding = new RectOffset(14, 40, 7, 0);
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Right;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.triggerButton = dropDown;
            dropDown.size = size ?? new Vector2(230f, 20f);
        }

        public static void CustomSettingsStyle(this UITextField textField)
        {
            textField.atlas = CommonTextures.Atlas;
            textField.normalBgSprite = CommonTextures.FieldNormal;
            textField.selectionSprite = CommonTextures.Empty;
            textField.color = new Color32(101, 101, 101, byte.MaxValue);
        }
    }
}
