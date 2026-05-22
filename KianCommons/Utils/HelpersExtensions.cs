using System;
using System.Threading;
using ColossalFramework;
using ICities;

namespace LaneController.KianCommons.Utils
{
    [Obsolete]
    internal static class HelpersExtensions
    {
        internal static bool[] ALL_BOOL = [false, true];

        [Obsolete]
        internal static bool VERBOSE
        {
            get
            {
                return Log.VERBOSE;
            }
            set
            {
                Log.VERBOSE = value;
            }
        }

        internal static AppMode CurrentMode => Singleton<SimulationManager>.instance.m_ManagersWrapper.loading.currentMode;

        internal static bool InGameOrEditor => !InStartup;

        internal static bool IsActive => InGameOrEditor;

        internal static bool InStartup => Helpers.InStartupMenu;

        internal static bool InGame => CheckGameMode(AppMode.Game);

        internal static bool InAssetEditor => CheckGameMode(AppMode.AssetEditor);

        internal static bool ShiftIsPressed => Helpers.ShiftIsPressed;

        internal static bool ControlIsPressed => Helpers.ControlIsPressed;

        internal static bool AltIsPressed => Helpers.AltIsPressed;

        internal static bool InSimulationThread()
        {
            return Thread.CurrentThread == Singleton<SimulationManager>.instance.m_simulationThread;
        }

        internal static bool CheckGameMode(AppMode mode)
        {
            try
            {
                if (CurrentMode == mode)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
    }
}
