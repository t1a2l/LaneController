using ColossalFramework;
using ColossalFramework.Math;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.Util
{
    public static class RenderUtil
    {
        public static void Render(this Bezier3 bezier, RenderManager.CameraInfo cameraInfo, Color color, float halfWidth, float cutStart, float cutEnd, bool alphaBlend = false)
        {
            Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls++;
            Singleton<RenderManager>.instance.OverlayEffect.DrawBezier(cameraInfo, color, bezier, halfWidth * 2f, cutStart, cutEnd, -1f, 1024f, renderLimits: false, alphaBlend);
        }

        public static void Render(this Bezier3 bezier, RenderManager.CameraInfo cameraInfo, Color color, float hw, bool alphaBlend = false)
        {
            bezier.Render(cameraInfo, color, hw, hw, hw, alphaBlend);
        }

        public static void RenderSegmentOverlay(RenderManager.CameraInfo cameraInfo, ushort segmentId, Color color, bool alphaBlend = false)
        {
            ref NetSegment reference = ref segmentId.ToSegment();
            RenderUncutSegmentOverlay(cameraInfo, segmentId, color, reference.Info.m_halfWidth, reference.Info.m_halfWidth, alphaBlend);
        }

        public static void RenderAutoCutSegmentOverlay(RenderManager.CameraInfo cameraInfo, ushort segmentId, Color color, bool alphaBlend = false)
        {
            ref NetSegment reference = ref segmentId.ToSegment();
            RenderUncutSegmentOverlay(cameraInfo, segmentId, color, reference.m_startNode.ToNode().CountSegments() > 1 ? reference.Info.m_halfWidth : 0f, reference.m_endNode.ToNode().CountSegments() > 1 ? reference.Info.m_halfWidth : 0f, alphaBlend);
        }

        public static void RenderUncutSegmentOverlay(RenderManager.CameraInfo cameraInfo, ushort segmentId, Color color, float cutStart, float cutEnd, bool alphaBlend = false)
        {
            if (segmentId != 0)
            {
                RenderRawSegmentOverlay(cameraInfo, segmentId, segmentId.ToSegment().Info.m_halfWidth, color, cutStart, cutEnd, alphaBlend);
            }
        }

        public static void RenderRawSegmentOverlay(RenderManager.CameraInfo cameraInfo, ushort segmentId, float width, Color color, float cutStart, float cutEnd, bool alphaBlend = false)
        {
            NetNode[] nodeBuffer;
            if (segmentId != 0)
            {
                ref NetSegment reference = ref segmentId.ToSegment();
                nodeBuffer = Singleton<NetManager>.instance.m_nodes.m_buffer;
                Bezier3 bezier = default;
                bezier.a = reference.m_startNode.ToNode().m_position;
                bezier.d = reference.m_endNode.ToNode().m_position;
                NetSegment.CalculateMiddlePoints(bezier.a, reference.m_startDirection, bezier.d, reference.m_endDirection, IsMiddle(reference.m_startNode), IsMiddle(reference.m_endNode), out bezier.b, out bezier.c);
                bezier.Render(cameraInfo, color, width, cutStart, cutEnd, alphaBlend);
            }
            bool IsMiddle(ushort nodeId)
            {
                return (nodeBuffer[nodeId].m_flags & NetNode.Flags.Middle) != 0;
            }
        }

        public static void RenderLaneOverlay(RenderManager.CameraInfo cameraInfo, LaneIdAndIndex laneData, Color color, bool alphaBlend = false)
        {
            float halfWidth = laneData.LaneInfo.m_width * 0.5f;
            laneData.Lane.m_bezier.Render(cameraInfo, color, halfWidth, 0f, 0f, alphaBlend);
        }

        public static void DrawNodeCircle(RenderManager.CameraInfo cameraInfo, Color color, ushort nodeId, bool alphaBlend = false)
        {
            DrawOverlayCircle(cameraInfo, color, nodeId.ToNode().m_position, nodeId.ToNode().Info.m_halfWidth, alphaBlend);
        }

        public static void DrawOverlayCircle(RenderManager.CameraInfo cameraInfo, Color color, Vector3 position, float radius, bool alphaBlend = false)
        {
            Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls++;
            Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, color, position, radius * 2f, position.y - 100f, position.y + 100f, renderLimits: false, alphaBlend);
        }
    }
}
