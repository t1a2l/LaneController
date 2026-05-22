using ColossalFramework;
using LaneController.KianCommons.UI;
using UnityEngine;

namespace LaneController.LifeCycle
{
    public static class LaneControllerSettings
    {
        public static SavedBool ShowToolTips;

        public static readonly SavedInputKey ActivationShortcut;

        public static string SettingsFile => "LaneControllerMod";

        static LaneControllerSettings()
        {
            ShowToolTips = new SavedBool("ShowToolTips", SettingsFile, def: true, autoUpdate: true);
            ActivationShortcut = new SavedInputKey("ActivationShortcut", SettingsFile, SavedInputKey.Encode(KeyCode.L, control: false, shift: false, alt: false), autoUpdate: true);
            if (GameSettings.FindSettingsFileByName(SettingsFile) == null)
            {
                GameSettings.AddSettingsFile(new SettingsFile
                {
                    fileName = SettingsFile
                });
            }
        }

        public static void OnSettingsUI(UIHelper helper)
        {
            helper.AddSavedToggle("Show Tooltips", ShowToolTips);
            UIKeymappingsPanel uIKeymappingsPanel = helper.AddKeymappingsPanel();
            uIKeymappingsPanel.AddKeymapping("Hotkey", ActivationShortcut);
        }
    }
}
