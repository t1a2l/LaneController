using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.UI;

namespace LaneController.NodeMarkup.UI
{
    public abstract class PopupEntity<ObjectType> : CustomUIButton, IReusable
    {
        bool IReusable.InCache { get; set; }

        public virtual ObjectType Object { get; protected set; }

        public bool Selected { get; set; }

        public event Action<ObjectType> OnSelected;

        public override void Update()
        {
            base.Update();
            if (Selected)
            {
                state = ButtonState.Focused;
            }
            else if (m_IsMouseHovering)
            {
                state = ButtonState.Hovered;
            }
            else
            {
                state = ButtonState.Normal;
            }
        }

        public virtual void DeInit()
        {
            OnSelected = null;
            Selected = false;
        }

        public virtual void SetObject(ObjectType value)
        {
            Object = value;
        }

        protected void Select()
        {
            OnSelected?.Invoke(Object);
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            base.OnClick(p);
            Select();
        }
    }
}
