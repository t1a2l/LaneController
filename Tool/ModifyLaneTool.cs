using LaneController.KianCommons.Utils;
using LaneController.UI.Editors;
using LaneController.UI.Gizmos;
using LaneController.UI.Marker;
using UnityEngine;

namespace LaneController.Tool
{
    public class ModifyLaneTool : BaseTool
    {
        public override ToolType Type => ToolType.ModifyLane;

        public override bool ShowPanel => true;

        public override void SimulationStep()
        {
            base.SimulationStep();
            if (base.Tool.BezierMarker != null)
            {
                ToolBase.RaycastInput raycastInput = new(LaneControllerTool.MouseRay, LaneControllerTool.MouseRayLength)
                {
                    m_ignoreTerrain = false
                };
                ToolBase.RaycastInput input = raycastInput;
                LaneControllerTool.RayCast(input, out var output);
                if (base.Tool.BezierMarker.Drag(output.m_hitPos) && base.Panel?.CurrentEditor is LaneEditor laneEditor)
                {
                    laneEditor.PullValues();
                }
            }
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            base.RenderOverlay(cameraInfo);
            int hoverIndex = (base.Panel.CurrentEditor as LaneEditor)?.HoveredControlPointIndex ?? (-1);
            base.Tool.BezierMarker?.RenderOverlay(cameraInfo, Color.green, hoverIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            base.Tool.BezierMarker?.OnUpdate();
        }

        public override string OnToolInfo()
        {
            ControlPointMarker[] array = base.Tool.BezierMarker?.controlMarkers;
            if (array != null)
            {
                ControlPointMarker[] array2 = array;
                foreach (ControlPointMarker controlPointMarker in array2)
                {
                    KeyTyping keyTyping = controlPointMarker?.Gizmo?.KeyTyping;
                    if (keyTyping != null)
                    {
                        if (!keyTyping.registeredString.IsNullorEmpty())
                        {
                            return keyTyping.registeredString;
                        }
                        if (controlPointMarker.Gizmo.Distance != 0f)
                        {
                            return controlPointMarker.Gizmo.Distance.ToString("R");
                        }
                        return "type meters to move";
                    }
                }
                if (!Helpers.ControlIsPressed)
                {
                    return "Hold control for vertical shift.";
                }
            }
            return null;
        }
    }
}
