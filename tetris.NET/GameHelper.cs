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

using System;
using System.Drawing;

namespace tetris.NET
{
    internal static class GameHelper
    {
        private static readonly Random Random = new Random();

        public static int IndexToPixel(int idx) => (idx * GameConstans.BRICKSIZE) + idx;

        public static FigureType RandomFigure()
        {
            switch (Random.Next(0, 6))
            {
                case (0): return FigureType.I;
                case (1): return FigureType.J;
                case (2): return FigureType.L;
                case (3): return FigureType.O;
                case (4): return FigureType.S;
                case (5): return FigureType.T;
                case (6): return FigureType.Z;
                default: return FigureType.I;
            }
        }

        public static Color RandomColor() =>
            Color.FromArgb(Random.Next(100, 255), Random.Next(100, 255), Random.Next(100, 255));
          
        public static int GetSpeedByLevel(int level)
        {
            switch (level)
            {
                case 1:
                    return 1000;
                case 2:
                    return 800;
                case 3:
                    return 600;
                case 4:
                    return 400;
                case 5:
                    return 200;
                case 6:
                    return 100;
                default:
                    return 1000;
            }
        }

        public static Brick RandomBrick() => new Brick(GameConstans.BRICKSIZE, RandomColor());
    }
}