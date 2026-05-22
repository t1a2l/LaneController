using System;

namespace LaneController.ModsCommon.UI
{
    public abstract class ComparableFieldRangePropertyPanel<ValueType, FieldType> : EditorPropertyPanel, IReusable where ValueType : IComparable<ValueType> where FieldType : ComparableUITextField<ValueType>
    {
        private bool _allowInvert;

        bool IReusable.InCache { get; set; }

        protected FieldType FieldA { get; set; }

        protected FieldType FieldB { get; set; }

        public float FieldWidth
        {
            get
            {
                return (FieldA.width + FieldB.width) * 0.5f;
            }
            set
            {
                FieldA.width = value;
                FieldB.width = value;
            }
        }

        public bool SubmitOnFocusLost
        {
            get
            {
                if (FieldA.submitOnFocusLost)
                {
                    return FieldB.submitOnFocusLost;
                }
                return false;
            }
            set
            {
                FieldA.submitOnFocusLost = value;
                FieldB.submitOnFocusLost = value;
            }
        }

        public ValueType ValueA
        {
            get
            {
                return FieldA;
            }
            set
            {
                FieldA.Value = value;
                SetLimits();
            }
        }

        public ValueType ValueB
        {
            get
            {
                return FieldB;
            }
            set
            {
                FieldB.Value = value;
                SetLimits();
            }
        }

        public string Format
        {
            set
            {
                FieldA.Format = value;
                FieldB.Format = value;
            }
        }

        public ValueType MinValue
        {
            get
            {
                return FieldA.MinValue;
            }
            set
            {
                FieldA.MinValue = value;
                SetLimits();
            }
        }

        public ValueType MaxValue
        {
            get
            {
                return FieldB.MaxValue;
            }
            set
            {
                FieldB.MaxValue = value;
                SetLimits();
            }
        }

        public bool CheckMin
        {
            get
            {
                return FieldA.CheckMin;
            }
            set
            {
                FieldA.CheckMin = value;
                SetLimits();
            }
        }

        public bool CheckMax
        {
            get
            {
                return FieldB.CheckMax;
            }
            set
            {
                FieldB.CheckMax = value;
                SetLimits();
            }
        }

        public bool AllowInvert
        {
            get
            {
                return _allowInvert;
            }
            set
            {
                if (value != _allowInvert)
                {
                    _allowInvert = value;
                    SetLimits();
                }
            }
        }

        public bool UseWheel
        {
            get
            {
                if (FieldA.UseWheel)
                {
                    return FieldB.UseWheel;
                }
                return false;
            }
            set
            {
                FieldA.UseWheel = value;
                FieldB.UseWheel = value;
            }
        }

        public ValueType WheelStep
        {
            set
            {
                FieldA.WheelStep = value;
                FieldB.WheelStep = value;
            }
        }

        public bool WheelTip
        {
            set
            {
                FieldA.MouseTips = value;
                FieldB.MouseTips = value;
            }
        }

        public event Action<ValueType, ValueType> OnValueChanged;

        public ComparableFieldRangePropertyPanel()
        {
            FieldA = Content.AddUIComponent<FieldType>();
            FieldA.SetDefaultStyle();
            FieldA.name = "FieldA";
            FieldA.CheckMax = true;
            FieldB = Content.AddUIComponent<FieldType>();
            FieldB.SetDefaultStyle();
            FieldB.name = "FieldB";
            FieldB.CheckMin = true;
            FieldA.OnValueChanged += ValueAChanged;
            FieldB.OnValueChanged += ValueBChanged;
        }

        public override void DeInit()
        {
            base.DeInit();
            OnValueChanged = null;
            AllowInvert = false;
            UseWheel = false;
            WheelStep = default;
            WheelTip = false;
            SubmitOnFocusLost = true;
            Format = null;
            FieldA.SetDefault();
            FieldB.SetDefault();
        }

        public void SetValues(ValueType valueA, ValueType valueB)
        {
            FieldA.Value = valueA;
            FieldB.Value = valueB;
            SetLimits();
        }

        private void ValueAChanged(ValueType value)
        {
            SetLimits();
            OnValueChanged?.Invoke(value, FieldB.Value);
        }

        private void ValueBChanged(ValueType value)
        {
            SetLimits();
            OnValueChanged?.Invoke(FieldA.Value, value);
        }

        private void SetLimits()
        {
            if (!AllowInvert)
            {
                FieldA.MaxValue = FieldB.Value;
                FieldA.CheckMax = true;
                FieldB.MinValue = FieldA.Value;
                FieldB.CheckMin = true;
            }
            else
            {
                FieldA.MaxValue = FieldB.MaxValue;
                FieldA.CheckMax = FieldB.CheckMax;
                FieldB.MinValue = FieldA.MinValue;
                FieldB.CheckMin = FieldB.CheckMin;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}: from {ValueA} to {ValueB}";
        }
    }
}
