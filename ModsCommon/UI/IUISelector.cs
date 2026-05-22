using System;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public interface IUISelector<ValueType> : IAutoLayoutPanel, IReusable
    {
        Func<ValueType, ValueType, bool> IsEqualDelegate { get; set; }

        void AddItem(ValueType item, string label = null);

        void Clear();

        void SetDefaultStyle(Vector2? size = null);
    }
}
