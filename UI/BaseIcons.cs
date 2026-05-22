using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.UI
{
    public class BaseIcons : UIButton
    {
        private static float Border => 1f;

        public Color32 BorderColor
        {
            set
            {
                base.color = value;
            }
        }

        public BaseIcons()
        {
            isInteractive = false;
            base.color = new Color32(29, 58, 77, byte.MaxValue);
            base.relativePosition = new Vector3(0f, 0f);
            base.width = 50f;
            base.height = 25f;
        }
    }
}
