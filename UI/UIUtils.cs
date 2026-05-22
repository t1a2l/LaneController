using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using UnityEngine;

namespace LaneController.UI
{
    public static class UIUtils
    {
        [Flags]
        public enum FindOptions
        {
            None = 0,
            NameContains = 1
        }

        public static Color32 ButtonNormal = Color.white;

        public static Color32 ButtonHovered = new(224, 224, 224, byte.MaxValue);

        public static Color32 ButtonPressed = new(192, 192, 192, byte.MaxValue);

        public static Color32 ButtonFocused = new(160, 160, 160, byte.MaxValue);

        private static UIView UiRoot { get; set; } = null;

        private static void FindUIRoot()
        {
            UiRoot = null;
            UIView[] array = UnityEngine.Object.FindObjectsOfType<UIView>();
            foreach (UIView uIView in array)
            {
                if (uIView.transform.parent == null && uIView.name == "UIView")
                {
                    UiRoot = uIView;
                    break;
                }
            }
        }

        public static string GetTransformPath(Transform transform)
        {
            string text = transform.name;
            Transform parent = transform.parent;
            while (parent != null)
            {
                text = parent.name + "/" + text;
                parent = parent.parent;
            }
            return text;
        }

        public static T FindComponent<T>(string name, UIComponent parent = null, FindOptions options = FindOptions.None) where T : UIComponent
        {
            if (UiRoot == null)
            {
                FindUIRoot();
                if (UiRoot == null)
                {
                    return null;
                }
            }
            T[] array = UnityEngine.Object.FindObjectsOfType<T>();
            foreach (T val in array)
            {
                if (((options & FindOptions.NameContains) > FindOptions.None) ? val.name.Contains(name) : (val.name == name))
                {
                    Transform transform = (parent != null) ? parent.transform : UiRoot.transform;
                    Transform parent2 = val.transform.parent;
                    while (parent2 != null && parent2 != transform)
                    {
                        parent2 = parent2.parent;
                    }
                    if (parent2 != null)
                    {
                        return val;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<T> GetCompenentsWithName<T>(string name) where T : UIComponent
        {
            T[] array = UnityEngine.Object.FindObjectsOfType<T>();
            T[] array2 = array;
            foreach (T val in array2)
            {
                if (val.name == name)
                {
                    yield return val;
                }
            }
        }

        public static void AddScrollbar(UIComponent parent, UIScrollablePanel scrollablePanel)
        {
            UIScrollbar scrollbar = parent.AddUIComponent<UIScrollbar>();
            scrollbar.orientation = UIOrientation.Vertical;
            scrollbar.pivot = UIPivotPoint.TopLeft;
            scrollbar.minValue = 0f;
            scrollbar.value = 0f;
            scrollbar.incrementAmount = 50f;
            scrollbar.autoHide = true;
            scrollbar.width = 10f;
            UISlicedSprite uISlicedSprite = scrollbar.AddUIComponent<UISlicedSprite>();
            uISlicedSprite.relativePosition = Vector2.zero;
            uISlicedSprite.autoSize = true;
            uISlicedSprite.anchor = UIAnchorStyle.All;
            uISlicedSprite.size = uISlicedSprite.parent.size;
            uISlicedSprite.fillDirection = UIFillDirection.Vertical;
            uISlicedSprite.spriteName = "ScrollbarTrack";
            scrollbar.trackObject = uISlicedSprite;
            UISlicedSprite uISlicedSprite2 = uISlicedSprite.AddUIComponent<UISlicedSprite>();
            uISlicedSprite2.relativePosition = Vector2.zero;
            uISlicedSprite2.fillDirection = UIFillDirection.Vertical;
            uISlicedSprite2.autoSize = true;
            uISlicedSprite2.width = uISlicedSprite2.parent.width;
            uISlicedSprite2.spriteName = "ScrollbarThumb";
            scrollbar.thumbObject = uISlicedSprite2;
            scrollbar.eventValueChanged += delegate (UIComponent component, float value)
            {
                scrollablePanel.scrollPosition = new Vector2(0f, value);
            };
            parent.eventMouseWheel += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                scrollbar.value -= (float)(int)eventParam.wheelDelta * scrollbar.incrementAmount;
            };
            scrollablePanel.eventMouseWheel += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                scrollbar.value -= (float)(int)eventParam.wheelDelta * scrollbar.incrementAmount;
            };
            scrollablePanel.verticalScrollbar = scrollbar;
        }

        public static void ScrollIntoViewRecursive(this UIScrollablePanel panel, UIComponent component)
        {
            Rect rect = new Rect(panel.scrollPosition.x + (float)panel.scrollPadding.left, panel.scrollPosition.y + (float)panel.scrollPadding.top, panel.size.x - (float)panel.scrollPadding.horizontal, panel.size.y - (float)panel.scrollPadding.vertical).RoundToInt();
            Vector3 zero = Vector3.zero;
            UIComponent uIComponent = component;
            while (uIComponent != null && uIComponent != panel)
            {
                zero += uIComponent.relativePosition;
                uIComponent = uIComponent.parent;
            }
            Vector2 size = component.size;
            Rect other = new Rect(panel.scrollPosition.x + zero.x, panel.scrollPosition.y + zero.y, size.x, size.y).RoundToInt();
            if (!rect.Intersects(other))
            {
                Vector2 scrollPosition = panel.scrollPosition;
                if (other.xMin < rect.xMin)
                {
                    scrollPosition.x = other.xMin - (float)panel.scrollPadding.left;
                }
                else if (other.xMax > rect.xMax)
                {
                    scrollPosition.x = other.xMax - Mathf.Max(panel.size.x, size.x) + (float)panel.scrollPadding.horizontal;
                }
                if (other.y < rect.y)
                {
                    scrollPosition.y = other.yMin - (float)panel.scrollPadding.top;
                }
                else if (other.yMax > rect.yMax)
                {
                    scrollPosition.y = other.yMax - Mathf.Max(panel.size.y, size.y) + (float)panel.scrollPadding.vertical;
                }
                panel.scrollPosition = scrollPosition;
            }
        }

        public static void SetDefaultStyle(this UIButton button)
        {
            button.atlas = TextureUtil.InGameAtlas;
            button.normalBgSprite = "ButtonWhite";
            button.disabledBgSprite = "ButtonWhite";
            button.hoveredBgSprite = "ButtonWhite";
            button.pressedBgSprite = "ButtonWhite";
            button.color = ButtonNormal;
            button.hoveredColor = ButtonHovered;
            button.pressedColor = ButtonPressed;
            Color32 color = button.focusedTextColor = Color.black;
            Color32 textColor = button.hoveredTextColor = color;
            button.textColor = textColor;
            button.pressedTextColor = Color.white;
        }
    }
}
