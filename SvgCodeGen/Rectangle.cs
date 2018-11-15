using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("rect")]
    public class Rectangle : Element
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


        public Rectangle() { }

        /// <summary>
        /// Contructs a rectangle with the specified width and height.
        /// </summary>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        public Rectangle(uint width, uint height)
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
        public Rectangle(int x, int y, uint width, uint height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override bool CanGenerateValidSvgCode()
        {
            return (Width > 0 && Height > 0) ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement rectNode = doc.CreateElement(string.Empty, tag, string.Empty);
            if (X != 0) rectNode.SetAttribute("x", X.ToString(ci));
            if (Y != 0) rectNode.SetAttribute("y", Y.ToString(ci));
            rectNode.SetAttribute("width", Width.ToString(ci));
            rectNode.SetAttribute("height", Height.ToString(ci));
            if (Stroke != null) rectNode.SetAttribute("stroke", Stroke);
            if (Fill != null) rectNode.SetAttribute("fill", Fill);
            if (StrokeWidth > 0) rectNode.SetAttribute("stroke-width", StrokeWidth.ToString(ci));
            return rectNode;
        }

    }
}
