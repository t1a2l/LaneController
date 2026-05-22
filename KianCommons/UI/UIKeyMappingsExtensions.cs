using ColossalFramework.UI;

namespace LaneController.KianCommons.UI
{
    internal static class UIKeyMappingsExtensions
    {
        internal static UIKeymappingsPanel AddKeymappingsPanel(this UIHelper helper)
        {
            return (helper.self as UIComponent).gameObject.AddComponent<UIKeymappingsPanel>();
        }
    }
}
