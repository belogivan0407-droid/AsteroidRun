using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Asteroid_Run
{
    public class BackgroundManager
    {
        private Texture2D _currentTexture;
        private Texture2D _nextTexture;
        private float _fadeAlpha;

        private float _speed;
        private float _offsetY;

        private struct Star { public Vector2 Position; public float SpeedMultiplier; public float Size; }
        private List<Star> _stars = new List<Star>();
        private Texture2D _starPixel;

        public BackgroundManager(Texture2D texture, float speed, GraphicsDevice graphicsDevice)
        {
            _currentTexture = texture;
            _speed = speed;
            _offsetY = 0;
            _fadeAlpha = 0f;

            _starPixel = new Texture2D(graphicsDevice, 1, 1);
            _starPixel.SetData(new[] { Color.White });

            Random rand = new Random();
            for (int i = 0; i < 60; i++)
            {
                _stars.Add(new Star
                {
                    Position = new Vector2(rand.Next(0, 800), rand.Next(0, 600)),
                    SpeedMultiplier = (float)(0.4f + rand.NextDouble() * 0.8f),
                    Size = rand.Next(1, 4) 
                });
            }
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _offsetY += (_speed * 0.4f) * deltaTime;

            if (_offsetY >= _currentTexture.Height)
            {
                _offsetY -= _currentTexture.Height;
            }

            if (_nextTexture != null)
            {
                _fadeAlpha += deltaTime * 0.5f;
                if (_fadeAlpha >= 1f)
                {
                    _currentTexture = _nextTexture;
                    _nextTexture = null;
                    _fadeAlpha = 0f;
                }
            }

            for (int i = 0; i < _stars.Count; i++)
            {
                var star = _stars[i];
                star.Position.Y += (_speed * star.SpeedMultiplier) * deltaTime;

                if (star.Position.Y > 600)
                {
                    star.Position.Y = -5;
                    star.Position.X = new Random().Next(0, 800);
                }
                _stars[i] = star;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentTexture, Vector2.Zero, new Rectangle(0, -(int)_offsetY, 800, 600), Color.White);

            if (_nextTexture != null)
            {
                spriteBatch.Draw(_nextTexture, Vector2.Zero, new Rectangle(0, -(int)_offsetY, 800, 600), Color.White * _fadeAlpha);
            }

            foreach (var star in _stars)
            {
                float alpha = star.SpeedMultiplier * 0.8f;
                spriteBatch.Draw(_starPixel, new Rectangle((int)star.Position.X, (int)star.Position.Y, (int)star.Size, (int)star.Size), Color.White * alpha);
            }
        }

        public void ChangeTexture(Texture2D newTexture)
        {
            if (_currentTexture == newTexture || _nextTexture == newTexture) return;
            _nextTexture = newTexture;
            _fadeAlpha = 0f;
        }

        public void ResetTexture(Texture2D texture)
        {
            _currentTexture = texture;
            _nextTexture = null;
            _fadeAlpha = 0f;
        }
    }
}