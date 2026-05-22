using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.CustomData;
using LaneController.KianCommons.Utils;
using LaneController.Manager;
using LaneController.ModsCommon.UI;
using LaneController.Util;
using UnityEngine;

namespace LaneController.UI.Editors
{
    public class LaneEditor : BaseEditor<LaneItem, CustomLane, LaneIcons>
    {
        private FloatPropertyPanel ShiftField;

        private FloatPropertyPanel HeightField;

        private Vector3PropertyPanel A;

        private Vector3PropertyPanel B;

        private Vector3PropertyPanel C;

        private Vector3PropertyPanel D;

        public override string Name => "Lane Editor";

        public override string SelectionMessage => "Select lane to edit it.";

        private Vector3PropertyPanel[] DeltaControlPoints => [A, B, C, D];

        public int HoveredControlPointIndex => DeltaControlPoints.FindIndex((c) => c?.containsMouse ?? false);

        private Vector3PropertyPanel GetDeltaControlPoint(int i)
        {
            return i switch
            {
                0 => A,
                1 => B,
                2 => C,
                3 => D,
                _ => throw new ArgumentException("i:" + i),
            };
        }

        public IEnumerable<CustomLane> IterateOtherSelectedLanes()
        {
            if (EditObject == null)
            {
                yield break;
            }
            foreach (ushort selectedSegmentId in ToolInstance.SelectedSegmentIds)
            {
                int index = EditObject.Index;
                if (selectedSegmentId != EditObject.LaneIdAndIndex.SegmentId)
                {
                    uint laneId = NetUtil.GetLaneId(selectedSegmentId, index);
                    yield return LaneControllerManager.Instance.GetOrCreateLane(new LaneIdAndIndex(laneId, index));
                }
            }
        }

        public IEnumerable<CustomLane> IterateSelectedLanes()
        {
            if (EditObject == null)
            {
                yield break;
            }
            foreach (ushort selectedSegmentId in ToolInstance.SelectedSegmentIds)
            {
                int index = EditObject.Index;
                uint laneId = NetUtil.GetLaneId(selectedSegmentId, index);
                yield return LaneControllerManager.Instance.GetOrCreateLane(new LaneIdAndIndex(laneId, index));
            }
        }

        protected override void FillItems()
        {
            CustomLane[] lanes = ToolInstance.SegmentInstance.Lanes;
            foreach (CustomLane editableObject in lanes)
            {
                AddItem(editableObject);
            }
        }

        public override void Render(RenderManager.CameraInfo cameraInfo)
        {
            if (IsHoverItem)
            {
                ToolInstance.RenderLanesOverlay(cameraInfo, HoverItem.Object.Index, Color.yellow);
            }
            if (IsSelectItem)
            {
                ToolInstance.RenderLanesOverlay(cameraInfo, SelectItem.Object.Index, Color.magenta);
            }
        }

