using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("svg")]
    public class Svg
    {
        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>
        { { "circle",   typeof(SvgCircle)    },
          { "g",        typeof(SvgGroup)     },
          { "line",     typeof(SvgLine)      },
          { "rect",     typeof(SvgRectangle) },
          { "path",     typeof(SvgPath)      },
          { "polygon",  typeof(SvgPolygon)   },
          { "polyline", typeof(SvgPolyline)  },
          { "text",     typeof(SvgText)      } };

        public double Width;
        public double Height;
        public string ViewBox;
        private readonly List<SvgElement> elements = new List<SvgElement>();

        public int ElementCount { get { return elements.Count;  } }

        private SvgGroup DeserializeGroup(XmlNode parent)
        {
            var group = new SvgGroup();
            foreach (XmlNode child in parent)
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name] != typeof(SvgGroup))
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        group.AddElement(elem);
                    }
                }
                else
                {
                    DeserializeGroup(child);
                }
            }
            return group;
        }

        public Svg(string filename)
        {
            var doc = new XmlDocument();
            string allText = File.ReadAllText(filename);
            allText = allText.Replace("xmlns=\"http://www.w3.org/2000/svg\"", string.Empty);
            doc.LoadXml(allText);
            XmlAttributeCollection rootAttributes = doc.ChildNodes[0].Attributes;
            Width = double.Parse(rootAttributes["width"].Value);
            Height = double.Parse(rootAttributes["height"].Value);
            if (rootAttributes["viewBox"] != null) ViewBox = rootAttributes["viewBox"].Value;
            foreach (XmlNode node in doc.ChildNodes[0])
            {
                var serializer = new XmlSerializer(typeMap[node.Name]);
                if (typeMap[node.Name] != typeof(SvgGroup))
                {                 
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(node.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        elements.Add(elem);
                    }
                }
                else
                {
                    elements.Add(DeserializeGroup(node));
                }
            }
        }

        public Svg(double width, double height)
        {
            Width = width;
            Height = height;
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

        public void SetViewBox(double x, double y, double width, double height)
        {
            ViewBox = string.Format("{0} {1} {2} {3}", x, y, width, height);
        }

        public string GenerateSvgCode()
        {
            return GenerateXmlDocument().OuterXml;
        }

        public XmlDocument GenerateXmlDocument()
        {
            var doc = new XmlDocument();
            XmlElement svgNode = doc.CreateElement(string.Empty, "svg", string.Empty);
            svgNode.SetAttribute("xmlns", @"http://www.w3.org/2000/svg");
            svgNode.SetAttribute("version", "1.1");
            svgNode.SetAttribute("baseProfile", "full");
            svgNode.SetAttribute("width", Width.ToString());
            svgNode.SetAttribute("height", Height.ToString());
            if (ViewBox != null) svgNode.SetAttribute("viewBox", ViewBox);
            foreach (SvgElement elem in elements)
            {
                if (elem.CanGenerateValidSvgCode())
                {
                    svgNode.AppendChild(elem.GenerateNode(ref doc));
                }
            }
            doc.AppendChild(svgNode);
            return doc;
        }

        public void SaveAs(string filename)
        {
            GenerateXmlDocument().Save(filename);
        }
    }
}
