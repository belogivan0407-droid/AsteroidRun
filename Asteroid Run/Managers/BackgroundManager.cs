using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Run
{
    public class BackgroundManager
    {
        private Texture2D _currentTexture;
        private Texture2D _nextTexture;
        private float _fadeAlpha;

        private float _speed;
        private float _offsetY;

        public BackgroundManager(Texture2D texture, float speed)
        {
            _currentTexture = texture;
            _speed = speed;
            _offsetY = 0;
            _fadeAlpha = 0f;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _offsetY += _speed * deltaTime;

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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _currentTexture,
                Vector2.Zero,
                new Rectangle(0, -(int)_offsetY, 800, 600),
                Color.White
            );

            if (_nextTexture != null)
            {
                spriteBatch.Draw(
                    _nextTexture,
                    Vector2.Zero,
                    new Rectangle(0, -(int)_offsetY, 800, 600),
                    Color.White * _fadeAlpha 
                );
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