using System.Collections.Generic;
using ColossalFramework;
using LaneController.KianCommons.Utils;

namespace LaneController.Util
{
    internal class TraverseUtil
    {
        private static readonly HashSet<ushort> traveresed_ = [];

        public static IEnumerable<ushort> GetSimilarSegmentsBetweenJunctions(ushort segmentId)
        {
            List<ushort> list =
            [
                segmentId,
                .. TraverseSegmentsUntilJunction(segmentId, segmentId.ToSegment().m_startNode),
                .. TraverseSegmentsUntilJunction(segmentId, segmentId.ToSegment().m_endNode),
            ];
            return list;
        }

        private static IEnumerable<ushort> TraverseSegmentsUntilJunction(ushort startSegmentId, ushort nodeId)
        {
            traveresed_.Clear();
            NetInfo info = startSegmentId.ToSegment().Info;
            ushort num = startSegmentId;
            int num2 = 0;
            while (nodeId != 0 && nodeId.ToNode().m_flags.IsFlagSet(NetNode.Flags.Middle | NetNode.Flags.Bend))
            {
                num = nodeId.ToNode().GetAnotherSegment(num);
                if (num.ToSegment().Info != info || num == startSegmentId)
                {
                    break;
                }
                if (traveresed_.Contains(num))
                {
                    Log.Error("unexpected Loop detected. send screenshot of networks to kian.");
                    break;
                }
                if (num2++ > 10000)
                {
                    Log.Error("watchdog limit exceeded. send screenshot of networks to kian.");
                    break;
                }
                traveresed_.Add(num);
                nodeId = num.ToSegment().GetOtherNode(nodeId);
            }
            return traveresed_;
        }

        public static IEnumerable<ushort> GetSimilarSegmentsInRoad(ushort segmentId)
        {
            List<ushort> list =
            [
                segmentId,
                .. TraverseRoad(segmentId, segmentId.ToSegment().m_startNode),
                .. TraverseRoad(segmentId, segmentId.ToSegment().m_endNode),
            ];
            return list;
        }

        private static IEnumerable<ushort> TraverseRoad(ushort startSegmentId, ushort nodeId)
        {
            traveresed_.Clear();
            NetInfo info = startSegmentId.ToSegment().Info;
            ushort num = startSegmentId;
            ushort nameSeed = startSegmentId.ToSegment().m_nameSeed;
            int num2 = 0;
            while (nodeId != 0)
            {
                ref NetNode reference = ref nodeId.ToNode();
                if (reference.m_flags.IsFlagSet(NetNode.Flags.Middle | NetNode.Flags.Bend))
                {
                    num = reference.GetAnotherSegment(num);
                    if (num.ToSegment().Info != info || num == startSegmentId || traveresed_.Contains(num))
                    {
                        break;
                    }
                }
                else
                {
                    if (!reference.m_flags.IsFlagSet(NetNode.Flags.Junction))
                    {
                        Log.Warning($"Unexpected node detected node{nodeId} flags:{reference.m_flags}");
                        break;
                    }
                    bool flag = false;
                    foreach (ushort item in new NodeSegmentIterator(nodeId))
                    {
                        if (item != num && item.ToSegment().Info == info && item.ToSegment().m_nameSeed == nameSeed && !traveresed_.Contains(item))
                        {
                            num = item;
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
                if (num2++ > 10000)
                {
                    Log.Error("watchdog limit exceeded. send screenshot of networks to kian.");
                    break;
                }
                traveresed_.Add(num);
                nodeId = num.ToSegment().GetOtherNode(nodeId);
            }
            return traveresed_;
        }
    }
}
