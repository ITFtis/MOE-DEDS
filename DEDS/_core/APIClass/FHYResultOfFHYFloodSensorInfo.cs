using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DEDS.FHYResultOfFHYFloodSensorInfo
{
    [XmlRoot(ElementName = "FHYFloodSensorInfo")]
    public class FHYFloodSensorInfo
    {

        [XmlElement(ElementName = "SensorUUID")]
        public string SensorUUID { get; set; }

        [XmlElement(ElementName = "Depth")]
        public int Depth { get; set; }

        [XmlElement(ElementName = "SourceTime")]
        public DateTime SourceTime { get; set; }

        [XmlElement(ElementName = "TransferTime")]
        public DateTime TransferTime { get; set; }

        [XmlElement(ElementName = "ToBeConfirm")]
        public bool ToBeConfirm { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class Data
    {

        [XmlElement(ElementName = "FHYFloodSensorInfo")]
        public List<FHYFloodSensorInfo> FHYFloodSensorInfo { get; set; }
    }

    [XmlRoot(ElementName = "FHYResultOfFHYFloodSensorInfo")]
    public class FHYResultOfFHYFloodSensorInfo
    {

        [XmlElement(ElementName = "UpdataTime")]
        public DateTime UpdataTime { get; set; }

        [XmlElement(ElementName = "Data")]
        public Data Data { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }




}