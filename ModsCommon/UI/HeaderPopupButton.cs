namespace LaneController.ModsCommon.UI
{
    public abstract class HeaderPopupButton<PopupType> : BaseHeaderPopupButton<PopupType> where PopupType : PopupPanel
    {
        protected override void OnPopupOpened()
        {
            base.OnPopupOpened();
            Popup.Refresh();
        }
    }
}
