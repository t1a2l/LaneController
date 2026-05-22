using System.Linq;
using System.Runtime.CompilerServices;
using ColossalFramework.UI;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.KianCommons.UI
{
    internal static class UIExtensions
    {
        public static void FitToScreen(this UIComponent target)
        {
            Vector3 absolutePosition = target.absolutePosition;
            Log.Called($"target={target} absolutePosition={absolutePosition}");
            Vector2 screenResolution = target.GetUIView().GetScreenResolution();
            float width = target.width;
            float height = target.height;
            Vector3 absolutePosition2 = new(Mathf.Clamp(absolutePosition.x, 0f, screenResolution.x - width), Mathf.Clamp(absolutePosition.y, 0f, screenResolution.y - height));
            target.absolutePosition = absolutePosition2;
            Log.Info($"target.absolutePosition={target.absolutePosition}, resolution={screenResolution}");
            target.MakePixelPerfect();
        }

        public static T AddUIComponent<T>(this UIView view) where T : UIComponent
        {
            return view.AddUIComponent(typeof(T)) as T;
        }

        public static void DestroyFull(this UIComponent c)
        {
            c.SetAllDeclaredFieldsToNull();
            Object.Destroy(c.gameObject);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool FPSOptimisedIsVisble(this UIComponent c)
        {
            return c.isVisible;
        }

        public static T FindObjectOfType<T>(string name) where T : Object
        {
            return Object.FindObjectsOfType<T>().FirstOrDefault((item) => item.name == name);
        }

        public static T[] FindObjectsOfType<T>(string name) where T : Object
        {
            return [.. (from item in Object.FindObjectsOfType<T>()
                    where item.name == name
                    select item)];
        }
    }
}
