using System;
using System.Collections;
using System.Collections.Generic;

namespace LaneController.KianCommons.Utils
{
    public struct LaneIterator2 : IEnumerable<LaneIdAndIndex>, IEnumerable, IEnumerator<LaneIdAndIndex>, IDisposable, IEnumerator
    {
        private readonly uint firstLaneId_;

        private readonly int laneCount_;

        private LaneIdAndIndex current_;

        public readonly LaneIdAndIndex Current => current_;

        readonly object IEnumerator.Current => Current;

        public LaneIterator2(ref NetSegment segment)
        {
            firstLaneId_ = segment.m_lanes;
            current_ = default;
            laneCount_ = segment.Info.m_lanes.Length;
        }

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
                if (current_.LaneIndex != 0)
                {
                    return false;
                }
                current_.LaneId = firstLaneId_;
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

        public readonly LaneIterator2 GetEnumerator()
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
