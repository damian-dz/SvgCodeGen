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
    [XmlRoot("circle")]
    public class SvgCircle : SvgElement
    {
        private const string tag = "circle";

        [XmlAttribute("cx")]
        public double Cx;
        [XmlAttribute("cy")]
        public double Cy;
        [XmlAttribute("r")]
        public double R;

        public SvgCircle()
        {

        }

        public SvgCircle(double cx, double cy, double r)
        {
            Cx = cx;
            Cy = cy;
            R = r;
        }

        public SvgCircle(Point p, double r)
        {
            Cx = p.X;
            Cy = p.Y;
            R = r;
        }

        public void SetCenter(Point p)
        {
            Cx = p.X;
            Cy = p.Y;
        }

        public Point GetCenter()
        {
            return new Point(Cx, Cy);
        }

        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement circleNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref circleNode, ref ci);
            if (Cx != 0) circleNode.SetAttribute("cx", Cx.ToString(ci));
            if (Cy != 0) circleNode.SetAttribute("cy", Cy.ToString(ci));
            if (R != 0) circleNode.SetAttribute("r", R.ToString(ci));
            SetCommonNodeAttributes(ref circleNode, ref ci);
            return circleNode;
        }
    }
}
