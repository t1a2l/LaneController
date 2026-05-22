using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class Vector3PropertyPanel : BaseVectorPropertyPanel<Vector3>
    {
        public override uint Dimension => 3u;

        public void Init(string name)
        {
            Init(0, 1, 2);
            Text = name;
            bool wheelTip = UseWheel = true;
            WheelTip = wheelTip;
            WheelStep = Vector3.one;
        }

        protected override float Get(ref Vector3 vector, int index)
        {
            return vector[index];
        }

        protected override void Set(ref Vector3 vector, int index, float value)
        {
            vector[index] = value;
        }

        protected override string GetName(int index)
        {
            return index switch
            {
                0 => "X",
                1 => "Y",
                2 => "Z",
                _ => "?",
            };
        }
    }
}
