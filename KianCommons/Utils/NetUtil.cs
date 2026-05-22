using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.Math;
using UnityEngine;

namespace LaneController.KianCommons.Utils
{
    public static class NetUtil
    {
        public struct NodeSegments
        {
            public ushort[] segments;

            public int count;

            private void Add(ushort segmentID)
            {
                segments[count++] = segmentID;
            }

            public NodeSegments(ushort nodeID)
            {
                segments = new ushort[8];
                count = 0;
                ushort num = GetFirstSegment(nodeID);
                Add(num);
                while (true)
                {
                    num = num.ToSegment().GetLeftSegment(nodeID);
                    if (num != segments[0])
                    {
                        Add(num);
                        continue;
                    }
                    break;
                }
            }
        }

        // public static Dictionary<string, int> kTags = ReflectionHelpers.GetFieldValue<NetInfo>("kTags") as Dictionary<string, int>;

        public const float SAFETY_NET = 0.02f;

        public static NetManager netMan = Singleton<NetManager>.instance;

        public const float MPU = 8f;

        private static readonly NetNode[] nodeBuffer_ = netMan.m_nodes.m_buffer;

        private static readonly NetSegment[] segmentBuffer_ = netMan.m_segments.m_buffer;

        private static readonly NetLane[] laneBuffer_ = netMan.m_lanes.m_buffer;

        public static NetTool NetTool => ToolsModifierControl.GetTool<NetTool>();

        public static SimulationManager SimMan => Singleton<SimulationManager>.instance;

        public static TerrainManager TerrainMan => Singleton<TerrainManager>.instance;

        public static bool LHT => TrafficDrivesOnLeft;

        public static bool RHT => !LHT;

        public static bool TrafficDrivesOnLeft
        {
            get
            {
                SimulationManager instance = Singleton<SimulationManager>.instance;
                if (instance is null)
                {
                    return false;
                }
                return instance.m_metaData?.m_invertTraffic == SimulationMetaData.MetaBool.True;
            }
        }

        public static ref NetNode ToNode(this ushort id)
        {
            return ref nodeBuffer_[id];
        }

        public static ref NetSegment ToSegment(this ushort id)
        {
            return ref segmentBuffer_[id];
        }

        public static ref NetLane ToLane(this uint id)
        {
            return ref laneBuffer_[id];
        }

        internal static NetLane.Flags Flags(this ref NetLane lane)
        {
            return (NetLane.Flags)lane.m_flags;
        }

        internal static void SafeUpdateSegment(ushort segmentId)
        {
            Singleton<SimulationManager>.instance.m_ThreadingWrapper.QueueSimulationThread(delegate
            {
                Singleton<NetManager>.instance.UpdateSegment(segmentId);
            });
        }

        public static NetInfo.Lane GetLaneInfo(uint laneId)
        {
            return laneId.ToLane().m_segment.ToSegment().Info.m_lanes[GetLaneIndex(laneId)];
        }

        public static IEnumerable<NetInfo> IterateLoadedNetInfos()
        {
            int n = PrefabCollection<NetInfo>.LoadedCount();
            for (uint i = 0u; i < n; i++)
            {
                NetInfo loaded = PrefabCollection<NetInfo>.GetLoaded(i);
                if (!(loaded == null))
                {
                    yield return loaded;
                }
            }
        }

        public static LaneIdAndIndex GetLaneIdAndIndex(uint laneId)
        {
            Assertion.Assert(laneId != 0, "laneId!=0");
            NetLane.Flags flags = laneId.ToLane().Flags();
            bool con = (flags & NetLane.Flags.Created | NetLane.Flags.Deleted) != NetLane.Flags.Created;
            Assertion.Assert(con, "valid");
            ushort segment = laneId.ToLane().m_segment;
            foreach (LaneIdAndIndex item in new LaneIterator(segment))
            {
                if (item.LaneId == laneId)
                {
                    return item;
                }
            }
            throw new Exception("Unreachable code. " + $"lane:{laneId} segment:{segment} info:{segment.ToSegment().Info}");
        }

        public static bool IsCSUR(this NetInfo info)
        {
            if (info == null || info.m_netAI.GetType() != typeof(RoadAI) && info.m_netAI.GetType() != typeof(RoadBridgeAI) && info.m_netAI.GetType() != typeof(RoadTunnelAI))
            {
                return false;
            }
            return info.name.Contains(".CSUR ");
        }

