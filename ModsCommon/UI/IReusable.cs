namespace LaneController.ModsCommon.UI
{
    public interface IReusable
    {
        bool InCache { get; set; }

        void DeInit();
    }
}
