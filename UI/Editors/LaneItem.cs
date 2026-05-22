using System;
using LaneController.CustomData;
using LaneController.LifeCycle;
using LaneController.Util;

namespace LaneController.UI.Editors
{
    public class LaneItem : DynamicButton<CustomLane, LaneIcons>
    {
        public override string DeleteCaptionDescription => "Delete";

        public override string DeleteMessageDescription => "Delete2";

        public override void Init()
        {
            Init(showIcon: true, showDelete: false);
        }

        protected override void OnObjectSet()
        {
            SetIcon();
        }

        public override void Refresh()
        {
            base.Refresh();
            SetIcon();
        }

        private void SetIcon()
        {
            if (!ShowIcon)
            {
                return;
            }
            switch (Object.LaneInfo.m_laneType)
            {
                case NetInfo.LaneType.Pedestrian:
                    Icon.LaneType = BaseEditor.LaneType.Pedestrian;
                    break;
                case NetInfo.LaneType.Parking:
                    Icon.LaneType = BaseEditor.LaneType.Parking;
                    break;
                case NetInfo.LaneType.None:
                    Icon.LaneType = BaseEditor.LaneType.None;
                    break;
                case NetInfo.LaneType.Vehicle:
                    switch (Object.LaneInfo.m_vehicleType)
                    {
                        case VehicleInfo.VehicleType.Bicycle:
                            Icon.LaneType = BaseEditor.LaneType.Bicycle;
                            break;
                        case VehicleInfo.VehicleType.Car:
                            Icon.LaneType = BaseEditor.LaneType.Car;
                            break;
                        case VehicleInfo.VehicleType.Tram:
                            Icon.LaneType = BaseEditor.LaneType.Tram;
                            break;
                        case VehicleInfo.VehicleType.CableCar:
                            Icon.LaneType = BaseEditor.LaneType.Cablecar;
                            break;
                        case VehicleInfo.VehicleType.Monorail:
                            Icon.LaneType = BaseEditor.LaneType.Monorail;
                            break;
                        case VehicleInfo.VehicleType.Metro:
                            Icon.LaneType = BaseEditor.LaneType.Metro;
                            break;
                        case VehicleInfo.VehicleType.Trolleybus:
                            Icon.LaneType = BaseEditor.LaneType.Trolleybus;
                            break;
                        case VehicleInfo.VehicleType.Train:
                            Icon.LaneType = BaseEditor.LaneType.Train;
                            break;
                        default:
                            if (MathUtil.OnesCount32(Convert.ToUInt32(Object.LaneInfo.m_vehicleType)) > 1)
                            {
                                Icon.LaneType = BaseEditor.LaneType.Multiple;
                            }
                            else
                            {
                                Icon.LaneType = BaseEditor.LaneType.None;
                            }
                            break;
                    }
                    break;
                default:
                    Icon.LaneType = BaseEditor.LaneType.None;
                    break;
            }
            Icon.DirectionType = Object.LaneInfo.m_finalDirection;
            Icon.SetDirectionToolTip(LaneControllerSettings.ShowToolTips ? Object.LaneInfo.m_finalDirection.ToString() : string.Empty);
            Icon.SetLaneToolTip(LaneControllerSettings.ShowToolTips ? Object.LaneInfo.m_vehicleType.ToString() : string.Empty);
        }
    }
}
