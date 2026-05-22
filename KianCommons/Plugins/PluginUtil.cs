using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Plugins;
using ICities;
using LaneController.KianCommons.Utils;

namespace LaneController.KianCommons.Plugins
{
    internal static class PluginUtil
    {
        [Flags]
        public enum SearchOptionT
        {
            None = 0,
            Contains = 1,
            StartsWidth = 2,
            [Obsolete("always active")]
            Equals = 4,
            AllModes = 3,
            CaseInsensetive = 8,
            IgnoreWhiteSpace = 0x10,
            AllOptions = 0x18,
            UserModName = 0x20,
            UserModType = 0x40,
            RootNameSpace = 0x80,
            PluginName = 0x100,
            AssemblyName = 0x200,
            AllTargets = 0x3E0
        }

        [Obsolete]
        internal static bool CSUREnabled;

        public const SearchOptionT DefaultsearchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName;

        public const SearchOptionT AssemblyEquals = SearchOptionT.AllOptions | SearchOptionT.AssemblyName;

        private static PluginManager Man => Singleton<PluginManager>.instance;

        public static PluginManager.PluginInfo GetCurrentAssemblyPlugin()
        {
            return GetPlugin(Assembly.GetExecutingAssembly());
        }

        public static void LogPlugins(bool detailed = false)
        {
            List<PluginManager.PluginInfo> list = [.. Man.GetPluginsInfo()];
            list.Sort(Comparison);
            string text = list.Select((p) => PluginToString(p)).JoinLines();
            Log.Info("Installed mods are:\n" + text, copyToGameLog: true);
            static int Comparison(PluginManager.PluginInfo a, PluginManager.PluginInfo b)
            {
                if (b == null)
                {
                    return 1;
                }
                if (a == null)
                {
                    return -1;
                }
                return b.isEnabled.CompareTo(a.isEnabled);
            }
            string PluginToString(PluginManager.PluginInfo p)
            {
                string text2 = p.isEnabled ? "*" : " ";
                string text3 = p.IsLocal() ? "(local)" : p.GetWorkshopID().ToString();
                text3.PadRight(12);
                if (!detailed)
                {
                    return "\t" + text2 + " " + text3 + " " + p.GetModName();
                }
                return "\t" + text2 + " " + text3 + " mod-name:" + (p?.GetModName()).ToSTR() + " asm-name:" + (p?.GetMainAssembly()?.Name()).ToSTR() + " user-mod-type:" + (p?.userModInstance?.GetType()?.Name).ToSTR();
            }
        }

        public static void ReportIncomaptibleMods(IEnumerable<PluginManager.PluginInfo> plugins)
        {
        }

        public static PluginManager.PluginInfo GetCSUR()
        {
            return GetPlugin("CSUR ToolBox", 1959342332uL);
        }

        public static PluginManager.PluginInfo GetAdaptiveRoads()
        {
            return GetPlugin("AdaptiveNetworks");
        }