        public static ToolBase.ToolErrors InsertNode(NetTool.ControlPoint controlPoint, out ushort nodeId, bool test = false)
        {
            ToolBase.ToolErrors result = NetTool.CreateNode(controlPoint.m_segment.ToSegment().Info, controlPoint, controlPoint, controlPoint, NetTool.m_nodePositionsSimulation, 0, test, visualize: false, autoFix: true, needMoney: false, invert: false, switchDir: false, 0, out nodeId, out ushort _, out _, out int _);
            if (!test)
            {
                nodeId.ToNode().m_flags |= NetNode.Flags.Middle | NetNode.Flags.Moveable;
            }
            return result;
        }

        public static int CountPedestrianLanes(this NetInfo info)
        {
            return info.m_lanes.Count((lane) => lane.m_laneType == NetInfo.LaneType.Pedestrian);
        }

        private static bool CheckID(this ref NetNode node1, ushort nodeId2)
        {
            ref NetNode reference = ref nodeId2.ToNode();
            if (node1.m_buildIndex == reference.m_buildIndex)
            {
                return node1.m_position == reference.m_position;
            }
            return false;
        }

        private static bool CheckID(this ref NetSegment segment1, ushort segmentId2)
        {
            ref NetSegment reference = ref segmentId2.ToSegment();
            return segment1.m_startNode == reference.m_startNode & segment1.m_endNode == reference.m_endNode;
        }

        public static ushort GetID(this ref NetNode node)
        {
            ref NetSegment reference = ref node.GetFirstSegment().ToSegment();
            if (!node.CheckID(reference.m_startNode))
            {
                return reference.m_endNode;
            }
            return reference.m_startNode;
        }

        public static ushort GetID(this ref NetSegment segment)
        {
            ref NetNode reference = ref segment.m_startNode.ToNode();
            for (int i = 0; i < 8; i++)
            {
                ushort segment2 = reference.GetSegment(i);
                if (segment.CheckID(segment2))
                {
                    return segment2;
                }
            }
            return 0;
        }

        public static ushort GetFirstSegment(ushort nodeID)
        {
            return nodeID.ToNode().GetFirstSegment();
        }

        public static ushort GetFirstSegment(this ref NetNode node)
        {
            ushort num = 0;
            for (int i = 0; i < 8; i++)
            {
                num = node.GetSegment(i);
                if (num != 0)
                {
                    break;
                }
            }
            return num;
        }

        public static Vector3 GetSegmentDir(ushort segmentID, ushort nodeID)
        {
            bool flag = IsStartNode(segmentID, nodeID);
            ref NetSegment reference = ref segmentID.ToSegment();
            if (!flag)
            {
                return reference.m_endDirection;
            }
            return reference.m_startDirection;
        }

        internal static float MaxNodeHW(ushort nodeId)
        {
            float num = 0f;
            foreach (ushort item in new NodeSegmentIterator(nodeId))
            {
                float halfWidth = item.ToSegment().Info.m_halfWidth;
                if (halfWidth > num)
                {
                    num = halfWidth;
                }
            }
            return num;
        }

        internal static Bezier3 CalculateSegmentBezier3(this ref NetSegment seg, bool bStartNode = true)
        {
            ref NetNode reference = ref seg.m_startNode.ToNode();
            ref NetNode reference2 = ref seg.m_endNode.ToNode();
            Bezier3 result = new()
            {
                a = reference.m_position,
                d = reference2.m_position
            };
            NetSegment.CalculateMiddlePoints(result.a, seg.m_startDirection, result.d, seg.m_endDirection, reference.m_flags.IsFlagSet(NetNode.Flags.Middle), reference2.m_flags.IsFlagSet(NetNode.Flags.Middle), out result.b, out result.c);
            if (!bStartNode)
            {
                result = result.Invert();
            }
            return result;
        }

        internal static void CalculateCorner(ushort segmentID, ushort nodeID, bool bLeft2, out Vector3 cornerPos, out Vector3 cornerDirection)
        {
            segmentID.ToSegment().CalculateCorner(segmentID, heightOffset: true, IsStartNode(segmentID, nodeID), !bLeft2, out cornerPos, out cornerDirection, out var _);
        }

        internal static void CalculateSegEndCenter(ushort segmentID, ushort nodeID, out Vector3 pos, out Vector3 dir)
        {
            CalculateCorner(segmentID, nodeID, bLeft2: false, out var cornerPos, out var cornerDirection);
            CalculateCorner(segmentID, nodeID, bLeft2: true, out var cornerPos2, out var cornerDirection2);
            pos = (cornerPos + cornerPos2) * 0.5f;
            dir = (cornerDirection + cornerDirection2) * 0.5f;
        }

