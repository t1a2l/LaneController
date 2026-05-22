using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class ComparableUITextField<ValueType> : UITextField<ValueType> where ValueType : IComparable<ValueType>
    {
        protected enum WheelMode
        {
            Normal,
            Low,
            High
        }

        public ValueType MinValue { get; set; }

        public ValueType MaxValue { get; set; }

        public bool CheckMax { get; set; }

        public bool CheckMin { get; set; }

        public bool CyclicalValue { get; set; }

        public bool Limited
        {
            get
            {
                if (CheckMax)
                {
                    return CheckMin;
                }
                return false;
            }
        }

        public bool UseWheel { get; set; }

        public ValueType WheelStep { get; set; }

        public bool MouseTips
        {
            set
            {
                tooltip = "";
                if (value)
                {
                    if (UseWheel)
                    {
                        tooltip += CommonLocalize.FieldPanel_ScrollWheel;
                    }
                    if (UseReset)
                    {
                        tooltip += "press delete to reset";
                    }
                }
            }
        }

        public bool CanWheel { get; set; }

        protected override void ValueChanged(ValueType value, bool callEvent = true)
        {
            if (CheckMin)
            {
                ValueType minValue = MinValue;
                if (value.CompareTo(minValue) < 0)
                {
                    value = MinValue;
                }
            }
            if (CheckMax)
            {
                ValueType maxValue = MaxValue;
                if (value.CompareTo(maxValue) > 0)
                {
                    value = MaxValue;
                }
            }
            base.ValueChanged(value, callEvent);
        }

        public override void DeInit()
        {
            base.DeInit();
            SetDefault();
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            base.OnMouseMove(p);
            CanWheel = true;
        }

        protected override void OnMouseLeave(UIMouseEventParameter p)
        {
            base.OnMouseLeave(p);
            CanWheel = false;
        }

        protected sealed override void OnMouseWheel(UIMouseEventParameter p)
        {
            m_TooltipShowing = true;
            tooltipBox.Hide();
            if (UseWheel && (CanWheel || Time.realtimeSinceStartup - m_HoveringStartTime >= 1f))
            {
                WheelMode mode = Utility.ShiftIsPressed ? WheelMode.High : Utility.CtrlIsPressed ? WheelMode.Low : WheelMode.Normal;
                if (p.wheelDelta < 0f)
                {
                    ValueChanged(Decrement(Limited && CyclicalValue && Value.CompareTo(MinValue) == 0 ? MaxValue : Value, WheelStep, mode));
                }
                else
                {
                    ValueChanged(Increment(Limited && CyclicalValue && Value.CompareTo(MaxValue) == 0 ? MinValue : Value, WheelStep, mode));
                }
                p.Use();
            }
        }

        protected override void OnTooltipEnter(UIMouseEventParameter p)
        {
            base.OnTooltipEnter(p);
            if (!isEnabled)
            {
                m_TooltipShowing = true;
            }
        }

        protected abstract ValueType Increment(ValueType value, ValueType step, WheelMode mode);

        protected abstract ValueType Decrement(ValueType value, ValueType step, WheelMode mode);

        public ComparableUITextField()
        {
            SetDefault();
        }

        public void SetDefault()
        {
            MinValue = default;
            MaxValue = default;
            CheckMin = false;
            CheckMax = false;
            CyclicalValue = false;
            UseWheel = false;
            MouseTips = false;
            WheelStep = default;
        }
    }
}
