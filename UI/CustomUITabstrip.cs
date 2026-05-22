using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using UnityEngine;

namespace LaneController.UI
{
    public class CustomUITabstrip : UITabstrip
    {
        public CustomUITabstrip()
        {
            base.atlas = TextureUtil.InGameAtlas;
            base.backgroundSprite = "";
        }

        public void AddTab(string name, float textScale = 0.85f)
        {
            UIButton uIButton = base.AddTab(name);
            uIButton.autoSize = true;
            uIButton.textPadding = new RectOffset(5, 5, 2, 2);
            uIButton.textScale = textScale;
            uIButton.textHorizontalAlignment = UIHorizontalAlignment.Center;
            uIButton.verticalAlignment = UIVerticalAlignment.Middle;
            uIButton.atlas = TextureUtil.InGameAtlas;
            uIButton.normalBgSprite = "SubBarButtonBase";
            uIButton.disabledBgSprite = "SubBarButtonBaseDisabled";
            uIButton.focusedBgSprite = "SubBarButtonBaseFocused";
            uIButton.hoveredBgSprite = "SubBarButtonBaseHovered";
            uIButton.pressedBgSprite = "SubBarButtonBasePressed";
            uIButton.Invalidate();
            FitChildrenVertically();
        }

        protected override void OnSizeChanged()
        {
            FitChildrenVertically();
        }
    }
}
