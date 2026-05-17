using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Asteroid_Run
{
    public class Game1 : Game
    {
        private enum GameState
        {
            Intro,
            Playing
        }

        private GameState _currentState = GameState.Intro;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _loreFont;

        // Фон
        private BackgroundManager _backgroundManager;
        private Texture2D _bgBlack, _bgIntro;

        // Игрок
        private Player _player;
        private Texture2D _playerTexture;

        // Снаряды
        private List<Projectile> _projectiles = new List<Projectile>();
        private Texture2D _bulletTexture;

        // Метеориты
        private List<Meteorite> _meteors = new List<Meteorite>();
        private List<Texture2D> _meteorTextures = new List<Texture2D>();
        private float _spawnTimer = 0f;
        private Random _random = new Random();

        private float _textTimer = 0f;
        private bool _showPressEnter = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _loreFont = Content.Load<SpriteFont>("Fonts/LoreFont");

            _bgBlack = Content.Load<Texture2D>("Backgrounds/black");
            _bgIntro = Content.Load<Texture2D>("Backgrounds/intro_bg");

            _backgroundManager = new BackgroundManager(_bgBlack, 120f);

            _playerTexture = Content.Load<Texture2D>("Sprites/playerShip1_orange");
            Vector2 startPos = new Vector2(400 - (_playerTexture.Width / 2), 520);
            _player = new Player(_playerTexture, startPos);

            _bulletTexture = Content.Load<Texture2D>("Sprites/laserBlue01");

            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_big1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_med1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_small1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_tiny1"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (_currentState)
            {
                case GameState.Intro:
                    UpdateIntro(gameTime);
                    break;

                case GameState.Playing:
                    _backgroundManager.Update(gameTime);
                    UpdateGameplay(gameTime, deltaTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateIntro(GameTime gameTime)
        {
            _textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_textTimer >= 0.5f)
            {
                _showPressEnter = !_showPressEnter;
                _textTimer = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _currentState = GameState.Playing;
            }
        }

        private void UpdateGameplay(GameTime gameTime, float deltaTime)
        {
            _player.Update(gameTime, _projectiles);

            _spawnTimer += deltaTime;
            if (_spawnTimer > 1.2f)
            {
                Texture2D randomTex = _meteorTextures[_random.Next(_meteorTextures.Count)];
                Vector2 pos = new Vector2(_random.Next(0, 800 - randomTex.Width), -randomTex.Height);
                float speed = _random.Next(150, 300);
                float rotSpeed = (float)(_random.NextDouble() * 2 - 1);

                _meteors.Add(new Meteorite(randomTex, pos, speed, rotSpeed));
                _spawnTimer = 0;
            }

            for (int i = 0; i < _meteors.Count; i++)
            {
                _meteors[i].Update(gameTime);
                if (_meteors[i].Position.Y > 650) { _meteors.RemoveAt(i); i--; }
            }

            for (int i = 0; i < _projectiles.Count; i++)
            {
                _projectiles[i].Update(gameTime);
                if (_projectiles[i].Position.Y < -50) { _projectiles.RemoveAt(i); i--; }
            }

            for (int i = 0; i < _projectiles.Count; i++)
            {
                for (int j = 0; j < _meteors.Count; j++)
                {
                    if (_projectiles[i].Bounds.Intersects(_meteors[j].Bounds))
                    {
                        _projectiles.RemoveAt(i);
                        _meteors.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            switch (_currentState)
            {
                case GameState.Intro:
                    DrawIntro();
                    break;

                case GameState.Playing:
                    _backgroundManager.Draw(_spriteBatch);
                    DrawGameplay();
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawIntro()
        {
            _spriteBatch.Draw(_bgIntro, new Rectangle(0, 0, 800, 600), Color.White);

            if (_showPressEnter)
            {
                _spriteBatch.DrawString(_loreFont, "НАЖМИТЕ [ENTER] ДЛЯ НАЧАЛА МИССИИ", new Vector2(170, 540), Color.Yellow);
            }
        }

        private void DrawGameplay()
        {
            foreach (var meteor in _meteors)
            {
                meteor.Draw(_spriteBatch);
            }

            foreach (var bullet in _projectiles)
            {
                bullet.Draw(_spriteBatch, _bulletTexture);
            }

            _player.Draw(_spriteBatch);
        }
    }
}