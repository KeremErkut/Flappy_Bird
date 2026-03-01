namespace Flappy_Bird.Core
{
    public interface IUpdatable
    {
        // deltaTime is essential for smooth movement regardless of frame rate
        void Update(float deltaTime);
    }
}