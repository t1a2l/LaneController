using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class CustomUIButton : UIButton
    {
        private Vector3 positionBefore;

        public Vector2 MinimumAutoSize
        {
            get
            {
                Vector2 result = minimumSize;
                if (m_Font != null && m_Font.isValid && !string.IsNullOrEmpty(m_Text))
                {
                    using UIFontRenderer uIFontRenderer = ObtainTextRenderer();
                    Vector2 vector = uIFontRenderer.MeasureString(m_Text);
                    result = new Vector2(vector.x + textPadding.horizontal, vector.y + textPadding.vertical);
                }
                return result;
            }
        }

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

        private UIFontRenderer ObtainTextRenderer()
        {
            UIFontRenderer uIFontRenderer = font.ObtainRenderer();
            uIFontRenderer.wordWrap = wordWrap;
            uIFontRenderer.multiLine = true;
            uIFontRenderer.maxSize = size - new Vector2(textPadding.horizontal, textPadding.vertical);
            uIFontRenderer.pixelRatio = PixelsToUnits();
            uIFontRenderer.textScale = textScale;
            uIFontRenderer.vectorOffset = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(textPadding.left, -textPadding.top)) * PixelsToUnits();
            uIFontRenderer.textAlign = textHorizontalAlignment;
            uIFontRenderer.processMarkup = processMarkup;
            uIFontRenderer.overrideMarkupColors = false;
            uIFontRenderer.opacity = CalculateOpacity();
            uIFontRenderer.shadow = useDropShadow;
            uIFontRenderer.shadowOffset = dropShadowOffset;
            uIFontRenderer.outline = useOutline;
            uIFontRenderer.outlineSize = outlineSize;
            return uIFontRenderer;
        }
    }
}
