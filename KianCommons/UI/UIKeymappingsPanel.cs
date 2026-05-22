using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.KianCommons.UI
{
    internal class UIKeymappingsPanel : UICustomControl
    {
        private static readonly string kKeyBindingTemplate = "KeyBindingTemplate";

        private SavedInputKey m_EditingBinding;

        private int count;

        internal UIComponent AddKeymapping(string label, SavedInputKey savedInputKey)
        {
            UIPanel uIPanel = component.AttachUIComponent(UITemplateManager.GetAsGameObject(kKeyBindingTemplate)) as UIPanel;
            if (count++ % 2 == 1)
            {
                uIPanel.backgroundSprite = null;
            }
            UILabel uILabel = uIPanel.Find<UILabel>("Name");
            UIButton uIButton = uIPanel.Find<UIButton>("Binding");
            uIButton.eventKeyDown += OnBindingKeyDown;
            uIButton.eventMouseDown += OnBindingMouseDown;
            uILabel.text = label;
            uIButton.text = savedInputKey.ToLocalizedString("KEYNAME");
            uIButton.objectUserData = savedInputKey;
            uIPanel.eventVisibilityChanged += delegate
            {
                RefreshBindableInputs();
            };
            return uIButton;
        }

        private static bool IsModifierKey(KeyCode code)
        {
            if (code != KeyCode.LeftControl && code != KeyCode.RightControl && code != KeyCode.LeftShift && code != KeyCode.RightShift && code != KeyCode.LeftAlt)
            {
                return code == KeyCode.RightAlt;
            }
            return true;
        }

        private static bool IsControlDown()
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                return Input.GetKey(KeyCode.RightControl);
            }
            return true;
        }

        private static bool IsShiftDown()
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                return Input.GetKey(KeyCode.RightShift);
            }
            return true;
        }

        private static bool IsAltDown()
        {
            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                return Input.GetKey(KeyCode.RightAlt);
            }
            return true;
        }

        private static bool IsUnbindableMouseButton(UIMouseButton code)
        {
            if (code != UIMouseButton.Left)
            {
                return code == UIMouseButton.Right;
            }
            return true;
        }

        private static KeyCode ButtonToKeycode(UIMouseButton button)
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

        private void RefreshBindableInputs()
        {
            UIButton[] componentsInChildren = GetComponentsInChildren<UIButton>();
            foreach (UIButton uIButton in componentsInChildren)
            {
                if (uIButton.name == "Binding" && uIButton.objectUserData is SavedInputKey savedInputKey)
                {
                    uIButton.text = savedInputKey.ToLocalizedString("KEYNAME");
                }
            }
        }

        private void OnBindingKeyDown(UIComponent comp, UIKeyEventParameter p)
        {
            if (m_EditingBinding != null && !IsModifierKey(p.keycode))
            {
                p.Use();
                UIView.PopModal();
                KeyCode keycode = p.keycode;
                InputKey value = p.keycode == KeyCode.Escape ? m_EditingBinding.value : SavedInputKey.Encode(keycode, p.control, p.shift, p.alt);
                if (p.keycode == KeyCode.Backspace)
                {
                    value = SavedInputKey.Empty;
                }
                m_EditingBinding.value = value;
                (p.source as UITextComponent).text = m_EditingBinding.ToLocalizedString("KEYNAME");
                m_EditingBinding = null;
            }
        }

        private void OnBindingMouseDown(UIComponent comp, UIMouseEventParameter p)
        {
            if (m_EditingBinding == null)
            {
                p.Use();
                m_EditingBinding = (SavedInputKey)p.source.objectUserData;
                UIButton uIButton = p.source as UIButton;
                uIButton.buttonsMask = UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1 | UIMouseButton.Special2 | UIMouseButton.Special3;
                uIButton.text = "Press any key";
                p.source.Focus();
                UIView.PushModal(p.source);
            }
            else if (!IsUnbindableMouseButton(p.buttons))
            {
                p.Use();
                UIView.PopModal();
                InputKey value = SavedInputKey.Encode(ButtonToKeycode(p.buttons), IsControlDown(), IsShiftDown(), IsAltDown());
                m_EditingBinding.value = value;
                UIButton uIButton2 = p.source as UIButton;
                uIButton2.text = m_EditingBinding.ToLocalizedString("KEYNAME");
                uIButton2.buttonsMask = UIMouseButton.Left;
                m_EditingBinding = null;
            }
        }
    }
}
