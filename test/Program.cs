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
            var container = new Svg(400, 300);
            var group = new SvgGroup();
            group.SetStroke(Color.Blue);
            for (int i = 10; i <= 390; i += 20) {
                var line = new SvgLine(i, 10, i, 290);
                if (i == 10 || i == 390) {
                    line.StrokeWidth = 3;
                }
                group.AddElement(line);
            }
            for (int i = 10; i <= 290; i += 20) {
                var line = new SvgLine(10, i, 390, i);
                if (i == 10 || i == 290) {
                    line.StrokeWidth = 3;
                }
                group.AddElement(line);
            }
            container.AddElement(group);
            container.SaveAs("grid.svg");
        }

        static SvgGroup GenerateGrid(int x, int y, int width, int height, int cellsX, int cellsY, Color color)
        {
            var group = new SvgGroup();
            group.SetStroke(Color.Black);
            group.SetFill(color);
            var rect = new SvgRectangle(x, y, width, height)
            {
                StrokeWidth = 3
            };
            int stepX = width / cellsX;
            int stepY = height / cellsY;
            group.AddElement(rect);
            for (double i = x + stepX; i < x + width; i += stepX) {
                var line = new SvgLine(i, y, i, y + height);
                group.AddElement(line);
            }
            for (int i = y + stepY; i < y + height; i += stepY) {
                var line = new SvgLine(x, i, x + width, i);
                group.AddElement(line);
            }
            return group;
        }

        static void GenerateArrow()
        {
            var container = new Svg(400, 300);
            var group = new SvgGroup();
            group.SetStroke(Color.Black);
            group.StrokeWidth = 20;
            var line = new SvgLine(10, 150, 300, 150);
            group.AddElement(line);
            var polygon = new SvgPolygon(300, 170, 350, 150, 300, 130);
            group.AddElement(polygon);
            container.AddElement(group);
            container.SaveAs("arrow.svg");
        }

        static Point RotatePoint(double cx, double cy, double angle, Point p)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);
            p.X -= cx;
            p.Y -= cy;
            double xnew = p.X * c - p.Y * s;
            double ynew = p.X * s + p.Y * c;
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }

        static Point RotatePoint(Point p, double angle, Point pivot)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);
            double px = p.X - pivot.X;
            double py = p.Y - pivot.Y;
            double xnew = px * c - py * s;
            double ynew = px * s + py * c;
            px = xnew + pivot.X;
            py = ynew + pivot.Y;
            return new Point(px, py);
        }

        static SvgGroup GenerateSideView()
        {
            var group = new SvgGroup
            {
                Fill = "none",
                StrokeWidth = 1,
            };
            group.SetStroke(Color.Black);
            var polygon = new SvgPolygon();
            polygon.AddPoint(new Point(54, 0));
            polygon.AddPoint(new Point(54, 226));
            polygon.AddPoint(new Point(40.5, 226));
            polygon.AddPoint(new Point(40.5, 176.5));
            polygon.AddPoint(new Point(0, 176.5));
            polygon.AddPoint(new Point(0, 49.5));
            polygon.AddPoint(new Point(40.5, 49.5));
            group.AddElement(polygon);
            return group;
        }

        static SvgGroup GenerateFrontView()
        {
            var group = new SvgGroup
            {
                Fill = "none",
                StrokeWidth = 1,
            };
            group.SetStroke(Color.Black);
            double r1 = 226 / 2d;
            double r2 = 127 / 2d;
            double r01 = r2 * 0.1;
            double r02 = r2 * 0.06;
            double cx = r1;
            double cy = r1;
            double rDashed = ((226 - 127) / 2d + 127) / 2d;
            var circle1 = new SvgCircle(cx, cy, r1);
            var circle2 = new SvgCircle(cx, cy, r2);
            var circleDashed = new SvgCircle(cx, cy, rDashed)
            {
                StrokeDashArray = "20 3 4 3"
            };
            //  var verLine = new Line(cx, cy - r1 - 10, cx, cy + r1 + 10)
            //  {
            //      StrokeDashArray = "20 3 4 3"
            //  };
            group.AddElement(circle1);
            group.AddElement(circle2);
            group.AddElement(circleDashed);
            var pivot = new Point(cx, cy);
            var startPoint = new Point(cx, cy - rDashed);
            for (double i = 0, alpha = 0; i < 4; i++, alpha += Math.PI / 2) {
                Point center = startPoint.RotateAround(pivot, alpha);
                group.AddElement(new SvgCircle(center, r01));
            }
            for (double i = 0, alpha = -Math.PI / 4; i < 2; i++, alpha += Math.PI) {
                Point center = startPoint.RotateAround(pivot, alpha);
                group.AddElement(new SvgCircle(center, r02));
            }

            var p1 = new Point(cx, cy - r2);
            var p2 = new Point(cx, 0);

            for (double i = 0, alpha = -Math.PI / 4; i < 2; i++, alpha += Math.PI) {
                var line = new SvgLine(p1.RotateAround(pivot, alpha), p2.RotateAround(pivot, alpha));
                line.StrokeDashArray = "20 3 4 3";
                group.AddElement(line);
            }

            // group.AddElement(verLine);    
            return group;
        }


        static void GenerateLeftArrow()
        {
            uint width = 200;
            uint height = 200;
            var container = new Svg(width, height);
            SvgPolygon pol = new SvgPolygon
            {
                StrokeWidth = 16
            };
            pol.Fill = "none";
            pol.SetStroke(Color.Black);
            pol.AddPoint(0, height / 2);
            pol.AddPoint(width / 5 * 3, 0);
            pol.AddPoint(width / 5 * 3, height / 4);
            pol.AddPoint(width, height / 4);
            pol.AddPoint(width, height / 4 * 3);
            pol.AddPoint(width / 5 * 3, height / 4 * 3);
            pol.AddPoint(width / 5 * 3, height);

            SvgGroup g = new SvgGroup();
            double s = (200 - 64) / 200d;
            g.Transform = string.Format("scale({0}, {1})", s, s);

            g.AddElement(pol);
            container.AddElement(g);
            container.SaveAs("left_arrow.svg");
        }

        static void GenerateRightArrow()
        {
            uint width = 200;
            uint height = 200;
            var container = new Svg(width, height);
            SvgPolygon pol = new SvgPolygon
            {
                StrokeWidth = 16
            };
            pol.Fill = "none";
            pol.SetStroke(Color.Black);
            pol.AddPoint(0, height / 4);
            pol.AddPoint(width / 5 * 2, height / 4);
            pol.AddPoint(width / 5 * 2, 0);
            pol.AddPoint(width, height / 2);
            pol.AddPoint(width / 5 * 2, height);
            pol.AddPoint(width / 5 * 2, height / 4 * 3);
            pol.AddPoint(0, height / 4 * 3);

            SvgGroup g = new SvgGroup();
            double s = (200 - 64) / 200d;
            g.Transform = string.Format("scale({0}, {1})", s, s);

            g.AddElement(pol);
            container.AddElement(g);
            container.SaveAs("right_arrow.svg");
        }

        private static double[] LinSpace(double x1, double x2, int n)
        {
            double step = (x2 - x1) / (n - 1);
            var y = new double[n];
            for (int i = 0; i < n; i++) {
                y[i] = x1 + step * i;
            }
            return y;
        }

        private static double[] SineWave(double[] x)
        {
            int n = x.Length;
            var y = new double[n];
            for (int i = 0; i < n; i++) {
                y[i] = Math.Sin(x[i]);
            }
            return y;
        }

        static void Main(string[] args)
        {
            //var path = new SvgPath();
            //path.Id = "test";
            //path.MoveTo(150, 0);
            //path.LineTo(75, 200);
            //path.LineTo(225, 200);
            //path.Close();



            var svg = new Svg(1000, 1000);
            svg.AddElement(GenerateGrid(10, 220, 100, 200, 2, 4, Color.Silver));
            svg.AddElement(GenerateGrid(120, 110, 200, 100, 4, 2, Color.White));
            svg.AddElement(GenerateGrid(120, 220, 200, 200, 4, 4, Color.Yellow));
            svg.AddElement(GenerateGrid(330, 10, 150, 200, 3, 4, Color.White));
            svg.AddElement(GenerateGrid(330, 220, 150, 200, 3, 4, Color.Yellow));
            svg.AddElement(GenerateGrid(490, 60, 100, 150, 2, 3, Color.White));
            svg.AddElement(GenerateGrid(490, 220, 100, 200, 2, 4, Color.Silver));
            svg.AddElement(path);
            svg.SaveAs("test_grid.svg");


            //var svg2 = new Svg("test_grid.svg");
            //  Console.WriteLine(svg2.GenerateSvgCode());




            var svgSine = new Svg(1000, 100);
            svgSine.SetViewBox(0, -1.025, 20, 2.05, "px");

            var polyline = new SvgPolyline();
            var xPoints = LinSpace(0, 20, 800);
            var yPoints = SineWave(xPoints);

            polyline.SetPoints(xPoints, yPoints);
            polyline.Style = "fill:none;stroke:black;stroke-width:0.05";
            // Console.WriteLine(polyline.GenerateSvgCode());

            Console.WriteLine(polyline.GetType().BaseType.Name);
            var svgPlot = new Svg("plot.svg");
            //   Console.WriteLine(svgPlot.GenerateSvgCode());
            svgPlot.SaveAs("plot_copy.svg");

            var gr = new SvgMarker();
            gr.RefX = 1234;
            gr.AddElement(polyline);
            svgSine.AddElement(gr);
            svgSine.SaveAs("test_sine.svg");

            //Console.WriteLine(((SvgGroup)svgPlot.GetElementAt(0)).SomeField);



            // GenerateLeftArrow();
            // GenerateRightArrow();
            //var cont = new Container(@"C:\Users\Damian\Documents\code\Git\QtBibleViewer\App\img_res\copy_plus.svg");

            //for (int i = 0; i < cont.GetElementAt(i).ElementCount; i++)
            //{
            //    cont.GetElementAt(i).Style = null;
            //    cont.GetElementAt(i).SetStroke(Color.Black);
            //    cont.GetElementAt(i).StrokeWidth = 2;
            //}

            //cont.SaveAs("test.svg");


            //Group sideView = GenerateSideView();
            //sideView.AddElement(sideView);
            //container.AddElement(sideView);

            //Group frontView = GenerateFrontView();
            //frontView.Transform = "translate(150,0)";
            // container.AddElement(frontView);

            // var horLine = new Line(20, cy, 390, cy)
            // {
            //     StrokeWidth = 0.8,
            //     StrokeDashArray = "20 3 4 3"
            // };
            // horLine.SetStroke(Color.Black);
            // container.AddElement(horLine);

            // container.AddElement(new Text(0, 15, "text"));

            // container.SaveAs("test.svg");
            // var cont = new Container("Diagram1.svg");
            // cont.SaveAs("Diagram1c.svg");

            // GenerateGrid();
            // GenerateArrow();
        }
    }
}
