using System;
using System.Linq;
using JetBrains.Annotations;
using LaneController.Manager;

namespace LaneController.CustomData
{
    public class SegmentDTO
    {
        public ushort SegmentId;

        public CustomLane[] Lanes;

        [UsedImplicitly]
        [Obsolete("XML only", true)]
        public SegmentDTO()
        {
        }

        public SegmentDTO(ushort segmentId)
        {
            SegmentId = segmentId;
            Lanes = LaneControllerManager.Instance.GetOrCreateLanes(segmentId);
        }

        public SegmentDTO Clone()
        {
            SegmentDTO segmentDTO = new(SegmentId);
            segmentDTO.Lanes = [.. segmentDTO.Lanes.Select((lane) => lane.Clone())];
            return segmentDTO;
        }

        public void CopyTo(ushort segmentId)
        {
            CustomLane[] orCreateLanes = LaneControllerManager.Instance.GetOrCreateLanes(segmentId);
            for (int i = 0; i < orCreateLanes.Length && i < Lanes.Length; i++)
            {
                orCreateLanes[i].CopyFrom(Lanes[i]);
            }
        }
    }
}
