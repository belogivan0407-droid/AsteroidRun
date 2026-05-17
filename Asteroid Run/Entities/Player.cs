using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Asteroid_Run
{
    public class Player
    {
        public Vector2 Position;
        private Texture2D _texture;
        private float _speed = 400f;

        private float _shootTimer = 0f;
        private const float ShootDelay = 0.2f; 

        public Player(Texture2D texture, Vector2 startPosition)
        {
            _texture = texture;
            Position = startPosition;
        }

        public void Update(GameTime gameTime, List<Projectile> projectiles)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                Position.X -= _speed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                Position.X += _speed * deltaTime;

            Position.X = MathHelper.Clamp(Position.X, 0, 800 - 32);

            _shootTimer += deltaTime;
            if (keyboard.IsKeyDown(Keys.Space) && _shootTimer >= ShootDelay)
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
                Position.Y
            );

            projectiles.Add(new Projectile(bulletPos, new Vector2(0, -700), false));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
