using System;

namespace LaneController.ModsCommon.UI
{
    public interface IValueChanger<TypeValue> : IReusable
    {
        TypeValue Value { get; set; }

        string Format { set; }

        event Action<TypeValue> OnValueChanged;
    }
}
