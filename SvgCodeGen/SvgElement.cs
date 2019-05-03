using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    public abstract class SvgElement
    {
        [XmlAttribute("id")]
        public string Id;
        [XmlAttribute("stroke")]
        public string Stroke;
        [XmlAttribute("stroke-width")]
        public double StrokeWidth;
        [XmlAttribute("stroke-dasharray")]
        public string StrokeDashArray;
        [XmlAttribute("fill")]
        public string Fill;
        [XmlAttribute("style")]
        public string Style;
        [XmlAttribute("transform")]
        public string Transform;

        protected string GetColorCode(int? color)
        {
            return "#" + ((int)color).ToString("x6", CultureInfo.InvariantCulture);
        }

        protected string PointToString(Point p)
        {
            var ci = CultureInfo.InvariantCulture;
            return p.X.ToString(ci) + "," + p.Y.ToString(ci);
        }

        protected void SetNodeIdAttribute(ref XmlElement node, ref CultureInfo ci)
        {
            if (Id != null) node.SetAttribute("id", Id);
        }

        protected void SetCommonNodeAttributes(ref XmlElement node, ref CultureInfo ci)
        {
            if (Fill != null) node.SetAttribute("fill", Fill);
            if (Stroke != null) node.SetAttribute("stroke", Stroke);
            if (StrokeDashArray != null) node.SetAttribute("stroke-dasharray", StrokeDashArray);
            if (StrokeWidth > 0) node.SetAttribute("stroke-width", StrokeWidth.ToString(ci));
            if (Style != null) node.SetAttribute("style", Style);
            if (Transform != null) node.SetAttribute("transform", Transform);
        }

        public abstract bool CanGenerateValidSvgCode();
        public abstract XmlElement GenerateNode(ref XmlDocument doc);

        public void SetFill(Color col)
        {
            Fill = "#" + ((int)col).ToString("x6");
        }

        public void SetStroke(Color col)
        {
            Stroke = "#" + ((int)col).ToString("x6");
        }

        public XmlElement GenerateNode()
        {
            var doc = new XmlDocument();
            return GenerateNode(ref doc);
        }

        public string GenerateSvgCode()
        {
            return GenerateNode().OuterXml;
        }
    }
}
