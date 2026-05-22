using System;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    [Obsolete]
    public class BoolPropertyPanel : EditorPropertyPanel
    {
        private UICheckBox CheckBox { get; set; }

        public bool Value
        {
            get
            {
                return CheckBox.isChecked;
            }
            set
            {
                CheckBox.isChecked = value;
            }
        }

        public event Action<bool> OnValueChanged;

        public BoolPropertyPanel()
        {
            CheckBox = Content.AddUIComponent<UICheckBox>();
            CheckBox.size = new Vector2(16f, 16f);
            CheckBox.eventCheckChanged += CheckBox_eventCheckChanged;
            AddUncheck();
            AddCheck();
        }

        private void AddUncheck()
        {
            UISprite uISprite = CheckBox.AddUIComponent<UISprite>();
            uISprite.spriteName = "check-unchecked";
            uISprite.size = new Vector2(16f, 16f);
            uISprite.relativePosition = new Vector2(0f, 0f);
        }

        private void AddCheck()
        {
            UISprite uISprite = CheckBox.AddUIComponent<UISprite>();
            uISprite.spriteName = "check-checked";
            uISprite.size = new Vector2(16f, 16f);
            uISprite.relativePosition = new Vector2(0f, 0f);
            CheckBox.checkedBoxObject = uISprite;
        }

        private void CheckBox_eventCheckChanged(UIComponent component, bool value)
        {
            OnValueChanged?.Invoke(value);
        }
    }
}
