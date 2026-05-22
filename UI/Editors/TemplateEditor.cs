using LaneController.CustomData;

namespace LaneController.UI.Editors
{
    public class TemplateEditor : BaseEditor<LaneItem, CustomLane, LaneIcons>
    {
        public override string Name => "Template Editor";

        public override string SelectionMessage => "No Templates";

        public override void Render(RenderManager.CameraInfo cameraInfo)
        {
        }
    }
}
