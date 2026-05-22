using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using LaneController.ModsCommon.Utilities;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public static class UIHelper
    {
        [Flags]
        public enum FindOptions
        {
            None = 0,
            NameContains = 1
        }

        public static Color32 ButtonNormal;

        public static Color32 ButtonHovered;

        public static Color32 ButtonPressed;

        public static Color32 ButtonFocused;

        private static UIView UIRoot { get; set; }

        public static CustomUIScrollbar ScrollBar { get; private set; }

        static UIHelper()
        {
            UIRoot = null;
            ButtonNormal = Color.white;
            ButtonHovered = new Color32(224, 224, 224, byte.MaxValue);
            ButtonPressed = new Color32(192, 192, 192, byte.MaxValue);
            ButtonFocused = new Color32(160, 160, 160, byte.MaxValue);
            GameObject gameObject = new(typeof(CustomUIScrollbar).Name);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            ScrollBar = gameObject.AddComponent<CustomUIScrollbar>();
            ScrollBar.orientation = UIOrientation.Vertical;
            ScrollBar.pivot = UIPivotPoint.TopLeft;
            ScrollBar.minValue = 0f;
            ScrollBar.value = 0f;
            ScrollBar.incrementAmount = 50f;
            ScrollBar.autoHide = true;
            ScrollBar.width = 10f;
            UISlicedSprite uISlicedSprite = ScrollBar.AddUIComponent<UISlicedSprite>();
            uISlicedSprite.relativePosition = Vector2.zero;
            uISlicedSprite.autoSize = true;
            uISlicedSprite.anchor = UIAnchorStyle.All;
            uISlicedSprite.size = uISlicedSprite.parent.size;
            uISlicedSprite.fillDirection = UIFillDirection.Vertical;
            uISlicedSprite.spriteName = "ScrollbarTrack";
            ScrollBar.trackObject = uISlicedSprite;
            UISlicedSprite uISlicedSprite2 = uISlicedSprite.AddUIComponent<UISlicedSprite>();
            uISlicedSprite2.relativePosition = Vector2.zero;
            uISlicedSprite2.fillDirection = UIFillDirection.Vertical;
            uISlicedSprite2.autoSize = true;
            uISlicedSprite2.width = uISlicedSprite2.parent.width;
            uISlicedSprite2.spriteName = "ScrollbarThumb";
            ScrollBar.thumbObject = uISlicedSprite2;
        }

        private static void FindUIRoot()
        {
            UIRoot = null;
            UIView[] array = UnityEngine.Object.FindObjectsOfType<UIView>();
            foreach (UIView uIView in array)
            {
                if (uIView.transform.parent == null && uIView.name == "UIView")
                {
                    UIRoot = uIView;
                    break;
                }
            }
        }

        public static bool FindComponent<T>(string name, out T component, UIComponent parent = null, FindOptions options = FindOptions.None) where T : MonoBehaviour
        {
            T val = FindComponents<T>(name, parent, options).FirstOrDefault();
            if ((object)val != null)
            {
                component = val;
                return true;
            }
            component = null;
            return false;
        }

        public static IEnumerable<T> FindComponents<T>(string name, UIComponent parent = null, FindOptions options = FindOptions.None) where T : MonoBehaviour
        {
            if (UIRoot == null)
            {
                FindUIRoot();
                if (UIRoot == null)
                {
                    yield break;
                }
            }
            T[] array = UnityEngine.Object.FindObjectsOfType<T>();
            foreach (T val in array)
            {
                if ((options & FindOptions.NameContains) > FindOptions.None ? val.name.Contains(name) : val.name == name)
                {
                    Transform transform = ((Component)((object)parent ?? UIRoot)).transform;
                    Transform parent2 = val.transform.parent;
                    while (parent2 != null && parent2 != transform)
                    {
                        parent2 = parent2.parent;
                    }
                    if (parent2 != null)
                    {
                        yield return val;
                    }
                }
            }
        }

        public static IEnumerable<T> GetCompenentsWithName<T>(string name) where T : UIComponent
        {
            return from c in UnityEngine.Object.FindObjectsOfType<T>()
                   where c.name == name
                   select c;
        }

        public static void AddScrollbar(this UIComponent parent, UIScrollablePanel scrollablePanel)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(ScrollBar.gameObject);
            parent.AttachUIComponent(gameObject);
            CustomUIScrollbar scrollbar = gameObject.GetComponent<CustomUIScrollbar>();
            scrollbar.eventValueChanged += delegate (UIComponent component, float value)
            {
                scrollablePanel.scrollPosition = new Vector2(0f, value);
            };
            parent.eventMouseWheel += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                scrollbar.value -= (int)eventParam.wheelDelta * scrollbar.incrementAmount;
            };
            scrollablePanel.eventMouseWheel += delegate (UIComponent component, UIMouseEventParameter eventParam)
            {
                scrollbar.value -= (int)eventParam.wheelDelta * scrollbar.incrementAmount;
            };
            scrollablePanel.eventSizeChanged += delegate
            {
                scrollbar.relativePosition = scrollablePanel.relativePosition + new Vector3(scrollablePanel.width, 0f);
                scrollbar.height = scrollablePanel.height;
            };
            scrollablePanel.verticalScrollbar = scrollbar;
        }

        public static void ScrollIntoViewRecursive(this UIScrollablePanel panel, UIComponent component)
        {
            Rect rect = new Rect(panel.scrollPosition.x + panel.scrollPadding.left, panel.scrollPosition.y + panel.scrollPadding.top, panel.size.x - panel.scrollPadding.horizontal, panel.size.y - panel.scrollPadding.vertical).RoundToInt();
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
                    scrollPosition.x = other.xMin - panel.scrollPadding.left;
                }
                else if (other.xMax > rect.xMax)
                {
                    scrollPosition.x = other.xMax - Mathf.Max(panel.size.x, size.x) + panel.scrollPadding.horizontal;
                }
                if (other.y < rect.y)
                {
                    scrollPosition.y = other.yMin - panel.scrollPadding.top;
                }
                else if (other.yMax > rect.yMax)
                {
                    scrollPosition.y = other.yMax - Mathf.Max(panel.size.y, size.y) + panel.scrollPadding.vertical;
                }
                panel.scrollPosition = scrollPosition;
            }
        }

        public static void SetDefaultStyle(this UIButton button)
        {
            button.atlas = TextureHelper.InGameAtlas;
            button.normalBgSprite = "ButtonWhite";
            button.disabledBgSprite = "ButtonWhite";
            button.hoveredBgSprite = "ButtonWhite";
            button.pressedBgSprite = "ButtonWhite";
            button.color = ButtonNormal;
            button.hoveredColor = ButtonHovered;
            button.pressedColor = ButtonPressed;
            button.disabledColor = ButtonFocused;
            Color32 color = button.focusedTextColor = Color.black;
            Color32 textColor = button.hoveredTextColor = color;
            button.textColor = textColor;
            textColor = button.disabledTextColor = Color.white;
            button.pressedTextColor = textColor;
        }
    }
}
