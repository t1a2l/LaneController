using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LaneController.CustomData;
using LaneController.KianCommons.Serialization;
using LaneController.KianCommons.Utils;

namespace LaneController.Manager
{
    public class LaneControllerManager
    {
        internal static Version LoadingVersion;

        [XmlIgnore]
        internal Dictionary<uint, CustomLane> Lanes = new(1000);

        public static LaneControllerManager Instance { get; private set; }

        [XmlAttribute("Version")]
        public string SavedVersion
        {
            get
            {
                return this.VersionOf().ToString();
            }
            set
            {
                LoadingVersion = new Version(value);
            }
        }

        [XmlArray("Lanes")]
        [XmlArrayItem("Lane")]
        public CustomLane[] SavedLanes
        {
            get
            {
                return Lanes.Values.Where(customLane => !customLane.IsDefault()).ToArray().LogRet();
            }
            set
            {
                Log.Called(value);
                Lanes.Clear();
                foreach (CustomLane customLane in value)
                {
                    Lanes[customLane.LaneId] = customLane;
                }
            }
        }

        public static bool Exists()
        {
            return Instance != null;
        }

        public static LaneControllerManager Create()
        {
            return Instance = new LaneControllerManager();
        }

        public static LaneControllerManager Ensure()
        {
            return Instance ??= Create();
        }

        public static LaneControllerManager Release()
        {
            return Instance = null;
        }

        public byte[] Serialize()
        {
            Log.Called();
            if (SavedLanes.Any())
            {
                string s = XMLSerializerUtil.Serialize(this);
                return Encoding.ASCII.GetBytes(s);
            }
            return null;
        }

        public static LaneControllerManager Deserialize(byte[] data)
        {
            try
            {
                Log.Called();
                if (data == null)
                {
                    return Create();
                }
                string data2 = Encoding.ASCII.GetString(data);
                Instance = XMLSerializerUtil.Deserialize<LaneControllerManager>(data2);
                Instance.UpdateAllLanes();
                return Instance;
            }
            catch (Exception ex)
            {
                ex.Log();
                return Create();
            }
        }

        public void UpdateAllLanes()
        {
            IEnumerable<ushort> enumerable = Lanes.Values.Select(lane => lane.LaneIdAndIndex.SegmentId).Distinct();
            Log.Called("segments:" + enumerable);
            foreach (ushort item in enumerable)
            {
                NetUtil.ToSegment(item).UpdateLanes(item, loading: true);
            }
        }

        public CustomLane GetLane(uint laneId)
        {
            return Lanes.GetorDefault(laneId);
        }

        public CustomLane[] GetLanes(ushort segmentId)
        {
            if (segmentId == 0)
            {
                return [];
            }
            return [.. (from laneIdAndIndex in new LaneIterator(segmentId)
                    select GetLane(laneIdAndIndex.LaneId) into lane
                    where lane != null
                    select lane)];
        }

        public CustomLane[] GetOrCreateLanes(ushort segmentId)
        {
            if (segmentId == 0)
            {
                return [];
            }
            return [.. new LaneIterator(segmentId).Select(GetOrCreateLane)];
        }

        public CustomLane GetOrCreateLane(LaneIdAndIndex laneIdAndIndex)
        {
            uint laneId = laneIdAndIndex.LaneId;
            if (Lanes.TryGetValue(laneId, out var value))
            {
                return value;
            }
            return Lanes[laneId] = new CustomLane(laneIdAndIndex);
        }

        public void TrimLane(uint laneId)
        {
            if (Lanes.TryGetValue(laneId, out var value) && value.IsDefault())
            {
                Lanes.Remove(laneId);
            }
        }

        public void TrimSegment(ushort segmentId)
        {
            foreach (LaneIdAndIndex item in new LaneIterator(segmentId))
            {
                TrimLane(item.LaneId);
            }
        }

        public void UpateLanes(ushort segmentId)
        {
            float num = 0f;
            int num2 = 0;
            foreach (LaneIdAndIndex item in new LaneIterator(segmentId))
            {
                GetLane(item.LaneId)?.PostfixLaneBezier();
                num += item.Lane.m_length;
                num2++;
            }
            if (num2 > 0)
            {
                NetUtil.ToSegment(segmentId).m_averageLength = num / (float)num2;
            }
        }
    }
}
