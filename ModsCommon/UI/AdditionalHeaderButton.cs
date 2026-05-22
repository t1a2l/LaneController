using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class AdditionalHeaderButton : HeaderPopupButton<AdditionalHeaderButton.AdditionalPopup>
    {
        public class AdditionalPopup : PopupPanel
        {
            protected override Color32 Background => new(64, 64, 64, byte.MaxValue);
        }
    }
}
