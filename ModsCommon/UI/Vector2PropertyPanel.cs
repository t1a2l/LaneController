using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class Vector2PropertyPanel : BaseVectorPropertyPanel<Vector2>
    {
        public override uint Dimension => 2u;

        protected override float Get(ref Vector2 vector, int index)
        {
            return vector[index];
        }

        protected override void Set(ref Vector2 vector, int index, float value)
        {
            vector[index] = value;
        }

        protected override string GetName(int index)
        {
            return index switch
            {
                0 => "X",
                1 => "Y",
                _ => "?",
            };
        }
    }
}
