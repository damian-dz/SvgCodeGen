namespace SvgCodeGen
{
    public enum Color
    {
        Aqua = 0x00FFFF,
        Black = 0x000000,
        Blue = 0x0000FF,
        Fuchsia = 0xFF00FF,
        Gray = 0x808080,
        Green = 0x008000,
        Lime = 0x00FF00,
        Maroon = 0x800000,
        Navy = 0x000080,
        Olive = 0x808000,
        Purple = 0x800080,
        Red = 0xFF0000,
        Silver = 0xC0C0C0,
        Teal = 0x008080,
        White = 0xFFFFFF,
        Yellow = 0xFFFF00
    }

    public struct Point
    {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "x: " + X + ", y: " + Y;
        }
    }
}