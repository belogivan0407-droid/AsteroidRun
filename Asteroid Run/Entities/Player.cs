using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
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
        public float InvincibleTimer = 0f;

        private SoundEffect _shootSound;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Player(Texture2D texture, Texture2D shieldTexture, Vector2 startPosition, SoundEffect shootSound)
        {
            _texture = texture;
            _shieldTexture = shieldTexture;
            Position = startPosition;
            _shootSound = shootSound;
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

            Position.X = MathHelper.Clamp(Position.X, 0, 800 - _texture.Width);

            if (InvincibleTimer > 0)
            {
                InvincibleTimer -= deltaTime;
                if (InvincibleTimer < 0) InvincibleTimer = 0;
            }

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

            _shootSound.Play(0.3f, 0f, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, bool isDead = false, Texture2D brokenTexture = null)
        {
            if (isDead && brokenTexture != null)
            {
                Vector2 brokenPos = new Vector2(
                    Position.X + (_texture.Width / 2) - (brokenTexture.Width / 2),
                    Position.Y + (_texture.Height / 2) - (brokenTexture.Height / 2)
                );
                spriteBatch.Draw(brokenTexture, brokenPos, Color.White);
                return;
            }

            Color tint = Color.White;

            if (InvincibleTimer > 0)
            {
                if (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 30) > 0)
                {
                    tint = Color.White * 0.2f;
                }
            }

            spriteBatch.Draw(_texture, Position, tint);

            if (ShieldHP > 0)
            {
                Vector2 shieldPos = new Vector2(
                    Position.X + (_texture.Width / 2) - (_shieldTexture.Width / 2),
                    Position.Y + (_texture.Height / 2) - (_shieldTexture.Height / 2)
                );

                float opacity = 0.4f + (ShieldHP * 0.2f);
                if (InvincibleTimer > 0 && tint.A < 255) opacity *= 0.3f;

                spriteBatch.Draw(_shieldTexture, shieldPos, Color.Cyan * opacity);
            }
        }
    }
}