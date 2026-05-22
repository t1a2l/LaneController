using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;

namespace LaneController.ModsCommon.Utilities
{
    public static class IntroUtility
    {
        private static List<Action> IntroActions { get; } = [];

        public static void OnLoaded(Action action)
        {
            if (UIView.GetAView() != null)
            {
                action();
                return;
            }
            Singleton<LoadingManager>.instance.m_introLoaded -= IntroLoaded;
            Singleton<LoadingManager>.instance.m_introLoaded += IntroLoaded;
            IntroActions.Add(action);
        }

        private static void IntroLoaded()
        {
            Singleton<LoadingManager>.instance.m_introLoaded -= IntroLoaded;
            foreach (Action introAction in IntroActions)
            {
                introAction();
            }
        }
    }
}
