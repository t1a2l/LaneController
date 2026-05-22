using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using LaneController.CustomData;
using LaneController.KianCommons.Utils;
using LaneController.LifeCycle;
using LaneController.Manager;
using LaneController.UI;
using LaneController.UI.Editors;
using LaneController.UI.Marker;
using LaneController.Util;
using UnifiedUI.Helpers;
using UnityEngine;

namespace LaneController.Tool
{
    public class LaneControllerTool : ToolBase
    {
        public static Camera Camera;

        private BezierMarker bezierMarker_;

        private SegmentDTO segmentInstance_;

        private CustomLane laneInstance_;

        private readonly HashSet<ushort> selectedSegmentIds_ = [];

        public SegmentDTO Cache;

        private UIComponent UUIButton;

        private const string kCursorInfoErrorColor = "<color #ff7e00>";

        private const string kCursorInfoNormalColor = "<color #87d3ff>";

        private const string kCursorInfoCloseColorTag = "</color>";

        public static bool CtrlIsPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    return Input.GetKey(KeyCode.RightControl);
                }
                return true;
            }
        }

        public static bool ShiftIsPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    return Input.GetKey(KeyCode.RightShift);
                }
                return true;
            }
        }

        public static Ray MouseRay { get; private set; }

        public static float MouseRayLength { get; private set; }

        public static bool MouseRayValid { get; private set; }

        public static Vector3 PrevMousePosition { get; private set; }

        public static Vector3 MousePosition { get; private set; }

        public static bool MouseMoved => MousePosition != PrevMousePosition;

        public static Vector3 MouseWorldPosition { get; private set; }

        public static LaneControllerManager Man => LaneControllerManager.Instance;

        public BaseTool CurrentTool { get; private set; }

        private Dictionary<ToolType, BaseTool> Tools { get; set; } = [];

        public bool ToolEnabled => base.enabled;

        public BezierMarker BezierMarker
        {
            get
            {
                return bezierMarker_;
            }
            private set
            {
                bezierMarker_?.Destroy();
                bezierMarker_ = value;
            }
        }

        public SegmentDTO SegmentInstance
        {
            get
            {
                return segmentInstance_;
            }
            private set
            {
                segmentInstance_ = value;
                UpdateMode();
            }
        }

        public CustomLane LaneInstance
        {
            get
            {
                return laneInstance_;
            }
            private set
            {
                Log.Called(value);
                laneInstance_ = value;
                if (value != null)
                {
                    BezierMarker = new BezierMarker(value.LaneIdAndIndex);
                }
                else
                {
                    BezierMarker = null;
                }
                if (Panel?.CurrentEditor is LaneEditor laneEditor && laneEditor.EditObject != value)
                {
                    laneEditor.UpdateEditor(value);
                }
                UpdateMode();
            }
        }

        public IEnumerable<ushort> SelectedSegmentIds => selectedSegmentIds_;

        public int ActiveLaneIndex => LaneInstance?.Index ?? (-1);

        public ushort ActiveSegmentId
        {
            get
            {
                return SegmentInstance?.SegmentId ?? 0;
            }
            set
            {
                Log.Called(value);
                foreach (ushort item in selectedSegmentIds_)
                {
                    if (item != value)
                    {
                        LaneControllerManager.Instance.TrimSegment(item);
                    }
                }
                selectedSegmentIds_.Clear();
                if (value != 0)
                {
                    selectedSegmentIds_.Add(value);
                }
                SetSegment(value);
            }
        }

        private LaneControllerPanel Panel => LaneControllerPanel.Instance;

        public static LaneControllerTool Instance { get; set; }

        private UIComponent PauseMenu { get; } = UIView.library.Get("PauseMenu");

        private bool IsMouseDown { get; set; }

        private bool IsMouseMove { get; set; }

        public bool IsSegmentSelected(ushort segmentId)
        {
            return selectedSegmentIds_.Contains(segmentId);
        }

        public void SelectSegment(ushort segmnetId)
        {
            if (segmnetId != 0)
            {
                if (selectedSegmentIds_.Count == 0)
                {
                    ActiveSegmentId = segmnetId;
                    return;
                }
                selectedSegmentIds_.Add(segmnetId);
                LaneControllerManager.Instance.GetOrCreateLanes(segmnetId);
            }
        }

        public void ToggleSelectedSegment(ushort segmnetId)
        {
            if (IsSegmentSelected(segmnetId))
            {
                DeselectSegment(segmnetId);
            }
            else
            {
                SelectSegment(segmnetId);
            }
        }

        public void DeselectSegment(ushort segmentId)
        {
            if (segmentId == 0)
            {
                return;
            }
            if (segmentId == ActiveSegmentId)
            {
                ushort segment = selectedSegmentIds_.FirstOrDefault(num => num != ActiveSegmentId);
                SetSegment(segment);
            }
            selectedSegmentIds_.Remove(segmentId);
            LaneControllerManager.Instance.TrimSegment(segmentId);
        }

        private void SetSegment(ushort segmentId)
        {
            Log.Called(segmentId);
            if (segmentId == 0)
            {
                SegmentInstance = null;
                SetLane(-1);
            }
            else
            {
                int lane = LaneInstance?.Index ?? (-1);
                SegmentInstance = new SegmentDTO(segmentId);
                SetLane(lane);
            }
            NetUtil.SafeUpdateSegment(segmentId);
        }

        public void SetLane(int laneIndex)
        {
            try
            {
                Log.Called(laneIndex);
                if (laneIndex < 0)
                {
                    LaneInstance = null;
                }
                else
                {
                    LaneInstance = SegmentInstance.Lanes.ElementAtOrDefault(laneIndex);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        private void SetLane(LaneIdAndIndex laneIdAndIndex)
        {
            Log.Called(laneIdAndIndex);
            SetSegment(laneIdAndIndex.SegmentId);
            SetLane(laneIdAndIndex.LaneIndex);
        }

        public static void Copy()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            Instance.Cache = Instance.SegmentInstance.Clone();
        }

        public static void Paste()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            foreach (ushort selectedSegmentId in Instance.SelectedSegmentIds)
            {
                Instance.Cache.CopyTo(selectedSegmentId);
                NetUtil.SafeUpdateSegment(selectedSegmentId);
            }
        }

        public static void DeleteAll()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            foreach (ushort selectedSegmentId in Instance.SelectedSegmentIds)
            {
                CustomLane[] lanes = Man.GetLanes(selectedSegmentId);
                foreach (CustomLane customLane in lanes)
                {
                    customLane.Reset();
                }
                NetUtil.SafeUpdateSegment(selectedSegmentId);
            }
        }

        public static void ResetControlPoints()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            foreach (ushort selectedSegmentId in Instance.SelectedSegmentIds)
            {
                CustomLane[] lanes = Man.GetLanes(selectedSegmentId);
                foreach (CustomLane customLane in lanes)
                {
                    customLane.DeltaControlPoints = default;
                }
                NetUtil.SafeUpdateSegment(selectedSegmentId);
            }
            if (Instance.Panel?.CurrentEditor is LaneEditor laneEditor)
            {
                laneEditor.PullValues();
            }
        }

        public static void ApplyBetweenIntersections()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            CustomLane[] lanes = Instance.SegmentInstance.Lanes;
            foreach (ushort similarSegmentsBetweenJunction in TraverseUtil.GetSimilarSegmentsBetweenJunctions(Instance.ActiveSegmentId))
            {
                CustomLane[] orCreateLanes = Man.GetOrCreateLanes(similarSegmentsBetweenJunction);
                for (int i = 0; i < lanes.Length; i++)
                {
                    orCreateLanes[i].CopyFrom(lanes[i]);
                }
                NetUtil.SafeUpdateSegment(similarSegmentsBetweenJunction);
            }
        }

        public static void ApplyWholeStreet()
        {
            Log.Called("Active segment: " + Instance.ActiveSegmentId);
            CustomLane[] lanes = Instance.SegmentInstance.Lanes;
            foreach (ushort item in TraverseUtil.GetSimilarSegmentsInRoad(Instance.ActiveSegmentId))
            {
                CustomLane[] orCreateLanes = Man.GetOrCreateLanes(item);
                for (int i = 0; i < lanes.Length; i++)
                {
                    orCreateLanes[i].CopyFrom(lanes[i]);
                }
                NetUtil.SafeUpdateSegment(item);
            }
        }

        protected override void Awake()
        {
            Log.Info("LaneManagerTool.Awake()");
            Instance = this;
            base.Awake();
            Camera = UIView.GetAView().uiCamera;
            Tools = new Dictionary<ToolType, BaseTool>
            {
                {
                    ToolType.Initial,
                    new SelectSegmentTool()
                },
                {
                    ToolType.SelectLane,
                    new SelectLaneTool()
                },
                {
                    ToolType.ModifyLane,
                    new ModifyLaneTool()
                }
            };
            LaneControllerPanel.CreatePanel();
            string fullPath = UUIHelpers.GetFullPath<LaneControllerMod>(["uui_lane_controller.png"]);
            UUIButton = UUIHelpers.RegisterToolButton("LaneController", null, "Lane Controller", this, UUIHelpers.LoadTexture(fullPath), new UUIHotKeys
            {
                ActivationKey = LaneControllerSettings.ActivationShortcut
            });
            base.enabled = false;
        }

        public static LaneControllerTool Create()
        {
            Log.Called();
            GameObject gameObject = ToolsModifierControl.toolController.gameObject;
            Instance = gameObject.AddComponent<LaneControllerTool>();
            Log.Info("Tool created");
            ToolsModifierControl.SetTool<DefaultTool>();
            return Instance;
        }

        public static void Remove()
        {
            Log.Called();
            if (Instance != null)
            {
                Destroy(Instance);
                Instance = null;
                Log.Info("Tool removed");
            }
        }

        protected override void OnDestroy()
        {
            Log.Called();
            base.OnDestroy();
            LaneControllerPanel.RemovePanel();
            UUIButton?.Destroy();
            base.enabled = false;
        }

        protected override void OnEnable()
        {
            try
            {
                base.OnEnable();
                Reset();
                Singleton<InfoManager>.instance.SetCurrentMode(InfoManager.InfoMode.None, InfoManager.SubInfoMode.Default);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        protected override void OnDisable()
        {
            try
            {
                base.OnDisable();
                Reset();
                ToolsModifierControl.SetTool<DefaultTool>();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public void Reset()
        {
            SetDefaultMode();
            LaneInstance = null;
            selectedSegmentIds_.Clear();
            SegmentInstance = null;
        }

        public void SetDefaultMode()
        {
            SetMode(ToolType.Initial);
        }

        public void SetMode(ToolType mode)
        {
            SetMode(Tools[mode]);
        }

        public void SetMode(BaseTool subtool)
        {
            Log.Called(subtool);
            CurrentTool?.DeInit();
            CurrentTool = subtool;
            CurrentTool?.Init();
            BaseTool currentTool = CurrentTool;
            if (currentTool != null && currentTool.ShowPanel)
            {
                Panel.SetSegment(ActiveSegmentId);
            }
            else
            {
                Panel.Hide();
            }
        }

        public void UpdateMode()
        {
            ToolType toolType = (selectedSegmentIds_.Count != 0) ? ((laneInstance_ == null) ? ToolType.SelectLane : ToolType.ModifyLane) : ToolType.Initial;
            BaseTool currentTool = CurrentTool;
            if (currentTool == null || currentTool.Type != toolType)
            {
                SetMode(toolType);
            }
        }

        protected override void OnToolUpdate()
        {
            UIComponent pauseMenu = PauseMenu;
            if (pauseMenu is not null && pauseMenu.isVisible)
            {
                UIView.library.Hide("PauseMenu");
                base.enabled = false;
                return;
            }
            MousePosition = Input.mousePosition;
            MouseRay = Camera.main.ScreenPointToRay(MousePosition);
            MouseRayLength = Camera.main.farClipPlane;
            MouseRayValid = !UIView.IsInsideUI() && Cursor.visible;
            RaycastInput input = new(MouseRay, MouseRayLength);
            RayCast(input, out var output);
            MouseWorldPosition = output.m_hitPos;
            CurrentTool.OnUpdate();
            if (MouseRayValid && (bool)LaneControllerSettings.ShowToolTips)
            {
                ShowToolInfo2(CurrentTool.OnToolInfo());
            }
            else
            {
                ShowToolInfo(show: false, null, default);
            }
            base.OnToolUpdate();
        }

        public override void SimulationStep()
        {
            try
            {
                base.SimulationStep();
                CurrentTool.SimulationStep();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            try
            {
                base.RenderOverlay(cameraInfo);
                CurrentTool?.RenderOverlay(cameraInfo);
                if (CurrentTool.ShowPanel)
                {
                    Panel?.Render(cameraInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public void RenderLanesOverlay(RenderManager.CameraInfo cameraInfo, int laneIndex, Color color)
        {
            if (laneIndex < 0)
            {
                return;
            }
            foreach (ushort selectedSegmentId in SelectedSegmentIds)
            {
                uint laneId = NetUtil.GetLaneId(selectedSegmentId, laneIndex);
                LaneIdAndIndex laneData = new(laneId, laneIndex);
                RenderUtil.RenderLaneOverlay(cameraInfo, laneData, color, alphaBlend: true);
            }
        }

        public new static bool RayCast(RaycastInput input, out RaycastOutput output)
        {
            return ToolBase.RayCast(input, out output);
        }

        public new string GetErrorString(ToolErrors e)
        {
            return base.GetErrorString(e);
        }

        protected void ShowToolInfo2(string text)
        {
            ShowToolInfo2(show: true, text, Input.mousePosition);
        }

        protected void ShowToolInfo2(bool show, string text, Vector2 pos)
        {
            if (ToolBase.cursorInfoLabel == null)
            {
                return;
            }
            ToolErrors errors = GetErrors();
            if ((errors & (ToolErrors.ObjectCollision | ToolErrors.OutOfArea | ToolErrors.NotEnoughMoney | ToolErrors.InvalidShape | ToolErrors.TooShort | ToolErrors.SlopeTooSteep | ToolErrors.WaterNotFound | ToolErrors.HeightTooHigh | ToolErrors.CannotConnect | ToolErrors.CannotBuildOnWater | ToolErrors.GridNotFound | ToolErrors.ShoreNotFound | ToolErrors.CannotUpgrade | ToolErrors.AlreadyExists | ToolErrors.TooManyConnections | ToolErrors.ObjectOnFire | ToolErrors.NotEmpty | ToolErrors.BurnedDown | ToolErrors.TooManyObjects | ToolErrors.CannotCrossTrack | ToolErrors.PathNotFound | ToolErrors.TooMuchDirt | ToolErrors.NotEnoughDirt | ToolErrors.CanalTooClose | ToolErrors.Collapsed | ToolErrors.Cooldown | ToolErrors.ParkAreaRequired | ToolErrors.LineTooLong | ToolErrors.ParkOrRoadRequired | ToolErrors.MainGateRequired | ToolErrors.MainGateExists | ToolErrors.IndustryAreaRequired | ToolErrors.MainBuildingExists | ToolErrors.MainBuildingRequired | ToolErrors.WrongAreaType | ToolErrors.CampusAreaRequired | ToolErrors.WrongCampusAreaType | ToolErrors.CampusOrRoadRequired | ToolErrors.AdministrationBuildingRequired | ToolErrors.AdministrationBuildingExists | ToolErrors.AirportAreaRequired | ToolErrors.CantLandscapeInAirportArea | ToolErrors.ConcourseRequired | ToolErrors.TaxiwayCannotConnect | ToolErrors.PedestrianZoneRequired | ToolErrors.MainPedestrianZoneRequired | ToolErrors.CannotBePlacedOnThisRoad | ToolErrors.RoadsCannotBeConnected)) != ToolErrors.None)
            {
                bool flag = !string.IsNullOrEmpty(text);
                text = (!flag) ? string.Empty : (text + "\n");
                text += "<color #ff7e00>";
                for (ToolErrors toolErrors = ToolErrors.ObjectCollision; toolErrors <= ToolErrors.AdministrationBuildingExists; toolErrors = (ToolErrors)((int)toolErrors << 1))
                {
                    if ((errors & toolErrors) != ToolErrors.None)
                    {
                        if (flag)
                        {
                            text += "\n";
                        }
                        flag = true;
                        text += GetErrorString(toolErrors);
                    }
                }
                text += "</color>";
            }
            if (!string.IsNullOrEmpty(text) && show)
            {
                text = "<color #87d3ff>" + text + "</color>";
                ToolBase.cursorInfoLabel.isVisible = true;
                UIView uIView = ToolBase.cursorInfoLabel.GetUIView();
                Vector2 vector = (!(ToolBase.fullscreenContainer != null)) ? uIView.GetScreenResolution() : ToolBase.fullscreenContainer.size;
                Vector2 vector2 = ToolBase.cursorInfoLabel.pivot.UpperLeftToTransform(ToolBase.cursorInfoLabel.size, ToolBase.cursorInfoLabel.arbitraryPivotOffset);
                Vector3 relativePosition = uIView.ScreenPointToGUI(pos / uIView.inputScale) + vector2;
                ToolBase.cursorInfoLabel.text = text;
                if (relativePosition.x < 0f)
                {
                    relativePosition.x = 0f;
                }
                if (relativePosition.y < 0f)
                {
                    relativePosition.y = 0f;
                }
                if (relativePosition.x + ToolBase.cursorInfoLabel.width > vector.x)
                {
                    relativePosition.x = vector.x - ToolBase.cursorInfoLabel.width;
                }
                if (relativePosition.y + ToolBase.cursorInfoLabel.height > vector.y)
                {
                    relativePosition.y = vector.y - ToolBase.cursorInfoLabel.height;
                }
                ToolBase.cursorInfoLabel.relativePosition = relativePosition;
            }
            else
            {
                ToolBase.cursorInfoLabel.isVisible = false;
            }
        }

        public static bool CheckIsUnderground(Vector3 position)
        {
            float num = Singleton<TerrainManager>.instance.SampleDetailHeightSmooth(position);
            return num > position.y;
        }

        protected override void OnToolGUI(Event e)
        {
            CurrentTool.OnGUI(e);
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (MouseRayValid && e.button == 0)
                    {
                        IsMouseDown = true;
                        IsMouseMove = false;
                        CurrentTool.OnMouseDown(e);
                    }
                    break;
                case EventType.MouseDrag:
                    if (MouseRayValid)
                    {
                        IsMouseMove = true;
                        CurrentTool.OnMouseDrag(e);
                    }
                    break;
                case EventType.MouseUp:
                    if (MouseRayValid && e.button == 0)
                    {
                        if (IsMouseMove)
                        {
                            CurrentTool.OnMouseUp(e);
                        }
                        else
                        {
                            CurrentTool.OnPrimaryMouseClicked(e);
                        }
                        IsMouseDown = false;
                    }
                    else if (MouseRayValid && e.button == 1)
                    {
                        CurrentTool.OnSecondaryMouseClicked();
                    }
                    break;
                case EventType.KeyUp:
                    CurrentTool.OnKeyUp(e);
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Escape)
                    {
                        e.Use();
                        base.enabled = false;
                    }
                    break;
                case EventType.MouseMove:
                    break;
            }
        }
    }
}
