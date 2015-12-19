/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
namespace tetris.NET
{
    public enum FigureType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    public enum GameState
    {
        Running,
        Stopped,
        Paused
    }

    public enum PlayerMove
    {
        Up,
        Down,
        Left,
        Right,
        Rotate
    }

    internal static class GameConstans
    {
        public const byte BRICKSIZE = 24;

        public const byte BOARD_WIDTH = 10;

        public const byte BOARD_HEIGHT = 18;

        public const int BOARD_WIDTH_PX = (BRICKSIZE * BOARD_WIDTH) + BOARD_WIDTH + 1;

        public const int BOARD_HEIGHT_PX = (BRICKSIZE * BOARD_HEIGHT) + BOARD_HEIGHT + 1;

        public const int INITIAL_INVERVAL_MS = 1000;

        public const int REFRESH_INTERVAL_MS = 10;
    }
}