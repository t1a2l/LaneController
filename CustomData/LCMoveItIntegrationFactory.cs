using MoveItIntegration;

namespace LaneController.CustomData
{
    public class LCMoveItIntegrationFactory : IMoveItIntegrationFactory
    {
        public MoveItIntegrationBase GetInstance()
        {
            return (MoveItIntegrationBase)(object)LCMoveItIntegration.Instance;
        }
    }
}