        protected override void OnObjectSelect()
        {
            try
            {
                HideEmptySelected();
                UIComponent[] array = [.. SettingsPanel.components];
                UIComponent[] array2 = array;
                foreach (UIComponent item in array2)
                {
                    DeleteUIComponent(item);
                }
                ShiftField = SettingsPanel.AddUIComponent<FloatPropertyPanel>();
                ShiftField.Init("Horizontal Shift");
                HeightField = SettingsPanel.AddUIComponent<FloatPropertyPanel>();
                HeightField.Init("Vertical Shift");
                A = SettingsPanel.AddUIComponent<Vector3PropertyPanel>();
                A.Init("Start Point");
                B = SettingsPanel.AddUIComponent<Vector3PropertyPanel>();
                B.Init("Control Point 1");
                C = SettingsPanel.AddUIComponent<Vector3PropertyPanel>();
                C.Init("Control Point 2");
                D = SettingsPanel.AddUIComponent<Vector3PropertyPanel>();
                D.Init("End Point");
                PullValues();
                AddEvents();
                ToolInstance.SetLane(EditObject.Index);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        protected override void OnObjectUpdate()
        {
            RemoveEvents();
            PullValues();
            AddEvents();
        }

        public void AddEvents()
        {
            RemoveEvents();
            ShiftField.OnValueChanged += PositionField_OnValueChanged;
            HeightField.OnValueChanged += HeightField_OnValueChanged;
            ShiftField.OnResetValue += PositionField_OnResetValue;
            HeightField.OnResetValue += HeightField_OnResetValue;
            A.OnValueChanged += DeltaControlPoints_OnValueChanged;
            B.OnValueChanged += DeltaControlPoints_OnValueChanged;
            C.OnValueChanged += DeltaControlPoints_OnValueChanged;
            D.OnValueChanged += DeltaControlPoints_OnValueChanged;
            A.OnResetValue += DeltaControlPoints_OnResetValue;
            B.OnResetValue += DeltaControlPoints_OnResetValue;
            C.OnResetValue += DeltaControlPoints_OnResetValue;
            D.OnResetValue += DeltaControlPoints_OnResetValue;
        }

        public void RemoveEvents()
        {
            ShiftField.OnValueChanged -= PositionField_OnValueChanged;
            HeightField.OnValueChanged -= HeightField_OnValueChanged;
            ShiftField.OnResetValue -= PositionField_OnResetValue;
            HeightField.OnResetValue -= HeightField_OnResetValue;
            A.OnValueChanged -= DeltaControlPoints_OnValueChanged;
            B.OnValueChanged -= DeltaControlPoints_OnValueChanged;
            C.OnValueChanged -= DeltaControlPoints_OnValueChanged;
            D.OnValueChanged -= DeltaControlPoints_OnValueChanged;
            A.OnResetValue -= DeltaControlPoints_OnResetValue;
            B.OnResetValue -= DeltaControlPoints_OnResetValue;
            C.OnResetValue -= DeltaControlPoints_OnResetValue;
            D.OnResetValue -= DeltaControlPoints_OnResetValue;
        }

        public void PullValues()
        {
            ShiftField.Value = EditObject.Shift;
            HeightField.Value = EditObject.VShift;
            A.Value = EditObject.DeltaControlPoints.a;
            B.Value = EditObject.DeltaControlPoints.b;
            C.Value = EditObject.DeltaControlPoints.c;
            D.Value = EditObject.DeltaControlPoints.d;
        }

        private void PositionField_OnValueChanged(float value)
        {
            Log.Called();
            foreach (CustomLane item in IterateSelectedLanes())
            {
                item.Shift = value;
                item.QueueUpdate();
            }
        }

        private void HeightField_OnValueChanged(float value)
        {
            Log.Called();
            foreach (CustomLane item in IterateSelectedLanes())
            {
                item.VShift = value;
                item.QueueUpdate();
            }
        }

        private void DeltaControlPoints_OnValueChanged(Vector3 value)
        {
            Log.Called();
            foreach (CustomLane item in IterateSelectedLanes())
            {
                for (int i = 0; i < 4; i++)
                {
                    item.DeltaControlPoints.ControlPoint(i) = GetDeltaControlPoint(i).Value;
                }
                item.QueueUpdate();
            }
        }

        private void PositionField_OnResetValue()
        {
            Log.Called();
            ShiftField.Value = EditObject.Shift = 0f;
        }

        private void HeightField_OnResetValue()
        {
            Log.Called();
            HeightField.Value = EditObject.VShift = 0f;
        }

        private void DeltaControlPoints_OnResetValue()
        {
            Log.Called();
            RemoveEvents();
            for (int i = 0; i < 4; i++)
            {
                ref Vector3 reference = ref EditObject.DeltaControlPoints.ControlPoint(i);
                Vector3 vector = GetDeltaControlPoint(i).Value = default;
                reference = vector;
            }
            AddEvents();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveEvents();
        }
    }
}
