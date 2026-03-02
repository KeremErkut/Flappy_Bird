using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Flappy_Bird.Core;
using Flappy_Bird.Objects;

namespace Flappy_Bird.Managers
{
    public class GameManager
    {
        // --- Screen Sizes ---
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        // --- Game Objects ---
        private Background _background;
        private Bird _bird;
        private List<Pipe> _pipes;

        // --- Managers ---
        private SoundManager _soundManager;

        // --- GameState ---
        public GameState State { get; private set; } = GameState.Menu;
        public int Score { get; private set; } = 0;
        public int HighScore { get; private set; } = 0;

        // --- Pipe Spawn Setting ---
        private float _pipeSpawnTimer = 0f;
        private const float PipeSpawnInterval = 2.2f;  // saniye
        private readonly Random _random = new Random();

        // --- Images ---
        private readonly Image _messageImage;   // başlangıç ekranı
        private readonly Image _gameoverImage;  // game over ekranı
        private readonly Image[] _numberImages; // skor rakamları

        public GameManager(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            // Upload the images
            _messageImage = Image.FromFile("Assets/Sprites/message.png");
            _gameoverImage = Image.FromFile("Assets/Sprites/gameover.png");

            _numberImages = new Image[10];
            for (int i = 0; i < 10; i++)
                _numberImages[i] = Image.FromFile($"Assets/Sprites/{i}.png");

            _soundManager = new SoundManager();

            InitializeGame();
        }

        // Initialize the game (called at the start and when restarting after death)
        private void InitializeGame()
        {
            _background = new Background(_screenWidth, _screenHeight);
            _bird = new Bird(_screenWidth / 4f, _screenHeight / 2f);
            _pipes = new List<Pipe>();
            Score = 0;
            _pipeSpawnTimer = 0f;
        }

        // --- Input ---
        public void OnInput()
        {
            switch (State)
            {
                case GameState.Menu:
                    State = GameState.Playing;
                    _soundManager.Play("swoosh");
                    _bird.Flap();
                    break;

                case GameState.Playing:
                    _bird.Flap();
                    _soundManager.Play("wing");
                    break;

                case GameState.Dead:
                    // Ölüm ekranında tıklayınca yeniden başla
                    InitializeGame();
                    State = GameState.Menu;
                    break;
            }
        }

        // --- Update ---
        public void Update(float deltaTime)
        {
            // Menu'de sadece arka plan hareket eder
            _background.Update(deltaTime);

            if (State != GameState.Playing) return;

            _bird.Update(deltaTime);
            UpdatePipes(deltaTime);
            CheckCollisions();
        }

        private void UpdatePipes(float deltaTime)
        {
            // Pipe spawn
            _pipeSpawnTimer += deltaTime;
            if (_pipeSpawnTimer >= PipeSpawnInterval)
            {
                _pipeSpawnTimer = 0f;
                SpawnPipe();
            }

            // Update all the pipes
            foreach (var pipe in _pipes)
                pipe.Update(deltaTime);

            // Delete off-screen pipes to save memory
            _pipes.RemoveAll(p => p.IsOffScreen);

            // Score Check
            foreach (var pipe in _pipes)
            {
                if (!pipe.IsPassed && pipe.X + Pipe.PipeWidth < _bird.X)
                {
                    pipe.MarkAsPassed();
                    Score++;
                    _soundManager.Play("point");

                    if (Score > HighScore)
                        HighScore = Score;
                }
            }
        }

        private void SpawnPipe()
        {
            // The gap of center setted randomly 
            int groundY = _background.GetGroundY();
            int minGapCenter = 150;
            int maxGapCenter = groundY - 150;
            int gapCenter = _random.Next(minGapCenter, maxGapCenter);

            _pipes.Add(new Pipe(_screenWidth + 50, gapCenter, groundY));
        }

        private void CheckCollisions()
        {
            Rectangle birdBounds = _bird.GetBounds();
            int groundY = _background.GetGroundY();

            // Is the bird hit the ground ?
            if (_bird.Y + _bird.Height >= groundY)
            {
                KillBird();
                return;
            }

            // Is the bird hit the ceiling ?
            if (_bird.Y < 0)
            {
                KillBird();
                return;
            }

            // Is the bird hit the pipes ?
            foreach (var pipe in _pipes)
            {
                if (birdBounds.IntersectsWith(pipe.GetTopBounds()) ||
                    birdBounds.IntersectsWith(pipe.GetBottomBounds()))
                {
                    KillBird();
                    return;
                }
            }
        }

        private void KillBird()
        {
            State = GameState.Dead;
            _soundManager.Play("hit");
            _soundManager.Play("die");
        }

        // --- Draw ---
        public void Draw(Graphics g)
        {
            _background.Draw(g);

            foreach (var pipe in _pipes)
                pipe.Draw(g);

            _bird.Draw(g);

            DrawScore(g);
            DrawUI(g);
        }

        private void DrawScore(Graphics g)
        {
            // Draw the score at the top center of the screen using the number images.
            string scoreStr = Score.ToString();
            int digitWidth = 24;
            int totalWidth = scoreStr.Length * digitWidth;
            int startX = (_screenWidth - totalWidth) / 2;

            for (int i = 0; i < scoreStr.Length; i++)
            {
                int digit = int.Parse(scoreStr[i].ToString());
                g.DrawImage(_numberImages[digit], startX + i * digitWidth, 40, digitWidth, 36);
            }
        }

        private void DrawUI(Graphics g)
        {
            switch (State)
            {
                case GameState.Menu:
                    // Draw the message image at the center of the screen
                    int msgX = (_screenWidth - _messageImage.Width) / 2;
                    int msgY = (_screenHeight - _messageImage.Height) / 2;
                    g.DrawImage(_messageImage, msgX, msgY);
                    break;

                case GameState.Dead:
                    // Draw the game over image at the center of the screen (slightly above the center)
                    int goX = (_screenWidth - _gameoverImage.Width) / 2;
                    int goY = (_screenHeight / 2) - _gameoverImage.Height;
                    g.DrawImage(_gameoverImage, goX, goY);
                    break;
            }
        }
    }
}