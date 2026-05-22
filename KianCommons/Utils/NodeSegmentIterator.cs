using System;
using System.Collections;
using System.Collections.Generic;

namespace LaneController.KianCommons.Utils
{
    public struct NodeSegmentIterator(ushort nodeId) : IEnumerable<ushort>, IEnumerable, IEnumerator<ushort>, IDisposable, IEnumerator
    {
        private int i_ = 0;

        private ushort segmentId_ = 0;

        private readonly ushort nodeId_ = nodeId;

        public readonly ushort Current => segmentId_;

        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (i_ < 8)
            {
                segmentId_ = nodeId_.ToNode().GetSegment(i_++);
                if (segmentId_ != 0)
                {
                    return true;
                }
            }
            segmentId_ = 0;
            return false;
        }

        public readonly NodeSegmentIterator GetEnumerator()
        {
            return this;
        }

        public void Reset()
        {
            i_ = segmentId_ = 0;
        }

        public void Dispose()
        {
            Reset();
        }

        readonly IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator()
        {
            return this;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
