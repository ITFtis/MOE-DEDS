using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DEDS.ArrayOfFloodEvent
{
    [XmlRoot(ElementName = "FloodEvent")]
    public class FloodEvent
    {

        [XmlElement(ElementName = "FloodEventCode")]
        public string FloodEventCode { get; set; }

        [XmlElement(ElementName = "FloodEventName")]
        public string FloodEventName { get; set; }

        [XmlElement(ElementName = "TyphoonClassType")]
        public int TyphoonClassType { get; set; }

        [XmlElement(ElementName = "FloodEventYear")]
        public int FloodEventYear { get; set; }

        [XmlElement(ElementName = "EventStartDate")]
        public DateTime EventStartDate { get; set; }

        [XmlElement(ElementName = "EventEndDate")]
        public DateTime EventEndDate { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfFloodEvent")]
    public class ArrayOfFloodEvent
    {

        [XmlElement(ElementName = "FloodEvent")]
        public List<FloodEvent> FloodEvent { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


}