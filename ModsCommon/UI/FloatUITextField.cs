using ColossalFramework.UI;

namespace LaneController.ModsCommon.UI
{
    public class FloatUITextField : ComparableUITextField<float>
    {
        private string _numberFormat;

        private static string DefaultNumberFormat => "0.###";

        public string NumberFormat
        {
            private get
            {
                if (string.IsNullOrEmpty(_numberFormat))
                {
                    return DefaultNumberFormat;
                }
                return _numberFormat;
            }
            set
            {
                _numberFormat = value;
                RefreshText();
            }
        }

        public override string text
        {
            get
            {
                return base.text.Replace(',', '.');
            }
            set
            {
                base.text = value;
            }
        }

        protected override float Decrement(float value, float step, WheelMode mode)
        {
            step = GetStep(step, mode);
            return (value - step).RoundToNearest(step);
        }

        protected override float Increment(float value, float step, WheelMode mode)
        {
            step = GetStep(step, mode);
            return (value + step).RoundToNearest(step);
        }

        private float GetStep(float step, WheelMode mode)
        {
            return mode switch
            {
                WheelMode.Low => step / 10f,
                WheelMode.High => step * 10f,
                _ => step,
            };
        }

        public override void DeInit()
        {
            base.DeInit();
            _numberFormat = null;
        }

        protected override string GetString(float value)
        {
            return value.ToString(NumberFormat);
        }
    }
}
