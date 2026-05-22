using System;
using System.Collections;
using System.Collections.Generic;

namespace LaneController.KianCommons.Utils
{
    public struct LaneIterator(ushort segmentID) : IEnumerable<LaneIdAndIndex>, IEnumerable, IEnumerator<LaneIdAndIndex>, IDisposable, IEnumerator
    {
        private readonly ushort segmentID_ = segmentID;

        private readonly int laneCount_ = segmentID.ToSegment().Info.m_lanes.Length;

        private LaneIdAndIndex current_ = default;

        public readonly LaneIdAndIndex Current => current_;

        readonly object IEnumerator.Current => Current;

        public void Reset()
        {
            current_ = default;
        }

        public readonly void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (current_.LaneId == 0)
            {
                if (current_.LaneIndex > 0)
                {
                    return false;
                }
                current_.LaneId = segmentID_.ToSegment().m_lanes;
            }
            else
            {
                current_.LaneId = current_.LaneId.ToLane().m_nextLane;
                current_.LaneIndex++;
            }
            if (current_.LaneId != 0)
            {
                return current_.LaneIndex < laneCount_;
            }
            return false;
        }

        public readonly LaneIterator GetEnumerator()
        {
            return this;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        readonly IEnumerator<LaneIdAndIndex> IEnumerable<LaneIdAndIndex>.GetEnumerator()
        {
            return this;
        }
    }
}
