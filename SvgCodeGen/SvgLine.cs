using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("line")]
    public class SvgLine : SvgElement
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

        public SvgLine()
        {

        }

        public SvgLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public SvgLine(Point p1, Point p2)
        {
            X1 = p1.X;
            Y1 = p1.Y;
            X2 = p2.X;
            Y2 = p2.Y;
        }

        public void RotateAround(Point pivot, double angle)
        {
            Point p1 = new Point(X1, Y1).RotateAround(pivot, angle);
            Point p2 = new Point(X2, Y2).RotateAround(pivot, angle);
            X1 = p1.X;
            Y1 = p1.Y;
            X2 = p2.X;
            Y2 = p2.Y;
        }

        public void Translate(double deltaX, double deltaY)
        {
            X1 += deltaX;
            Y1 += deltaY;
            X2 += deltaX;
            Y2 += deltaY;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return true;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement lineNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref lineNode, ref ci);
            lineNode.SetAttribute("x1", X1.ToString(ci));
            lineNode.SetAttribute("y1", Y1.ToString(ci));
            lineNode.SetAttribute("x2", X2.ToString(ci));
            lineNode.SetAttribute("y2", Y2.ToString(ci));
            SetCommonNodeAttributes(ref lineNode, ref ci);
            return lineNode;
        }
    }
}
