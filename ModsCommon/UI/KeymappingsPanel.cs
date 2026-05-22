using System;
using ColossalFramework;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class KeymappingsPanel : UICustomControl
    {
        private static Shortcut EditShortcut { get; set; }

        public event Action<Shortcut> BindingChanged;

        public void AddKeymapping(Shortcut shortcut)
        {
            UIPanel uIPanel = component.AttachUIComponent(UITemplateManager.GetAsGameObject("KeyBindingTemplate")) as UIPanel;
            if (component.components.Count % 2 == 0)
            {
                uIPanel.backgroundSprite = null;
            }
            UILabel uILabel = uIPanel.Find<UILabel>("Name");
            uILabel?.text = shortcut.Label;
            UIButton uIButton = uIPanel.Find<UIButton>("Binding");
            if (uIButton is not null)
            {
                uIButton.eventKeyDown += OnBindingKeyDown;
                uIButton.eventMouseDown += OnBindingMouseDown;
                uIButton.text = shortcut.ToString();
                uIButton.objectUserData = shortcut;
            }
        }

        private void OnBindingKeyDown(UIComponent comp, UIKeyEventParameter p)
        {
            if (EditShortcut == null || IsModifierKey(p.keycode))
            {
                return;
            }
            p.Use();
            UIView.PopModal();
            if (p.keycode == KeyCode.Backspace)
            {
                EditShortcut.InputKey.value = SavedInputKey.Empty;
            }
            else if (p.keycode != KeyCode.Escape)
            {
                if (EditShortcut.IgnoreModifiers)
                {
                    EditShortcut.InputKey.value = SavedInputKey.Encode(p.keycode, control: false, shift: false, alt: false);
                }
                else
                {
                    EditShortcut.InputKey.value = SavedInputKey.Encode(p.keycode, p.control, p.shift, p.alt);
                }
            }
            (p.source as UITextComponent).text = EditShortcut.InputKey.GetLocale();
            BindingChanged?.Invoke(EditShortcut);
            EditShortcut = null;
        }

        private void OnBindingMouseDown(UIComponent comp, UIMouseEventParameter p)
        {
            if (EditShortcut == null)
            {
                p.Use();
                EditShortcut = (Shortcut)p.source.objectUserData;
                UIButton uIButton = p.source as UIButton;
                uIButton.buttonsMask = UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1 | UIMouseButton.Special2 | UIMouseButton.Special3;
                uIButton.text = CommonLocalize.Settings_PressAnyKey;
                p.source.Focus();
                UIView.PushModal(p.source);
            }
            else if (!IsUnbindableMouseButton(p.buttons))
            {
                p.Use();
                UIView.PopModal();
                if (EditShortcut.IgnoreModifiers)
                {
                    EditShortcut.InputKey.value = SavedInputKey.Encode(ButtonToKeycode(p.buttons), control: false, shift: false, alt: false);
                }
                else
                {
                    EditShortcut.InputKey.value = SavedInputKey.Encode(ButtonToKeycode(p.buttons), Utility.CtrlIsPressed, Utility.ShiftIsPressed, Utility.AltIsPressed);
                }
                UIButton uIButton2 = p.source as UIButton;
                uIButton2.text = EditShortcut.InputKey.GetLocale();
                uIButton2.buttonsMask = UIMouseButton.Left;
                BindingChanged?.Invoke(EditShortcut);
                EditShortcut = null;
            }
        }

        private KeyCode ButtonToKeycode(UIMouseButton button)
        {
            return button switch
            {
                UIMouseButton.Left => KeyCode.Mouse0,
                UIMouseButton.Right => KeyCode.Mouse1,
                UIMouseButton.Middle => KeyCode.Mouse2,
                UIMouseButton.Special0 => KeyCode.Mouse3,
                UIMouseButton.Special1 => KeyCode.Mouse4,
                UIMouseButton.Special2 => KeyCode.Mouse5,
                UIMouseButton.Special3 => KeyCode.Mouse6,
                _ => KeyCode.None,
            };
        }

        private bool IsModifierKey(KeyCode code)
        {
            if (code != KeyCode.LeftControl && code != KeyCode.RightControl && code != KeyCode.LeftShift && code != KeyCode.RightShift && code != KeyCode.LeftAlt)
            {
                return code == KeyCode.RightAlt;
            }
            return true;
        }

        private bool IsUnbindableMouseButton(UIMouseButton code)
        {
            if (code != UIMouseButton.Left)
            {
                return code == UIMouseButton.Right;
            }
            return true;
        }
    }
}
