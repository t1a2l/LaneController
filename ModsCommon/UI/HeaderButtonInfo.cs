using System;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public class HeaderButtonInfo<TypeButton> : IHeaderButtonInfo where TypeButton : HeaderButton
    {
        HeaderButton IHeaderButtonInfo.Button => Button;

        public TypeButton Button { get; }

        public HeaderButtonState State { get; }

        public Func<string> TextGetter { get; }

        private Action OnClick { get; }

        public bool Visible { get; set; } = true;

        public bool Enable
        {
            get
            {
                return Button.isEnabled;
            }
            set
            {
                Button.isEnabled = value;
            }
        }

        public event MouseEventHandler ClickedEvent
        {
            add
            {
                Button.eventClicked += value;
            }
            remove
            {
                Button.eventClicked -= value;
            }
        }

        private HeaderButtonInfo(HeaderButtonState state, UITextureAtlas atlas, string sprite, Func<string> textGetter, Action onClick)
        {
            State = state;
            TextGetter = textGetter;
            OnClick = onClick;
            Button = new GameObject(typeof(TypeButton).Name).AddComponent<TypeButton>();
            Button.SetIcon(atlas, sprite);
            Button.eventClicked += ButtonClicked;
        }

        public HeaderButtonInfo(HeaderButtonState state, UITextureAtlas atlas, string sprite, string text, Action onClick = null)
            : this(state, atlas, sprite, () => text, onClick)
        {
        }

        public HeaderButtonInfo(HeaderButtonState state, UITextureAtlas atlas, string sprite, string text, Shortcut shortcut)
            : this(state, atlas, sprite, () => GetText(text, shortcut), shortcut.Press)
        {
        }

        public void AddButton(UIComponent parent, bool showText)
        {
            RemoveButton();
            parent.AttachUIComponent(Button.gameObject);
            Button.transform.parent = parent.cachedTransform;
            Button.text = showText ? TextGetter() : string.Empty;
            Button.tooltip = showText ? string.Empty : TextGetter();
        }

        public void RemoveButton()
        {
            Button.parent?.RemoveUIComponent(Button);
            Button.transform.parent = null;
        }

        private void ButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnClick?.Invoke();
        }

        protected static string GetText(string text, Shortcut shortcut)
        {
            if (!shortcut.NotSet)
            {
                return $"{text} ({shortcut})";
            }
            return text;
        }
    }
}
