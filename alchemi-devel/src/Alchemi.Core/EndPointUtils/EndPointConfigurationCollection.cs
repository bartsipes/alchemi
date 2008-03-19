using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Alchemi.Core.EndPointUtils
{
    /// <summary>
    /// The class for holding a serializable collection of EndPointConfiguration instances under string keys.
    /// </summary>
    [XmlRoot("dictionary")]
    public class EndPointConfigurationCollection : Dictionary<string, EndPointConfiguration>, IXmlSerializable
    {
        #region IXmlSerializable Members

        #region GetSchema
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        #endregion

        #region ReadXml
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(EndPointConfiguration));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {

                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                string key = (string)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                EndPointConfiguration value = (EndPointConfiguration)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }
        #endregion

        #region WriteXml
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(EndPointConfiguration));

            foreach (string key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                EndPointConfiguration value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion

        #endregion
    }

}
