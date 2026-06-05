using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Run
{
    public enum PowerUpType
    {
        Shield,
        Slowdown,
        SuperShot
    }

    public class PowerUp
    {
        public Vector2 Position;
        public Texture2D Texture;
        public PowerUpType Type;
        private float _speed;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public PowerUp(Texture2D texture, Vector2 startPos, PowerUpType type)
        {
            Texture = texture;
            Position = startPos;
            Type = type;
            _speed = 100f; 
        }

        public void Update(float deltaTime)
        {
            Position.Y += _speed * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
