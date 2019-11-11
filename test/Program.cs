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
        static void Main()
        {
            var svg = new Svg(1000, 800);
            SvgGroup grid = Templates.Grid(0, 0, svg.Width, svg.Height, 10, 8);
            grid.SetFill(Color.Aqua);
            grid.SetStroke(Color.Black);
            svg.AddElement(grid);
            svg.SaveAs("grid.svg");
        }
    }
}
