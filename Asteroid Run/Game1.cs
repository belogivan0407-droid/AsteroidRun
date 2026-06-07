using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace Asteroid_Run
{
    public class Game1 : Game
    {
        private enum GameState { MainMenu, ShipSelection, Instructions, Intro, Playing, GameOver }

        private GameState _currentState = GameState.MainMenu;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _loreFont;

        private UIManager _uiManager;
        private BackgroundManager _backgroundManager;

        private Texture2D _bgBlack, _bgIntro, _bgBlue, _bgDarkPurple, _bgPurple;
        private Texture2D _cursorTexture;

        private Player _player;
        private List<Texture2D> _shipTextures = new List<Texture2D>();
        private int _currentShipIndex = 2;
        private Texture2D _playerShieldTexture;
        private Texture2D _brokenShipTexture;

        private List<Projectile> _projectiles = new List<Projectile>();
        private Texture2D _bulletTexture, _enemyBulletTexture;

        private List<Meteorite> _meteors = new List<Meteorite>();
        private List<Texture2D> _meteorTextures = new List<Texture2D>();

        private List<Ufo> _ufos = new List<Ufo>();
        private Texture2D _ufoTexture;

        private List<Phantom> _phantoms = new List<Phantom>();
        private List<Texture2D> _phantomTextures = new List<Texture2D>();

        private List<PowerUp> _powerups = new List<PowerUp>();
        private Texture2D _powerupShieldTex, _powerupBoltTex, _powerupStarTex;
        private float _slowdownTimer = 0f;

        private float _meteorTimer = 0f, _ufoTimer = 0f, _phantomTimer = 0f;
        private Random _random = new Random();
        private float _textTimer = 0f;
        private bool _showPressEnter = true;

        private int _score = 0;
        private float _distance = 0f;
        private int _currentZone = 1;

        private KeyboardState _previousKeyboard;
        private SaveData _saveData;

        private Song _backgroundMusic; 
        private Song _menuMusic;       

        private SoundEffect _sfxLaser1, _sfxLaser2, _sfxLose, _sfxShieldUp, _sfxShieldDown, _sfxZap, _sfxTwoTone;

        private ParticleManager _particleManager;

        private RenderTarget2D _renderTarget;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            _saveData = SaveManager.Load();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _renderTarget = new RenderTarget2D(GraphicsDevice, 800, 600);

            _loreFont = Content.Load<SpriteFont>("Fonts/LoreFont");
            _bgBlack = Content.Load<Texture2D>("Backgrounds/black");
            _bgBlue = Content.Load<Texture2D>("Backgrounds/blue");
            _bgDarkPurple = Content.Load<Texture2D>("Backgrounds/darkPurple");
            _bgPurple = Content.Load<Texture2D>("Backgrounds/purple");
            _bgIntro = Content.Load<Texture2D>("Backgrounds/intro_bg");

            _cursorTexture = Content.Load<Texture2D>("Sprites/cursor");
            _brokenShipTexture = Content.Load<Texture2D>("Sprites/playerShip1_damage3");

            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip1_blue"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip1_green"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip1_orange"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip1_red"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip2_blue"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip2_green"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip2_orange"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip2_red"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip3_blue"));
            _shipTextures.Add(Content.Load<Texture2D>("Sprites/playerShip3_green"));

            _backgroundManager = new BackgroundManager(_bgBlack, 120f, GraphicsDevice);
            _uiManager = new UIManager(_loreFont, _bgBlack, _shipTextures);

            _playerShieldTexture = Content.Load<Texture2D>("Sprites/shield3");

            _backgroundMusic = Content.Load<Song>("music/Sector_Seven_Ascent");
            _menuMusic = Content.Load<Song>("music/Glass_Horizon");

            _sfxLaser1 = Content.Load<SoundEffect>("Sounds/sfx_laser1");
            _sfxLaser2 = Content.Load<SoundEffect>("Sounds/sfx_laser2");
            _sfxLose = Content.Load<SoundEffect>("Sounds/sfx_lose");
            _sfxShieldUp = Content.Load<SoundEffect>("Sounds/sfx_shieldUp");
            _sfxShieldDown = Content.Load<SoundEffect>("Sounds/sfx_shieldDown");
            _sfxZap = Content.Load<SoundEffect>("Sounds/sfx_zap");
            _sfxTwoTone = Content.Load<SoundEffect>("Sounds/sfx_twoTone");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;

            MediaPlayer.Play(_menuMusic);

            _particleManager = new ParticleManager(GraphicsDevice);

            Vector2 startPos = new Vector2(400 - (_shipTextures[_currentShipIndex].Width / 2), 480);
            _player = new Player(_shipTextures[_currentShipIndex], _playerShieldTexture, startPos, _sfxLaser1);

            _bulletTexture = Content.Load<Texture2D>("Sprites/laserBlue01");
            _enemyBulletTexture = Content.Load<Texture2D>("Sprites/laserRed01");
            _ufoTexture = Content.Load<Texture2D>("Sprites/ufo");

            _phantomTextures.Add(Content.Load<Texture2D>("Sprites/phantom"));
            _phantomTextures.Add(Content.Load<Texture2D>("Sprites/enemyGreen3"));
            _phantomTextures.Add(Content.Load<Texture2D>("Sprites/enemyRed4"));
            _phantomTextures.Add(Content.Load<Texture2D>("Sprites/enemyBlack5"));

            _powerupShieldTex = Content.Load<Texture2D>("Sprites/powerupRed_shield");
            _powerupBoltTex = Content.Load<Texture2D>("Sprites/powerupRed_bolt");
            _powerupStarTex = Content.Load<Texture2D>("Sprites/powerupRed_star");

            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_big1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_med1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_small1"));
            _meteorTextures.Add(Content.Load<Texture2D>("Sprites/meteorBrown_tiny1"));
        }

        private void ResetGame()
        {
            _meteors.Clear(); _ufos.Clear(); _phantoms.Clear(); _projectiles.Clear(); _powerups.Clear(); _particleManager.Clear();
            _meteorTimer = 0f; _ufoTimer = 0f; _phantomTimer = 0f; _slowdownTimer = 0f;
            _player.Position = new Vector2(400 - (_shipTextures[_currentShipIndex].Width / 2), 480);
            _player.ShieldHP = 0; _player.SuperShotTimer = 0f; _player.InvincibleTimer = 0f;
            _score = 0; _distance = 0f; _currentZone = 1;
            _backgroundManager.ResetTexture(_bgBlack);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboard = Keyboard.GetState();

            _uiManager.ScaleX = (float)Window.ClientBounds.Width / 800f;
            _uiManager.ScaleY = (float)Window.ClientBounds.Height / 600f;
            _uiManager.UpdateInput();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboard.IsKeyDown(Keys.Escape)) Exit();

            if (currentKeyboard.IsKeyDown(Keys.F11) && !_previousKeyboard.IsKeyDown(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (_currentState)
            {
                case GameState.MainMenu:
                    int clickedMain = _uiManager.UpdateMainMenu();
                    if (clickedMain == 0) _currentState = GameState.Intro;
                    else if (clickedMain == 1) _currentState = GameState.ShipSelection;
                    else if (clickedMain == 2) _currentState = GameState.Instructions;
                    break;
                case GameState.ShipSelection:
                    int clickedShip = _uiManager.UpdateShipSelection();
                    if (clickedShip != -1) { _currentShipIndex = clickedShip; _player.SetTexture(_shipTextures[_currentShipIndex]); }
                    if (_uiManager.UpdateBackButton()) _currentState = GameState.MainMenu;
                    break;
                case GameState.Instructions:
                    if (_uiManager.UpdateBackButton()) _currentState = GameState.MainMenu;
                    break;
                case GameState.Intro:
                    UpdateIntro(gameTime);
                    break;
                case GameState.Playing:
                    UpdateGameplay(gameTime, deltaTime);
                    break;
                case GameState.GameOver:
                    int clickedOver = _uiManager.UpdateGameOver();
                    if (clickedOver == 0)
                    {
                        ResetGame();
                        _currentState = GameState.Playing;
                        MediaPlayer.Play(_backgroundMusic); 
                    }
                    else if (clickedOver == 1)
                    {
                        ResetGame();
                        _currentState = GameState.MainMenu;
                        MediaPlayer.Play(_menuMusic); 
                    }
                    break;
            }

            _previousKeyboard = currentKeyboard;
            _uiManager.PostUpdateInput();
            base.Update(gameTime);
        }

        private void UpdateIntro(GameTime gameTime)
        {
            _textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_textTimer >= 0.5f) { _showPressEnter = !_showPressEnter; _textTimer = 0f; }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _currentState = GameState.Playing;
                MediaPlayer.Play(_backgroundMusic);
            }
        }

        private void UpdateGameplay(GameTime gameTime, float deltaTime)
        {
            float difficultyMultiplier = 1.0f + (_distance / 1000f);
            float currentEnemySpeedMult = _slowdownTimer > 0 ? 0.4f : 1.0f;
            if (_slowdownTimer > 0) _slowdownTimer -= deltaTime;

            GameTime enemyTime = new GameTime(gameTime.TotalGameTime, TimeSpan.FromSeconds(deltaTime * currentEnemySpeedMult));
            _backgroundManager.SetSpeed(120f * difficultyMultiplier * currentEnemySpeedMult);
            _backgroundManager.Update(gameTime);

            _particleManager.Update(deltaTime);

            _particleManager.CreateEngineTrail(_player.Position + new Vector2(_player.Bounds.Width / 2, _player.Bounds.Height - 10));

            _distance += (5f * difficultyMultiplier) * deltaTime;

            if (_distance >= 900f && _currentZone < 3) { _currentZone = 3; _backgroundManager.ChangeTexture(_bgPurple); }
            else if (_distance >= 400f && _currentZone < 2) { _currentZone = 2; _backgroundManager.ChangeTexture(_bgDarkPurple); }

            _player.Update(gameTime, _projectiles);

            _meteorTimer += deltaTime;
            float spawnDelay = MathHelper.Max(0.3f, 1.2f - (_distance / 1500f));
            if (_meteorTimer > spawnDelay)
            {
                Texture2D randomTex = _meteorTextures[_random.Next(_meteorTextures.Count)];
                _meteors.Add(new Meteorite(randomTex, new Vector2(_random.Next(0, 800 - randomTex.Width), -randomTex.Height), _random.Next(150, 300) * difficultyMultiplier, (float)(_random.NextDouble() * 2 - 1)));
                _meteorTimer = 0;
            }

            _ufoTimer += deltaTime;
            if (_ufoTimer > 6f && _currentZone >= 2)
            {
                _ufos.Add(new Ufo(_ufoTexture, new Vector2(_random.Next(100, 700), -50), 120f * difficultyMultiplier)); _ufoTimer = 0;
            }

            _phantomTimer += deltaTime;
            if (_phantomTimer > 12f && _currentZone >= 3)
            {
                Texture2D randomPhantomTex = _phantomTextures[_random.Next(_phantomTextures.Count)];
                Vector2 pos = new Vector2(_random.Next(100, 700), -100);
                _phantoms.Add(new Phantom(randomPhantomTex, pos, 100f * difficultyMultiplier, _sfxLaser2));
                _phantomTimer = 0;
            }

            for (int i = 0; i < _meteors.Count; i++) { _meteors[i].Update(enemyTime); if (_meteors[i].Position.Y > 650) { _meteors.RemoveAt(i); i--; } }
            for (int i = 0; i < _ufos.Count; i++) { _ufos[i].Update(enemyTime); if (_ufos[i].Position.Y > 650) { _ufos.RemoveAt(i); i--; } }
            for (int i = 0; i < _phantoms.Count; i++) { _phantoms[i].Update(enemyTime, _player.Position, _projectiles); if (_phantoms[i].Position.Y > 650) { _phantoms.RemoveAt(i); i--; } }

            for (int i = 0; i < _projectiles.Count; i++)
            {
                if (_projectiles[i].IsEnemy) _projectiles[i].Update(enemyTime); else _projectiles[i].Update(gameTime);
                if (_projectiles[i].Position.Y < -50 || _projectiles[i].Position.Y > 650) { _projectiles.RemoveAt(i); i--; }
            }

            for (int i = 0; i < _powerups.Count; i++)
            {
                _powerups[i].Update(deltaTime);
                if (_player.Bounds.Intersects(_powerups[i].Bounds))
                {
                    switch (_powerups[i].Type)
                    {
                        case PowerUpType.Shield: _player.ShieldHP = 3; _sfxShieldUp.Play(); break;
                        case PowerUpType.SuperShot: _player.SuperShotTimer = 5f; _sfxZap.Play(); break;
                        case PowerUpType.Slowdown: _slowdownTimer = 5f; _sfxTwoTone.Play(); break;
                    }
                    _powerups.RemoveAt(i); i--; continue;
                }
                if (_powerups[i].Position.Y > 650) { _powerups.RemoveAt(i); i--; }
            }

            for (int i = 0; i < _projectiles.Count; i++)
            {
                if (_projectiles[i].IsEnemy) continue;
                bool bulletDestroyed = false;

                for (int j = 0; j < _meteors.Count; j++)
                {
                    if (_projectiles[i].Bounds.Intersects(_meteors[j].Bounds))
                    {
                        int mw = _meteors[j].Bounds.Width; _score += (mw > 80) ? 10 : (mw > 40) ? 25 : 50;
                        _particleManager.CreateExplosion(_meteors[j].Position + new Vector2(mw / 2, mw / 2), 15);
                        _meteors.RemoveAt(j); bulletDestroyed = true; break;
                    }
                }
                if (!bulletDestroyed)
                {
                    for (int j = 0; j < _ufos.Count; j++)
                    {
                        if (_projectiles[i].Bounds.Intersects(_ufos[j].Bounds))
                        {
                            _score += 150;
                            if (_random.Next(100) < 50)
                            {
                                PowerUpType type = (PowerUpType)_random.Next(3);
                                Texture2D tex = type == PowerUpType.Shield ? _powerupShieldTex : (type == PowerUpType.Slowdown ? _powerupBoltTex : _powerupStarTex);
                                _powerups.Add(new PowerUp(tex, _ufos[j].Position, type));
                            }
                            _particleManager.CreateExplosion(_ufos[j].Position + new Vector2(30, 30), 25);
                            _ufos.RemoveAt(j); bulletDestroyed = true; break;
                        }
                    }
                }
                if (!bulletDestroyed)
                {
                    for (int j = 0; j < _phantoms.Count; j++)
                    {
                        if (_projectiles[i].Bounds.Intersects(_phantoms[j].Bounds))
                        {
                            _score += 300;
                            _particleManager.CreateExplosion(_phantoms[j].Position + new Vector2(_phantoms[j].Bounds.Width / 2, _phantoms[j].Bounds.Height / 2), 40);
                            _phantoms.RemoveAt(j); bulletDestroyed = true; break;
                        }
                    }
                }
                if (bulletDestroyed) { _projectiles.RemoveAt(i); i--; continue; }
            }

            bool TakeDamage()
            {
                if (_player.InvincibleTimer > 0) return false;
                if (_player.ShieldHP > 0) { _player.ShieldHP--; _sfxShieldDown.Play(); _player.InvincibleTimer = 0.8f; return false; }
                _particleManager.CreateExplosion(_player.Position + new Vector2(_player.Bounds.Width / 2, _player.Bounds.Height / 2), 100);
                return true;
            }

            for (int i = 0; i < _meteors.Count; i++)
            {
                if (_player.Bounds.Intersects(_meteors[i].Bounds))
                {
                    bool hitRegistered = _player.InvincibleTimer <= 0;
                    if (TakeDamage()) { _currentState = GameState.GameOver; }
                    if (hitRegistered) { _meteors.RemoveAt(i); i--; }
                    if (_currentState == GameState.GameOver) break;
                }
            }
            if (_currentState != GameState.GameOver)
            {
                for (int i = 0; i < _ufos.Count; i++)
                {
                    if (_player.Bounds.Intersects(_ufos[i].Bounds))
                    {
                        bool hitRegistered = _player.InvincibleTimer <= 0;
                        if (TakeDamage()) { _currentState = GameState.GameOver; }
                        if (hitRegistered) { _ufos.RemoveAt(i); i--; }
                        if (_currentState == GameState.GameOver) break;
                    }
                }
            }
            if (_currentState != GameState.GameOver)
            {
                for (int i = 0; i < _phantoms.Count; i++)
                {
                    if (_player.Bounds.Intersects(_phantoms[i].Bounds))
                    {
                        bool hitRegistered = _player.InvincibleTimer <= 0;
                        if (TakeDamage()) { _currentState = GameState.GameOver; }
                        if (hitRegistered) { _phantoms.RemoveAt(i); i--; }
                        if (_currentState == GameState.GameOver) break;
                    }
                }
            }
            if (_currentState != GameState.GameOver)
            {
                for (int i = 0; i < _projectiles.Count; i++)
                {
                    if (_projectiles[i].IsEnemy && _player.Bounds.Intersects(_projectiles[i].Bounds))
                    {
                        bool hitRegistered = _player.InvincibleTimer <= 0;
                        if (TakeDamage()) { _currentState = GameState.GameOver; }
                        if (hitRegistered) { _projectiles.RemoveAt(i); i--; }
                        if (_currentState == GameState.GameOver) break;
                    }
                }
            }

            if (_currentState == GameState.GameOver)
            {
                MediaPlayer.Stop(); 
                _sfxLose.Play();
                bool isNewRecord = false;
                if (_score > _saveData.HighScore) { _saveData.HighScore = _score; isNewRecord = true; }
                if ((int)_distance > _saveData.HighDistance) { _saveData.HighDistance = (int)_distance; isNewRecord = true; }
                if (isNewRecord) SaveManager.Save(_saveData);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            switch (_currentState)
            {
                case GameState.MainMenu: _uiManager.DrawMainMenu(_spriteBatch, _saveData.HighScore, _saveData.HighDistance); break;
                case GameState.ShipSelection: _uiManager.DrawShipSelection(_spriteBatch, _currentShipIndex); break;
                case GameState.Instructions: _uiManager.DrawInstructions(_spriteBatch); break;
                case GameState.Intro: DrawIntro(); break;
                case GameState.Playing: _backgroundManager.Draw(_spriteBatch); DrawGameplay(gameTime); break;
                case GameState.GameOver: _backgroundManager.Draw(_spriteBatch); DrawGameplay(gameTime); _uiManager.DrawGameOver(_spriteBatch, _score, (int)_distance); break;
            }

            if (_currentState == GameState.MainMenu || _currentState == GameState.ShipSelection || _currentState == GameState.Instructions || _currentState == GameState.GameOver)
            {
                MouseState mouseState = Mouse.GetState();
                _spriteBatch.Draw(_cursorTexture, new Vector2(mouseState.X / _uiManager.ScaleX, mouseState.Y / _uiManager.ScaleY), Color.White);
            }
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawIntro()
        {
            _spriteBatch.Draw(_bgIntro, new Rectangle(0, 0, 800, 600), Color.White);
            if (_showPressEnter) _spriteBatch.DrawString(_loreFont, "НАЖМИТЕ [ENTER] ДЛЯ НАЧАЛА МИССИИ", new Vector2(170, 540), Color.Yellow);
        }

        private void DrawGameplay(GameTime gameTime)
        {
            foreach (var meteor in _meteors) meteor.Draw(_spriteBatch);
            foreach (var ufo in _ufos) ufo.Draw(_spriteBatch);
            foreach (var phantom in _phantoms) phantom.Draw(_spriteBatch);
            foreach (var powerup in _powerups) powerup.Draw(_spriteBatch);

            foreach (var bullet in _projectiles)
            {
                Texture2D tex = bullet.IsEnemy ? _enemyBulletTexture : _bulletTexture;
                bullet.Draw(_spriteBatch, tex);
            }

            _particleManager.Draw(_spriteBatch);

            bool isDead = _currentState == GameState.GameOver;
            _player.Draw(_spriteBatch, gameTime, isDead, _brokenShipTexture);

            _spriteBatch.DrawString(_loreFont, $"ОЧКИ: {_score}", new Vector2(22, 22), Color.Black);
            _spriteBatch.DrawString(_loreFont, $"ОЧКИ: {_score}", new Vector2(20, 20), Color.Cyan);
            _spriteBatch.DrawString(_loreFont, $"ПРОГРЕСС: {(int)_distance} ПАРСЕК", new Vector2(22, 52), Color.Black);
            _spriteBatch.DrawString(_loreFont, $"ПРОГРЕСС: {(int)_distance} ПАРСЕК", new Vector2(20, 50), Color.Orange);
            _spriteBatch.DrawString(_loreFont, $"ЗОНА: {_currentZone}", new Vector2(22, 82), Color.Black);
            _spriteBatch.DrawString(_loreFont, $"ЗОНА: {_currentZone}", new Vector2(20, 80), Color.Red);

            int yOffset = 112;
            if (_player.ShieldHP > 0) { _spriteBatch.DrawString(_loreFont, $"ЩИТ: {_player.ShieldHP}", new Vector2(20, yOffset), Color.DeepSkyBlue); yOffset += 30; }
            if (_player.SuperShotTimer > 0) { _spriteBatch.DrawString(_loreFont, $"СУПЕР: {(int)_player.SuperShotTimer + 1} СЕК", new Vector2(20, yOffset), Color.Gold); yOffset += 30; }
            if (_slowdownTimer > 0) { _spriteBatch.DrawString(_loreFont, $"ЗАМЕДЛЕНИЕ: {(int)_slowdownTimer + 1} СЕК", new Vector2(20, yOffset), Color.LimeGreen); }
        }
    }
}