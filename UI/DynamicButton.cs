using System;
using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using UnityEngine;

namespace LaneController.UI
{
    public abstract class DynamicButton : UIButton
    {
        private bool _isSelect;

        public static UITextureAtlas ItemAtlas { get; } = GetStyleIcons();

        public virtual Color32 NormalColor => new(29, 58, 77, byte.MaxValue);

        public virtual Color32 HoveredColor => new(44, 87, 112, byte.MaxValue);

        public virtual Color32 PressedColor => new(51, 100, 132, byte.MaxValue);

        public virtual Color32 FocusColor => new(171, 185, 196, byte.MaxValue);

        public virtual Color32 TextColor => Color.white;

        public bool IsSelect
        {
            get
            {
                return _isSelect;
            }
            set
            {
                if (_isSelect != value)
                {
                    _isSelect = value;
                    OnSelectChanged();
                }
            }
        }

        protected UILabel Label { get; set; }

        public string Text
        {
            get
            {
                return Label.text;
            }
            set
            {
                Label.text = value;
            }
        }

        private static UITextureAtlas GetStyleIcons()
        {
            string[] spriteNames = ["Item"];
            return TextureUtil.GetAtlasOrNull("ItemAtlas") ?? TextureUtil.CreateTextureAtlas("ListItem.png", "ItemAtlas", 21, 26, spriteNames, new RectOffset(1, 1, 1, 1));
        }

        public DynamicButton()
        {
            AddLabel();
            base.atlas = ItemAtlas;
            base.normalBgSprite = "Item";
            base.height = 25f;
            OnSelectChanged();
        }

        private void AddLabel()
        {
            Label = AddUIComponent<UILabel>();
            Label.textAlignment = UIHorizontalAlignment.Left;
            Label.verticalAlignment = UIVerticalAlignment.Middle;
            Label.autoSize = false;
            Label.autoHeight = false;
            Label.textScale = 0.55f;
            Label.padding = new RectOffset(50, 0, 7, 0);
            Label.autoHeight = true;
            Label.wordWrap = true;
        }

        protected virtual void OnSelectChanged()
        {
            base.color = NormalColor;
            base.hoveredColor = HoveredColor;
            base.pressedColor = PressedColor;
            base.focusedColor = FocusColor;
            Label.textColor = TextColor;
        }
    }
    public abstract class DynamicButton<DynamicObject, IconType> : DynamicButton where DynamicObject : class where IconType : UIComponent
    {
        private DynamicObject _object;

        private bool Inited { get; set; }

        public abstract string DeleteCaptionDescription { get; }

        public abstract string DeleteMessageDescription { get; }

        public DynamicObject Object
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
                Refresh();
                OnObjectSet();
            }
        }

        protected IconType Icon { get; set; }

        private UIButton DeleteButton { get; set; }

        public bool ShowIcon { get; set; }

        public bool ShowDelete { get; set; }

        public event Action<DynamicButton<DynamicObject, IconType>> OnDelete;

        public DynamicButton()
        {
            base.Label.eventSizeChanged += LabelSizeChanged;
        }

        public abstract void Init();

        public void Init(bool showIcon, bool showDelete)
        {
            if (!Inited)
            {
                ShowIcon = showIcon;
                ShowDelete = showDelete;
                if (ShowIcon)
                {
                    AddIcon();
                }
                if (ShowDelete)
                {
                    AddDeleteButton();
                }
                OnSizeChanged();
                Inited = true;
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            float num = base.size.x;
            if (ShowIcon)
            {
                Icon.relativePosition = new Vector2(0f, 0f);
                num -= 50f;
            }
            if (ShowDelete)
            {
                DeleteButton.size = new Vector2(base.size.x - 6f, base.size.y - 6f);
                DeleteButton.relativePosition = new Vector2(base.size.x - (base.size.y - 3f), 3f);
                num -= 19f;
            }
            base.Label.size = new Vector2(ShowIcon ? num : (num - 3f), base.size.y);
        }

        private void LabelSizeChanged(UIComponent component, Vector2 value)
        {
            base.Label.relativePosition = new Vector3(ShowIcon ? base.size.y : 3f, (base.size.y - base.Label.height) / 2f);
        }

        private void AddIcon()
        {
            Icon = AddUIComponent<IconType>();
            Icon.size = new Vector2(50f, 25f);
        }

        private void AddDeleteButton()
        {
            DeleteButton = AddUIComponent<UIButton>();
            DeleteButton.atlas = TextureUtil.InGameAtlas;
            DeleteButton.normalBgSprite = "buttonclose";
            DeleteButton.hoveredBgSprite = "buttonclosehover";
            DeleteButton.pressedBgSprite = "buttonclosepressed";
            DeleteButton.size = new Vector2(20f, 20f);
            DeleteButton.isEnabled = ShowDelete;
            DeleteButton.eventClick += DeleteClick;
        }

        protected override void OnSelectChanged()
        {
            if (base.IsSelect)
            {
                base.color = FocusColor;
                base.hoveredColor = FocusColor;
                base.pressedColor = FocusColor;
                base.Label.textColor = TextColor;
            }
            else
            {
                base.OnSelectChanged();
            }
        }

        private void DeleteClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            this.OnDelete?.Invoke(this);
        }

        protected virtual void OnObjectSet()
        {
        }

        public virtual void Refresh()
        {
            base.Text = Object.ToString();
        }
    }
}
