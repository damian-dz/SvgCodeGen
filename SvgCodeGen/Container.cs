using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("svg")]
    public class Container
    {
        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>
        { { "rect",     typeof(Rectangle)   },
          { "line",     typeof(Line)        },
          { "polygon",  typeof(Polygon)     } };

        public double Width;
        public double Height;
        private List<Element> elements = new List<Element>();

        public int ElementCount { get { return elements.Count;  } }

        public Container(string fileName)
        {
            var doc = new XmlDocument();
            string allText = File.ReadAllText(fileName);
            allText = allText.Replace("xmlns=\"http://www.w3.org/2000/svg\"", string.Empty);
            doc.LoadXml(allText);
            XmlAttributeCollection rootAttributes = doc.ChildNodes[0].Attributes;
            Width = double.Parse(doc.ChildNodes[0].Attributes["width"].Value);
            Height = double.Parse(doc.ChildNodes[0].Attributes["height"].Value);
            foreach (XmlNode node in doc.ChildNodes[0])
            {
                var serializer = new XmlSerializer(typeMap[node.Name]);
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(node.OuterXml)))
                {
                    Element elem = (Element)serializer.Deserialize(XmlReader.Create(stream));
                    elements.Add(elem);
                }

            }
        }

        public Container(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

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

        public void SaveAs(string fileName)
        {
            var doc = new XmlDocument();
            XmlElement svgNode = doc.CreateElement(string.Empty, "svg", string.Empty);
            svgNode.SetAttribute("version", "1.1");
            svgNode.SetAttribute("baseProfile", "full");
            svgNode.SetAttribute("width", Width.ToString());
            svgNode.SetAttribute("height", Height.ToString());
            svgNode.SetAttribute("xmlns", @"http://www.w3.org/2000/svg");
            foreach (Element elem in elements)
            {
                if (elem.CanGenerateValidSvgCode())
                {
                    svgNode.AppendChild(elem.GenerateNode(ref doc));
                }
            }
            doc.AppendChild(svgNode);
            doc.Save(fileName);
        }
    }
}
