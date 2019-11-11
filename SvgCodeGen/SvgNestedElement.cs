using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SvgCodeGen
{
    public abstract class SvgNestedElement : SvgElement
    {
        protected readonly List<SvgElement> elements = new List<SvgElement>();

        public int ElementCount { get { return elements.Count; } }

        public SvgNestedElement()
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

        public abstract override XmlElement GenerateNode(ref XmlDocument doc);
    }
}
