using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseHeaderContent : CustomUIPanel
    {
        protected abstract Color32 ButtonHoveredColor { get; }

        protected abstract Color32 ButtonPressedColor { get; }

        protected abstract Color32 AdditionalButtonHoveredColor { get; }

        protected abstract Color32 AdditionalButtonPressedColor { get; }

        protected abstract Color32 IconNormalColor { get; }

        protected abstract Color32 IconHoverColor { get; }

        protected abstract Color32 IconPressedColor { get; }

        protected abstract Color32 IconDisabledColor { get; }

        private List<IHeaderButtonInfo> Infos { get; } = [];

        private List<IHeaderButtonInfo> MainInfos { get; set; }

        private List<IHeaderButtonInfo> AdditionalInfos { get; set; }

        private bool ShowAdditional => AdditionalInfos.Count > 0;

        private AdditionalHeaderButton Additional { get; }

        public BaseHeaderContent()
        {
            autoLayoutDirection = LayoutDirection.Horizontal;
            autoLayoutPadding = new RectOffset(0, 0, 0, 0);
            Additional = AddUIComponent<AdditionalHeaderButton>();
            Additional.tooltip = CommonLocalize.Panel_Additional;
            Additional.SetIcon(CommonTextures.Atlas, CommonTextures.HeaderAdditionalButton);
            Additional.PopupOpenedEvent += OnPopupOpened;
            Additional.PopupCloseEvent += OnPopupClose;
            SetButtonColors(Additional);
            Refresh();
        }

        public void AddButton(IHeaderButtonInfo info, bool refresh = false)
        {
            Infos.Add(info);
            if (refresh)
            {
                Refresh();
            }
        }

        private void OnPopupOpened(AdditionalHeaderButton.AdditionalPopup popup)
        {
            foreach (IHeaderButtonInfo additionalInfo in AdditionalInfos)
            {
                additionalInfo.AddButton(popup.Content, showText: true);
                additionalInfo.Button.autoSize = true;
                additionalInfo.Button.autoSize = false;
                additionalInfo.ClickedEvent += PopupClicked;
                SetAdditionalButtonColors(additionalInfo.Button);
            }
            float num = AdditionalInfos.Max((i) => i.Button.width);
            popup.Width = num;
        }

        private void OnPopupClose(AdditionalHeaderButton.AdditionalPopup popup)
        {
            foreach (IHeaderButtonInfo additionalInfo in AdditionalInfos)
            {
                additionalInfo.RemoveButton();
                additionalInfo.ClickedEvent -= PopupClicked;
            }
        }

        private void PopupClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Additional.ClosePopup();
        }

        public virtual void Refresh()
        {
            PlaceButtons();
            autoLayout = true;
            autoLayout = false;
            FitChildrenHorizontally();
            foreach (UIComponent component in components)
            {
                component.relativePosition = new Vector2(component.relativePosition.x, (height - component.height) / 2f);
            }
        }

        private void PlaceButtons()
        {
            MainInfos = [.. Infos.Where((i) => i.Visible && i.State == HeaderButtonState.Main)];
            AdditionalInfos = [.. Infos.Where((i) => i.Visible && i.State == HeaderButtonState.Additional)];
            foreach (IHeaderButtonInfo info in Infos)
            {
                info.RemoveButton();
            }
            foreach (IHeaderButtonInfo mainInfo in MainInfos)
            {
                mainInfo.AddButton(this, showText: false);
                SetButtonColors(mainInfo.Button);
            }
            Additional.isVisible = ShowAdditional;
            Additional.zOrder = int.MaxValue;
        }

        private void SetButtonColors(HeaderButton button)
        {
            button.HoveredBgColor = ButtonHoveredColor;
            button.PressedBgColor = ButtonPressedColor;
            button.FocusedBgColor = ButtonPressedColor;
            button.NormalFgColor = IconNormalColor;
            button.HoveredFgColor = IconHoverColor;
            button.PressedFgColor = IconPressedColor;
            button.DisabledFgColor = IconDisabledColor;
        }

        private void SetAdditionalButtonColors(HeaderButton button)
        {
            button.HoveredBgColor = AdditionalButtonHoveredColor;
            button.PressedBgColor = AdditionalButtonPressedColor;
            button.FocusedBgColor = AdditionalButtonPressedColor;
            button.NormalFgColor = IconNormalColor;
            button.HoveredFgColor = IconHoverColor;
            button.PressedFgColor = IconPressedColor;
            button.DisabledFgColor = IconDisabledColor;
        }
    }
}
