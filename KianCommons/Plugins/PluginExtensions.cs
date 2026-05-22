using System.Reflection;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;

namespace LaneController.KianCommons.Plugins
{
    internal static class PluginExtensions
    {
        public static IUserMod GetUserModInstance(this PluginManager.PluginInfo plugin)
        {
            return plugin?.userModInstance as IUserMod;
        }

        public static string GetModName(this PluginManager.PluginInfo plugin)
        {
            return plugin.GetUserModInstance()?.Name;
        }

        public static ulong GetWorkshopID(this PluginManager.PluginInfo plugin)
        {
            return plugin.publishedFileID.AsUInt64;
        }

        public static bool IsActive(this PluginManager.PluginInfo plugin)
        {
            return plugin?.isEnabled ?? false;
        }

        public static Assembly GetMainAssembly(this PluginManager.PluginInfo plugin)
        {
            return plugin?.userModInstance?.GetType()?.Assembly;
        }

        public static bool IsLocal(this PluginManager.PluginInfo plugin)
        {
            if (plugin != null)
            {
                if (plugin.GetWorkshopID() != 0L)
                {
                    return plugin.publishedFileID == PublishedFileId.invalid;
                }
                return true;
            }
            return false;
        }

        public static PluginManager.PluginInfo GetPlugin(this Assembly assembly)
        {
            return PluginUtil.GetPlugin(assembly);
        }
    }
}
