using ColossalFramework.UI;
using LaneController.UI.Editors;
using UnityEngine;

namespace LaneController.UI
{
    public class LaneIcons : BaseIcons
    {
        protected UIButton Lane { get; set; }

        protected UIButton Direction { get; set; }

        public BaseEditor.LaneType LaneType
        {
            set
            {
                if (!BaseEditor.LaneSpriteNames.TryGetValue(value, out var value2))
                {
                    value2 = "None";
                }
                UIButton lane = Lane;
                string text = Lane.normalFgSprite = value2;
                lane.normalBgSprite = text;
            }
        }

        public NetInfo.Direction DirectionType
        {
            set
            {
                if (!BaseEditor.DirectionSpriteNames.TryGetValue(value, out var value2))
                {
                    value2 = "None";
                }
                UIButton direction = Direction;
                string text = Direction.normalFgSprite = value2;
                direction.normalBgSprite = text;
            }
        }

        public void SetLaneToolTip(string toolTip)
        {
            Lane.tooltip = toolTip;
        }

        public void SetDirectionToolTip(string toolTip)
        {
            Direction.tooltip = toolTip;
        }

        public void OnTooltipEnter(UIComponent component, UIMouseEventParameter eventParam)
        {
            base.OnTooltipEnter(eventParam);
        }

        public LaneIcons()
        {
            Lane = AddUIComponent<UIButton>();
            Lane.atlas = BaseEditor.LaneAtlas;
            Lane.relativePosition = new Vector3(0f, 0f);
            Lane.size = new Vector2(25f, 25f);
            Lane.isInteractive = true;
            Direction = AddUIComponent<UIButton>();
            Direction.atlas = BaseEditor.DirectionAtlas;
            Direction.relativePosition = new Vector3(25f, 0f);
            Direction.size = new Vector2(25f, 25f);
            Direction.isInteractive = true;
            Direction.eventTooltipEnter += OnTooltipEnter;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            Lane?.size = new Vector2(base.size.x / 2f, base.size.y);
            Direction?.size = new Vector2(base.size.x / 2f, base.size.y);
        }
    }
}
