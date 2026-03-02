using System.Drawing;
using Flappy_Bird.Core;

namespace Flappy_Bird.Objects
{
    public class Pipe : GameObject
    {
        // Constants
        private const float ScrollSpeed = 160f;
        public const int PipeWidth = 52;
        private const int GapSize = 150;

        // Top and bottom pipe images
        private readonly Image _pipeBody;
        private readonly Image _pipeBodyFlipped;

        // The vertical center of the gap between the pipes
        private readonly int _gapCenterY;

        // Is the pipe passed by the bird ? (for scoring purposes)
        public bool IsPassed { get; private set; } = false;

        // Is the pipe off-screen (for cleanup purposes) ?
        public bool IsOffScreen => X + Width < 0;

        public Pipe(float startX, int gapCenterY, int screenHeight)
            : base(startX, 0, PipeWidth, screenHeight)
        {
            _gapCenterY = gapCenterY;

            _pipeBody = Image.FromFile("Assets/Sprites/pipe-green.png");

            // Flip the pipe image vertically for the top pipe
            _pipeBodyFlipped = (Image)_pipeBody.Clone();
            _pipeBody.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public override void Update(float deltaTime)
        {
            // Move the pipe to the left
            X -= ScrollSpeed * deltaTime;
        }

        public override void Draw(Graphics g)
        {
            // Bottom edge of the top pipe is at (gapCenterY - GapSize / 2)
            int gapTop =  _gapCenterY - GapSize / 2;
            // Top edge of the bottom pipe is at (gapCenterY + GapSize / 2)
            int gapBottom = _gapCenterY + GapSize / 2;

            // Top pipe(flipped image) is drawn from the top of the screen down to gapTop
            g.DrawImage(_pipeBodyFlipped, new Rectangle((int)X, 0, PipeWidth, gapTop));
            // Bottom pipe is drawn from gapBottom down to the bottom of the screen
            g.DrawImage(_pipeBody, new Rectangle((int)X, gapBottom, PipeWidth, Height - gapBottom));
        }

        // Is bird passed through the gap between the pipes ? (for scoring purposes)
        public void MarkAsPassed() { IsPassed = true; }

        // Collision detection
        public Rectangle GetTopBounds()
        {
            int gapTop = _gapCenterY - GapSize / 2;
            return new Rectangle((int)X, 0, PipeWidth, gapTop);
        }

        public Rectangle GetBottomBounds()
        {
            int gapBottom = _gapCenterY + GapSize / 2;
            return new Rectangle((int)X, gapBottom, PipeWidth, Height - gapBottom);
        }
    }
}
