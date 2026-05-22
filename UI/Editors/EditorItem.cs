using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using UnityEngine;

namespace LaneController.UI.Editors
{
    public abstract class EditorItem : UIPanel
    {
        protected const float defaultHeight = 30f;

        public static UITextureAtlas EditorItemAtlas { get; } = GetAtlas();

        private static UITextureAtlas GetAtlas()
        {
            string[] spriteNames = ["TextFieldPanel", "TextFieldPanelHovered", "TextFieldPanelFocus", "EmptySprite"];
            return TextureUtil.GetAtlasOrNull("EditorItemAtlas") ?? TextureUtil.CreateTextureAtlas("TextFieldPanel.png", "EditorItemAtlas", 32, 32, spriteNames, new RectOffset(4, 4, 4, 4), 2);
        }

        public virtual void Init()
        {
            Init(30f);
        }

        public void Init(float height)
        {
            if (parent is UIScrollablePanel uIScrollablePanel)
            {
                width = uIScrollablePanel.width - uIScrollablePanel.autoLayoutPadding.horizontal;
            }
            else if (parent is UIPanel uIPanel)
            {
                width = uIPanel.width - uIPanel.autoLayoutPadding.horizontal;
            }
            else
            {
                width = parent.width;
            }
            base.height = height;
        }

        protected UIButton AddButton(UIComponent parent)
        {
            UIButton uIButton = parent.AddUIComponent<UIButton>();
            uIButton.SetDefaultStyle();
            return uIButton;
        }
    }
}
