using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Asteroid_Run
{
    public class Phantom
    {
        public Vector2 Position;
        private Texture2D _texture;
        private float _speed;
        private float _shootTimer;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Phantom(Texture2D texture, Vector2 startPos, float speed)
        {
            _texture = texture;
            Position = startPos;
            _speed = speed;
            _shootTimer = 0f;
        }

        public void Update(GameTime gameTime, Vector2 targetPosition, List<Projectile> projectiles)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position.X = MathHelper.Lerp(Position.X, targetPosition.X, 1.2f * deltaTime);
            Position.Y += _speed * deltaTime;

            _shootTimer += deltaTime;
            if (_shootTimer >= 1.5f) 
            {
                Shoot(projectiles);
                _shootTimer = 0f;
            }
        }

        private void Shoot(List<Projectile> projectiles)
        {
            float laserWidth = 9f;
            Vector2 bulletPos = new Vector2(
                Position.X + (_texture.Width / 2) - (laserWidth / 2),
                Position.Y + _texture.Height 
            );

            projectiles.Add(new Projectile(bulletPos, new Vector2(0, 500), true));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, new Color(255, 100, 100));
        }
    }
}