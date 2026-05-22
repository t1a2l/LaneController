using System;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class MultyAtlasUIButton : CustomUIButton
    {
        private UITextureAtlas _atlasForeground;

        private UITextureAtlas _atlasBackground;

        private Color32? _normalBgColor;

        private Color32? _focusedBgColor;

        private Color32? _hoveredBgColor;

        private Color32? _pressedBgColor;

        private Color32? _disabledBgColor;

        private Color32? _normalFgColor;

        private Color32? _focusedFgColor;

        private Color32? _hoveredFgColor;

        private Color32? _pressedFgColor;

        private Color32? _disabledFgColor;

        public UITextureAtlas AtlasForeground
        {
            get
            {
                return _atlasForeground ?? atlas;
            }
            set
            {
                if (!Equals(value, _atlasForeground))
                {
                    _atlasForeground = value;
                    Invalidate();
                }
            }
        }

        public UITextureAtlas AtlasBackground
        {
            get
            {
                return _atlasBackground ?? atlas;
            }
            set
            {
                if (!Equals(value, _atlasBackground))
                {
                    _atlasBackground = value;
                    Invalidate();
                }
            }
        }

        public Color32 NormalBgColor
        {
            get
            {
                return _normalBgColor ?? color;
            }
            set
            {
                _normalBgColor = value;
                Invalidate();
            }
        }

        public Color32 FocusedBgColor
        {
            get
            {
                return _focusedBgColor ?? color;
            }
            set
            {
                _focusedBgColor = value;
                Invalidate();
            }
        }

        public Color32 HoveredBgColor
        {
            get
            {
                return _hoveredBgColor ?? color;
            }
            set
            {
                _hoveredBgColor = value;
                Invalidate();
            }
        }

        public Color32 PressedBgColor
        {
            get
            {
                return _pressedBgColor ?? color;
            }
            set
            {
                _pressedBgColor = value;
                Invalidate();
            }
        }

        public Color32 DisabledBgColor
        {
            get
            {
                return _disabledBgColor ?? disabledColor;
            }
            set
            {
                _disabledBgColor = value;
                Invalidate();
            }
        }

        public Color32 NormalFgColor
        {
            get
            {
                return _normalFgColor ?? color;
            }
            set
            {
                _normalFgColor = value;
                Invalidate();
            }
        }

        public Color32 FocusedFgColor
        {
            get
            {
                return _focusedFgColor ?? color;
            }
            set
            {
                _focusedFgColor = value;
                Invalidate();
            }
        }

        public Color32 HoveredFgColor
        {
            get
            {
                return _hoveredFgColor ?? color;
            }
            set
            {
                _hoveredFgColor = value;
                Invalidate();
            }
        }

        public Color32 PressedFgColor
        {
            get
            {
                return _pressedFgColor ?? color;
            }
            set
            {
                _pressedFgColor = value;
                Invalidate();
            }
        }

        public Color32 DisabledFgColor
        {
            get
            {
                return _disabledFgColor ?? disabledColor;
            }
            set
            {
                _disabledFgColor = value;
                Invalidate();
            }
        }

        protected UIRenderData BgRenderData { get; set; }

        protected UIRenderData FgRenderData { get; set; }

        protected UIRenderData TextRenderData { get; set; }

        public override void OnDisable()
        {
            BgRenderData = null;
            FgRenderData = null;
            TextRenderData = null;
            base.OnDisable();
        }

        protected override void OnRebuildRenderData()
        {
            if (BgRenderData == null)
            {
                BgRenderData = UIRenderData.Obtain();
                m_RenderData.Add(BgRenderData);
            }
            else
            {
                BgRenderData.Clear();
            }
            if (FgRenderData == null)
            {
                FgRenderData = UIRenderData.Obtain();
                m_RenderData.Add(FgRenderData);
            }
            else
            {
                FgRenderData.Clear();
            }
            if (TextRenderData == null)
            {
                TextRenderData = UIRenderData.Obtain();
                m_RenderData.Add(TextRenderData);
            }
            else
            {
                TextRenderData.Clear();
            }
            UITextureAtlas uITextureAtlas = AtlasBackground;
            if (uITextureAtlas is not null)
            {
                UITextureAtlas uITextureAtlas2 = AtlasForeground;
                if (uITextureAtlas2 is not null)
                {
                    BgRenderData.material = uITextureAtlas.material;
                    FgRenderData.material = uITextureAtlas2.material;
                    TextRenderData.material = uITextureAtlas.material;
                    RenderBackground();
                    RenderForeground();
                    RenderText();
                }
            }
        }

        private void RenderText()
        {
            if (m_Font == null || !m_Font.isValid)
            {
                return;
            }
            using UIFontRenderer uIFontRenderer = ObtainTextRenderer();
            if (uIFontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer)
            {
                dynamicFontRenderer.spriteAtlas = AtlasBackground;
                dynamicFontRenderer.spriteBuffer = BgRenderData;
            }
            uIFontRenderer.Render(m_Text, TextRenderData);
        }

        private UIFontRenderer ObtainTextRenderer()
        {
            Vector2 vector = size - new Vector2(textPadding.horizontal, textPadding.vertical);
            Vector2 maxSize = autoSize ? Vector2.one * 2.1474836E+09f : vector;
            float num = PixelsToUnits();
            Vector3 vectorOffset = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(textPadding.left, -textPadding.top)) * num;
            GetTextScaleMultiplier();
            UIFontRenderer uIFontRenderer = font.ObtainRenderer();
            uIFontRenderer.wordWrap = wordWrap;
            uIFontRenderer.multiLine = true;
            uIFontRenderer.maxSize = maxSize;
            uIFontRenderer.pixelRatio = num;
            uIFontRenderer.textScale = textScale;
            uIFontRenderer.vectorOffset = vectorOffset;
            uIFontRenderer.textAlign = textHorizontalAlignment;
            uIFontRenderer.processMarkup = processMarkup;
            uIFontRenderer.defaultColor = ApplyOpacity(GetTextColorForState());
            uIFontRenderer.bottomColor = null;
            uIFontRenderer.overrideMarkupColors = false;
            uIFontRenderer.opacity = CalculateOpacity();
            uIFontRenderer.shadow = useDropShadow;
            uIFontRenderer.shadowColor = dropShadowColor;
            uIFontRenderer.shadowOffset = dropShadowOffset;
            uIFontRenderer.outline = useOutline;
            uIFontRenderer.outlineSize = outlineSize;
            uIFontRenderer.outlineColor = outlineColor;
            if (!autoSize && m_TextVerticalAlign != UIVerticalAlignment.Top)
            {
                uIFontRenderer.vectorOffset = GetVertAlignOffset(uIFontRenderer);
            }
            return uIFontRenderer;
        }

        private Color32 GetTextColorForState()
        {
            if (!isEnabled)
            {
                return disabledTextColor;
            }
            return state switch
            {
                ButtonState.Normal => textColor,
                ButtonState.Focused => focusedTextColor,
                ButtonState.Hovered => hoveredTextColor,
                ButtonState.Pressed => pressedTextColor,
                ButtonState.Disabled => disabledTextColor,
                _ => Color.white,
            };
        }

        private Color32 GetBackgroundColor()
        {
            return state switch
            {
                ButtonState.Focused => FocusedBgColor,
                ButtonState.Hovered => HoveredBgColor,
                ButtonState.Pressed => PressedBgColor,
                ButtonState.Disabled => DisabledBgColor,
                _ => NormalBgColor,
            };
        }

        private Color32 GetForegroundColor()
        {
            return state switch
            {
                ButtonState.Focused => FocusedFgColor,
                ButtonState.Hovered => HoveredFgColor,
                ButtonState.Pressed => PressedFgColor,
                ButtonState.Disabled => DisabledFgColor,
                _ => NormalFgColor,
            };
        }

        private Vector3 GetVertAlignOffset(UIFontRenderer fontRenderer)
        {
            float num = PixelsToUnits();
            Vector2 vector = fontRenderer.MeasureString(m_Text) * num;
            Vector3 vectorOffset = fontRenderer.vectorOffset;
            float num2 = (height - textPadding.vertical) * num;
            if (vector.y >= num2)
            {
                return vectorOffset;
            }
            switch (m_TextVerticalAlign)
            {
                case UIVerticalAlignment.Middle:
                    vectorOffset.y -= (num2 - vector.y) * 0.5f;
                    break;
                case UIVerticalAlignment.Bottom:
                    vectorOffset.y -= num2 - vector.y;
                    break;
            }
            return vectorOffset;
        }

        protected override UITextureAtlas.SpriteInfo GetBackgroundSprite()
        {
            UITextureAtlas uITextureAtlas = AtlasBackground;
            if (uITextureAtlas is null)
            {
                return null;
            }
            return state switch
            {
                ButtonState.Normal => uITextureAtlas[normalBgSprite],
                ButtonState.Focused => uITextureAtlas[focusedBgSprite],
                ButtonState.Hovered => uITextureAtlas[hoveredBgSprite],
                ButtonState.Pressed => uITextureAtlas[pressedBgSprite],
                ButtonState.Disabled => uITextureAtlas[disabledBgSprite],
                _ => null,
            } ?? uITextureAtlas[normalBgSprite];
        }

        protected override UITextureAtlas.SpriteInfo GetForegroundSprite()
        {
            UITextureAtlas uITextureAtlas = AtlasForeground;
            if (uITextureAtlas is null)
            {
                return null;
            }
            return state switch
            {
                ButtonState.Normal => uITextureAtlas[normalFgSprite],
                ButtonState.Focused => uITextureAtlas[focusedFgSprite],
                ButtonState.Hovered => uITextureAtlas[hoveredFgSprite],
                ButtonState.Pressed => uITextureAtlas[pressedFgSprite],
                ButtonState.Disabled => uITextureAtlas[disabledFgSprite],
                _ => null,
            } ?? uITextureAtlas[normalFgSprite];
        }

        protected override void RenderBackground()
        {
            UITextureAtlas.SpriteInfo backgroundSprite = GetBackgroundSprite();
            if (backgroundSprite is not null)
            {
                Color32 color = ApplyOpacity(GetBackgroundColor());
                RenderOptions options = new()
                {
                    _atlas = AtlasBackground,
                    _color = color,
                    _fillAmount = 1f,
                    _flip = UISpriteFlip.None,
                    _offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
                    _pixelsToUnits = PixelsToUnits(),
                    _size = size,
                    _spriteInfo = backgroundSprite
                };
                if (backgroundSprite.isSliced)
                {
                    Render.RenderSlicedSprite(BgRenderData, options);
                }
                else
                {
                    Render.RenderSprite(BgRenderData, options);
                }
            }
        }

        protected override void RenderForeground()
        {
            UITextureAtlas.SpriteInfo foregroundSprite = GetForegroundSprite();
            if (foregroundSprite is not null)
            {
                Vector2 foregroundRenderSize = GetForegroundRenderSize(foregroundSprite);
                Vector2 foregroundRenderOffset = GetForegroundRenderOffset(foregroundRenderSize);
                Color32 color = ApplyOpacity(GetForegroundColor());
                RenderOptions options = new()
                {
                    _atlas = AtlasForeground,
                    _color = color,
                    _fillAmount = 1f,
                    _flip = UISpriteFlip.None,
                    _offset = foregroundRenderOffset,
                    _pixelsToUnits = PixelsToUnits(),
                    _size = foregroundRenderSize,
                    _spriteInfo = foregroundSprite
                };
                if (foregroundSprite.isSliced)
                {
                    Render.RenderSlicedSprite(FgRenderData, options);
                }
                else
                {
                    Render.RenderSprite(FgRenderData, options);
                }
            }
        }
    }
}
