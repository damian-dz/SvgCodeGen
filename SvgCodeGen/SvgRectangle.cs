using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("rect")]
    public class SvgRectangle : SvgElement
    {
        private const string tag = "rect";

        [XmlAttribute("x")]
        public double X;
        [XmlAttribute("y")]
        public double Y;
        [XmlAttribute("width")]
        public double Width;
        [XmlAttribute("height")]
        public double Height;

        public SvgRectangle()
        {

        }

        /// <summary>
        /// Contructs a rectangle with the specified width and height.
        /// </summary>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        public SvgRectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Contructs a rectangle with the specified width and height at the given position.
        /// </summary>
        /// <param name="x">Left top corner X-coordinate.</param>
        /// <param name="y">Left top corner Y-coordinate.</param>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        public SvgRectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Scale(double factor)
        {
            X *= factor;
            Y *= factor;
        }

        public void ScaleX(double factor)
        {
            X *= factor;
        }

        public void ScaleY(double factor)
        {
            Y *= factor;
        }

        public void Translate(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return (Width > 0 && Height > 0) ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement rectNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref rectNode, ref ci);
            if (X != 0) rectNode.SetAttribute("x", X.ToString(ci));
            if (Y != 0) rectNode.SetAttribute("y", Y.ToString(ci));
            rectNode.SetAttribute("width", Width.ToString(ci));
            rectNode.SetAttribute("height", Height.ToString(ci));
            SetCommonNodeAttributes(ref rectNode, ref ci);
            return rectNode;
        }

    }
}