        public static bool CanConnectPathToSegment(ushort segmentID)
        {
            return segmentID.ToSegment().CanConnectPath();
        }

        public static bool CanConnectPath(this ref NetSegment segment)
        {
            return segment.Info.m_netAI is RoadAI & segment.Info.m_hasPedestrianLanes;
        }

        public static bool CanConnectPath(this NetInfo info)
        {
            return info.m_netAI is RoadAI & info.m_hasPedestrianLanes;
        }

        internal static bool IsInvert(this ref NetSegment segment)
        {
            return segment.m_flags.IsFlagSet(NetSegment.Flags.Invert);
        }

        internal static bool Smooth(this ref NetSegment segment, bool start)
        {
            return segment.GetNode(start).ToNode().m_flags.IsFlagSet(NetNode.Flags.Middle);
        }

        internal static bool SmoothStart(this ref NetSegment segment)
        {
            return segment.Smooth(start: true);
        }

        internal static bool SmoothEnd(this ref NetSegment segment)
        {
            return segment.Smooth(start: false);
        }

        internal static bool IsJunction(this ref NetNode node)
        {
            return node.m_flags.IsFlagSet(NetNode.Flags.Junction);
        }

        internal static NetInfo.Direction Invert(this NetInfo.Direction direction, bool invert = true)
        {
            if (invert)
            {
                direction = NetInfo.InvertDirection(direction);
            }
            return direction;
        }

        public static bool IsGoingBackward(this NetInfo.Direction direction)
        {
            if ((direction & NetInfo.Direction.Both) != NetInfo.Direction.Backward)
            {
                return (direction & NetInfo.Direction.AvoidBoth) == NetInfo.Direction.AvoidForward;
            }
            return true;
        }

        public static bool IsGoingForward(this NetInfo.Direction direction)
        {
            if ((direction & NetInfo.Direction.Both) != NetInfo.Direction.Forward)
            {
                return (direction & NetInfo.Direction.AvoidBoth) == NetInfo.Direction.AvoidBackward;
            }
            return true;
        }

        public static bool IsGoingBackward(this NetInfo.Lane laneInfo, bool invertDirection = false)
        {
            return laneInfo.m_finalDirection.Invert(invertDirection).IsGoingBackward();
        }

        public static bool IsGoingForward(this NetInfo.Lane laneInfo, bool invertDirection = false)
        {
            return laneInfo.m_finalDirection.Invert(invertDirection).IsGoingForward();
        }

        public static bool IsStartNode(ushort segmentId, ushort nodeId)
        {
            return segmentId.ToSegment().m_startNode == nodeId;
        }

        public static bool IsStartNode(this ref NetSegment segment, ushort nodeId)
        {
            return segment.m_startNode == nodeId;
        }

        public static ushort GetSegmentNode(ushort segmentID, bool startNode)
        {
            return segmentID.ToSegment().GetNode(startNode);
        }

        public static ushort GetNode(this ref NetSegment segment, bool startNode)
        {
            if (!startNode)
            {
                return segment.m_endNode;
            }
            return segment.m_startNode;
        }

        public static bool HasNode(ushort segmentId, ushort nodeId)
        {
            if (segmentId.ToSegment().m_startNode != nodeId)
            {
                return segmentId.ToSegment().m_endNode == nodeId;
            }
            return true;
        }

        public static ushort GetSharedNode(ushort segmentID1, ushort segmentID2)
        {
            return segmentID1.ToSegment().GetSharedNode(segmentID2);
        }

        public static bool IsSegmentValid(ushort segmentId)
        {
            if (segmentId == 0 || segmentId >= 36864)
            {
                return false;
            }
            return segmentId.ToSegment().IsValid();
        }

        public static bool IsValid(this ref NetSegment segment)
        {
            if (!segment.Info)
            {
                return false;
            }
            return segment.m_flags.CheckFlags(NetSegment.Flags.Created, NetSegment.Flags.Deleted);
        }

        public static bool IsValid(this InstanceID instance)
        {
            if (instance.IsEmpty)
            {
                return false;
            }
            if (instance.Type == InstanceType.NetNode)
            {
                return IsNodeValid(instance.NetNode);
            }
            if (instance.Type == InstanceType.NetSegment)
            {
                return IsSegmentValid(instance.NetSegment);
            }
            if (instance.Type == InstanceType.NetLane)
            {
                return IsLaneValid(instance.NetLane);
            }
            return true;
        }

