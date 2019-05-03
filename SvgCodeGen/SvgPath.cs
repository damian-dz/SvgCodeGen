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
    [XmlRoot("path")]
    public class SvgPath : SvgElement
    {
        private const string tag = "path";

        [XmlAttribute("d")]
        public string D;

        public SvgPath()
        {

        }

        public void Close()
        {
            D += " Z";
        }

        public void LineTo(double x, double y)
        {
            D += string.Format(" L{0} {1}", x, y);
        }

        public void MoveTo(double x, double y)
        {
            string space = D == null ? string.Empty : " ";
            D += string.Format("{0}M{1} {2}", space, x, y);
        }

        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement pathNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref pathNode, ref ci);
            pathNode.SetAttribute("d", D.ToString(ci));
            SetCommonNodeAttributes(ref pathNode, ref ci);
            return pathNode;
        }
    }
}
