using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public interface IHeaderButtonInfo
    {
        HeaderButton Button { get; }

        HeaderButtonState State { get; }

        bool Visible { get; set; }

        event MouseEventHandler ClickedEvent;

        void AddButton(UIComponent parent, bool showText);

        void RemoveButton();
    }
}
