using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SvgCodeGen
{
    [XmlRoot("svg")]
    public class Svg
    {
        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>
        { { "circle",   typeof(SvgCircle)    },
          { "defs",     typeof(SvgDefs)      },
          { "ellipse",  typeof(SvgEllipse)   },
          { "g",        typeof(SvgGroup)     },
          { "line",     typeof(SvgLine)      },
          { "marker",   typeof(SvgMarker)    },       
          { "path",     typeof(SvgPath)      },
          { "polygon",  typeof(SvgPolygon)   },
          { "polyline", typeof(SvgPolyline)  },
          { "rect",     typeof(SvgRectangle) },
          { "text",     typeof(SvgText)      },
          { "use",      typeof(SvgUse)       } };

        public double Width;
        public double Height;
        public string ViewBox;
        private readonly List<SvgElement> elements = new List<SvgElement>();

        public int ElementCount { get { return elements.Count;  } }

        public string RemoveAllChildren(XmlNode xmlNode)
        {
            while (xmlNode.HasChildNodes)
            {
                xmlNode.RemoveChild(xmlNode.FirstChild);
            }
            return xmlNode.OuterXml;
        }

        private SvgDefs DeserializeDefs(XmlNode parent)
        {
            SvgDefs defs = new SvgDefs();
            var defsSerializer = new XmlSerializer(typeof(SvgDefs));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(parent.OuterXml)))
            {
                defs = (SvgDefs)defsSerializer.Deserialize(XmlReader.Create(stream));
            }
            foreach (XmlNode child in parent)
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name] != typeof(SvgDefs)
                    && typeMap[child.Name] != typeof(SvgGroup)
                    && typeMap[child.Name] != typeof(SvgMarker))
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        defs.AddElement(elem);
                    }
                }
                else if (typeMap[child.Name] == typeof(SvgDefs))
                {
                    defs.AddElement(DeserializeDefs(child));
                }
                else if (typeMap[child.Name] == typeof(SvgGroup))
                {
                    defs.AddElement(DeserializeGroup(child));
                }
                else if (typeMap[child.Name] == typeof(SvgMarker))
                {
                    defs.AddElement(DeserializeMarker(child));
                }
            }
            return defs;
        }

        private SvgGroup DeserializeGroup(XmlNode parent)
        {
            SvgGroup group;
            var groupSerializer = new XmlSerializer(typeof(SvgGroup));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(parent.OuterXml)))
            {
                group = (SvgGroup)groupSerializer.Deserialize(XmlReader.Create(stream));
            }
            foreach (XmlNode child in parent)
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name] != typeof(SvgDefs)
                    && typeMap[child.Name] != typeof(SvgGroup)
                    && typeMap[child.Name] != typeof(SvgMarker))
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        group.AddElement(elem);
                    }
                }
                else if (typeMap[child.Name] == typeof(SvgDefs))
                {
                    group.AddElement(DeserializeDefs(child));
                }
                else if (typeMap[child.Name] == typeof(SvgGroup))
                {
                    group.AddElement(DeserializeGroup(child));
                }
                else if (typeMap[child.Name] == typeof(SvgMarker))
                {
                    group.AddElement(DeserializeMarker(child));
                }
            }
            return group;
        }

        private SvgMarker DeserializeMarker(XmlNode parent)
        {
            SvgMarker marker;
            var markerSerializer = new XmlSerializer(typeof(SvgMarker));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(parent.OuterXml)))
            {
                marker = (SvgMarker)markerSerializer.Deserialize(XmlReader.Create(stream));
            }
            foreach (XmlNode child in parent)
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name] != typeof(SvgDefs)
                    && typeMap[child.Name] != typeof(SvgGroup)
                    && typeMap[child.Name] != typeof(SvgMarker))
                {
                   
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        marker.AddElement(elem);
                    }
                }
                else if (typeMap[child.Name] == typeof(SvgDefs))
                {
                    marker.AddElement(DeserializeDefs(child));
                }
                else if (typeMap[child.Name] == typeof(SvgGroup))
                {
                    marker.AddElement(DeserializeGroup(child));
                }
                else if (typeMap[child.Name] == typeof(SvgMarker))
                {
                    marker.AddElement(DeserializeMarker(child));
                }
            }
            return marker;
        }

        private SvgNestedElement DeserializeNestedElement(XmlNode parent)
        {
            SvgNestedElement nestedElem = GetNestedElementInstance(typeMap[parent.Name]);
            var markerSerializer = new XmlSerializer(nestedElem.GetType());
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(parent.OuterXml)))
            {
                nestedElem = (SvgNestedElement)markerSerializer.Deserialize(XmlReader.Create(stream));
            }
            foreach (XmlNode child in parent)
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name].BaseType != typeof(SvgNestedElement))
                {

                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        nestedElem.AddElement(elem);
                    }
                }
                else
                {
                    nestedElem.AddElement(DeserializeNestedElement(child));
                }
            }
            return nestedElem;
        }


        public SvgNestedElement GetNestedElementInstance(Type t)
        {
            if (t == typeof(SvgDefs))
            {
                return new SvgDefs();
            }
            else if (t == typeof(SvgGroup))
            {
                return new SvgGroup();
            }
            else
            {
                return new SvgMarker();
            }
        }

        public Svg(string filename)
        {
            var doc = new XmlDocument();
            string allText = File.ReadAllText(filename);
            allText = allText.Replace("xmlns=\"http://www.w3.org/2000/svg\"", string.Empty);
            allText = Regex.Replace(allText, @"<!--[^<]*-->", string.Empty);
            Console.WriteLine(allText);
            doc.LoadXml(allText);
            XmlAttributeCollection rootAttributes = doc.ChildNodes[0].Attributes;
            if (rootAttributes["width"] != null) Width = double.Parse(rootAttributes["width"].Value);
            if (rootAttributes["height"] != null) Height = double.Parse(rootAttributes["height"].Value);
            if (rootAttributes["viewBox"] != null) ViewBox = rootAttributes["viewBox"].Value;
            foreach (XmlNode child in doc.ChildNodes[0])
            {
                var serializer = new XmlSerializer(typeMap[child.Name]);
                if (typeMap[child.Name] != typeof(SvgDefs)
                    && typeMap[child.Name] != typeof(SvgGroup)
                    && typeMap[child.Name] != typeof(SvgMarker))
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(child.OuterXml)))
                    {
                        var elem = (SvgElement)serializer.Deserialize(XmlReader.Create(stream));
                        elements.Add(elem);
                    }
                }
                else if (typeMap[child.Name] == typeof(SvgDefs))
                {
                    elements.Add(DeserializeDefs(child));
                }
                else if (typeMap[child.Name] == typeof(SvgGroup))
                {
                    elements.Add(DeserializeGroup(child));
                }
                else if (typeMap[child.Name] == typeof(SvgMarker))
                {
                    elements.Add(DeserializeMarker(child));
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

        public void SetViewBox(double x, double y, double width, double height, string unit)
        {
            ViewBox = string.Format("{0}{1} {2}{3} {4}{5} {6}{7}", x, unit, y, unit, width, unit, height, unit);
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
            if (Width > 0) svgNode.SetAttribute("width", Width.ToString());
            if (Height > 0) svgNode.SetAttribute("height", Height.ToString());
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
