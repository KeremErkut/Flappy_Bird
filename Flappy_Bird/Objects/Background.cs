using System.Drawing;
using Flappy_Bird.Core;

namespace Flappy_Bird.Objects
{
    public class Background : GameObject
    {
        private const float ScrollSpeed = 60f;  

        private readonly Image _bgImage;
        private readonly Image _baseImage;

        
        private float _bgX1, _bgX2;
        private float _baseX1, _baseX2;

        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly int _baseHeight = 112;  

        public Background(int screenWidth, int screenHeight)
            : base(0, 0, screenWidth, screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _bgImage = Image.FromFile("Assets/Sprites/background-day.png");
            _baseImage = Image.FromFile("Assets/Sprites/base.png");

            
            _bgX1 = 0;
            _bgX2 = screenWidth;
            _baseX1 = 0;
            _baseX2 = screenWidth;
        }

        public override void Update(float deltaTime)
        {
        
            _bgX1 -= ScrollSpeed * deltaTime;           
            _bgX2 -= ScrollSpeed * deltaTime;
            _baseX1 -= ScrollSpeed * deltaTime * 2f; 
            _baseX2 -= ScrollSpeed * deltaTime * 2f;

            
            if (_bgX1 + _screenWidth <= 0) _bgX1 = _bgX2 + _screenWidth;
            if (_bgX2 + _screenWidth <= 0) _bgX2 = _bgX1 + _screenWidth;

            if (_baseX1 + _screenWidth <= 0) _baseX1 = _baseX2 + _screenWidth;
            if (_baseX2 + _screenWidth <= 0) _baseX2 = _baseX1 + _screenWidth;
        }

        public override void Draw(Graphics g)
        {
            
            g.DrawImage(_bgImage, _bgX1, 0, _screenWidth, _screenHeight - _baseHeight);
            g.DrawImage(_bgImage, _bgX2, 0, _screenWidth, _screenHeight - _baseHeight);

            
            g.DrawImage(_baseImage, _baseX1, _screenHeight - _baseHeight, _screenWidth, _baseHeight);
            g.DrawImage(_baseImage, _baseX2, _screenHeight - _baseHeight, _screenWidth, _baseHeight);
        }

        
        public int GetGroundY() => _screenHeight - _baseHeight;
    }
}