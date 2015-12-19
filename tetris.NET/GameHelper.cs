/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System;
using System.Drawing;

namespace tetris.NET
{
    static class GameHelper
    {
        private static readonly Random Random = new Random();

        public static int IndexToPixelX(int columnIndex) { return (columnIndex * GameConstans.BRICKSIZE) + columnIndex; }

        public static int IndexToPixelY(int rowIndex) { return (rowIndex * GameConstans.BRICKSIZE) + rowIndex; }

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

        public static Color RandomColor()
        {
            return Color.FromArgb(Random.Next(100, 255), Random.Next(100, 255), Random.Next(100, 255));
        }

        public static bool Between(int number, int lower, int upper)
        {
            return number >= lower && number < upper;
        }

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

        public static Brick RandomBrick()
        {
            return new Brick(GameConstans.BRICKSIZE, RandomColor());
        }
    }
}