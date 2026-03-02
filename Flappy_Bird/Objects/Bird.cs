using System.Drawing;
using System.Windows.Forms;
using Flappy_Bird.Core;

namespace Flappy_Bird.Objects
{
    public class Bird : GameObject
    {
        // ------------------- Physics Constants -------------------
        private const float Gravity = 1200f; // pixels per second squared
        private const float FlapForce = -380f; // pixels per second (negative for upward movement)
        private const float MaxFallSpeed = 800f; // pixels per second (terminal velocity)

        // ------------------- Pyhsics State -------------------
        private float _velocityY = 0f;

        // Animation
        private readonly Image[] _frames; // Array to hold bird's animation frames
        private int _currentFrame = 0;
        private float _animationTimer = 0f;
        private const float FrameDuration = 0.1f;

        // --------------------- Rotation -------------------
        // The bird will tilt up when flapping and tilt down when falling.
        private float _rotation = 0f;

        public Bird(float x, float y) : base(x, y, 34, 24) // Assuming the bird's sprite is 34x24 pixels
        {
            // Asset Path 
            _frames = new Image[]
            {
                Image.FromFile("Assets/Sprites/bluebird-upflap.png"),
                Image.FromFile("Assets/Sprites/bluebird-midflap.png"),
                Image.FromFile("Assets/Sprites/bluebird-downflap.png")
            };
        }

        // Called when the player presses the space button.
        public void Flap()
        {
            _velocityY = FlapForce;
        }

        public override void Update(float deltaTime)
        {
            // Add gravity to speed in each frame
            _velocityY += Gravity * deltaTime;

            // Terminal velocity
            if (_velocityY > MaxFallSpeed)
                _velocityY = MaxFallSpeed;

            // Update position
            Y += _velocityY * deltaTime;

            // Rotation : +90 degrees when falling, -30 degrees when flapping
            float targetRotation = _velocityY > 0 ? 90f : -30f;
            // Smoothly interpolate towards the target rotation for a more natural look
            _rotation += (targetRotation - _rotation) * 10f * deltaTime; // 10f is the rotation speed factor

            //Update animation frame
            _animationTimer += deltaTime;
            if (_animationTimer >= FrameDuration)
            {
                _animationTimer -= 0f;
                _currentFrame = (_currentFrame + 1) % _frames.Length; 
            }
        }

        public override void Draw(Graphics g)
        {
            var state = g.Save();

            g.TranslateTransform(X + Width / 2f, Y + Height / 2f);
            g.RotateTransform(_rotation);
            g.TranslateTransform(-Width / 2f, -Height / 2f);

            g.DrawImage(_frames[_currentFrame], 0, 0, Width, Height);

            g.Restore(state);

        }

    }



}