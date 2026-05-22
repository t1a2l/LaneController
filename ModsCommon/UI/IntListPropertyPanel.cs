namespace LaneController.ModsCommon.UI
{
    public class IntListPropertyPanel : ListPropertyPanel<int, IntListPropertyPanel.IntSegmented>
    {
        public class IntSegmented : UIOnceSegmented<int>
        {
        }

        protected override bool AllowNull => false;

        protected override bool IsEqual(int first, int second)
        {
            return first == second;
        }

        public override void Init()
        {
            Init(2);
        }

        public void Init(int count)
        {
            Init(null);
            Selector.StopLayout();
            for (int i = 1; i <= count; i++)
            {
                Selector.AddItem(i, i.ToString());
            }
            Selector.StartLayout();
        }
    }
}
