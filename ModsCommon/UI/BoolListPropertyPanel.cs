namespace LaneController.ModsCommon.UI
{
    public class BoolListPropertyPanel : ListOncePropertyPanel<bool, BoolListPropertyPanel.BoolSegmented>
    {
        public class BoolSegmented : UIOnceSegmented<bool>
        {
        }

        protected override bool AllowNull => false;

        protected override bool IsEqual(bool first, bool second)
        {
            return first == second;
        }

        public override void Init()
        {
            Init(CommonLocalize.MessageBox_No, CommonLocalize.MessageBox_Yes);
        }

        public void Init(string falseLabel, string trueLabel, bool invert = true)
        {
            Init(null);
            Selector.StopLayout();
            if (invert)
            {
                Selector.AddItem(item: true, trueLabel);
                Selector.AddItem(item: false, falseLabel);
            }
            else
            {
                Selector.AddItem(item: false, falseLabel);
                Selector.AddItem(item: true, trueLabel);
            }
            Selector.StartLayout();
        }
    }
}
