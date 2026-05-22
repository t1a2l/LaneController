using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class SizeChanger : CustomUIPanel
    {
        private UIComponent _target;

        public UIComponent Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != null)
                {
                    _target.eventSizeChanged -= TargetSizeChanged;
                }
                _target = value;
                if (_target != null)
                {
                    _target.eventSizeChanged += TargetSizeChanged;
                }
            }
        }

        public bool HasTarget => _target != null;

        private Vector2 LastPosition { get; set; }

        private Vector2 LastSize { get; set; }

        private Vector2 CurrentPosition
        {
            get
            {
                UIView aView = UIView.GetAView();
                return aView.ScreenPointToGUI(Input.mousePosition / aView.inputScale);
            }
        }

        private void TargetSizeChanged(UIComponent component, Vector2 value)
        {
            SetPosition();
        }

        public SizeChanger()
        {
            size = new Vector2(9f, 9f);
            atlas = CommonTextures.Atlas;
            backgroundSprite = CommonTextures.Resize;
            color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 160);
        }

        public override void Start()
        {
            base.Start();
            if (Target is null)
            {
                _ = Target = parent;
            }
        }

        private void SetPosition()
        {
            relativePosition = Target.size - size;
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            p.Use();
            LastPosition = CurrentPosition;
            UIComponent target = Target;
            if (target is not null)
            {
                target.BringToFront();
                LastSize = target.size;
            }
            else
            {
                GetRootContainer().BringToFront();
                LastSize = Vector2.zero;
            }
            base.OnMouseDown(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            base.OnMouseUp(p);
            Target?.MakePixelPerfect();
            SetPosition();
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            p.Use();
            if (p.buttons.IsFlagSet(UIMouseButton.Left))
            {
                UIComponent target = Target;
                if (target is not null)
                {
                    Vector2 vector = CurrentPosition - LastPosition;
                    target.size = LastSize + vector;
                }
            }
            base.OnMouseMove(p);
        }
    }
}
