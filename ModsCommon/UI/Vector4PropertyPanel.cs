using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class Vector4PropertyPanel : BaseVectorPropertyPanel<Vector4>
    {
        public override uint Dimension => 4u;

        protected override float Get(ref Vector4 vector, int index)
        {
            return vector[index];
        }

        protected override void Set(ref Vector4 vector, int index, float value)
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
                3 => "W",
                _ => "?",
            };
        }
    }
}
