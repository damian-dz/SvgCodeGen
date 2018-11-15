using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("line")]
    public class Line : Element
    {
        private const string tag = "line";

        [XmlAttribute("x1")]
        public double X1;
        [XmlAttribute("y1")]
        public double Y1;
        [XmlAttribute("x2")]
        public double X2;
        [XmlAttribute("y2")]
        public double Y2;

        public Line() { }

        public Line(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement lineNode = doc.CreateElement(string.Empty, tag, string.Empty);
            lineNode.SetAttribute("x1", X1.ToString(ci));
            lineNode.SetAttribute("y1", Y1.ToString(ci));
            lineNode.SetAttribute("x2", X2.ToString(ci));
            lineNode.SetAttribute("y2", Y2.ToString(ci));
            if (Stroke != null) lineNode.SetAttribute("stroke", Stroke);
            if (StrokeWidth > 0) lineNode.SetAttribute("stroke-width", StrokeWidth.ToString(ci));
            if (Style != null) lineNode.SetAttribute("style", Style);
            return lineNode;
        }
    }
}
