using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class HeaderMoveablePanel<TypeContent> : BaseHeaderPanel<TypeContent> where TypeContent : BaseHeaderContent
    {
        protected bool CanMove
        {
            get
            {
                UIView uIView = GetUIView();
                Vector2 point = uIView.ScreenPointToGUI(Input.mousePosition / uIView.inputScale);
                return !new Rect(Content.absolutePosition, Content.size).Contains(point);
            }
        }

        private bool Move { get; set; }

        private CustomUILabel Caption { get; set; }

        public UIComponent Target { get; set; }

        private Vector3 LastPosition { get; set; }

        public string Text
        {
            get
            {
                return Caption.text;
            }
            set
            {
                Caption.text = value;
            }
        }

        public HeaderMoveablePanel()
        {
            CreateCaption();
        }

        protected override void Init(float? height)
        {
            base.Init(height);
            CaptionSizeChanged();
        }

        public override void Start()
        {
            base.Start();
            Target ??= parent;
            if (size.magnitude <= float.Epsilon)
            {
                if (parent != null)
                {
                    size = new Vector2(Target.width, 30f);
                    anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;
                    relativePosition = Vector2.zero;
                }
                else
                {
                    size = new Vector2(200f, 25f);
                }
            }
        }

        private void CreateCaption()
        {
            Caption = AddUIComponent<CustomUILabel>();
            Caption.zOrder = 0;
            Caption.autoSize = false;
            Caption.autoHeight = true;
            Caption.textAlignment = UIHorizontalAlignment.Center;
            Caption.verticalAlignment = UIVerticalAlignment.Middle;
            Caption.eventSizeChanged += delegate
            {
                CaptionSizeChanged();
            };
        }

        private void CaptionSizeChanged()
        {
            Caption.relativePosition = new Vector2(10f, (height - Caption.height) / 2f);
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            Refresh();
        }

        public override void Refresh()
        {
            Content.height = height;
            base.Refresh();
            Caption.width = width - Content.width - 20f;
            Content.relativePosition = new Vector2(Caption.width - 5f + 20f, (height - Content.height) / 2f);
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            Move = CanMove;
            (Target ?? GetRootContainer()).BringToFront();
            p.Use();
            new Plane(Target.transform.TransformDirection(Vector3.back), Target.transform.position).Raycast(p.ray, out var enter);
            LastPosition = p.ray.origin + p.ray.direction * enter;
            base.OnMouseDown(p);
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            p.Use();
            if (Move && p.buttons.IsFlagSet(UIMouseButton.Left))
            {
                UIView uIView = GetUIView();
                float num = uIView.PixelsToUnits();
                Vector3 inNormal = uIView.uiCamera.transform.TransformDirection(Vector3.back);
                new Plane(inNormal, LastPosition).Raycast(p.ray, out var enter);
                Vector3 vector = (p.ray.origin + p.ray.direction * enter).Quantize(num);
                Vector3[] corners = GetUIView().GetCorners();
                Vector3 vector2 = (Target.transform.position + vector - LastPosition).Quantize(num);
                Vector3 vector3 = Target.pivot.TransformToUpperLeft(Target.size, Target.arbitraryPivotOffset);
                Vector3 vector4 = vector3 + new Vector3(Target.size.x, 0f - Target.size.y);
                vector3 *= num;
                vector4 *= num;
                if (vector2.x + vector3.x < corners[0].x)
                {
                    vector2.x = corners[0].x - vector3.x;
                }
                if (vector2.x + vector4.x > corners[1].x)
                {
                    vector2.x = corners[1].x - vector4.x;
                }
                if (vector2.y + vector3.y > corners[0].y)
                {
                    vector2.y = corners[0].y - vector3.y;
                }
                if (vector2.y + vector4.y < corners[2].y)
                {
                    vector2.y = corners[2].y - vector4.y;
                }
                Target.transform.position = vector2;
                LastPosition = vector;
            }
            base.OnMouseMove(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            base.OnMouseUp(p);
            Target.MakePixelPerfect();
        }
    }
}
