using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("use")]
    public class SvgUse : SvgElement
    {
        private const string tag = "use";

        [XmlAttribute("href")]
        public string HRef { get; set; }
        [XmlAttribute("x")]
        public double X { get; set; }
        [XmlAttribute("y")]
        public double Y { get; set; }
        [XmlAttribute("width")]
        public double Width { get; set; }
        [XmlAttribute("height")]
        public double Height { get; set; }

        public SvgUse()
        {

        }

        public SvgUse(string href)
        {
            HRef = href;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement useNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref useNode, ref ci);
            if (HRef != null) useNode.SetAttribute("href", HRef);
            if (X != 0) useNode.SetAttribute("x", X.ToString(ci));
            if (Y != 0) useNode.SetAttribute("y", Y.ToString(ci));
            if (Width != 0) useNode.SetAttribute("x", Width.ToString(ci));
            if (Height != 0) useNode.SetAttribute("y", Height.ToString(ci));
            SetCommonNodeAttributes(ref useNode, ref ci);
            return useNode;
        }
    }
}
