using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LaneController.KianCommons.Serialization
{
    public class XmlPrefabInfo<T> : IXmlSerializable where T : PrefabInfo
    {
        public string name;

        public XmlPrefabInfo()
        {
        }

        public XmlPrefabInfo(T prefab)
        {
            name = prefab.name;
        }

        public static implicit operator XmlPrefabInfo<T>(T prefab)
        {
            return new XmlPrefabInfo<T>(prefab);
        }

        public static implicit operator T(XmlPrefabInfo<T> prefab)
        {
            if (string.IsNullOrEmpty(prefab.name))
            {
                return null;
            }
            return PrefabCollection<T>.FindLoaded(prefab.name);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(name);
        }

        public void ReadXml(XmlReader reader)
        {
            name = reader.ReadString();
        }
    }
}
