/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace tetris.NET
{
    public class MainForm : Form
    {
        private Panel _boardPanel;
        private Panel _nextPanel;
        private Label _title;
        private Label _score;
        private Label _level;
        private Label _nextFigure;
        private Button _start;
        private Timer _gameTimer;
        private Timer _refreshTimer;

        private void InitForm()
        {
            FontFamily mainFont = new FontFamily("Verdana");

            Text = "Tetris.NET (K!) 16k";
            ClientSize = new Size(430, 510);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(35, 25, 54);
            AutoSize = false;
            KeyPreview = true;

            _boardPanel = new Panel
            {
                Location = new Point(16, 40),
                Size = new Size(GameConstans.BOARD_WIDTH_PX, GameConstans.BOARD_HEIGHT_PX),
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            _boardPanel.Paint += DrawBoard;
            SetDoubleBuffer(_boardPanel);
            Controls.Add(_boardPanel);

            _nextPanel = new Panel
            {
                Location = new Point(301, 100),
                Size = new Size(GameConstans.BRICKSIZE * 4 + 5, GameConstans.BRICKSIZE * 5 + 5),
            };
            _nextPanel.Paint += DrawNext;
            SetDoubleBuffer(_nextPanel);
            Controls.Add(_nextPanel);

            _title = new Label
            {
                Text = "tetris.NET",
                Location = new Point(273, 8),
                AutoSize = true,
                ForeColor = Color.LightGreen,
                Font = new Font(mainFont, 15.0f, FontStyle.Bold)
            };
            Controls.Add(_title);

            _score = new Label
            {
                Location = new Point(16, 7),
                Size = new Size(GameConstans.BOARD_WIDTH_PX, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font(mainFont, 14.0f, FontStyle.Bold)
            };
            Controls.Add(_score);

            _level = new Label
            {
                Location = new Point(280, 225),
                Size = new Size(140, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Coral,
                Font = new Font(mainFont, 12.0f, FontStyle.Bold),
            };
            Controls.Add(_level);

            _nextFigure = new Label
            {
                Text = "Next:",
                Location = new Point(278, 60),
                AutoSize = true,
                ForeColor = Color.LightSkyBlue,
                Font = new Font(mainFont, 14.0f, FontStyle.Bold)
            };
            Controls.Add(_nextFigure);

            _start = new Button
            {
                Text = "START",
                Location = new Point(285, 450),
                Size = new Size(130, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(96, 60, 187),
                Font = new Font(mainFont, 13.0f, FontStyle.Bold),
                Enabled = true
            };
            _start.FlatAppearance.BorderSize = 0;
            _start.Click += StartClick;

            Controls.Add(_start);

            _gameTimer = new Timer();
            _gameTimer.Tick += GameTick;

            _refreshTimer = new Timer { Interval = GameConstans.REFRESH_INTERVAL_MS };
            _refreshTimer.Tick += RefreshTick;

            UpdateScoreText();
        }

        public MainForm() { InitForm(); }

        private void StartClick(object sender, EventArgs e)
        {
            switch (GameEngine.Instance.State)
            {
                case GameState.Stopped:
                    GameEngine.Instance.Start();
                    _gameTimer.Interval = GameConstans.INITIAL_INVERVAL_MS;
                    _refreshTimer.Interval = GameConstans.REFRESH_INTERVAL_MS;
                    _level.Text = "Level: 1";
                    SetTimersState(true, "PAUSE");
                    break;
                case GameState.Running:
                    GameEngine.Instance.Pause();
                    SetTimersState(false, "PAUSED");
                    break;
                case GameState.Paused:
                    GameEngine.Instance.Resume();
                    GameEngine.Instance.MovePlayer(PlayerMove.Down);
                    SetTimersState(true, "PAUSE");
                    break;
            }
        }

        private void SetTimersState(bool start, string buttonText)
        {
            if (start)
            {
                _gameTimer.Start();
                _refreshTimer.Start();
            }
            else
            {
                _gameTimer.Stop();
                _refreshTimer.Stop();
            }
            _start.Text = buttonText;

        }

        private void UpdateScoreText()
        {
            string score = GameEngine.Instance.Score.ToString();
            _score.Text = "Score: " + score.PadLeft(8, '0');
        }

        private void DrawNext(object sender, PaintEventArgs e)
        {
            int column = GameEngine.Instance.NextFigure.RowsCount == 5 ? -1 : 0;
            GameEngine.Instance.DrawMatrix(column, 0, GameEngine.Instance.NextFigure, e.Graphics);
        }

        private void DrawBoard(object sender, PaintEventArgs e)
        {
            GameEngine.Instance.DrawMatrix(GameEngine.Instance.Board, e.Graphics);
            GameEngine.Instance.DrawMatrix(GameEngine.Instance.PlayerColumn, GameEngine.Instance.PlayerRow,
                GameEngine.Instance.Player, e.Graphics);
        }

        private static void SetDoubleBuffer(object targetControl)
        {
            try
            {
                typeof(Panel).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, targetControl,
                    new object[] { true });
            }
            catch { }
        }

        private void GameTick(object sender, EventArgs e)
        {
            switch (GameEngine.Instance.State)
            {
                case GameState.Running:
                    GameEngine.Instance.MovePlayer(PlayerMove.Down);
                    ChangeSpeed();
                    break;
                case GameState.Stopped:
                    SetTimersState(false, "RESTART");
                    _refreshTimer.Start();
                    break;
            }
        }

        private void ChangeSpeed()
        {
            if (GameEngine.Instance.TryIncreaseLevel())
            {
                _gameTimer.Interval = GameHelper.GetSpeedByLevel(GameEngine.Instance.Level);
                _gameTimer.Stop();
                _gameTimer.Start();
                _level.Text = "Level: " + GameEngine.Instance.Level;
            }

        }

        private void RefreshTick(object sender, EventArgs e)
        {
            if (GameEngine.Instance.State.Equals(GameState.Running))
            {
                UpdateScoreText();
                _boardPanel.Refresh();
                _nextPanel.Refresh();
            }
            if (GameEngine.Instance.State.Equals(GameState.Stopped))
            {
                _boardPanel.Refresh();
                bool isLastRow = GameEngine.Instance.FillEmptyRow();
                if (isLastRow)
                    _refreshTimer.Stop();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                GameEngine.Instance.MovePlayer(PlayerMove.Rotate);
                return true;
            }
            if (keyData == Keys.Down)
            {
                GameEngine.Instance.MovePlayer(PlayerMove.Down);
                return true;
            }
            if (keyData == Keys.Left)
            {
                GameEngine.Instance.MovePlayer(PlayerMove.Left);
                return true;
            }
            if (keyData == Keys.Right)
            {
                GameEngine.Instance.MovePlayer(PlayerMove.Right);
                return true;
            }
            if (keyData == Keys.Escape)
            {
                GameEngine.Instance.Stop();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}