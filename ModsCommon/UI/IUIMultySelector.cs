using System;
using System.Collections.Generic;

namespace LaneController.ModsCommon.UI
{
    public interface IUIMultySelector<ValueType> : IUISelector<ValueType>, IAutoLayoutPanel, IReusable
    {
        List<ValueType> SelectedObjects { get; set; }

        event Action<List<ValueType>> OnSelectObjectsChanged;
    }
}
