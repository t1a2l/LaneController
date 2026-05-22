using System;
using System.Linq;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseVectorPropertyPanel<TypeVector> : EditorPropertyPanel, IReusable where TypeVector : struct
    {
        public readonly struct Field(BaseVectorPropertyPanel<TypeVector> property, int index)
        {
            private BaseVectorPropertyPanel<TypeVector> Property { get; } = property;

            private int Index { get; } = index;

            public float Value
            {
                get
                {
                    return Property.Fields[Index].Value;
                }
                set
                {
                    Property.Fields[Index].Value = value;
                }
            }

            public float Width
            {
                get
                {
                    return Property.Fields[Index].width;
                }
                set
                {
                    Property.Fields[Index].width = value;
                }
            }

            public string Format
            {
                set
                {
                    Property.Fields[Index].Format = value;
                }
            }

            public string NumberFormat
            {
                set
                {
                    Property.Fields[Index].NumberFormat = value;
                }
            }

            public Color32 FieldTextColor
            {
                set
                {
                    Property.Fields[Index].textColor = value;
                }
            }

            public bool SubmitOnFocusLost
            {
                get
                {
                    return Property.Fields[Index].submitOnFocusLost;
                }
                set
                {
                    Property.Fields[Index].submitOnFocusLost = value;
                }
            }

            public float MinValue
            {
                get
                {
                    return Property.Fields[Index].MinValue;
                }
                set
                {
                    Property.Fields[Index].MinValue = value;
                }
            }

            public float MaxValue
            {
                get
                {
                    return Property.Fields[Index].MaxValue;
                }
                set
                {
                    Property.Fields[Index].MaxValue = value;
                }
            }

            public bool CheckMin
            {
                get
                {
                    return Property.Fields[Index].CheckMin;
                }
                set
                {
                    Property.Fields[Index].CheckMin = value;
                }
            }

            public bool CheckMax
            {
                get
                {
                    return Property.Fields[Index].CheckMax;
                }
                set
                {
                    Property.Fields[Index].CheckMax = value;
                }
            }

            public bool CyclicalValue
            {
                get
                {
                    return Property.Fields[Index].CyclicalValue;
                }
                set
                {
                    Property.Fields[Index].CyclicalValue = value;
                }
            }

            public bool UseWheel
            {
                get
                {
                    return Property.Fields[Index].UseWheel;
                }
                set
                {
                    Property.Fields[Index].UseWheel = value;
                }
            }

            public float WheelStep
            {
                get
                {
                    return Property.Fields[Index].WheelStep;
                }
                set
                {
                    Property.Fields[Index].WheelStep = value;
                }
            }

            public bool MouseTips
            {
                set
                {
                    Property.Fields[Index].MouseTips = value;
                }
            }
        }

        private TypeVector _value;

        private float _fieldsWidth = 30f;

        bool IReusable.InCache { get; set; }

        public abstract uint Dimension { get; }

        public Field this[int index]
        {
            get
            {
                if (index < 0 && index >= Dimension)
                {
                    throw new IndexOutOfRangeException();
                }
                return new Field(this, index);
            }
        }

        public TypeVector Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].Value = Get(ref _value, i);
                }
            }
        }

        public string Format
        {
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].Format = value;
                }
            }
        }

        public string NumberFormat
        {
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].NumberFormat = value;
                }
            }
        }

        public float FieldsWidth
        {
            get
            {
                return _fieldsWidth;
            }
            set
            {
                if (value != _fieldsWidth && value > 0f)
                {
                    _fieldsWidth = value;
                    FloatUITextField[] fields = Fields;
                    foreach (FloatUITextField floatUITextField in fields)
                    {
                        floatUITextField.width = _fieldsWidth;
                    }
                }
            }
        }

        protected CustomUILabel[] Labels { get; }

        protected FloatUITextField[] Fields { get; }

        public Color32 FieldTextColor
        {
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].textColor = value;
                }
            }
        }

        public bool SubmitOnFocusLost
        {
            get
            {
                return Fields.All((f) => f.submitOnFocusLost);
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].submitOnFocusLost = value;
                }
            }
        }

        public TypeVector MinValue
        {
            get
            {
                TypeVector vector = default;
                for (int i = 0; i < Dimension; i++)
                {
                    Set(ref vector, i, Fields[i].MinValue);
                }
                return vector;
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].MinValue = Get(ref value, i);
                }
            }
        }

        public TypeVector MaxValue
        {
            get
            {
                TypeVector vector = default;
                for (int i = 0; i < Dimension; i++)
                {
                    Set(ref vector, i, Fields[i].MaxValue);
                }
                return vector;
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].MaxValue = Get(ref value, i);
                }
            }
        }

        public bool CheckMin
        {
            get
            {
                return Fields.All((f) => f.CheckMin);
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].CheckMin = value;
                }
            }
        }

        public bool CheckMax
        {
            get
            {
                return Fields.All((f) => f.CheckMax);
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].CheckMax = value;
                }
            }
        }

        public bool CyclicalValue
        {
            get
            {
                return Fields.All((f) => f.CyclicalValue);
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].CyclicalValue = value;
                }
            }
        }

        public bool UseWheel
        {
            get
            {
                return Fields.All((f) => f.UseWheel);
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].UseWheel = value;
                }
            }
        }

        public TypeVector WheelStep
        {
            get
            {
                TypeVector vector = default;
                for (int i = 0; i < Dimension; i++)
                {
                    Set(ref vector, i, Fields[i].WheelStep);
                }
                return vector;
            }
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].WheelStep = Get(ref value, i);
                }
            }
        }

        public bool WheelTip
        {
            set
            {
                for (int i = 0; i < Dimension; i++)
                {
                    Fields[i].MouseTips = value;
                }
            }
        }

        public event Action<TypeVector> OnValueChanged;

        public event Action OnResetValue;

        public BaseVectorPropertyPanel()
        {
            Labels = new CustomUILabel[Dimension];
            Fields = new FloatUITextField[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                AddField(i, out Labels[i], out Fields[i]);
            }
        }

        public void Init(params int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < Dimension)
                {
                    Labels[indexes[i]].isVisible = true;
                    Fields[indexes[i]].isVisible = true;
                    Labels[indexes[i]].zOrder = i * 2;
                    Fields[indexes[i]].zOrder = i * 2 + 1;
                }
            }
            base.Init();
        }

        public void SetLabels(params string[] labels)
        {
            for (int i = 0; i < labels.Length && i < Dimension; i++)
            {
                Labels[i].text = labels[i];
            }
        }

        public override void DeInit()
        {
            base.DeInit();
            for (int i = 0; i < Dimension; i++)
            {
                Labels[i].isVisible = false;
                Labels[i].text = GetName(i);
                Fields[i].isVisible = false;
                Fields[i].SetDefault();
            }
            OnValueChanged = null;
        }

        protected abstract string GetName(int index);

        protected abstract float Get(ref TypeVector vector, int index);

        protected abstract void Set(ref TypeVector vector, int index, float value);

        protected void AddField(int index, out CustomUILabel label, out FloatUITextField field)
        {
            label = Content.AddUIComponent<CustomUILabel>();
            label.isVisible = false;
            label.text = GetName(index);
            label.textScale = 0.7f;
            label.padding = new RectOffset(0, 0, 2, 0);
            field = Content.AddUIComponent<FloatUITextField>();
            field.isVisible = false;
            field.SetDefaultStyle();
            field.UseWheel = true;
            field.WheelStep = 1f;
            field.width = FieldsWidth;
            field.NumberFormat = "0.##";
            field.OnValueChanged += delegate (float value)
            {
                FieldChanged(index, value);
            };
            field.UseReset = true;
            field.OnResetValue += ResetValue;
        }

        private void FieldChanged(int index, float value)
        {
            Set(ref _value, index, value);
            OnValueChanged?.Invoke(_value);
        }

        private void ResetValue()
        {
            OnResetValue?.Invoke();
        }
    }
}
