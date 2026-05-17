using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Run
{
    public class Projectile
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool IsEnemy; 
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 9, 37);

        public Projectile(Vector2 position, Vector2 velocity, bool isEnemy)
        {
            Position = position;
            Velocity = velocity;
            IsEnemy = isEnemy;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}