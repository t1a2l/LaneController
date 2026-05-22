using System;

namespace LaneController.ModsCommon.UI
{
    public class IntUITextField : ComparableUITextField<int>
    {
        protected override int Decrement(int value, int step, WheelMode mode)
        {
            if (value != int.MinValue)
            {
                return value - GetStep(step, mode);
            }
            return value;
        }

        protected override int Increment(int value, int step, WheelMode mode)
        {
            if (value != int.MaxValue)
            {
                return value + GetStep(step, mode);
            }
            return value;
        }

        private int GetStep(int step, WheelMode mode)
        {
            return mode switch
            {
                WheelMode.Low => Math.Max(step / 10, 1),
                WheelMode.High => step * 10,
                _ => step,
            };
        }
    }
}
