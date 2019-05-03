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
    [XmlRoot("polyline")]
    public class SvgPolyline : SvgElement
    {
        private const string tag = "polyline";

        [XmlAttribute("points")]
        public string Points;

        public int PointCount { get { return Points.Split(' ').Length; } }

        public SvgPolyline()
        {

        }

        public SvgPolyline(Point p1, Point p2)
        {
            Points += PointToString(p1) + " ";
            Points += PointToString(p2);
        }

        public SvgPolyline(Point p1, Point p2, Point p3)
        {
            Points += PointToString(p1) + " ";
            Points += PointToString(p2) + " ";
            Points += PointToString(p3);
        }

        public SvgPolyline(double x1, double y1, double x2, double y2)
        {
            Points += PointToString(new Point(x1, y1)) + " ";
            Points += PointToString(new Point(x2, y2));
        }

        public SvgPolyline(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            Points += PointToString(new Point(x1, y1)) + " ";
            Points += PointToString(new Point(x2, y2)) + " ";
            Points += PointToString(new Point(x3, y3));
        }

        public void AddPoint(Point p)
        {
            Points += Points == null ? PointToString(p) : " " + PointToString(p);
        }

        public void AddPoint(double x, double y)
        {
            Points += Points == null ? PointToString(new Point(x, y)) : " " + PointToString(new Point(x, y));
        }
        public void AddPoints(double[] xPoints, double[] yPoints)
        {
            if (xPoints.Length != yPoints.Length)
            {
                throw new ArgumentException("Arrays must have the same length.");
            }
            for (int i = 0; i < xPoints.Length; i++)
            {
                AddPoint(xPoints[i], yPoints[i]);
            }
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

        public void SetPoints(double[] xPoints, double[] yPoints)
        {
            if (xPoints.Length != yPoints.Length)
            {
                throw new ArgumentException("Arrays must have the same length.");
            }
            Points = null;
            for (int i = 0; i < xPoints.Length; i++)
            {
                AddPoint(xPoints[i], yPoints[i]);
            }
        }

        public override bool CanGenerateValidSvgCode()
        {
            return Points.Split(' ').Length > 1 ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement polylineNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref polylineNode, ref ci);
            polylineNode.SetAttribute("points", Points);
            SetCommonNodeAttributes(ref polylineNode, ref ci);
            return polylineNode;
        }
    }
}
