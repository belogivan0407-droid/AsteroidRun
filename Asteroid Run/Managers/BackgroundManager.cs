using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Run
{
    public class BackgroundManager
    {
        private Texture2D _texture;
        private float _speed;
        private float _offsetY;

        public BackgroundManager(Texture2D texture, float speed)
        {
            _texture = texture;
            _speed = speed;
            _offsetY = 0;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _offsetY += _speed * deltaTime;

            if (_offsetY >= _texture.Height)
            {
                _offsetY -= _texture.Height;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Vector2.Zero,
                new Rectangle(0, -(int)_offsetY, 800, 600),
                Color.White
            );
        }
    }
}