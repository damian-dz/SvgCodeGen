using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("defs")]
    public class SvgDefs : SvgNestedElement
    {
        private const string tag = "defs";

        public SvgDefs()
        {

        }

        public override XmlElement GenerateNode(ref XmlDocument doc)
        {
            XmlElement markerNode = doc.CreateElement(string.Empty, tag, string.Empty);
            foreach (SvgElement elem in elements)
            {
                if (elem.CanGenerateValidSvgCode())
                {
                    markerNode.AppendChild(elem.GenerateNode(ref doc));
                }
            }
            return markerNode;
        }
    }
}
