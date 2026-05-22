using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public abstract class BaseHeaderPanel<TypeContent> : EditorItem, IReusable where TypeContent : BaseHeaderContent
    {
        bool IReusable.InCache { get; set; }

        protected override float DefaultHeight => HeaderButton.Size + 10;

        protected TypeContent Content { get; set; }

        public BaseHeaderPanel()
        {
            AddContent();
        }

        protected override void Init(float? height)
        {
            base.Init(height);
            Refresh();
        }

        private void AddContent()
        {
            Content = AddUIComponent<TypeContent>();
            Content.relativePosition = new Vector2(0f, 0f);
        }

        public virtual void Refresh()
        {
            Content.Refresh();
        }
    }
}
