using LaneController.KianCommons.Utils;
using LaneController.Util;
using UnityEngine;

namespace LaneController.Tool
{
    public class SelectLaneTool : SelectSegmentTool
    {
        private enum State
        {
            None,
            SelectLane,
            SetActiveSegment,
            SetActiveSegmentMulti,
            SelectSegment,
            Illigal,
            Deselect,
            SelectSegmentMutli
        }

        public override ToolType Type => ToolType.SelectLane;

        public override bool ShowPanel => true;

        public LaneIdAndIndex HoveredLaneIdAndIndex { get; private set; }

        protected override void Reset()
        {
            Log.Called();
            base.PreferNodeHover = false;
            base.HoveredNodeId = 0;
            base.HoveredSegmentId = 0;
            base.Tool.SetLane(-1);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (base.Tool.IsSegmentSelected(base.HoveredSegmentId) && NetUtil.ToSegment(base.HoveredSegmentId).GetClosestLanePosition(LaneControllerTool.MouseWorldPosition, NetInfo.LaneType.All, VehicleInfo.VehicleType.All, VehicleInfo.VehicleCategory.All, out var _, out var laneID, out var laneIndex, out var _))
            {
                HoveredLaneIdAndIndex = new LaneIdAndIndex(laneID, laneIndex);
            }
            else
            {
                HoveredLaneIdAndIndex = default;
            }
        }

        public override string OnToolInfo()
        {
            return CalculateState() switch
            {
                State.SelectLane => $"Click => edit lane[{HoveredLaneIdAndIndex.LaneIndex}]\n" + $"Ctrl+Click => Deselect segment #{base.HoveredSegmentId}",
                State.Deselect => $"Deselect Segment #{base.HoveredSegmentId}",
                State.SetActiveSegment => $"Segment #{base.HoveredSegmentId}\n" + "Click => Select this segment\nShift+Click => Select until intersection\nCtrl+Click => Add to selection\nShift+Click => Add to selection until intersection",
                State.SetActiveSegmentMulti => "Select until intersection.\n",
                State.Illigal => "Cannot add mismatching segment to selection.\nSegment must match selection.",
                State.SelectSegment => $"Add Segment #{base.HoveredSegmentId} to selection",
                State.SelectSegmentMutli => "Add to selection until intersection",
                _ => "Select a lane to edit\nOr select more segments",
            };
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            State state = CalculateState();
            Color? color;
            switch (state)
            {
                case State.None:
                    return;
                case State.SelectLane:
                    base.Tool.RenderLanesOverlay(cameraInfo, HoveredLaneIdAndIndex.LaneIndex, Color.yellow);
                    return;
                case State.SetActiveSegment:
                    color = Colors.GameBlue;
                    break;
                case State.SetActiveSegmentMulti:
                    color = Colors.GameBlue;
                    break;
                case State.SelectSegment:
                    color = Color.green;
                    break;
                case State.SelectSegmentMutli:
                    color = Color.green;
                    break;
                case State.Deselect:
                    color = Colors.OrangeWeb;
                    break;
                case State.Illigal:
                    color = Color.red;
                    break;
                default:
                    color = null;
                    break;
            }
            Color? color2 = color;
            bool flag = state == State.SelectSegmentMutli || state == State.SetActiveSegmentMulti;
            if (color2.HasValue)
            {
                RenderUtil.RenderSegmentOverlay(cameraInfo, base.HoveredSegmentId, color2.Value);
                if (flag)
                {
                    foreach (ushort similarSegmentsBetweenJunction in TraverseUtil.GetSimilarSegmentsBetweenJunctions(base.HoveredSegmentId))
                    {
                        RenderUtil.RenderSegmentOverlay(cameraInfo, similarSegmentsBetweenJunction, color2.Value);
                    }
                }
            }
            if (state != State.SelectSegment && state != State.SelectSegmentMutli && state != State.Deselect && state != State.Illigal)
            {
                return;
            }
            foreach (ushort selectedSegmentId in base.Tool.SelectedSegmentIds)
            {
                if (selectedSegmentId != base.HoveredSegmentId)
                {
                    RenderUtil.RenderSegmentOverlay(cameraInfo, selectedSegmentId, Colors.GameBlue);
                }
            }
        }

        public override void OnPrimaryMouseClicked(Event e)
        {
            Log.Called();
            switch (CalculateState())
            {
                case State.None:
                    break;
                case State.SelectLane:
                    base.Tool.SetLane(HoveredLaneIdAndIndex.LaneIndex);
                    break;
                case State.SetActiveSegment:
                    base.Tool.ActiveSegmentId = base.HoveredSegmentId;
                    break;
                case State.SetActiveSegmentMulti:
                    base.Tool.ActiveSegmentId = base.HoveredSegmentId;
                    SelectMulti();
                    break;
                case State.SelectSegment:
                    base.Tool.SelectSegment(base.HoveredSegmentId);
                    break;
                case State.SelectSegmentMutli:
                    SelectMulti();
                    break;
                case State.Deselect:
                    base.Tool.DeselectSegment(base.HoveredSegmentId);
                    break;
                case State.Illigal:
                    break;
            }
            void SelectMulti()
            {
                foreach (ushort similarSegmentsBetweenJunction in TraverseUtil.GetSimilarSegmentsBetweenJunctions(base.HoveredSegmentId))
                {
                    base.Tool.SelectSegment(similarSegmentsBetweenJunction);
                }
            }
        }

        public override void OnSecondaryMouseClicked()
        {
            base.Tool.ActiveSegmentId = 0;
            base.OnSecondaryMouseClicked();
        }

        private State CalculateState()
        {
            if (!base.HoverValid)
            {
                return State.None;
            }
            bool flag = base.Tool.IsSegmentSelected(base.HoveredSegmentId);
            if (!Helpers.ControlIsPressed)
            {
                if (Helpers.ShiftIsPressed)
                {
                    return State.SetActiveSegmentMulti;
                }
                if (!flag)
                {
                    return State.SetActiveSegment;
                }
                return State.SelectLane;
            }
            if (HoverMatchesSelection())
            {
                if (Helpers.ShiftIsPressed)
                {
                    return State.SelectSegmentMutli;
                }
                if (!flag)
                {
                    return State.SelectSegment;
                }
                return State.Deselect;
            }
            return State.Illigal;
        }

        private bool HoverMatchesSelection()
        {
            if (base.Tool.ActiveSegmentId != 0)
            {
                return NetUtil.ToSegment(base.Tool.ActiveSegmentId).Info == NetUtil.ToSegment(base.HoveredSegmentId).Info;
            }
            return true;
        }
    }
}
