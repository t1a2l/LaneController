using System;
using ColossalFramework;
using ICities;
using LaneController.KianCommons.IImplict;
using UnityEngine.SceneManagement;

namespace LaneController.KianCommons.Utils
{
    public abstract class LifeCycleBase : ILoadingExtension, IModWithSettings, IUserMod
    {
        public static LifeCycleBase Instance { get; private set; }

        public static string Scene => SceneManager.GetActiveScene().name;

        public static Version ModVersion => typeof(LifeCycleBase).Assembly.GetName().Version;

        public string Name => ModName + " V" + ModVersion.ToString(1);

        public abstract string ModName { get; }

        public abstract string Description { get; }

        public static SimulationManager.UpdateMode UpdateMode => Singleton<SimulationManager>.instance.m_metaData.m_updateMode;

        public static LoadMode Mode => (LoadMode)UpdateMode;

        internal LifeCycleBase()
        {
            Instance = this;
        }

        public void OnEnabled()
        {
            try
            {
                Log.VERBOSE = false;
                Start();
                if (!Helpers.InStartupMenu)
                {
                    HotReload();
                }
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public void OnDisabled()
        {
            try
            {
                UnLoad();
                End();
                Log.Flush();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public abstract void Start();

        public abstract void End();

        public abstract void OnSettingsUI(UIHelper helper);

        public void OnCreated(ILoading _)
        {
        }

        public void OnReleased()
        {
        }

        public virtual void OnLevelLoaded(LoadMode _)
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public virtual void OnLevelUnloading()
        {
            try
            {
                UnLoad();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public virtual void HotReload()
        {
            Load();
        }

        public abstract void Load();

        public abstract void UnLoad();
    }
}