        public static void AssertSegmentValid(ushort segmentId)
        {
            Assertion.AssertNeq(segmentId, 0, "segmentId");
            Assertion.AssertGT(36864, segmentId);
            Assertion.AssertNotNull(segmentId.ToSegment().Info, $"segment:{segmentId} info");
            NetSegment.Flags flags = segmentId.ToSegment().m_flags;
            bool con = flags.CheckFlags(NetSegment.Flags.Created, NetSegment.Flags.Deleted);
            Assertion.Assert(con, $"segment {segmentId} {segmentId.ToSegment().Info} has bad flags: {flags}");
        }

        public static bool IsNodeValid(ushort nodeId)
        {
            if (nodeId == 0 || nodeId >= 32768)
            {
                return false;
            }
            return nodeId.ToNode().IsValid();
        }

        public static bool IsValid(this ref NetNode node)
        {
            if (node.Info == null)
            {
                return false;
            }
            return node.m_flags.CheckFlags(NetNode.Flags.Created, NetNode.Flags.Deleted);
        }

        public static bool IsLaneValid(uint laneId)
        {
            if (laneId != 0 && laneId < 262144)
            {
                return laneId.ToLane().Flags().CheckFlags(NetLane.Flags.Created, NetLane.Flags.Deleted);
            }
            return false;
        }

        public static ushort GetHeadNode(this ref NetSegment segment)
        {
            bool flag = (segment.m_flags & NetSegment.Flags.Invert) != 0;
            if (flag ^ LHT)
            {
                return segment.m_startNode;
            }
            return segment.m_endNode;
        }

        public static ushort GetHeadNode(ushort segmentId)
        {
            return segmentId.ToSegment().GetHeadNode();
        }

        public static ushort GetTailNode(this ref NetSegment segment)
        {
            bool flag = (segment.m_flags & NetSegment.Flags.Invert) != 0;
            if (!(flag ^ LHT))
            {
                return segment.m_startNode;
            }
            return segment.m_endNode;
        }

        public static ushort GetTailNode(ushort segmentId)
        {
            return segmentId.ToSegment().GetTailNode();
        }

        public static bool CalculateIsOneWay(ushort segmentId)
        {
            int forward = 0;
            int backward = 0;
            segmentId.ToSegment().CountLanes(segmentId, NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle, VehicleInfo.VehicleType.Car | VehicleInfo.VehicleType.Metro | VehicleInfo.VehicleType.Train | VehicleInfo.VehicleType.Tram | VehicleInfo.VehicleType.Monorail, VehicleInfo.VehicleCategory.All, ref forward, ref backward);
            return forward == 0 ^ backward == 0;
        }

        public static IEnumerable<ushort> GetCCSegList(ushort nodeID)
        {
            ushort segmentID0 = GetFirstSegment(nodeID);
            Assertion.Assert(segmentID0 != 0, "GetFirstSegment!=0");
            yield return segmentID0;
            ushort segmentID1 = segmentID0;
            while (true)
            {
                segmentID1 = segmentID1.ToSegment().GetRightSegment(nodeID);
                if (segmentID1 == 0 || segmentID1 == segmentID0)
                {
                    break;
                }
                yield return segmentID1;
            }
        }

        public static IEnumerable<ushort> GetCWSegList(ushort nodeID)
        {
            ushort segmentID0 = GetFirstSegment(nodeID);
            Assertion.Assert(segmentID0 != 0, "GetFirstSegment!=0");
            yield return segmentID0;
            ushort segmentID1 = segmentID0;
            while (true)
            {
                segmentID1 = segmentID1.ToSegment().GetLeftSegment(nodeID);
                if (segmentID1 == 0 || segmentID1 == segmentID0)
                {
                    break;
                }
                yield return segmentID1;
            }
        }

        public static NodeSegmentIterator IterateSegments(this ref NetNode node)
        {
            return new NodeSegmentIterator(node.GetID());
        }

        public static LaneIterator2 IterateLanes(this ref NetSegment segment)
        {
            return new LaneIterator2(ref segment);
        }

        public static ushort GetAnotherSegment(this ref NetNode node, ushort segmentId0)
        {
            for (int i = 0; i < 8; i++)
            {
                ushort segment = node.GetSegment(i);
                if (segment != segmentId0 && segment != 0)
                {
                    return segment;
                }
            }
            return 0;
        }

