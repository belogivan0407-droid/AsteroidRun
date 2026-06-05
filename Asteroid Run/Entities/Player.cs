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
        private Texture2D _shieldTexture; 
        private float _speed = 400f;

        private float _shootTimer = 0f;
        private const float ShootDelay = 0.2f;

        public int ShieldHP = 0;
        public float SuperShotTimer = 0f;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Player(Texture2D texture, Texture2D shieldTexture, Vector2 startPosition)
        {
            _texture = texture;
            _shieldTexture = shieldTexture;
            Position = startPosition;
        }

        public void SetTexture(Texture2D newTexture)
        {
            _texture = newTexture;
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

            if (SuperShotTimer > 0)
            {
                SuperShotTimer -= deltaTime;
                if (SuperShotTimer < 0) SuperShotTimer = 0;
            }

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

            if (SuperShotTimer > 0)
            {
                projectiles.Add(new Projectile(bulletPos, new Vector2(0, -700), false));
                projectiles.Add(new Projectile(bulletPos, new Vector2(-150, -700), false));
                projectiles.Add(new Projectile(bulletPos, new Vector2(150, -700), false));
            }
            else
            {
                projectiles.Add(new Projectile(bulletPos, new Vector2(0, -700), false));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);

            if (ShieldHP > 0)
            {
                Vector2 shieldPos = new Vector2(
                    Position.X + (_texture.Width / 2) - (_shieldTexture.Width / 2),
                    Position.Y + (_texture.Height / 2) - (_shieldTexture.Height / 2)
                );

                float opacity = 0.4f + (ShieldHP * 0.2f);

                spriteBatch.Draw(_shieldTexture, shieldPos, Color.Cyan * opacity);
            }
        }
    }
}