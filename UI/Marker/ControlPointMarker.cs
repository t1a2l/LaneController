using ColossalFramework;
using LaneController.KianCommons.Utils;
using LaneController.Tool;
using LaneController.UI.Gizmos;
using UnityEngine;

namespace LaneController.UI.Marker
{
    public class ControlPointMarker
    {
        internal bool UnderGround;

        internal Vector3 TerrainPosition;

        internal Vector3 Position;

        internal static float Radius = 1.5f;

        internal const float MAX_ERROR = 2.5f;

        internal bool Hovered;

        internal bool Selected;

        internal int i;

        internal YGizmo Gizmo;

        public bool GizmoMod
        {
            get
            {
                if (Gizmo == null)
                {
                    return false;
                }
                if (Gizmo.AxisClicked)
                {
                    return true;
                }
                return Helpers.ControlIsPressed;
            }
        }

        public ControlPointMarker(Vector3 pos, int i)
        {
            this.i = i;
            UpdatePosition(pos);
            Gizmo = YGizmo.CreatePositionGizmo(pos);
        }

        public void OnUpdate()
        {
            Gizmo?.OnUpdate();
        }

        public void Destroy()
        {
            Gizmo?.Destroy();
            Gizmo = null;
        }

        public void UpdatePosition(Vector3 pos)
        {
            Position = pos;
            UnderGround = false;
            TerrainPosition = pos;
            TerrainPosition.y = Singleton<TerrainManager>.instance.SampleDetailHeightSmooth(pos);
            Gizmo?.UpdatePosition(pos);
        }

        public void CalculateMode()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Selected = Hovered = IntersectRay();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Selected = false;
                Hovered = IntersectRay();
            }
            else
            {
                Hovered = Selected || IntersectRay();
            }
        }

        private bool IntersectRay()
        {
            Vector3 center = UnderGround ? TerrainPosition : Position;
            return new Bounds(center, Vector3.one * Radius).IntersectRay(LaneControllerTool.MouseRay);
        }

        public void RenderOverlay(RenderManager.CameraInfo cameraInfo, Color color, bool fieldHovered = false)
        {
            CalculateMode();
            float num = (Hovered || fieldHovered) ? 2f : 1f;
            if (Selected)
            {
                num = 2.5f;
            }
            if (!GizmoMod)
            {
                Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, color, TerrainPosition, Radius * num, TerrainPosition.y - 100f, TerrainPosition.y + 100f, renderLimits: false, alphaBlend: true);
                Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, Selected ? Color.white : Color.black, TerrainPosition, Radius * 0.75f * num, TerrainPosition.y - 100f, TerrainPosition.y + 100f, renderLimits: false, alphaBlend: false);
            }
            if (Event.current.type == EventType.Repaint && Gizmo != null)
            {
                Gizmo.IsVisible = GizmoMod;
            }
        }

        public bool Drag(Vector3 hitPos)
        {
            if (GizmoMod)
            {
                if (Gizmo != null && Gizmo.Drag())
                {
                    Position = Gizmo.Origin;
                    return true;
                }
            }
            else if (Selected)
            {
                hitPos.y = Position.y;
                if ((double)(hitPos - Position).sqrMagnitude > 0.0001)
                {
                    Position = hitPos;
                    return true;
                }
            }
            return false;
        }
    }
}
