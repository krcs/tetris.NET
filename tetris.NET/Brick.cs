/*

Tetris.NET

MIT License

Copyright (c) 2014 Krzysztof Cieślak

Permission  is hereby granted, free  of charge,  to any person obtaining a copy
of this software and associated documentation files (the "Software"),  to  deal
in  the Software without restriction,  including without limitation the  rights
to  use, copy,  modify,  merge, publish, distribute,  sublicense,  and/or  sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this  permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE  IS PROVIDED  "AS IS",  WITHOUT WARRANTY OF ANY  KIND,  EXPRESS OR
IMPLIED,  INCLUDING  BUT NOT  LIMITED  TO  THE WARRANTIES  OF  MERCHANTABILITY,
FITNESS  FOR  A  PARTICULAR  PURPOSE AND NONINFRINGEMENT.  IN  NO  EVENT  SHALL
THE  AUTHORS  OR  COPYRIGHT  HOLDERS  BE  LIABLE  FOR  ANY  CLAIM,  DAMAGES  OR
OTHER  LIABILITY,  WHETHER  IN  AN  ACTION  OF  CONTRACT,  TORT  OR  OTHERWISE,
ARISING  FROM,  OUT  OF  OR  IN CONNECTION WITH  THE SOFTWARE  OR  THE  USE  OR
OTHER DEALINGS IN THE SOFTWARE.

*/

using System.Drawing;

namespace tetris.NET
{
    internal class Brick
    {
        private readonly Pen _pen;
        private Rectangle _rectangle;
        private readonly SolidBrush _brush;

        public int X
        {
            set => _rectangle.X = value; 
            get => _rectangle.X; 
        }

        public int Y
        {
            set => _rectangle.Y = value;
            get => _rectangle.Y;
        }

        public Brick(int size, Color color) 
        {
            _pen = new Pen(Color.Black);
            _brush = new SolidBrush(color);
            _rectangle = new Rectangle(Point.Empty, new Size(size, size));
        }

        public void Draw(Graphics e)
        {
            e.DrawRectangle(_pen, _rectangle);
            e.FillRectangle(_brush, _rectangle);
        }
    }
}