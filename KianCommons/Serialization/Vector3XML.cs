using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace LaneController.KianCommons.Serialization
{
    public struct Vector3XML : IXmlSerializable
    {
        public Vector3 Vector;

        private float[] dims =>
        [
            Vector[0],
            Vector[1],
            Vector[2]
        ];

        public static implicit operator Vector3(Vector3XML v)
        {
            return v.Vector;
        }

        public static implicit operator Vector3XML(Vector3 v)
        {
            return new Vector3XML
            {
                Vector = v
            };
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            int num = 0;
            string[] array = reader.ReadString().Split(',');
            foreach (string s in array)
            {
                Vector[num++] = float.Parse(s);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        public override string ToString()
        {
            IEnumerable<string> source = dims.Select((v) => v.ToString("G9"));
            return string.Join(", ", [.. source]);
        }
    }
}