        public static void LaneTest(ushort segmentId)
        {
            string text = "STRANGE LANE ISSUE: lane count mismatch for " + $"segment:{segmentId} Info:{segmentId.ToSegment().Info} IsSegmentValid={IsSegmentValid(segmentId)}\n";
            if (segmentId.ToSegment().Info != null)
            {
                List<uint> list = [];
                for (uint num = segmentId.ToSegment().m_lanes; num != 0; num = num.ToLane().m_nextLane)
                {
                    list.Add(num);
                }
                NetInfo.Lane[] lanes = segmentId.ToSegment().Info.m_lanes;
                if (list.Count == lanes.Length)
                {
                    return;
                }
                string text2 = "laneIDs=\n";
                foreach (uint item in list)
                {
                    text2 += $"\tlaneID:{item} flags:{item.ToLane().m_flags} segment:{item.ToLane().m_segment} bezier.a={item.ToLane().m_bezier.a}\n";
                }
                string text3 = "laneInfoss=\n";
                for (int i = 0; i < lanes.Length; i++)
                {
                    text3 += $"\tlaneID:{lanes[i]} dir:{lanes[i].m_direction} laneType:{lanes[i].m_laneType} vehicleType:{lanes[i].m_vehicleType} pos:{lanes[i].m_position}\n";
                }
                text = text + text2 + text3;
            }
            Log.Error(text);
            Log.LogToFileSimple("NodeControler.Strange.log", text);
        }

        public static IEnumerable<uint> IterateNodeLanes(ushort nodeId)
        {
            int idx = 0;
            if (nodeId.ToNode().Info == null)
            {
                Log.Error("null info: potentially caused by missing assets");
                yield break;
            }
            uint laneID = nodeId.ToNode().m_lane;
            while (laneID != 0)
            {
                yield return laneID;
                laneID = laneID.ToLane().m_nextLane;
                idx++;
            }
        }

        public static NetInfo.Lane SortedLane(this NetInfo info, int index)
        {
            int num = info.m_sortedLanes[index];
            return info.m_lanes[num];
        }

        public static IEnumerable<NetInfo.Lane> SortedLanes(this NetInfo info)
        {
            int i = 0;
            while (i < info.m_sortedLanes.Length)
            {
                int num = info.m_sortedLanes[i];
                yield return info.m_lanes[num];
                int num2 = i + 1;
                i = num2;
            }
        }

        public static LaneIdAndIndex[] GetSortedLanes(ushort segmentId, bool? startNode = null, NetInfo.LaneType? laneType = null, VehicleInfo.VehicleType? vehicleType = null)
        {
            LaneDataIterator laneDataIterator = new(segmentId, startNode, laneType, vehicleType);
            return [.. laneDataIterator.OrderBy((lane) => lane.LaneInfo.m_position)];
        }

        public static int GetLaneIndex(uint laneID)
        {
            ushort segment = laneID.ToLane().m_segment;
            uint num = segment.ToSegment().m_lanes;
            int num2 = segment.ToSegment().Info.m_lanes.Length;
            for (int i = 0; i < num2; i++)
            {
                if (num == 0)
                {
                    break;
                }
                if (num == laneID)
                {
                    return i;
                }
                num = num.ToLane().m_nextLane;
            }
            return -1;
        }

        public static uint GetLaneId(ushort segmentID, int laneIndex)
        {
            uint num = segmentID.ToSegment().m_lanes;
            int num2 = segmentID.ToSegment().Info.m_lanes.Length;
            for (int i = 0; i < num2; i++)
            {
                if (num == 0)
                {
                    break;
                }
                if (i == laneIndex)
                {
                    return num;
                }
                num = num.ToLane().m_nextLane;
            }
            return 0u;
        }

        public static string PrintSegmentLanes(ushort segmentID)
        {
            ref NetSegment reference = ref segmentID.ToSegment();
            List<string> list = [$"ushort segment:{segmentID} info:{reference.Info}"];
            NetInfo.Lane[] lanes = reference.Info.m_lanes;
            int num = 0;
            for (uint num2 = reference.m_lanes; num2 != 0; num2 = num2.ToLane().m_nextLane)
            {
                if (num < lanes.Length)
                {
                    NetInfo.Lane lane = lanes[num];
                    list.Add($"lane[{num}]:{num2} {lane.m_laneType}:{lane.m_vehicleType}");
                }
                else
                {
                    list.Add($"WARNING: laneId:{num2} laneIndex:{num} exceeds laneCount:{lanes.Length} lane.segment:{num2.ToLane().m_segment}");
                }
                num++;
            }
            return list.Join("\n");
        }
    }
}
