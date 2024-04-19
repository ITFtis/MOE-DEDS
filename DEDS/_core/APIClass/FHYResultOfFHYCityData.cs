using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DEDS.FHYResultOfFHYCityData
{
    [XmlRoot(ElementName = "Name")]
    public class Name
    {

        [XmlElement(ElementName = "zh_TW")]
        public string ZhTW { get; set; }

        [XmlElement(ElementName = "En")]
        public string En { get; set; }
    }

    [XmlRoot(ElementName = "FHYCityData")]
    public class FHYCityData
    {

        [XmlElement(ElementName = "Code")]
        public int Code { get; set; }

        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class Data
    {

        [XmlElement(ElementName = "FHYCityData")]
        public List<FHYCityData> FHYCityData { get; set; }
    }

    [XmlRoot(ElementName = "FHYResultOfFHYCityData")]
    public class FHYResultOfFHYCityData
    {

        [XmlElement(ElementName = "UpdataTime")]
        public string UpdataTime { get; set; }

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