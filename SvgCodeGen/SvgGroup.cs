using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("g")]
    public class SvgGroup : SvgElement
    {
        private const string tag = "g";

        private readonly List<SvgElement> elements = new List<SvgElement>();

        public int ElementCount { get { return elements.Count; } }

        public SvgGroup()
        {

        }

        public void AddElement(SvgElement elem)
        {
            elements.Add(elem);
        }

        public SvgElement GetElementAt(int idx)
        {
            return elements[idx];
        }

        public void RemoveElementAt(int idx)
        {
            elements.RemoveAt(idx);
        }

        public override bool CanGenerateValidSvgCode()
        {
            return elements.Count > 0 ? true : false;
        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            var ci = CultureInfo.InvariantCulture;
            XmlElement gNode = doc.CreateElement(string.Empty, tag, string.Empty);
            SetNodeIdAttribute(ref gNode, ref ci);
            SetCommonNodeAttributes(ref gNode, ref ci);
            foreach (SvgElement elem in elements)
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
