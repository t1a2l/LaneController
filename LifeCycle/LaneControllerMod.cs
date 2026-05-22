using CitiesHarmony.API;
using LaneController.KianCommons.Utils;
using LaneController.Manager;

namespace LaneController.LifeCycle
{
    public class LaneControllerMod : LifeCycleBase
    {
        public const string HARMONY_ID = "cs.lanecontroller";

        public override string ModName => "Lane Controller";

        public override string Description => "Adjust lane paths.";

        public override void Start()
        {
            HarmonyHelper.DoOnHarmonyReady(delegate
            {
                HarmonyUtil.InstallHarmony("cs.lanecontroller");
            });
        }

        public override void OnSettingsUI(UIHelper helper)
        {
            LaneControllerSettings.OnSettingsUI(helper);
        }

        public override void Load()
        {
            LaneControllerManager.Ensure();
        }

        public override void HotReload()
        {
            SerializableDataExtension.Load();
            Load();
        }

        public override void UnLoad()
        {
            LaneControllerManager.Release();
        }

        public override void End()
        {
            HarmonyUtil.UninstallHarmony("cs.lanecontroller");
        }
    }
}
