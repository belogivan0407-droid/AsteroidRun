using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Run
{
    public class Meteorite
    {
        public Vector2 Position;
        private Texture2D _texture;
        private float _speed;
        private float _rotation;
        private float _rotationSpeed;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Meteorite(Texture2D texture, Vector2 position, float speed, float rotationSpeed)
        {
            _texture = texture;
            Position = position;
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _rotation = 0f;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position.Y += _speed * deltaTime;

            _rotation += _rotationSpeed * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            spriteBatch.Draw(_texture, Position + origin, null, Color.White, _rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}