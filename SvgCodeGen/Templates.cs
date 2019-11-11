using System;
using System.Collections.Generic;
using System.Text;

namespace SvgCodeGen
{
    public static class Templates
    {
        public static SvgGroup Grid(double x, double y, double width, double height, int cellsX, int cellsY)
        {
            var group = new SvgGroup();
            var rect = new SvgRectangle(x, y, width, height)
            {
                StrokeWidth = 3
            };
            double stepX = width / cellsX;
            double stepY = height / cellsY;
            group.AddElement(rect);
            for (double i = x + stepX; i < x + width; i += stepX) {
                var line = new SvgLine(i, y, i, y + height);
                group.AddElement(line);
            }
            for (double i = y + stepY; i < y + height; i += stepY) {
                var line = new SvgLine(x, i, x + width, i);
                group.AddElement(line);
            }
            return group;
        }
    }
}
