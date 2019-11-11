using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("g")]
    public class SvgGroup : SvgNestedElement
    {
        private const string tag = "g";

        public SvgGroup()
        {

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
