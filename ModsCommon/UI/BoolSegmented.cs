namespace LaneController.ModsCommon.UI
{
    public class BoolSegmented : UIOnceSegmented<bool>
    {
        public BoolSegmented()
        {
            IsEqualDelegate = (x, y) => x == y;
        }
    }
}
