using LaneController.ModsCommon.UI;
using LaneController.ModsCommon.Utilities;
using LaneController.Tool;

namespace LaneController.UI
{
    public class PanelHeader : HeaderMoveablePanel<PanelHeaderContent>
    {
        public bool Available
        {
            set
            {
                base.Content.SetAvailable(value);
            }
        }

        private HeaderButtonInfo<HeaderButton> PasteButton { get; }

        private HeaderButtonInfo<HeaderButton> BeetwenIntersectionsButton { get; }

        private HeaderButtonInfo<HeaderButton> WholeStreetButton { get; }

        public PanelHeader()
        {
            base.Content.AddButton(new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Main, LaneControllerTextures.Atlas, LaneControllerTextures.CopyHeaderButton, "Copy", LaneControllerTool.Copy));
            PasteButton = new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Main, LaneControllerTextures.Atlas, LaneControllerTextures.PasteHeaderButton, "Paste", LaneControllerTool.Paste);
            base.Content.AddButton(PasteButton);
            base.Content.AddButton(new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Main, LaneControllerTextures.Atlas, LaneControllerTextures.ResetHeaderButton, "Reset", LaneControllerTool.DeleteAll));
            base.Content.AddButton(new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Additional, LaneControllerTextures.Atlas, LaneControllerTextures.ResetControlPointsHeaderButton, "Reset control points", LaneControllerTool.ResetControlPoints));
            BeetwenIntersectionsButton = new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Additional, LaneControllerTextures.Atlas, LaneControllerTextures.BeetwenIntersectionsHeaderButton, "Apply between intersections", LaneControllerTool.ApplyBetweenIntersections);
            base.Content.AddButton(BeetwenIntersectionsButton);
            WholeStreetButton = new HeaderButtonInfo<HeaderButton>(HeaderButtonState.Additional, LaneControllerTextures.Atlas, LaneControllerTextures.WholeStreetHeaderButton, "Apply to whole street", LaneControllerTool.ApplyWholeStreet);
            base.Content.AddButton(WholeStreetButton);
        }

        public void Init(float height)
        {
            Init((float?)height);
        }

        public override void Refresh()
        {
            PasteButton.Enable = false;
            base.Refresh();
        }
    }
}
