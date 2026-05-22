using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.Utilities
{
    public static class CommonTextures
    {
        public static UITextureAtlas Atlas;

        public static string FieldNormal => "FieldNormal";

        public static string FieldHovered => "FieldHovered";

        public static string FieldFocused => "FieldFocused";

        public static string FieldDisabled => "FieldDisabled";

        public static string FieldNormalLeft => "FieldNormalLeft";

        public static string FieldHoveredLeft => "FieldHoveredLeft";

        public static string FieldFocusedLeft => "FieldFocusedLeft";

        public static string FieldDisabledLeft => "FieldDisabledLeft";

        public static string FieldNormalRight => "FieldNormalRight";

        public static string FieldHoveredRight => "FieldHoveredRight";

        public static string FieldFocusedRight => "FieldFocusedRight";

        public static string FieldDisabledRight => "FieldDisabledRight";

        public static string FieldNormalMiddle => "FieldNormalMiddle";

        public static string FieldHoveredMiddle => "FieldHoveredMiddle";

        public static string FieldFocusedMiddle => "FieldFocusedMiddle";

        public static string FieldDisabledMiddle => "FieldDisabledMiddle";

        public static string Tab { get; }

        public static string TabNormal { get; }

        public static string TabHover { get; }

        public static string TabPressed { get; }

        public static string TabFocused { get; }

        public static string TabDisabled { get; }

        public static string Empty => "Empty";

        public static string OpacitySliderBoard { get; }

        public static string OpacitySliderColor { get; }

        public static string ColorPickerNormal { get; }

        public static string ColorPickerHovered { get; }

        public static string ColorPickerDisabled { get; }

        public static string ColorPickerColor { get; }

        public static string ColorPickerBoard { get; }

        public static string Resize { get; }

        public static string HeaderHover { get; }

        public static string CloseButtonNormal { get; }

        public static string CloseButtonHovered { get; }

        public static string CloseButtonPressed { get; }

        public static string HeaderAdditionalButton { get; }

        static CommonTextures()
        {
            Tab = "Tab";
            TabNormal = "TabNormal";
            TabHover = "TabHover";
            TabPressed = "TabPressed";
            TabFocused = "TabFocused";
            TabDisabled = "TabDisabled";
            OpacitySliderBoard = "OpacitySliderBoard";
            OpacitySliderColor = "OpacitySliderColor";
            ColorPickerNormal = "ColorPickerNormal";
            ColorPickerHovered = "ColorPickerHovered";
            ColorPickerDisabled = "ColorPickerDisabled";
            ColorPickerColor = "ColorPickerColor";
            ColorPickerBoard = "ColorPickerBoard";
            Resize = "Resize";
            HeaderHover = "HeaderHover";
            CloseButtonNormal = "CloseButtonNormal";
            CloseButtonHovered = "CloseButtonHovered";
            CloseButtonPressed = "CloseButtonPressed";
            HeaderAdditionalButton = "HeaderAdditionalButton";
            Atlas = TextureHelper.CreateAtlas("ModsCommon", new Dictionary<string, RectOffset>
            {
                [CloseButtonNormal] = new RectOffset(),
                [CloseButtonHovered] = new RectOffset(),
                [CloseButtonPressed] = new RectOffset(),
                [ColorPickerNormal] = new RectOffset(),
                [ColorPickerHovered] = new RectOffset(),
                [ColorPickerDisabled] = new RectOffset(),
                [ColorPickerColor] = new RectOffset(),
                [ColorPickerBoard] = new RectOffset(),
                [FieldNormal] = new RectOffset(4, 4, 4, 4),
                [FieldHovered] = new RectOffset(4, 4, 4, 4),
                [FieldFocused] = new RectOffset(4, 4, 4, 4),
                [FieldDisabled] = new RectOffset(4, 4, 4, 4),
                [FieldNormalLeft] = new RectOffset(4, 4, 4, 4),
                [FieldHoveredLeft] = new RectOffset(4, 4, 4, 4),
                [FieldFocusedLeft] = new RectOffset(4, 4, 4, 4),
                [FieldDisabledLeft] = new RectOffset(4, 4, 4, 4),
                [FieldNormalRight] = new RectOffset(4, 4, 4, 4),
                [FieldHoveredRight] = new RectOffset(4, 4, 4, 4),
                [FieldFocusedRight] = new RectOffset(4, 4, 4, 4),
                [FieldDisabledRight] = new RectOffset(4, 4, 4, 4),
                [FieldNormalMiddle] = new RectOffset(4, 4, 4, 4),
                [FieldHoveredMiddle] = new RectOffset(4, 4, 4, 4),
                [FieldFocusedMiddle] = new RectOffset(4, 4, 4, 4),
                [FieldDisabledMiddle] = new RectOffset(4, 4, 4, 4),
                [Tab] = new RectOffset(4, 4, 4, 4),
                [TabNormal] = new RectOffset(4, 4, 4, 0),
                [TabHover] = new RectOffset(4, 4, 4, 0),
                [TabPressed] = new RectOffset(4, 4, 4, 0),
                [TabFocused] = new RectOffset(4, 4, 4, 0),
                [TabDisabled] = new RectOffset(4, 4, 4, 0),
                [OpacitySliderBoard] = new RectOffset(),
                [OpacitySliderColor] = new RectOffset(),
                [HeaderAdditionalButton] = new RectOffset(),
                [HeaderHover] = new RectOffset(4, 4, 4, 4),
                [Empty] = new RectOffset(),
                [Resize] = new RectOffset()
            });
        }
    }
}
