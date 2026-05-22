using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class CustomUIDropDown : UIDropDown
    {
        private Vector3 positionBefore;

        protected UIButton.ButtonState m_State;

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

        private Color32? _focusedTextColor;

        private Color32? _hoveredTextColor;

        private Color32? _pressedTextColor;

        public UIButton.ButtonState State
        {
            get
            {
                return m_State;
            }
            set
            {
                if (value != m_State)
                {
                    OnButtonStateChanged(value);
                    Invalidate();
                }
            }
        }

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

        public UITextureAtlas Atlas
        {
            set
            {
                _atlasForeground = value;
                _atlasBackground = value;
                atlas = value;
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

        public Color32 FocusedTextColor
        {
            get
            {
                return _focusedTextColor ?? textColor;
            }
            set
            {
                _focusedTextColor = value;
                Invalidate();
            }
        }

        public Color32 HoveredTextColor
        {
            get
            {
                return _hoveredTextColor ?? textColor;
            }
            set
            {
                _hoveredTextColor = value;
                Invalidate();
            }
        }

        public Color32 PressedTextColor
        {
            get
            {
                return _pressedTextColor ?? textColor;
            }
            set
            {
                _pressedTextColor = value;
                Invalidate();
            }
        }

        protected UIRenderData BgRenderData { get; set; }

        protected UIRenderData FgRenderData { get; set; }

        protected UIRenderData TextRenderData { get; set; }

        public event PropertyChangedEventHandler<UIButton.ButtonState> EventButtonStateChanged;

        public override void ResetLayout()
        {
            positionBefore = relativePosition;
        }

        public override void PerformLayout()
        {
            if ((double)(relativePosition - positionBefore).sqrMagnitude > 0.001)
            {
                relativePosition = positionBefore;
            }
        }

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

        private void RenderText()
        {
            if (m_Font == null || !m_Font.isValid || selectedIndex < 0 || selectedIndex >= items.Length)
            {
                return;
            }
            using UIFontRenderer uIFontRenderer = ObtainTextRenderer();
            if (uIFontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer)
            {
                dynamicFontRenderer.spriteAtlas = AtlasBackground;
                dynamicFontRenderer.spriteBuffer = BgRenderData;
            }
            uIFontRenderer.Render(items[selectedIndex], TextRenderData);
        }

        private UIFontRenderer ObtainTextRenderer()
        {
            float num = PixelsToUnits();
            Vector2 maxSize = new(size.x - textFieldPadding.horizontal, size.y - textFieldPadding.vertical);
            Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            Vector3 vectorOffset = new Vector3(vector.x + textFieldPadding.left, vector.y - textFieldPadding.top, 0f) * num;
            UIFontRenderer uIFontRenderer = font.ObtainRenderer();
            uIFontRenderer.wordWrap = false;
            uIFontRenderer.maxSize = maxSize;
            uIFontRenderer.pixelRatio = num;
            uIFontRenderer.textScale = textScale;
            uIFontRenderer.characterSpacing = characterSpacing;
            uIFontRenderer.vectorOffset = vectorOffset;
            uIFontRenderer.multiLine = false;
            uIFontRenderer.textAlign = UIHorizontalAlignment.Left;
            uIFontRenderer.processMarkup = processMarkup;
            uIFontRenderer.colorizeSprites = colorizeSprites;
            uIFontRenderer.defaultColor = ApplyOpacity(GetTextColorForState());
            uIFontRenderer.bottomColor = useGradient ? new Color32?(bottomColor) : null;
            uIFontRenderer.overrideMarkupColors = false;
            uIFontRenderer.opacity = CalculateOpacity();
            uIFontRenderer.outline = useOutline;
            uIFontRenderer.outlineSize = outlineSize;
            uIFontRenderer.outlineColor = outlineColor;
            uIFontRenderer.shadow = useDropShadow;
            uIFontRenderer.shadowColor = dropShadowColor;
            uIFontRenderer.shadowOffset = dropShadowOffset;
            return uIFontRenderer;
        }

        protected virtual void OnButtonStateChanged(UIButton.ButtonState value)
        {
            if (isEnabled || value == UIButton.ButtonState.Disabled)
            {
                m_State = value;
                EventButtonStateChanged?.Invoke(this, value);
                Invoke("OnButtonStateChanged", value);
                Invalidate();
            }
        }

        protected override void OnEnterFocus(UIFocusEventParameter p)
        {
            if (State != UIButton.ButtonState.Pressed)
            {
                State = UIButton.ButtonState.Focused;
            }
            base.OnEnterFocus(p);
        }

        protected override void OnLeaveFocus(UIFocusEventParameter p)
        {
            State = containsMouse ? UIButton.ButtonState.Hovered : UIButton.ButtonState.Normal;
            base.OnLeaveFocus(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (State != UIButton.ButtonState.Focused)
            {
                State = UIButton.ButtonState.Pressed;
            }
            base.OnMouseDown(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            if (m_IsMouseHovering)
            {
                if (containsFocus)
                {
                    State = UIButton.ButtonState.Focused;
                }
                else
                {
                    State = UIButton.ButtonState.Hovered;
                }
            }
            else if (hasFocus)
            {
                State = UIButton.ButtonState.Focused;
            }
            else
            {
                State = UIButton.ButtonState.Normal;
            }
            base.OnMouseUp(p);
        }

        protected override void OnMouseEnter(UIMouseEventParameter p)
        {
            if (State != UIButton.ButtonState.Focused)
            {
                State = UIButton.ButtonState.Hovered;
            }
            base.OnMouseEnter(p);
        }

        protected override void OnMouseLeave(UIMouseEventParameter p)
        {
            if (containsFocus)
            {
                State = UIButton.ButtonState.Focused;
            }
            else
            {
                State = UIButton.ButtonState.Normal;
            }
            base.OnMouseLeave(p);
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            listWidth = (int)width;
            UIComponent uIComponent = triggerButton;
            if (uIComponent is not null && uIComponent != this)
            {
                uIComponent.size = size;
            }
        }

        protected override void OnIsEnabledChanged()
        {
            if (!isEnabled)
            {
                State = UIButton.ButtonState.Disabled;
            }
            else
            {
                State = UIButton.ButtonState.Normal;
            }
            base.OnIsEnabledChanged();
        }

        private Color32 GetTextColorForState()
        {
            if (!isEnabled)
            {
                return disabledTextColor;
            }
            return State switch
            {
                UIButton.ButtonState.Normal => textColor,
                UIButton.ButtonState.Focused => FocusedTextColor,
                UIButton.ButtonState.Hovered => HoveredTextColor,
                UIButton.ButtonState.Pressed => PressedTextColor,
                UIButton.ButtonState.Disabled => disabledTextColor,
                _ => Color.white,
            };
        }

        private Color32 GetBackgroundColor()
        {
            return State switch
            {
                UIButton.ButtonState.Focused => FocusedBgColor,
                UIButton.ButtonState.Hovered => HoveredBgColor,
                UIButton.ButtonState.Pressed => PressedBgColor,
                UIButton.ButtonState.Disabled => DisabledBgColor,
                _ => NormalBgColor,
            };
        }

        private Color32 GetForegroundColor()
        {
            return State switch
            {
                UIButton.ButtonState.Focused => FocusedFgColor,
                UIButton.ButtonState.Hovered => HoveredFgColor,
                UIButton.ButtonState.Pressed => PressedFgColor,
                UIButton.ButtonState.Disabled => DisabledFgColor,
                _ => NormalFgColor,
            };
        }

        protected override UITextureAtlas.SpriteInfo GetBackgroundSprite()
        {
            UITextureAtlas uITextureAtlas = AtlasBackground;
            if (uITextureAtlas is null)
            {
                return null;
            }
            return State switch
            {
                UIButton.ButtonState.Normal => uITextureAtlas[normalBgSprite],
                UIButton.ButtonState.Focused => uITextureAtlas[focusedBgSprite],
                UIButton.ButtonState.Hovered => uITextureAtlas[hoveredBgSprite],
                UIButton.ButtonState.Disabled => uITextureAtlas[disabledBgSprite],
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
            return State switch
            {
                UIButton.ButtonState.Normal => uITextureAtlas[normalFgSprite],
                UIButton.ButtonState.Focused => uITextureAtlas[focusedFgSprite],
                UIButton.ButtonState.Hovered => uITextureAtlas[hoveredFgSprite],
                UIButton.ButtonState.Disabled => uITextureAtlas[disabledFgSprite],
                _ => null,
            } ?? uITextureAtlas[normalFgSprite];
        }
    }
}
