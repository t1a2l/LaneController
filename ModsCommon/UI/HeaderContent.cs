using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class HeaderContent : BaseHeaderContent
    {
        protected override Color32 ButtonHoveredColor => new(32, 32, 32, byte.MaxValue);

        protected override Color32 ButtonPressedColor => Color.black;

        protected override Color32 AdditionalButtonHoveredColor => new(32, 32, 32, byte.MaxValue);

        protected override Color32 AdditionalButtonPressedColor => Color.black;

        protected override Color32 IconNormalColor => Color.white;

        protected override Color32 IconHoverColor => Color.white;

        protected override Color32 IconPressedColor => new(224, 224, 224, byte.MaxValue);

        protected override Color32 IconDisabledColor => new(144, 144, 144, byte.MaxValue);
    }
}
