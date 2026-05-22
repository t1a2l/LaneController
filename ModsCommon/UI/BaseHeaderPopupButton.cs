using System;
using System.Linq;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseHeaderPopupButton<PopupType> : HeaderButton where PopupType : UIComponent
    {
        public PopupType Popup { get; private set; }

        public event Action PopupOpenEvent;

        public event Action<PopupType> PopupOpenedEvent;

        public event Action<PopupType> PopupCloseEvent;

        public event Action PopupClosedEvent;

        protected override void OnClick(UIMouseEventParameter p)
        {
            if (Popup == null)
            {
                OpenPopup();
            }
            else
            {
                ClosePopup();
            }
        }

        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            if (!isVisible)
            {
                ClosePopup();
            }
        }

        protected void OpenPopup()
        {
            OnPopupOpen();
            UIComponent rootContainer = GetRootContainer();
            Popup = rootContainer.AddUIComponent<PopupType>();
            Popup.eventLostFocus += OnPopupLostFocus;
            Popup.eventKeyDown += OnPopupKeyDown;
            Popup.Focus();
            OnPopupOpened();
            SetPopupPosition();
            Popup.parent.eventPositionChanged += SetPopupPosition;
        }

        public virtual void ClosePopup()
        {
            if (Popup != null)
            {
                OnPopupClose();
                Popup.eventLostFocus -= OnPopupLostFocus;
                Popup.eventKeyDown -= OnPopupKeyDown;
                UIComponent[] array = [.. Popup.components];
                foreach (UIComponent component in array)
                {
                    ComponentPool.Free(component);
                }
                ComponentPool.Free(Popup);
                Popup = null;
                OnPopupClosed();
            }
        }

        protected virtual void OnPopupOpen()
        {
            PopupOpenEvent?.Invoke();
        }

        protected virtual void OnPopupOpened()
        {
            PopupOpenedEvent?.Invoke(Popup);
        }

        protected virtual void OnPopupClose()
        {
            PopupCloseEvent?.Invoke(Popup);
        }

        protected virtual void OnPopupClosed()
        {
            PopupClosedEvent?.Invoke();
        }

        private void OnPopupLostFocus(UIComponent component, UIFocusEventParameter eventParam)
        {
            UIView uIView = Popup.GetUIView();
            Vector2 point = uIView.ScreenPointToGUI(Input.mousePosition / uIView.inputScale);
            Rect rect = new(Popup.absolutePosition, Popup.size);
            Rect rect2 = new(absolutePosition, size);
            if (!rect.Contains(point) && !rect2.Contains(point))
            {
                ClosePopup();
            }
            else
            {
                Popup.Focus();
            }
        }

        private void OnPopupKeyDown(UIComponent component, UIKeyEventParameter p)
        {
            if (p.keycode == KeyCode.Escape)
            {
                ClosePopup();
                p.Use();
            }
        }

        private void SetPopupPosition(UIComponent component = null, Vector2 value = default)
        {
            if (Popup != null)
            {
                UIView uIView = Popup.GetUIView();
                Vector2 screenResolution = uIView.GetScreenResolution();
                Vector3 vector = absolutePosition + new Vector3(0f, height);
                vector.x = MathPos(vector.x, Popup.width, screenResolution.x);
                vector.y = MathPos(vector.y, Popup.height, screenResolution.y);
                Popup.relativePosition = vector - Popup.parent.absolutePosition;
            }
            static float MathPos(float pos, float size, float screen)
            {
                if (!(pos + size > screen))
                {
                    return Mathf.Max(pos, 0f);
                }
                if (!(screen - size < 0f))
                {
                    return screen - size;
                }
                return 0f;
            }
        }
    }
}
