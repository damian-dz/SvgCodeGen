using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("g")]
    public class Group : Element
    {
        private const string tag = "g";

        private List<Element> elements = new List<Element>();

        public Group() { }

        public int ElementCount { get { return elements.Count; } }

        public void AddElement(Element elem)
        {
            elements.Add(elem);
        }

        public Element GetElementAt(int idx)
        {
            return elements[idx];
        }

        public void RemoveElementAt(int idx)
        {
            elements.RemoveAt(idx);
        }

        public override bool CanGenerateValidSvgCode()
        {
            return elements.Count > 1 ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement gNode = doc.CreateElement(string.Empty, tag, string.Empty);
            if (Stroke != null) gNode.SetAttribute("stroke", Stroke);
            if (Fill != null) gNode.SetAttribute("fill", Fill);
            if (StrokeWidth > 0) gNode.SetAttribute("stroke-width", StrokeWidth.ToString(ci));
            foreach (Element elem in elements)
            {
                if (elem.CanGenerateValidSvgCode())
                {
                    gNode.AppendChild(elem.GenerateNode(ref doc));
                }
            }
            return gNode;
        }
    }
}
