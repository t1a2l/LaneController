using System;

namespace LaneController.KianCommons.Utils
{
    internal static class LogExtensions
    {
        internal static T LogRet<T>(this T a, string m = null)
        {
            _ = m ?? ReflectionHelpers.CurrentMethod(2);
            return a;
        }

        internal static void Log(this Exception ex, string message, bool showInPannel = true)
        {
            ex.Exception(message, showInPannel);
        }

        internal static void Log(this Exception ex, bool showInPannel = true)
        {
            ex.Exception("", showInPannel);
        }
    }
}
