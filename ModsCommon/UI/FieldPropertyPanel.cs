using System;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class FieldPropertyPanel<ValueType, FieldType> : EditorPropertyPanel, IReusable where FieldType : UITextField<ValueType>
    {
        bool IReusable.InCache { get; set; }

        protected FieldType Field { get; set; }

        public float FieldWidth
        {
            get
            {
                return Field.width;
            }
            set
            {
                Field.width = value;
            }
        }

        public Color32 FieldTextColor
        {
            get
            {
                return Field.textColor;
            }
            set
            {
                Field.textColor = value;
            }
        }

        public bool SubmitOnFocusLost
        {
            get
            {
                return Field.submitOnFocusLost;
            }
            set
            {
                Field.submitOnFocusLost = value;
            }
        }

        public ValueType Value
        {
            get
            {
                return Field;
            }
            set
            {
                Field.Value = value;
            }
        }

        public string Format
        {
            set
            {
                Field.Format = value;
            }
        }

        public event Action<ValueType> OnValueChanged;

        public event Action OnResetValue;

        public FieldPropertyPanel()
        {
            Field = Content.AddUIComponent<FieldType>();
            Field.SetDefaultStyle();
            Field.name = "Field";
            Field.OnValueChanged += ValueChanged;
            Field.OnResetValue += ResetValue;
        }

        public void SimulateEnterValue(ValueType value)
        {
            Field.SimulateEnterValue(value);
        }

        private void ValueChanged(ValueType value)
        {
            OnValueChanged?.Invoke(value);
        }

        private void ResetValue()
        {
            OnResetValue?.Invoke();
        }

        public override void DeInit()
        {
            base.DeInit();
            OnValueChanged = null;
            Format = null;
            SubmitOnFocusLost = true;
        }

        public void Edit()
        {
            Field.Focus();
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Value}";
        }

        public static implicit operator ValueType(FieldPropertyPanel<ValueType, FieldType> property)
        {
            return property.Value;
        }
    }
}
