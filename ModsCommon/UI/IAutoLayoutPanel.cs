namespace LaneController.ModsCommon.UI
{
    public interface IAutoLayoutPanel
    {
        void StopLayout();

        void StartLayout(bool layoutNow = true);
    }
}
