using System;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class ResetableSelectPropertyPanel<Type, PanelType> : SelectListPropertyPanel<Type, PanelType> where PanelType : ResetableSelectPropertyPanel<Type, PanelType>
    {
        protected abstract string ResetToolTip { get; }

        public event Action<PanelType> OnReset;

        public ResetableSelectPropertyPanel()
        {
            AddReset();
        }

        private void AddReset()
        {
            CustomUIButton customUIButton = AddButton(Content);
            customUIButton.size = new Vector2(20f, 20f);
            customUIButton.text = "×";
            customUIButton.tooltip = ResetToolTip;
            customUIButton.textScale = 1.3f;
            customUIButton.textPadding = new RectOffset(0, 0, 0, 0);
            customUIButton.eventClick += ResetClick;
        }

        public override void DeInit()
        {
            base.DeInit();
            OnReset = null;
        }

        protected virtual void ResetClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            Value = default;
            OnReset?.Invoke((PanelType)this);
        }
    }
}
