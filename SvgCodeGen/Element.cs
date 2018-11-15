using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    public abstract class Element
    {
        [XmlAttribute("stroke")]
        public string Stroke;
        [XmlAttribute("stroke-width")]
        public double StrokeWidth;
        [XmlAttribute("fill")]
        public string Fill;
        [XmlAttribute("style")]
        public string Style;

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

        protected string GetColorCode(int? color)
        {
            return "#" + ((int)color).ToString("x6", CultureInfo.InvariantCulture);
        }
    }
}
