using System;

namespace LaneController.ModsCommon.UI
{
    public class ByteUITextField : ComparableUITextField<byte>
    {
        protected override byte Decrement(byte value, byte step, WheelMode mode)
        {
            step = GetStep(step, mode);
            if (value >= step)
            {
                return (byte)(value - step);
            }
            return 0;
        }

        protected override byte Increment(byte value, byte step, WheelMode mode)
        {
            step = GetStep(step, mode);
            if (255 - value >= step)
            {
                return (byte)(value + step);
            }
            return byte.MaxValue;
        }

        private byte GetStep(byte step, WheelMode mode)
        {
            return mode switch
            {
                WheelMode.Low => (byte)Math.Max(step / 10, 1),
                WheelMode.High => (byte)Math.Min(step * 10, 255),
                _ => step,
            };
        }
    }
}
