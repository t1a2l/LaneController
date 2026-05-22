using ICities;

namespace LaneController.KianCommons.IImplict
{
    internal interface IModWithSettings : IUserMod
    {
        void OnSettingsUI(UIHelper helper);
    }
}
