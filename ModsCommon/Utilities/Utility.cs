using System.Diagnostics;
using ColossalFramework.PlatformServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LaneController.ModsCommon.Utilities
{
    public static class Utility
    {
        public static bool InGame
        {
            get
            {
                if (!OnStartup)
                {
                    return !OnMenu;
                }
                return false;
            }
        }

        public static bool OnGame
        {
            get
            {
                string name = SceneManager.GetActiveScene().name;
                if (name != null)
                {
                    return name == "Game";
                }
                return false;
            }
        }

        public static bool OnMenu
        {
            get
            {
                string name = SceneManager.GetActiveScene().name;
                if (name != null)
                {
                    return name == "MainMenu";
                }
                return false;
            }
        }

        public static bool OnStartup
        {
            get
            {
                string name = SceneManager.GetActiveScene().name;
                if (name != null)
                {
                    return name == "Startup";
                }
                return false;
            }
        }

        public static bool AltIsPressed
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

        public static bool ShiftIsPressed
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

        public static bool CtrlIsPressed
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

        public static bool OnlyAltIsPressed
        {
            get
            {
                if (AltIsPressed && !ShiftIsPressed)
                {
                    return !CtrlIsPressed;
                }
                return false;
            }
        }

        public static bool OnlyShiftIsPressed
        {
            get
            {
                if (ShiftIsPressed && !AltIsPressed)
                {
                    return !CtrlIsPressed;
                }
                return false;
            }
        }

        public static bool OnlyCtrlIsPressed
        {
            get
            {
                if (CtrlIsPressed && !AltIsPressed)
                {
                    return !ShiftIsPressed;
                }
                return false;
            }
        }

        public static bool NotPressed
        {
            get
            {
                if (!AltIsPressed && !ShiftIsPressed)
                {
                    return !CtrlIsPressed;
                }
                return false;
            }
        }

        public static void OpenUrl(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (PlatformService.IsOverlayEnabled())
                {
                    PlatformService.ActivateGameOverlayToWebPage(url);
                }
                else
                {
                    Process.Start(url);
                }
            }
        }

        public static void OpenWorkshop(this ulong id)
        {
            id.GetWorkshopUrl().OpenUrl();
        }

        public static string GetWorkshopUrl(this ulong id)
        {
            return $"https://steamcommunity.com/sharedfiles/filedetails/?id={id}";
        }

        public static void OpenPatreon()
        {
            "https://www.patreon.com/MacSergey".OpenUrl();
        }

        public static void OpenPayPal()
        {
            "https://www.paypal.me/macsergey".OpenUrl();
        }
    }
}
