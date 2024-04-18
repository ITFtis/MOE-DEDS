using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DEDS.ArrayOfCountyXY
{
    [XmlRoot(ElementName = "CountyXY")]
    public class CountyXY
    {

        [XmlElement(ElementName = "PK_ID")]
        public int PKID { get; set; }

        [XmlElement(ElementName = "CountyName")]
        public string CountyName { get; set; }

        [XmlElement(ElementName = "C_Longitude")]
        public double CLongitude { get; set; }

        [XmlElement(ElementName = "C_Latitude")]
        public double CLatitude { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfCountyXY")]
    public class ArrayOfCountyXY
    {

        [XmlElement(ElementName = "CountyXY")]
        public List<CountyXY> d { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}