using SvgCodeGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace test
{
    class Program
    {

        static void GenerateGrid()
        {
            var container = new Container(400, 300);
            var group = new Group();
            group.SetStroke(Color.Blue);
            for (int i = 10; i <= 390; i += 20)
            {
                var line = new Line(i, 10, i, 290);
                if (i == 10 || i == 390)
                {
                    line.StrokeWidth = 3;
                }
                group.AddElement(line);
            }
            for (int i = 10; i <= 290; i += 20)
            {
                var line = new Line(10, i, 390, i);
                if (i == 10 || i == 290)
                {
                    line.StrokeWidth = 3;
                }
                group.AddElement(line);
            }
            container.AddElement(group);
            container.SaveAs("grid.svg");
        }

        static void GenerateArrow()
        {
            var container = new Container(400, 300);
            var group = new Group();
            group.SetStroke(Color.Black);
            group.StrokeWidth = 20;
            var line = new Line(10, 150, 300, 150);
            group.AddElement(line);
            var polygon = new Polygon(300, 170, 350, 150, 300, 130);
            group.AddElement(polygon);
            container.AddElement(group);
            container.SaveAs("arrow.svg");
        }

        static void Main(string[] args)
        {
            GenerateGrid();
            GenerateArrow();
        }
    }
}
