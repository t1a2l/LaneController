using System;
using ColossalFramework;
using ICities;
using LaneController.KianCommons.Utils;
using LaneController.Manager;

namespace LaneController.LifeCycle
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        private const string DATA_ID = "LaneController";

        private SerializableDataExtension Instance;

        private static ISerializableData SerializableData => Singleton<SimulationManager>.instance.m_SerializableDataWrapper;

        public override void OnCreated(ISerializableData serializableData)
        {
            Instance = this;
        }

        public override void OnReleased()
        {
            Instance = null;
        }

        public override void OnLoadData()
        {
            Load();
        }

        public static void Load()
        {
            Log.Called();
            try
            {
                Log.Called();
                byte[] data = SerializableData.LoadData("LaneController");
                LaneControllerManager.Deserialize(data);
                Log.Succeeded();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public override void OnSaveData()
        {
            Save();
        }

        public static void Save()
        {
            Log.Called();
            try
            {
                Log.Called();
                byte[] array = LaneControllerManager.Instance.Serialize();
                if (array != null)
                {
                    SerializableData.SaveData("LaneController", array);
                }
                Log.Succeeded();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
    }
}
