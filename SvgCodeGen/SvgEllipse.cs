using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("ellipse")]
    class SvgEllipse : SvgElement
    {
        private const string tag = "ellipse";

        [XmlAttribute("cx")]
        public double Cx;
        [XmlAttribute("cy")]
        public double Cy;
        [XmlAttribute("rx")]
        public double Rx;
        [XmlAttribute("ry")]
        public double Ry;

        public SvgEllipse()
        {

        }

        public SvgEllipse(double cx, double cy, double rx, double ry)
        {
            Cx = cx;
            Cy = cy;
            Rx = rx;
            Ry = ry;
        }

        public SvgEllipse(double cx, double cy, Point r)
        {
            Cx = cx;
            Cy = cy;
            Rx = r.X;
            Ry = r.Y;
        }

        public SvgEllipse(Point c, double rx, double ry)
        {
            Cx = c.X;
            Cy = c.Y;
            Rx = rx;
            Ry = ry;
        }

        public SvgEllipse(Point c, Point r)
        {
            Cx = c.X;
            Cy = c.Y;
            Rx = r.X;
            Ry = r.Y;
        }


        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement ellipseNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref ellipseNode, ref ci);
            if (Cx != 0) ellipseNode.SetAttribute("cx", Cx.ToString(ci));
            if (Cy != 0) ellipseNode.SetAttribute("cy", Cy.ToString(ci));
            if (Rx != 0) ellipseNode.SetAttribute("rx", Rx.ToString(ci));
            if (Ry != 0) ellipseNode.SetAttribute("ry", Ry.ToString(ci));
            SetCommonNodeAttributes(ref ellipseNode, ref ci);
            return ellipseNode;
        }
    }
}
