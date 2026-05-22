using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class CustomUIScrollablePanel : UIScrollablePanel
    {
        private Vector3 positionBefore;

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
    }
}
