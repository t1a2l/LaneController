using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LaneController.KianCommons.Utils;

namespace LaneController.KianCommons.Serialization
{
    internal static class XMLSerializerUtil
    {
        private static XmlSerializerNamespaces NoNamespaces
        {
            get
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new();
                xmlSerializerNamespaces.Add("", "");
                return xmlSerializerNamespaces;
            }
        }

        private static XmlSerializer Serilizer<T>()
        {
            return new XmlSerializer(typeof(T));
        }

        private static void Serialize<T>(TextWriter writer, T value)
        {
            Serilizer<T>().Serialize(writer, value, NoNamespaces);
        }

        private static T Deserialize<T>(TextReader reader)
        {
            return (T)Serilizer<T>().Deserialize(reader);
        }

        public static string Serialize<T>(T value)
        {
            try
            {
                using TextWriter textWriter = new StringWriter();
                using XmlTextWriter xmlTextWriter = new(textWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.Namespaces = false;
                Serialize(textWriter, value);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                ex.Exception();
                return null;
            }
        }

        public static T Deserialize<T>(string data)
        {
            try
            {
                using TextReader reader = new StringReader(data);
                return Deserialize<T>(reader);
            }
            catch (Exception ex)
            {
                ex.Exception("", showInPanel: false);
                return default;
            }
        }

        public static Version ExtractVersion(string xmlData)
        {
            XDocument xDocument = XDocument.Parse(xmlData);
            string value = xDocument.Root.Attribute("version").Value;
            return new Version(value);
        }

        public static object XMLConvert(object value, Type type)
        {
            object obj = XMLPrefabConvert<PropInfo>(value, type);
            if (obj != null)
            {
                return obj;
            }
            obj = XMLPrefabConvert<TreeInfo>(value, type);
            if (obj != null)
            {
                return obj;
            }
            return null;
        }

        public static object XMLPrefabConvert<T>(object value, Type type) where T : PrefabInfo
        {
            if (value is T val && type == typeof(XmlPrefabInfo<T>))
            {
                return (XmlPrefabInfo<T>)val;
            }
            if (value is XmlPrefabInfo<T> xmlPrefabInfo && type == typeof(T))
            {
                return (T)xmlPrefabInfo;
            }
            return null;
        }
    }
}
