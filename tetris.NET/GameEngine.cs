/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System.Drawing;

namespace tetris.NET
{
    class GameEngine
    {
        private static GameEngine _instance;
        private int _score;
        private byte _level;
        private GameState _state;
        private readonly CMatrix<Brick> _board;
        private CMatrix<Brick> _nextFigure;
        private CMatrix<Brick> _player;

        private int _playerColumn;
        private int _playerRow;

        public static GameEngine Instance => _instance ?? (_instance = new GameEngine());

        public GameState State => _state;

        public int Score => _score;

        public byte Level => _level;

        public CMatrix<Brick> Board => _board;

        public CMatrix<Brick> NextFigure => _nextFigure;

        public CMatrix<Brick> Player => _player;

        public int PlayerColumn => _playerColumn;

        public int PlayerRow => _playerRow;

        private GameEngine()
        {
            _board = new CMatrix<Brick>(GameConstans.BOARD_WIDTH, GameConstans.BOARD_HEIGHT);
            _player = new CMatrix<Brick>(1, 1);
            _nextFigure = new CMatrix<Brick>(1, 1);
            _state = GameState.Stopped;
        }

        public void Start()
        {
            _state = GameState.Running;
            _score = _level = 0;
            _board.Clear();
            _nextFigure = GenerateFigure(GameHelper.RandomFigure(), GameHelper.RandomColor());
            InitPlayer();
        }

        public void Pause() { _state = GameState.Paused; }

        public void Resume() { _state = GameState.Running; }

        public void Stop() { _state = GameState.Stopped; }

        private void SetPlayerPosition(int column, int row)
        {
            _playerColumn = column;
            _playerRow = row;
        }

        public void MovePlayer(PlayerMove direction)
        {
            if (!_state.Equals(GameState.Running))
                return;

            switch (direction)
            {
                case (PlayerMove.Left):
                    CheckAndMove(_playerColumn - 1, _playerRow);
                    break;
                case (PlayerMove.Right):
                    CheckAndMove(_playerColumn + 1, _playerRow);
                    break;
                case (PlayerMove.Rotate):
                    CheckAndRotate();
                    break;
                case (PlayerMove.Up):
                    CheckAndMove(_playerColumn, _playerRow - 1);
                    break;
                case (PlayerMove.Down):
                    if (!CheckAndMove(_playerColumn, _playerRow + 1))
                        NextSet();
                    break;
            }
        }

        private bool CheckAndMove(int column, int row)
        {
            bool result = _board.CanOverlay(_player, column, row);

            if (result)
                SetPlayerPosition(column, row);

            return result;
        }

        private void CheckAndRotate()
        {
            CMatrix<Brick> buf = _player.RotateCW90();
            bool result = _board.CanOverlay(buf, _playerColumn, _playerRow);

            if (result)
                _player = buf;
        }

        private void NextSet()
        {
            _board.Merge(_player, _playerColumn, _playerRow);

            _score += Scoring(_board.ClearAndMoveFullRows());

            InitPlayer();
        }

        private void InitPlayer()
        {
            _player = _nextFigure;

            SetPlayerPosition(3, 0);

            if (!_board.CanOverlay(_player, _playerColumn, _playerRow))
                _state = GameState.Stopped;

            _nextFigure = GenerateFigure(GameHelper.RandomFigure(), GameHelper.RandomColor());
        }

        private int Scoring(int clearedRows)
        {
            switch (clearedRows)
            {
                case 1:
                    return 40 * _level;
                case 2:
                    return 100 * _level;
                case 3:
                    return 300 * _level;
                case 4:
                    return 1200 * _level;
                default:
                    return 0;
            }
        }

        public bool TryIncreaseLevel()
        {
            byte newSpeed = 1;

            if (GameHelper.Between(_level, 0, 2000))
                newSpeed = 1;
            if (GameHelper.Between(_score, 2000, 10000))
                newSpeed = 2;
            if (GameHelper.Between(_score, 10000, 20000))
                newSpeed = 3;
            if (GameHelper.Between(_score, 20000, 40000))
                newSpeed = 4;
            if (GameHelper.Between(_score, 40000, 60000))
                newSpeed = 5;
            if (_score >= 60000)
                newSpeed = 6;

            if (_level == newSpeed)
                return false;

            _level = newSpeed;
            return true;
        }

        private CMatrix<Brick> GenerateFigure(FigureType type, Color color)
        {
            CMatrix<Brick> result;

            switch (type)
            {
                case FigureType.I:
                    result = new CMatrix<Brick>(5, 5);
                    result[2, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                    result[2, 3].Set(new Brick(GameConstans.BRICKSIZE, color)); // 00100
                                                                                // 00000
                    break;

                case FigureType.J:
                    result = new CMatrix<Brick>(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[0, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.L:
                    result = new CMatrix<Brick>(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[2, 2].Set(new Brick(GameConstans.BRICKSIZE, color));
                    break;

                case FigureType.O:
                    result = new CMatrix<Brick>(2, 2);
                    result[0, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 11
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 11
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.S:
                    result = new CMatrix<Brick>(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[2, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 000
                    break;

                case FigureType.T:
                    result = new CMatrix<Brick>(3, 3);
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[0, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 010
                    result[1, 2].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    break;

                case FigureType.Z:
                    result = new CMatrix<Brick>(3, 3);
                    result[0, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 
                    result[1, 0].Set(new Brick(GameConstans.BRICKSIZE, color)); // 110
                    result[1, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 011
                    result[2, 1].Set(new Brick(GameConstans.BRICKSIZE, color)); // 000
                    break;
                default:
                    result = new CMatrix<Brick>(1, 1);
                    break;
            }

            return result;
        }

        public void DrawMatrix(CMatrix<Brick> matrix, Graphics gfx)
        {
            DrawMatrix(0, 0, matrix, gfx);
        }

        public void DrawMatrix(int columnPos, int rowPos, CMatrix<Brick> matrix, Graphics gfx)
        {
            for (int row = 0; row < matrix.RowsCount; row++)
                for (int column = 0; column < matrix.ColumnsCount; column++)
                {
                    if (matrix[column, row].HasElement)
                    {
                        Brick currentBrick = matrix[column, row].Peek();
                        currentBrick.X = GameHelper.IndexToPixelX(column + columnPos);
                        currentBrick.Y = GameHelper.IndexToPixelY(row + rowPos);
                        currentBrick.Draw(gfx);
                    }
                }
        }

        public bool FillEmptyRow()
        {
            return _board.FillFirstNotFullRow(GameHelper.RandomBrick());
        }

    }
}