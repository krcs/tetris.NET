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
    internal class GameEngine
    {
        private static GameEngine _instance;

        public static GameEngine Instance => _instance ?? (_instance = new GameEngine());

        public GameState State { get; private set; }

        public int Score { get; private set; }

        public byte Level { get; private set; }

        public CMatrix Board { get; }

        public CMatrix NextFigure { get; private set; }

        public CMatrix Player { get; private set; }

        public int PlayerColumn { get; private set; }

        public int PlayerRow { get; private set; }

        private GameEngine()
        {
            Board = new CMatrix(GameConstans.BOARD_WIDTH, GameConstans.BOARD_HEIGHT);
            Player = new CMatrix(1, 1);
            NextFigure = new CMatrix(1, 1);
            State = GameState.Stopped;
        }

        public void Start()
        {
            State = GameState.Running;
            Score = Level = 0;
            Board.Clear();
            NextFigure = GenerateFigure(GameHelper.RandomFigure(), GameHelper.RandomColor());
            InitPlayer();
        }

        public void Pause() => State = GameState.Paused; 

        public void Resume() => State = GameState.Running; 

        public void Stop() => State = GameState.Stopped; 

        private void SetPlayerPosition(int column, int row)
        {
            PlayerColumn = column;
            PlayerRow = row;
        }

        public void MovePlayer(PlayerMove direction)
        {
            if (!State.Equals(GameState.Running))
                return;
  
            switch (direction)
            {
                case (PlayerMove.Left):
                    CheckAndMove(PlayerColumn - 1, PlayerRow);
                    break;
                case (PlayerMove.Right):
                    CheckAndMove(PlayerColumn + 1, PlayerRow);
                    break;
                case (PlayerMove.Rotate):
                    CheckAndRotate();
                    break;
                case (PlayerMove.Up):
                    CheckAndMove(PlayerColumn, PlayerRow - 1);
                    break;
                case (PlayerMove.Down):
                    if (!CheckAndMove(PlayerColumn, PlayerRow + 1))
                        NextSet();
                    break;
            }
        }

        private bool CheckAndMove(int column, int row)
        {
            var result = Board.CanOverlay(Player, column, row);

            if (result)
                SetPlayerPosition(column, row);

            return result;
        }

        private void CheckAndRotate()
        {
            var buf = Player.RotateCW90();
            if (Board.CanOverlay(buf, PlayerColumn, PlayerRow))
                Player = buf;
        }

        private void NextSet()
        {
            Board.Merge(Player, PlayerColumn, PlayerRow);
            Score += Scoring(Board.ClearAndMoveFullRows());
            InitPlayer();
        }

        private void InitPlayer()
        {
            Player = NextFigure;

            SetPlayerPosition(3, 0);

            if (!Board.CanOverlay(Player, PlayerColumn, PlayerRow))
                State = GameState.Stopped;

            NextFigure = GenerateFigure(GameHelper.RandomFigure(), GameHelper.RandomColor());
        }

        private int Scoring(int clearedRows)
        {
            switch (clearedRows)
            {
                case 1:
                    return 40 * Level;
                case 2:
                    return 100 * Level;
                case 3:
                    return 300 * Level;
                case 4:
                    return 1200 * Level;
                default:
                    return 0;
            }
        }

        public bool TryIncreaseLevel()
        {
            byte newSpeed = 1;

            if (Score >= 0 && Score < 2000)
                newSpeed = 1;
            if (Score >= 2000 && Score < 10000)
                newSpeed = 2;
            if (Score >= 10000 && Score < 20000)
                newSpeed = 3;
            if (Score >= 20000 && Score < 40000)
                newSpeed = 4;
            if (Score >= 40000 && Score < 60000)
                newSpeed = 5;
            if (Score >= 60000)
                newSpeed = 6;

            if (Level == newSpeed)
                return false;

            Level = newSpeed;
            return true;
        }

        private CMatrix GenerateFigure(FigureType type, Color color)
        {
            CMatrix result;

            switch (type)
            {
                case FigureType.I:
                    result = new CMatrix(5, 5);
                    result[2, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 3].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                                                                                // 00000
                    break;

                case FigureType.J:
                    result = new CMatrix(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[0, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.L:
                    result = new CMatrix(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[2, 2].Set(new Brick(GameConstans.BRICKSIZE, color));
                    break;

                case FigureType.O:
                    result = new CMatrix(2, 2);
                    result[0, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 11
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 11
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.S:
                    result = new CMatrix(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[2, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 000
                    break;

                case FigureType.T:
                    result = new CMatrix(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.Z:
                    result = new CMatrix(3, 3);
                    result[0, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[2, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 000
                    break;
                default:
                    result = new CMatrix(1, 1);
                    break;
            }
            return result;
        }

        public void DrawMatrix(CMatrix matrix, Graphics gfx) => DrawMatrix(0, 0, matrix, gfx);
        
        public void DrawMatrix(int columnPos, int rowPos, CMatrix matrix, Graphics gfx)
        {
            for (var row = 0; row < matrix.RowsCount; row++)
                for (var column = 0; column < matrix.ColumnsCount; column++)
                    if (matrix[column, row].HasElement)
                    {
                        var currentBrick = matrix[column, row].Peek();
                        currentBrick.X = GameHelper.IndexToPixel(column + columnPos);
                        currentBrick.Y = GameHelper.IndexToPixel(row + rowPos);
                        currentBrick.Draw(gfx);
                    }
        }

        public bool FillEmptyRow() => Board.FillFirstNotFullRow(GameHelper.RandomBrick());
    }
}