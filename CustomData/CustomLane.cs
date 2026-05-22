using System;
using System.Xml.Serialization;
using ColossalFramework.Math;
using JetBrains.Annotations;
using LaneController.KianCommons.Utils;
using LaneController.Util;
using UnityEngine;

namespace LaneController.CustomData
{
    public class CustomLane : ICustomInstanceID
    {
        public LaneIdAndIndex LaneIdAndIndex;

        public float Shift;

        public float VShift;

        [XmlElement("Displacement")]
        public Bezier3 DeltaControlPoints;

        [XmlElement("Bezier0")]
        private Bezier3 Beizer0;

        [XmlIgnore]
        public float Position
        {
            get
            {
                return LaneInfo.m_position + Shift;
            }
            set
            {
                Shift = value - LaneInfo.m_position;
            }
        }

        [XmlIgnore]
        public float Height
        {
            get
            {
                return LaneInfo.m_verticalOffset + VShift;
            }
            set
            {
                VShift = value - LaneInfo.m_verticalOffset;
            }
        }

        public NetInfo.Lane LaneInfo => LaneIdAndIndex.LaneInfo;

        public int Index => LaneIdAndIndex.LaneIndex;

        public uint LaneId => LaneIdAndIndex.LaneId;

        public ref Bezier3 FinalBezier => ref LaneIdAndIndex.Lane.m_bezier;

        [UsedImplicitly]
        [Obsolete("XML only", true)]
        public CustomLane()
        {
        }

        public CustomLane(LaneIdAndIndex laneIdAndIndex)
        {
            LaneIdAndIndex = laneIdAndIndex;
            Beizer0 = FinalBezier;
        }

        public void UpdateControlPoint(int i, Vector3 newPos)
        {
            DeltaControlPoints.ControlPoint(i) = newPos - Beizer0.ControlPoint(i);
            FinalBezier.ControlPoint(i) = newPos;
            QueueUpdate();
        }

        public Vector3 GetControlPoint(int i)
        {
            return FinalBezier.ControlPoint(i);
        }

        public void CopyFrom(CustomLane lane)
        {
            Shift = lane.Shift;
            VShift = lane.VShift;
            DeltaControlPoints = lane.DeltaControlPoints;
        }

        public CustomLane Clone()
        {
            return MemberwiseClone() as CustomLane;
        }

        public bool IsDefault()
        {
            if (Shift == 0f && VShift == 0f)
            {
                return DeltaControlPoints.IsDefault();
            }
            return false;
        }

        public void Reset()
        {
            Shift = 0f;
            VShift = 0f;
            DeltaControlPoints = default;
        }

        public void QueueUpdate()
        {
            NetUtil.SafeUpdateSegment(LaneIdAndIndex.SegmentId);
        }

        public void RecalculateLaneBezier()
        {
            Log.Called(LaneIdAndIndex);
            ref NetLane lane = ref LaneIdAndIndex.Lane;
            ushort segment = lane.m_segment;
            ref NetSegment reference = ref segment.ToSegment();
            reference.CalculateCorner(segment, heightOffset: true, start: true, leftSide: true, out var cornerPos, out var cornerDirection, out _);
            reference.CalculateCorner(segment, heightOffset: true, start: false, leftSide: true, out var cornerPos2, out var cornerDirection2, out _);
            reference.CalculateCorner(segment, heightOffset: true, start: true, leftSide: false, out var cornerPos3, out var cornerDirection3, out bool smooth);
            reference.CalculateCorner(segment, heightOffset: true, start: false, leftSide: false, out var cornerPos4, out var cornerDirection4, out bool smooth2);
            float num = (Position / (reference.Info.m_halfWidth * 2f)) + 0.5f;
            if ((reference.m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
            {
                num = 1f - num;
            }
            Vector3 vector = cornerPos + ((cornerPos3 - cornerPos) * num);
            Vector3 vector2 = cornerPos4 + ((cornerPos2 - cornerPos4) * num);
            Vector3 startDir = Vector3.Lerp(cornerDirection, cornerDirection3, num);
            Vector3 endDir = Vector3.Lerp(cornerDirection4, cornerDirection2, num);
            vector.y += Height;
            vector2.y += Height;
            NetSegment.CalculateMiddlePoints(vector, startDir, vector2, endDir, smooth, smooth2, out var middlePos, out var middlePos2);
            Beizer0 = new Bezier3(vector, middlePos, middlePos2, vector2);
            lane.m_bezier = Beizer0.Add(DeltaControlPoints);
            lane.m_segment = segment;
            lane.UpdateLength();
            float num2 = 0f;
            if (reference.Info.m_lanes.Length != 0)
            {
                foreach (LaneIdAndIndex item in new LaneIterator(segment))
                {
                    num2 += item.Lane.m_length;
                }
            }
            if (reference.Info.m_lanes.Length != 0)
            {
                reference.m_averageLength = num2 / reference.Info.m_lanes.Length;
            }
            else
            {
                reference.m_averageLength = 0f;
            }
        }

        public void CalculateBeizer0()
        {
            ushort segment = LaneIdAndIndex.Lane.m_segment;
            ref NetSegment reference = ref segment.ToSegment();
            reference.CalculateCorner(segment, heightOffset: true, start: true, leftSide: true, out var cornerPos, out var cornerDirection, out _);
            reference.CalculateCorner(segment, heightOffset: true, start: false, leftSide: true, out var cornerPos2, out var cornerDirection2, out _);
            reference.CalculateCorner(segment, heightOffset: true, start: true, leftSide: false, out var cornerPos3, out var cornerDirection3, out bool smooth);
            reference.CalculateCorner(segment, heightOffset: true, start: false, leftSide: false, out var cornerPos4, out var cornerDirection4, out bool smooth2);
            float num = (Position / (reference.Info.m_halfWidth * 2f)) + 0.5f;
            if ((reference.m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
            {
                num = 1f - num;
            }
            Vector3 vector = cornerPos + ((cornerPos3 - cornerPos) * num);
            Vector3 vector2 = cornerPos4 + ((cornerPos2 - cornerPos4) * num);
            Vector3 startDir = Vector3.Lerp(cornerDirection, cornerDirection3, num);
            Vector3 endDir = Vector3.Lerp(cornerDirection4, cornerDirection2, num);
            vector.y += Height;
            vector2.y += Height;
            NetSegment.CalculateMiddlePoints(vector, startDir, vector2, endDir, smooth, smooth2, out var middlePos, out var middlePos2);
            Beizer0 = new Bezier3(vector, middlePos, middlePos2, vector2);
        }

        public void PostfixLaneBezier()
        {
            try
            {
                ref NetLane lane = ref LaneIdAndIndex.Lane;
                Beizer0 = lane.m_bezier.Shift(Shift, VShift);
                lane.m_bezier = Beizer0.Add(DeltaControlPoints);
                lane.UpdateLength();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public override string ToString()
        {
            return $"Lane {Index}";
        }
    }
}
