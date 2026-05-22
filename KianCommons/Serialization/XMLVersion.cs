using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LaneController.KianCommons.Serialization
{
    public class XMLVersion : IXmlSerializable
    {
        private Version version_;

        public static implicit operator Version(XMLVersion v)
        {
            return v.version_;
        }

        public static implicit operator XMLVersion(Version v)
        {
            return new XMLVersion
            {
                version_ = v
            };
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            version_ = new Version(reader.ReadString());
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(version_.ToString());
        }
    }
}
