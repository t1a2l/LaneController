using ColossalFramework.Math;
using LaneController.CustomData;
using LaneController.KianCommons.Utils;
using LaneController.Manager;
using LaneController.Util;
using UnityEngine;

namespace LaneController.UI.Marker
{
    public class BezierMarker
    {
        public LaneIdAndIndex LaneIdAndIndex;

        public ControlPointMarker[] controlMarkers = new ControlPointMarker[4];

        internal bool Focused
        {
            get
            {
                if (!A.Hovered && !A.Selected && !B.Hovered && !B.Selected && !C.Hovered && !C.Selected && !D.Hovered)
                {
                    return D.Selected;
                }
                return true;
            }
        }

        public CustomLane CustomLane => LaneControllerManager.Instance.GetOrCreateLane(LaneIdAndIndex);

        public ControlPointMarker A => controlMarkers[0];

        public ControlPointMarker B => controlMarkers[1];

        public ControlPointMarker C => controlMarkers[2];

        public ControlPointMarker D => controlMarkers[3];

        public BezierMarker(LaneIdAndIndex laneIdAndIndex)
        {
            LaneIdAndIndex = laneIdAndIndex;
            Bezier3 bezier = laneIdAndIndex.Lane.m_bezier;
            for (int i = 0; i < 4; i++)
            {
                controlMarkers[i] = new ControlPointMarker(bezier.ControlPoint(i), i);
            }
        }

        public void Destroy()
        {
            ControlPointMarker[] array = controlMarkers;
            for (int i = 0; i < array.Length; i++)
            {
                array[i]?.Destroy();
            }
        }

        public void OnUpdate()
        {
            ControlPointMarker[] array = controlMarkers;
            for (int i = 0; i < array.Length; i++)
            {
                array[i]?.OnUpdate();
            }
        }

        public void RenderOverlay(RenderManager.CameraInfo cameraInfo, Color color, int hoverIndex = -1)
        {
            for (int i = 0; i < 4; i++)
            {
                controlMarkers[i].RenderOverlay(cameraInfo, color, hoverIndex == i);
            }
        }

        public bool Drag(Vector3 hitPos)
        {
            for (int i = 0; i < 4; i++)
            {
                ControlPointMarker controlPointMarker = controlMarkers[i];
                controlPointMarker.UpdatePosition(CustomLane.GetControlPoint(i));
            }
            for (int j = 0; j < 4; j++)
            {
                ControlPointMarker controlPointMarker2 = controlMarkers[j];
                if (controlPointMarker2.Drag(hitPos))
                {
                    CustomLane.UpdateControlPoint(j, controlPointMarker2.Position);
                    return true;
                }
            }
            return false;
        }
    }
}
