using System.Linq;
using System.Threading;
using ColossalFramework;
using ColossalFramework.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LaneController.KianCommons.Utils
{
    internal static class Helpers
    {
        internal static string[] StartupScenes = ["IntroScreen", "IntroScreen2", "Startup", "MainMenu"];

        internal static bool InStartupMenu => StartupScenes.Contains(SceneManager.GetActiveScene().name);

        internal static bool ShiftIsPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    return Input.GetKey(KeyCode.RightShift);
                }
                return true;
            }
        }

        internal static bool ControlIsPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    return Input.GetKey(KeyCode.RightControl);
                }
                return true;
            }
        }

        internal static bool AltIsPressed
        {
            get
            {
                if (!Input.GetKey(KeyCode.LeftAlt))
                {
                    return Input.GetKey(KeyCode.RightAlt);
                }
                return true;
            }
        }

        internal static void Swap<T>(ref T a, ref T b)
        {
            T val = a;
            a = b;
            b = val;
        }

        internal static bool InSimulationThread()
        {
            return Thread.CurrentThread == Singleton<SimulationManager>.instance.m_simulationThread;
        }

        internal static bool InMainThread()
        {
            return Dispatcher.currentSafe == ThreadHelper.dispatcher;
        }
    }
}
