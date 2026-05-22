using System;

namespace LaneController.ModsCommon.UI
{
    public abstract class ComparableFieldPropertyPanel<ValueType, FieldType> : FieldPropertyPanel<ValueType, FieldType> where ValueType : IComparable<ValueType> where FieldType : ComparableUITextField<ValueType>
    {
        public ValueType MinValue
        {
            get
            {
                return Field.MinValue;
            }
            set
            {
                Field.MinValue = value;
            }
        }

        public ValueType MaxValue
        {
            get
            {
                return Field.MaxValue;
            }
            set
            {
                Field.MaxValue = value;
            }
        }

        public bool CheckMin
        {
            get
            {
                return Field.CheckMin;
            }
            set
            {
                Field.CheckMin = value;
            }
        }

        public bool CheckMax
        {
            get
            {
                return Field.CheckMax;
            }
            set
            {
                Field.CheckMax = value;
            }
        }

        public bool CyclicalValue
        {
            get
            {
                return Field.CyclicalValue;
            }
            set
            {
                Field.CyclicalValue = value;
            }
        }

        public bool UseWheel
        {
            get
            {
                return Field.UseWheel;
            }
            set
            {
                Field.UseWheel = value;
            }
        }

        public bool UseReset
        {
            get
            {
                return Field.UseReset;
            }
            set
            {
                Field.UseReset = value;
            }
        }

        public ValueType WheelStep
        {
            get
            {
                return Field.WheelStep;
            }
            set
            {
                Field.WheelStep = value;
            }
        }

        public bool MouseTips
        {
            set
            {
                Field.MouseTips = value;
            }
        }

        public ComparableFieldPropertyPanel()
        {
            Field.SetDefault();
        }

        public override void DeInit()
        {
            base.DeInit();
            Field.SetDefault();
        }
    }
}
