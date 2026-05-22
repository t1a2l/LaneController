using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LaneController.KianCommons.Serialization;
using LaneController.KianCommons.Utils;
using LaneController.Manager;
using MoveItIntegration;

namespace LaneController.CustomData
{
    public class LCMoveItIntegration : MoveItIntegrationBase
    {
        public static LCMoveItIntegration Instance = new();

        private static LaneControllerManager Man => LaneControllerManager.Instance;

        public override string ID { get; } = "LaneController";

        public override Version DataVersion { get; } = typeof(LCMoveItIntegration).VersionOf();

        public override object Copy(InstanceID sourceInstanceID)
        {
            if (sourceInstanceID.Type == InstanceType.NetSegment)
            {
                return new SegmentDTO(sourceInstanceID.NetSegment).Clone();
            }
            return null;
        }

        public override void Paste(InstanceID targetInstanceID, object record, Dictionary<InstanceID, InstanceID> map)
        {
            if (targetInstanceID.Type == InstanceType.NetSegment && record is SegmentDTO segmentDTO)
            {
                segmentDTO.CopyTo(targetInstanceID.NetSegment);
            }
        }

        public override string Encode64(object record)
        {
            if (record is SegmentDTO value)
            {
                return XMLSerializerUtil.Serialize(value);
            }
            return null;
        }

        public override object Decode64(string base64Data, Version dataVersion)
        {
            if (new XDocument(base64Data).Root.Name == "SegmentDTO")
            {
                return XMLSerializerUtil.Deserialize<SegmentDTO>(base64Data);
            }
            throw new NotImplementedException(base64Data);
        }
    }
}
