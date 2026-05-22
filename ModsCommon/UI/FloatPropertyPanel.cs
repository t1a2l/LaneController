namespace LaneController.ModsCommon.UI
{
    public class FloatPropertyPanel : ComparableFieldPropertyPanel<float, FloatUITextField>
    {
        public virtual void Init(string name = null)
        {
            Init((float?)null);
            Text = name;
            bool flag = UseReset = true;
            bool mouseTips = UseWheel = flag;
            MouseTips = mouseTips;
            WheelStep = 1f;
            Refresh();
        }
    }
}
