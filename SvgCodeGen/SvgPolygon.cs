using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("polygon")]
    public class SvgPolygon : SvgElement
    {
        private const string tag = "polygon";

        [XmlAttribute("points")]
        public string Points;

        public int PointCount { get { return Points.Split(' ').Length; } }

        public SvgPolygon()
        {

        }

        public SvgPolygon(Point p1, Point p2, Point p3)
        {
            Points += PointToString(p1) + " ";
            Points += PointToString(p2) + " ";
            Points += PointToString(p3);
        }

        public SvgPolygon(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            Points += PointToString(new Point(x1, y1)) + " ";
            Points += PointToString(new Point(x2, y2)) + " ";
            Points += PointToString(new Point(x3, y3));
        }

        public void AddPoint(Point p)
        {
            Points += string.IsNullOrWhiteSpace(Points) ? PointToString(p) : " " + PointToString(p);
        }

        public void AddPoint(double x, double y)
        {
            Points += " " + PointToString(new Point(x, y));
        }

        public void InsertPoint(int idx, Point p)
        {
            var pointList = new List<string>(Points.Split(' '));
            pointList.Insert(idx, PointToString(p));
            Points = string.Join(" ", pointList.ToArray());
        }

        public void InsertPoint(int idx, double x, double y)
        {
            var pointList = new List<string>(Points.Split(' '));
            pointList.Insert(idx, PointToString(new Point(x, y)));
            Points = string.Join(" ", pointList.ToArray());
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
            SetNodeIdAttribute(ref polygonNode, ref ci);
            polygonNode.SetAttribute("points", Points);
            SetCommonNodeAttributes(ref polygonNode, ref ci);
            return polygonNode;
        }
    }
}
