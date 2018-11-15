using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("polygon")]
    public class Polygon : Element
    {
        private const string tag = "polygon";

        [XmlAttribute("points")]
        public string Points;

        public int PointCount { get { return Points.Split(' ').Length; } }

        public Polygon() { }

        public Polygon(Point p1, Point p2, Point p3)
        {
            Points += PointToString(p1) + " ";
            Points += PointToString(p2) + " ";
            Points += PointToString(p3);
        }

        public Polygon(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            Points += PointToString(new Point(x1, y1)) + " ";
            Points += PointToString(new Point(x2, y2)) + " ";
            Points += PointToString(new Point(x3, y3));
        }

        public void AddPoint(Point p)
        {
            Points += " " + PointToString(p);
        }

        public void AddPoint(double x, double y)
        {
            Points += " " + PointToString(new Point(x, y));
        }

        public void InsertPoint(int idx, Point p)
        {
            var temp = new List<string>(Points.Split(' '));
            temp.Insert(idx, PointToString(p));
            Points = string.Join(" ", temp.ToArray());
        }

        public void InsertPoint(int idx, double x, double y)
        {
            var temp = new List<string>(Points.Split(' '));
            temp.Insert(idx, PointToString(new Point(x, y)));
            Points = string.Join(" ", temp.ToArray());
        }

        private string PointToString(Point p)
        {
            var ci = CultureInfo.InvariantCulture;
            return p.X.ToString(ci) + "," + p.Y.ToString(ci);
        }

        public void RemovePoint(int idx)
        {
            var temp = new List<string>(Points.Split(' '));
            temp.RemoveAt(idx);
            Points = string.Join(" ", temp.ToArray());
        }

        public override bool CanGenerateValidSvgCode()
        {
            return Points.Split(' ').Length > 2 ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement polygonNode = doc.CreateElement(string.Empty, tag, string.Empty);
            polygonNode.SetAttribute("points", Points);
            if (Stroke != null) polygonNode.SetAttribute("stroke", Stroke);
            if (Fill != null) polygonNode.SetAttribute("fill", Fill);
            if (StrokeWidth > 0) polygonNode.SetAttribute("stroke-width", StrokeWidth.ToString(ci));
            return polygonNode;
        }
    }
}
