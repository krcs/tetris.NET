﻿/*

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

namespace tetris.NET
{
    internal enum FigureType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    internal enum GameState
    {
        Running,
        Stopped,
        Paused
    }

    internal enum PlayerMove
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