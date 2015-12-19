/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System.Drawing;

namespace tetris.NET
{
    public class Brick
    {
        private readonly Pen _pen;
        private Rectangle _rectangle;
        private readonly SolidBrush _brush;

        public int X
        {
            set { _rectangle.X = value; }
            get { return _rectangle.X; }
        }

        public int Y
        {
            set { _rectangle.Y = value; }
            get { return _rectangle.Y; }
        }

        public Brick(Point position, int size, Color color)
        {
            _pen = new Pen(Color.Black);
            _brush = new SolidBrush(color);
            _rectangle = new Rectangle(position, new Size(size, size));
        }

        public Brick(int size, Color color) : this(new Point(0, 0), size, color)
        {
        }

        public void Draw(Graphics e)
        {
            e.DrawRectangle(_pen, _rectangle);
            e.FillRectangle(_brush, _rectangle);
        }
    }
}