        public static PluginManager.PluginInfo GetHideCrossings()
        {
            return GetPlugin("HideCrosswalks", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetHideUnconnectedTracks()
        {
            return GetPlugin("HideUnconnectedTracks", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetDirectConnectRoads()
        {
            return GetPlugin("DirectConnectRoads", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetTrafficManager()
        {
            return GetPlugin("TrafficManager", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetNetworkDetective()
        {
            return GetPlugin("NetworkDetective", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetNetworkSkins()
        {
            return GetPlugin("NetworkSkins", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        public static PluginManager.PluginInfo GetNodeController()
        {
            return GetPlugin("Node Controller");
        }

        public static PluginManager.PluginInfo GetPedestrianBridge()
        {
            return GetPlugin("Pedestrian Bridge");
        }

        public static PluginManager.PluginInfo GetIMT()
        {
            return GetPlugin("Intersection Marking", [2140418403uL, 2159934925uL]);
        }

        public static PluginManager.PluginInfo GetRAB()
        {
            return GetPlugin("Roundabout Builder");
        }

        public static PluginManager.PluginInfo GetLoadOrderMod()
        {
            return GetPlugin("LoadOrderMod", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
        }

        [Obsolete]
        private static bool IsCSUR(PluginManager.PluginInfo current)
        {
            if (!current.name.Contains("CSUR ToolBox"))
            {
                return 1959342332 == (int)current.publishedFileID.AsUInt64;
            }
            return true;
        }

        [Obsolete]
        public static void Init()
        {
            CSUREnabled = false;
            foreach (PluginManager.PluginInfo item in Man.GetPluginsInfo())
            {
                if (item.isEnabled && IsCSUR(item))
                {
                    CSUREnabled = true;
                    break;
                }
            }
        }

        public static PluginManager.PluginInfo GetPlugin(this IUserMod userMod)
        {
            foreach (PluginManager.PluginInfo item in Man.GetPluginsInfo())
            {
                if (userMod == item.userModInstance)
                {
                    return item;
                }
            }
            return null;
        }

        public static PluginManager.PluginInfo GetPlugin<UserModT>() where UserModT : IUserMod
        {
            foreach (PluginManager.PluginInfo item in Man.GetPluginsInfo())
            {
                if (item.userModInstance is UserModT)
                {
                    return item;
                }
            }
            return null;
        }

        public static PluginManager.PluginInfo GetPlugin(Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            foreach (PluginManager.PluginInfo item in Man.GetPluginsInfo())
            {
                if (item.ContainsAssembly(assembly))
                {
                    return item;
                }
            }
            return null;
        }

        public static PluginManager.PluginInfo GetPlugin(string searchName, ulong searchId, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
        {
            return GetPlugin(searchName, [searchId], searchOptions);
        }

        public static PluginManager.PluginInfo GetPlugin(string searchName, ulong[] searchIds = null, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
        {
            foreach (PluginManager.PluginInfo item in Singleton<PluginManager>.instance.GetPluginsInfo())
            {
                if (item == null)
                {
                    continue;
                }
                bool flag = Matches(item, searchIds);
                if (item.userModInstance is IUserMod userMod)
                {
                    if (searchOptions.IsFlagSet(SearchOptionT.UserModName))
                    {
                        flag = flag || Match(userMod.Name, searchName, searchOptions);
                    }
                    Type type = userMod.GetType();
                    if (searchOptions.IsFlagSet(SearchOptionT.UserModType))
                    {
                        flag = flag || Match(type.Name, searchName, searchOptions);
                    }
                    if (searchOptions.IsFlagSet(SearchOptionT.RootNameSpace))
                    {
                        string text = type.Namespace;
                        string name = text.Split('.')[0];
                        flag = flag || Match(name, searchName, searchOptions);
                    }
                    if (searchOptions.IsFlagSet(SearchOptionT.PluginName))
                    {
                        flag = flag || Match(item.name, searchName, searchOptions);
                    }
                    if (searchOptions.IsFlagSet(SearchOptionT.AssemblyName))
                    {
                        Assembly mainAssembly = item.GetMainAssembly();
                        flag = flag || Match(mainAssembly?.Name(), searchName, searchOptions);
                    }
                    if (flag)
                    {
                        Log.Info("Found plug-in:" + item.GetModName());
                        return item;
                    }
                }
            }
            Log.Info($"plug-in not found: keyword={searchName} options={searchOptions}");
            return null;
        }

        public static bool Match(string name1, string name2, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
        {
            if (string.IsNullOrEmpty(name1))
            {
                return false;
            }
            Assertion.Assert((searchOptions & SearchOptionT.AllTargets) != 0);
            if (searchOptions.IsFlagSet(SearchOptionT.CaseInsensetive))
            {
                name1 = name1.ToLower();
                name2 = name2.ToLower();
            }
            if (searchOptions.IsFlagSet(SearchOptionT.IgnoreWhiteSpace))
            {
                name1 = name1.Replace(" ", "");
                name2 = name2.Replace(" ", "");
            }
            _ = Log.VERBOSE;
            if (name1 == name2)
            {
                return true;
            }
            if (searchOptions.IsFlagSet(SearchOptionT.Contains) && name1.Contains(name2))
            {
                return true;
            }
            if (searchOptions.IsFlagSet(SearchOptionT.StartsWidth) && name1.StartsWith(name2))
            {
                return true;
            }
            return false;
        }

        public static bool Matches(PluginManager.PluginInfo plugin, ulong[] searchIds)
        {
            Assertion.AssertNotNull(plugin);
            if (searchIds == null)
            {
                return false;
            }
            foreach (ulong num in searchIds)
            {
                if (num == 0L)
                {
                    Log.Error("unexpected 0 as mod search id");
                }
                else if (num == plugin.GetWorkshopID())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
