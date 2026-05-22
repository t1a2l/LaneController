namespace LaneController.ModsCommon.UI
{
    public class UIAutoLayoutScrollablePanel : CustomUIScrollablePanel, IAutoLayoutPanel
    {
        public UIAutoLayoutScrollablePanel()
        {
            m_AutoLayout = true;
        }

        public virtual void StopLayout()
        {
            m_AutoLayout = false;
        }

        public virtual void StartLayout(bool layoutNow = true)
        {
            m_AutoLayout = true;
            if (layoutNow)
            {
                Reset();
            }
        }
    }
}
