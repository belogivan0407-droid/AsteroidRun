using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroid_Run
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _playerTexture;
        private Vector2 _playerPosition;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _playerTexture = new Texture2D(GraphicsDevice, 32, 32);
            Color[] colorData = new Color[32 * 32];
            for (int i = 0; i < colorData.Length; i++)
                colorData[i] = Color.White;
            _playerTexture.SetData(colorData);

            _playerPosition = new Vector2(400, 500);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState keyboard = Keyboard.GetState();
            float speed = 300f;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboard.IsKeyDown(Keys.Left))
                _playerPosition.X -= speed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Right))
                _playerPosition.X += speed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Up))
                _playerPosition.Y -= speed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Down))
                _playerPosition.Y += speed * deltaTime;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
            _spriteBatch.End();
        }
    }
}
