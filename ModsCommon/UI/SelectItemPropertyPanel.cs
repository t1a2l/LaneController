namespace LaneController.ModsCommon.UI
{
    public abstract class SelectItemPropertyPanel<Type, PanelType> : SelectPropertyPanel<Type, PanelType>, IReusable where PanelType : SelectItemPropertyPanel<Type, PanelType>
    {
        private Type _value;

        public override Type Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ValueChanged();
            }
        }
    }
}
