using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("marker")]
    public class SvgMarker : SvgNestedElement
    {
        private const string tag = "marker";

        [XmlAttribute("viewBox")]
        public string ViewBox { get; set; }
        [XmlAttribute("refX")]
        public double RefX { get; set; }
        [XmlAttribute("refY")]
        public double RefY { get; set; }
        [XmlAttribute("markerWidth")]
        public double MarkerWidth { get; set; }
        [XmlAttribute("markerHeight")]
        public double MarkerHeight { get; set; }
        [XmlAttribute("orient")]
        public string Orient { get; set; }

        public SvgMarker()
        {

        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement markerNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref markerNode, ref ci);
            if (MarkerWidth != 0) markerNode.SetAttribute("markerWidth", MarkerWidth.ToString(ci));
            if (MarkerHeight != 0) markerNode.SetAttribute("markerHeight", MarkerHeight.ToString(ci));
            if (Orient != null) markerNode.SetAttribute("orient", Orient);
            if (RefX != 0) markerNode.SetAttribute("refX", RefX.ToString(ci));
            if (RefY != 0) markerNode.SetAttribute("refY", RefY.ToString(ci));
            if (ViewBox != null) markerNode.SetAttribute("viewBox", ViewBox);
            SetCommonNodeAttributes(ref markerNode, ref ci);
            foreach (SvgElement elem in elements)
            {
                if (elem.CanGenerateValidSvgCode())
                {
                    markerNode.AppendChild(elem.GenerateNode(ref doc));
                }
            }
            return markerNode;
        }
    }
}
