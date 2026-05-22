using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class PanelHeaderContent : BaseHeaderContent
    {
        protected override Color32 ButtonHoveredColor => new(112, 112, 112, byte.MaxValue);

        protected override Color32 ButtonPressedColor => new(144, 144, 144, byte.MaxValue);

        protected override Color32 AdditionalButtonHoveredColor => new(112, 112, 112, byte.MaxValue);

        protected override Color32 AdditionalButtonPressedColor => new(144, 144, 144, byte.MaxValue);

        protected override Color32 IconNormalColor => Color.white;

        protected override Color32 IconHoverColor => Color.white;

        protected override Color32 IconPressedColor => Color.white;

        protected override Color32 IconDisabledColor => new(144, 144, 144, byte.MaxValue);
    }
}
