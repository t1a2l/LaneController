using System;
using ColossalFramework;
using LaneController.KianCommons.Utils;
using LaneController.Util;
using UnityEngine;

namespace LaneController.Tool
{
    public class SelectSegmentTool : BaseTool
    {
        private enum State
        {
            None,
            SetActive,
            SetActiveMulti
        }

        private const string kCursorInfoErrorColor = "<color #ff7e00>";

        private const string kCursorInfoNormalColor = "<color #87d3ff>";

        private const string kCursorInfoCloseColorTag = "</color>";

        public override ToolType Type => ToolType.Initial;

        public override bool ShowPanel => false;

        protected bool PreferNodeHover { get; set; }

        public ushort HoveredNodeId { get; protected set; }

        public ushort HoveredSegmentId { get; protected set; }

        public bool HoveredStartNode => NetUtil.IsStartNode(ref NetUtil.ToSegment(HoveredSegmentId), HoveredNodeId);

        public Vector3 HitPos { get; protected set; }

        protected bool HoverValid
        {
            get
            {
                if (LaneControllerTool.MouseRayValid)
                {
                    return HoveredSegmentId != 0;
                }
                return false;
            }
        }

        private static InfoManager InfoMan => Singleton<InfoManager>.instance;

        protected override void Reset()
        {
            Log.Called();
            base.Reset();
            PreferNodeHover = false;
            HoveredNodeId = 0;
            HoveredSegmentId = 0;
            Assertion.Assert(base.Tool, "Tool");
            base.Tool.ActiveSegmentId = 0;
        }

        public static void SetUnderGroundView()
        {
            InfoMan.SetCurrentMode(InfoManager.InfoMode.Underground, InfoManager.SubInfoMode.Default);
        }

        public static void SetOverGroundView()
        {
            InfoMan.SetCurrentMode(InfoManager.InfoMode.None, InfoManager.SubInfoMode.None);
        }

        protected virtual void OnPageDown()
        {
            Log.Called();
            if (LaneControllerTool.MouseRayValid)
            {
                SetUnderGroundView();
            }
        }

        protected virtual void OnPageUp()
        {
            if (LaneControllerTool.MouseRayValid)
            {
                SetOverGroundView();
            }
        }

        public override void OnKeyUp(Event e)
        {
            base.OnKeyUp(e);
            switch (e.keyCode)
            {
                case KeyCode.PageDown:
                    OnPageDown();
                    break;
                case KeyCode.PageUp:
                    OnPageUp();
                    break;
            }
        }

        private State CalculateState()
        {
            if (!HoverValid)
            {
                return State.None;
            }
            if (LaneControllerTool.ShiftIsPressed)
            {
                return State.SetActiveMulti;
            }
            return State.SetActive;
        }

        public override void OnPrimaryMouseClicked(Event e)
        {
            Log.Called();
            switch (CalculateState())
            {
                case State.SetActive:
                    base.Tool.ActiveSegmentId = HoveredSegmentId;
                    break;
                case State.SetActiveMulti:
                    base.Tool.ActiveSegmentId = HoveredSegmentId;
                    {
                        foreach (ushort similarSegmentsBetweenJunction in TraverseUtil.GetSimilarSegmentsBetweenJunctions(HoveredSegmentId))
                        {
                            base.Tool.SelectSegment(similarSegmentsBetweenJunction);
                        }
                        break;
                    }
            }
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            base.RenderOverlay(cameraInfo);
            switch (CalculateState())
            {
                case State.SetActive:
                    RenderUtil.RenderSegmentOverlay(cameraInfo, HoveredSegmentId, Colors.GameBlue);
                    break;
                case State.SetActiveMulti:
                    {
                        foreach (ushort similarSegmentsBetweenJunction in TraverseUtil.GetSimilarSegmentsBetweenJunctions(HoveredSegmentId))
                        {
                            RenderUtil.RenderSegmentOverlay(cameraInfo, similarSegmentsBetweenJunction, Colors.GameBlue);
                        }
                        break;
                    }
            }
        }

        public override string OnToolInfo()
        {
            if (HoveredSegmentId != 0)
            {
                return $"Segment #{HoveredSegmentId}\n" + "Click => select to edit lanes\nShift+Click => select until intersection";
            }
            return "Select a segment to edit lanes";
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            DetermineHoveredElements();
        }

