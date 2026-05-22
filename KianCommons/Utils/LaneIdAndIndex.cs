using System;
using System.Linq;
using System.Xml.Serialization;

namespace LaneController.KianCommons.Utils
{
    [Serializable]
    public struct LaneIdAndIndex
    {
        public uint LaneId;

        public int LaneIndex;

        public readonly bool IsValid
        {
            get
            {
                if (LaneId != 0)
                {
                    return LaneIndex >= 0;
                }
                return false;
            }
        }

        public readonly NetInfo.Lane LaneInfo => Segment.Info?.m_lanes?.ElementAtOrDefault(LaneIndex);

        public readonly ushort SegmentId => Lane.m_segment;

        public readonly ref NetSegment Segment => ref SegmentId.ToSegment();

        public readonly ref NetLane Lane => ref LaneId.ToLane();

        [XmlIgnore]
        public readonly NetLane.Flags Flags
        {
            get
            {
                return (NetLane.Flags)Lane.m_flags;
            }
            set
            {
                LaneId.ToLane().m_flags = (ushort)value;
            }
        }

        public bool HeadsToStartNode => Segment.IsInvert() ^ LaneInfo.IsGoingBackward();

        public ushort HeadNode => Segment.GetNode(HeadsToStartNode);

        public ushort TailNode => Segment.GetNode(!HeadsToStartNode);

        public bool LeftSide => LaneInfo.m_position < 0f != Segment.m_flags.IsFlagSet(NetSegment.Flags.Invert);

        public bool RightSide => !LeftSide;

        public LaneIdAndIndex(uint laneId, int laneIndex = -1)
        {
            LaneId = laneId;
            if (laneIndex < 0)
            {
                laneIndex = NetUtil.GetLaneIndex(laneId);
            }
            LaneIndex = laneIndex;
        }

        public override string ToString()
        {
            return $"LaneData:[segment:{SegmentId} segmentInfo:{Segment.Info} laneId:{LaneId} Index={LaneIndex} {LaneInfo?.m_laneType} {LaneInfo?.m_vehicleType}]";
        }
    }
}
