using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Asteroid_Run
{
    public class Ufo
    {
        public Vector2 Position;
        private Texture2D _texture;
        private float _speed;
        private float _startX;
        private float _timeAlive;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Ufo(Texture2D texture, Vector2 startPos, float speed)
        {
            _texture = texture;
            Position = startPos;
            _startX = startPos.X; 
            _speed = speed;
            _timeAlive = 0f;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeAlive += deltaTime;

            Position.Y += _speed * deltaTime;

            Position.X = _startX + (float)Math.Sin(_timeAlive * 3f) * 120f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}