using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DEDS.FHYResultOfFHYFloodSensorStation
{
    [XmlRoot(ElementName = "Point")]
    public class Point
    {

        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }

        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }
    }

    [XmlRoot(ElementName = "FHYFloodSensorStation")]
    public class FHYFloodSensorStation
    {

        [XmlElement(ElementName = "SensorUUID")]
        public string SensorUUID { get; set; }

        [XmlElement(ElementName = "Operator")]
        public int Operator { get; set; }

        [XmlElement(ElementName = "CityCode")]
        public int CityCode { get; set; }

        [XmlElement(ElementName = "TownCode")]
        public int TownCode { get; set; }

        [XmlElement(ElementName = "SensorName")]
        public string SensorName { get; set; }

        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }

        [XmlElement(ElementName = "Point")]
        public Point Point { get; set; }

        [XmlElement(ElementName = "SensorType")]
        public string SensorType { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class Data
    {

        [XmlElement(ElementName = "FHYFloodSensorStation")]
        public List<FHYFloodSensorStation> FHYFloodSensorStation { get; set; }
    }

    [XmlRoot(ElementName = "FHYResultOfFHYFloodSensorStation")]
    public class FHYResultOfFHYFloodSensorStation
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