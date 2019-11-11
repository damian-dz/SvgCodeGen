using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("text")]
    public class SvgText : SvgElement
    {
        private const string tag = "text";

        [XmlAttribute("x")]
        public double X;
        [XmlAttribute("y")]
        public double Y;
        [XmlText]
        public string InnerText { get; set; }

        public SvgText()
        {

        }

        public SvgText(double x, double y, string text)
        {
            X = x;
            Y = y;
            InnerText = text;
        }

        public SvgText(string text)
        {
            InnerText = text;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return !string.IsNullOrWhiteSpace(InnerText);
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement textNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref textNode, ref ci);
            textNode.SetAttribute("x", X.ToString(ci));
            textNode.SetAttribute("y", Y.ToString(ci));
            textNode.InnerText = InnerText;
            SetCommonNodeAttributes(ref textNode, ref ci);
            return textNode;
        }
    }
}
