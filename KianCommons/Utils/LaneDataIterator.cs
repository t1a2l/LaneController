using System;
using System.Collections;
using System.Collections.Generic;

namespace LaneController.KianCommons.Utils
{
    public struct LaneDataIterator(ushort segmentID, bool? startNode = null, NetInfo.LaneType? laneType = null, VehicleInfo.VehicleType? vehicleType = null) : IEnumerable<LaneIdAndIndex>, IEnumerable, IEnumerator<LaneIdAndIndex>, IDisposable, IEnumerator
    {
        private readonly ushort segmentID_ = segmentID;

        private readonly bool? startNode_ = startNode;

        private readonly NetInfo.LaneType? laneType_ = laneType;

        private readonly VehicleInfo.VehicleType? vehicleType_ = vehicleType;

        private readonly int nLanes_ = segmentID.ToSegment().Info.m_lanes.Length;

        private LaneIdAndIndex current_ = default;

        public readonly LaneIdAndIndex Current => current_;

        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            uint num;
            int num2;
            if (current_.LaneId != 0)
            {
                num = current_.Lane.m_nextLane;
                num2 = current_.LaneIndex + 1;
            }
            else
            {
                num = segmentID_.ToSegment().m_lanes;
                num2 = 0;
            }
            if (num == 0)
            {
                return false;
            }
            if (num2 >= nLanes_)
            {
                if (Log.VERBOSE)
                {
                    Log.Warning($"lane count mismatch! segment:{segmentID_} laneId:{num} laneIndex:{num2}", copyToGameLog: false);
                    Log.Warning(NetUtil.PrintSegmentLanes(segmentID_), copyToGameLog: false);
                }
                return false;
            }
            if (num.ToLane().m_segment != segmentID_)
            {
                if (Log.VERBOSE)
                {
                    Log.Warning($"lane has different segment:{num.ToLane().m_segment}! segment:{segmentID_} laneId:{num} laneIndex:{num2}", copyToGameLog: false);
                    Log.Warning(NetUtil.PrintSegmentLanes(segmentID_), copyToGameLog: false);
                }
                return false;
            }
            try
            {
                current_ = new LaneIdAndIndex(num, num2);
            }
            catch (Exception ex)
            {
                ex.Log($"bad lane! segment:{segmentID_} laneId:{num} laneIndex:{num2}", showInPannel: false);
            }
            if (startNode_.HasValue && startNode_.Value != current_.HeadsToStartNode)
            {
                return MoveNext();
            }
            if (laneType_.HasValue && !current_.LaneInfo.m_laneType.IsFlagSet(laneType_.Value))
            {
                return MoveNext();
            }
            if (vehicleType_.HasValue && !current_.LaneInfo.m_vehicleType.IsFlagSet(vehicleType_.Value))
            {
                return MoveNext();
            }
            return true;
        }

        public void Reset()
        {
            current_ = default;
        }

        public readonly LaneDataIterator GetEnumerator()
        {
            return this;
        }

        public readonly void Dispose()
        {
        }

        readonly IEnumerator<LaneIdAndIndex> IEnumerable<LaneIdAndIndex>.GetEnumerator()
        {
            return this;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
