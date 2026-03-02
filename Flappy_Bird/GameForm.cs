using System;
using System.Drawing;
using System.Windows.Forms;
using Flappy_Bird.Managers;

namespace Flappy_Bird
{
    public partial class GameForm : Form
    {
        // --- Screen dimensions ---
        private const int ScreenWidth = 360;
        private const int ScreenHeight = 512;

        // --- Game Loop ---
        private readonly Timer _gameTimer;
        private DateTime _lastUpdate;

        // --- Manager ---
        private readonly GameManager _gameManager;

        // --- Double Buffering ---
        private BufferedGraphicsContext _graphicsContext;
        private BufferedGraphics _bufferedGraphics;

        public GameForm()
        {
            InitializeComponent();

            // Screen Settings
            ClientSize = new Size(ScreenWidth, ScreenHeight);
            Text = "Flappy Bird";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Double buffering Setup
            _graphicsContext = BufferedGraphicsManager.Current;
            _bufferedGraphics = _graphicsContext.Allocate(
                CreateGraphics(),
                new Rectangle(0, 0, ScreenWidth, ScreenHeight)
            );

            // Initialize Game Manager
            _gameManager = new GameManager(ScreenWidth, ScreenHeight);

            // Input — Space or mouse click
            KeyDown += OnKeyDown;
            MouseDown += OnMouseDown;

            // Game loop timer — 60 FPS goal
            _gameTimer = new Timer();
            _gameTimer.Interval = 16; // ~60 FPS (1000ms / 60)
            _gameTimer.Tick += GameLoop;
            _lastUpdate = DateTime.Now;
            _gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Calculate DeltaTime 
            DateTime now = DateTime.Now;
            float deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            // Limit DeltaTime to prevent big jumps (e.g., when debugging or if the game lags)
            if (deltaTime > 0.05f) deltaTime = 0.05f;

            // Update
            _gameManager.Update(deltaTime);

            // Draw
            _gameManager.Draw(_bufferedGraphics.Graphics);

            // Copy the buffered graphics to the screen
            _bufferedGraphics.Render();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                _gameManager.OnInput();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _gameManager.OnInput();
        }
    }
}