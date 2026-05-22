using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class HeaderButton : MultyAtlasUIButton, IReusable
    {
        bool IReusable.InCache { get; set; }

        public static int Size => IconSize + 2 * IconPadding;

        public static int IconSize => 25;

        public static int IconPadding => 2;

        public HeaderButton()
        {
            AtlasBackground = CommonTextures.Atlas;
            hoveredBgSprite = pressedBgSprite = focusedBgSprite = CommonTextures.HeaderHover;
            size = new Vector2(Size, Size);
            clipChildren = true;
            textPadding = new RectOffset(IconSize + 5, 5, 5, 0);
            textScale = 0.8f;
            textHorizontalAlignment = UIHorizontalAlignment.Left;
            minimumSize = size;
            foregroundSpriteMode = UIForegroundSpriteMode.Fill;
        }

        public void SetIcon(UITextureAtlas atlas, string sprite)
        {
            AtlasForeground = atlas ?? TextureHelper.InGameAtlas;
            normalFgSprite = sprite;
            hoveredFgSprite = sprite;
            pressedFgSprite = sprite;
        }

        public override void Update()
        {
            base.Update();
            if (state == ButtonState.Focused)
            {
                state = ButtonState.Normal;
            }
        }

        public virtual void DeInit()
        {
            SetIcon(null, string.Empty);
        }
    }
}
