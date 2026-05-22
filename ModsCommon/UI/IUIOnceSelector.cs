using System;

namespace LaneController.ModsCommon.UI
{
    public interface IUIOnceSelector<ValueType> : IUISelector<ValueType>, IAutoLayoutPanel, IReusable
    {
        ValueType SelectedObject { get; set; }

        bool UseWheel { get; set; }

        bool WheelTip { set; }

        event Action<ValueType> OnSelectObjectChanged;
    }
}
