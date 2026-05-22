using System.Collections.Generic;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.UI
{
    public static class LaneControllerTextures
    {
        public static UITextureAtlas Atlas;

        public static Texture2D Texture => Atlas.texture;

        public static string CopyHeaderButton => "CopyHeaderButton";

        public static string PasteHeaderButton => "PasteHeaderButton";

        public static string ResetHeaderButton => "ResetHeaderButton";

        public static string ResetControlPointsHeaderButton => "ResetControlPointsHeaderButton";

        public static string BeetwenIntersectionsHeaderButton => "BeetwenIntersectionsHeaderButton";

        public static string WholeStreetHeaderButton => "WholeStreetHeaderButton";

        static LaneControllerTextures()
        {
            Atlas = TextureHelper.CreateAtlas("NodeMarkup", new Dictionary<string, RectOffset>
            {
                [CopyHeaderButton] = new RectOffset(),
                [PasteHeaderButton] = new RectOffset(),
                [ResetHeaderButton] = new RectOffset(),
                [ResetControlPointsHeaderButton] = new RectOffset(),
                [BeetwenIntersectionsHeaderButton] = new RectOffset(),
                [WholeStreetHeaderButton] = new RectOffset()
            });
        }
    }
}