        private bool DetermineHoveredElements()
        {
            try
            {
                HoveredSegmentId = 0;
                HoveredNodeId = 0;
                HitPos = Vector3.zero;
                if (!LaneControllerTool.MouseRayValid)
                {
                    return false;
                }
                ToolBase.RaycastInput raycastInput = new(LaneControllerTool.MouseRay, LaneControllerTool.MouseRayLength)
                {
                    m_netService = GetService(),
                    m_ignoreTerrain = true,
                    m_ignoreNodeFlags = NetNode.Flags.None
                };
                ToolBase.RaycastInput input = raycastInput;
                if (LaneControllerTool.RayCast(input, out var output))
                {
                    HoveredNodeId = output.m_netNode;
                    HitPos = output.m_hitPos;
                }
                HoveredSegmentId = GetSegmentFromNode();
                if (HoveredSegmentId != 0)
                {
                    return true;
                }
                raycastInput = new ToolBase.RaycastInput(LaneControllerTool.MouseRay, LaneControllerTool.MouseRayLength)
                {
                    m_netService = GetService(),
                    m_ignoreTerrain = true,
                    m_ignoreSegmentFlags = NetSegment.Flags.None
                };
                ToolBase.RaycastInput input2 = raycastInput;
                if (LaneControllerTool.RayCast(input2, out var output2))
                {
                    HoveredSegmentId = output2.m_netSegment;
                    HitPos = output2.m_hitPos;
                }
                if (HoveredNodeId <= 0 && HoveredSegmentId > 0)
                {
                    ushort startNode = NetUtil.ToSegment(HoveredSegmentId).m_startNode;
                    ushort endNode = NetUtil.ToSegment(HoveredSegmentId).m_endNode;
                    Vector3 vector = output2.m_hitPos - NetUtil.ToNode(startNode).m_position;
                    Vector3 vector2 = output2.m_hitPos - NetUtil.ToNode(endNode).m_position;
                    float magnitude = vector.magnitude;
                    float magnitude2 = vector2.magnitude;
                    if (magnitude < magnitude2 && magnitude < 75f)
                    {
                        HoveredNodeId = startNode;
                    }
                    else if (magnitude2 < magnitude && magnitude2 < 75f)
                    {
                        HoveredNodeId = endNode;
                    }
                }
                PreferNodeHover = output.m_netNode != 0;
                if (PreferNodeHover)
                {
                    Vector3 position = NetUtil.ToNode(HoveredNodeId).m_position;
                    Vector3 middlePosition = NetUtil.ToSegment(HoveredSegmentId).m_middlePosition;
                    PreferNodeHover = (HitPos - position).sqrMagnitude < (HitPos - middlePosition).sqrMagnitude;
                }
                return HoveredNodeId != 0 || HoveredSegmentId != 0;
            }
            catch (Exception ex)
            {
                ex.Log(showInPannel: false);
                return false;
            }
        }

        private static float GetAngle(Vector3 v1, Vector3 v2)
        {
            float num = Vector3.Angle(v1, v2);
            if (num > 180f)
            {
                num -= 180f;
            }
            return Math.Abs(num);
        }

        private ushort GetSegmentFromNode()
        {
            bool flag = false;
            ushort result = 0;
            if (HoveredNodeId != 0)
            {
                NetNode netNode = NetUtil.ToNode(HoveredNodeId);
                Vector3 v = netNode.m_position - LaneControllerTool.MousePosition;
                float num = float.MaxValue;
                for (int i = 0; i < 8; i++)
                {
                    ushort segment = netNode.GetSegment(i);
                    if (segment != 0)
                    {
                        NetSegment netSegment = NetUtil.ToSegment(segment);
                        Vector3 vector = (netSegment.m_startNode != HoveredNodeId) ? netSegment.m_endDirection : netSegment.m_startDirection;
                        float num2 = GetAngle(-vector, v);
                        if (flag)
                        {
                            num2 *= netSegment.m_averageLength;
                        }
                        if (num2 < num)
                        {
                            num = num2;
                            result = segment;
                        }
                    }
                }
            }
            return result;
        }

        public virtual ToolBase.RaycastService GetService()
        {
            InfoManager.InfoMode currentMode = Singleton<InfoManager>.instance.CurrentMode;
            InfoManager.SubInfoMode currentSubMode = Singleton<InfoManager>.instance.CurrentSubMode;
            ItemClass.Availability mode = Singleton<ToolManager>.instance.m_properties.m_mode;
            if ((mode & ItemClass.Availability.MapAndAsset) == 0)
            {
                switch (currentMode)
                {
                    case InfoManager.InfoMode.Underground:
                        if (currentSubMode == InfoManager.SubInfoMode.Default)
                        {
                            return new ToolBase.RaycastService
                            {
                                m_itemLayers = ItemClass.Layer.MetroTunnels
                            };
                        }
                        return new ToolBase.RaycastService
                        {
                            m_itemLayers = ItemClass.Layer.Default
                        };
                    case InfoManager.InfoMode.Transport:
                        return new ToolBase.RaycastService(ItemClass.Service.PublicTransport, ItemClass.SubService.None, ItemClass.Layer.Default | ItemClass.Layer.MetroTunnels);
                    default:
                        return new ToolBase.RaycastService
                        {
                            m_itemLayers = ItemClass.Layer.Default
                        };
                    case InfoManager.InfoMode.Water:
                    case InfoManager.InfoMode.Heating:
                        return new ToolBase.RaycastService
                        {
                            m_itemLayers = ItemClass.Layer.Default
                        };
                    case InfoManager.InfoMode.Fishing:
                        return new ToolBase.RaycastService
                        {
                            m_itemLayers = ItemClass.Layer.Default
                        };
                    case InfoManager.InfoMode.Traffic:
                    case InfoManager.InfoMode.TrafficRoutes:
                    case InfoManager.InfoMode.Tours:
                        return new ToolBase.RaycastService
                        {
                            m_itemLayers = ItemClass.Layer.Default | ItemClass.Layer.MetroTunnels
                        };
                }
            }
            return currentMode switch
            {
                InfoManager.InfoMode.Transport => new ToolBase.RaycastService
                {
                    m_itemLayers = ItemClass.Layer.Default | ItemClass.Layer.MetroTunnels
                },
                InfoManager.InfoMode.Traffic or InfoManager.InfoMode.Tours => new ToolBase.RaycastService
                {
                    m_itemLayers = ItemClass.Layer.Default | ItemClass.Layer.MetroTunnels | ItemClass.Layer.Markers
                },
                InfoManager.InfoMode.Underground => new ToolBase.RaycastService
                {
                    m_itemLayers = ItemClass.Layer.MetroTunnels
                },
                _ => new ToolBase.RaycastService
                {
                    m_itemLayers = ItemClass.Layer.Default | ItemClass.Layer.Markers
                },
            };
        }
    }
}
