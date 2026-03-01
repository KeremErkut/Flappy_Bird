using System.Drawing;

namespace Flappy_Bird.Core
{
    public abstract class  GameObject : IDrawable, IUpdatable
    {
        // Encapsulate the position and size of the game object
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        // Constructor to initialize the game object with position and size
        protected GameObject(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // Sub classes must implement the Draw and Update methods
        public abstract void Draw(Graphics g);
        public abstract void Update(float deltaTime);

        // Collision detection - Common for all game objects
        // Virtual method allows sub classes to override if its necessary (e.g., for more complex shapes)
        public virtual Rectangle GetBounds()
        {
            return new Rectangle((int)X, (int)Y, Width, Height);
        }
    }               
}
