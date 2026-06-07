using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Asteroid_Run
{
    internal class UIManager
    {
        public struct MenuButton
        {
            public Rectangle Bounds; public string Text; public Color CurrentColor;
            public MenuButton(Rectangle bounds, string text) { Bounds = bounds; Text = text; CurrentColor = Color.White; }
        }

        public struct ShipButton
        {
            public Rectangle Bounds; public Texture2D Texture; public Color CurrentColor;
            public ShipButton(Rectangle bounds, Texture2D texture) { Bounds = bounds; Texture = texture; CurrentColor = Color.White; }
        }

        private List<MenuButton> _mainMenuButtons = new List<MenuButton>();
        private List<MenuButton> _gameOverButtons = new List<MenuButton>();
        private List<ShipButton> _shipButtons = new List<ShipButton>();
        private MenuButton _backButton;

        private SpriteFont _font;
        private Texture2D _bgTexture;

        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public float ScaleX = 1f;
        public float ScaleY = 1f;

        public UIManager(SpriteFont font, Texture2D bgTexture, List<Texture2D> shipTextures)
        {
            _font = font; _bgTexture = bgTexture;
            int buttonWidth = 300; int buttonHeight = 50; int startX = 400 - (buttonWidth / 2);

            _mainMenuButtons.Add(new MenuButton(new Rectangle(startX, 260, buttonWidth, buttonHeight), "ИГРАТЬ"));
            _mainMenuButtons.Add(new MenuButton(new Rectangle(startX, 330, buttonWidth, buttonHeight), "ВЫБОР КОРАБЛЯ"));
            _mainMenuButtons.Add(new MenuButton(new Rectangle(startX, 400, buttonWidth, buttonHeight), "ИНСТРУКЦИЯ"));

            _gameOverButtons.Add(new MenuButton(new Rectangle(startX, 300, buttonWidth, buttonHeight), "НАЧАТЬ ЗАНОВО"));
            _gameOverButtons.Add(new MenuButton(new Rectangle(startX, 370, buttonWidth, buttonHeight), "В ГЛАВНОЕ МЕНЮ"));

            _backButton = new MenuButton(new Rectangle(startX, 520, buttonWidth, buttonHeight), "НАЗАД");

            int shipStartX = 65; int shipStartY = 220; int spacingX = 140; int spacingY = 150;
            for (int i = 0; i < shipTextures.Count; i++)
            {
                int row = i / 5; int col = i % 5;
                Rectangle bounds = new Rectangle(shipStartX + col * spacingX, shipStartY + row * spacingY, shipTextures[i].Width, shipTextures[i].Height);
                _shipButtons.Add(new ShipButton(bounds, shipTextures[i]));
            }
        }

        public void UpdateInput() { _currentMouse = Mouse.GetState(); }
        public void PostUpdateInput() { _previousMouse = _currentMouse; }
        private bool IsClicked() => _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed;

        private Point GetMousePos()
        {
            return new Point((int)(_currentMouse.X / ScaleX), (int)(_currentMouse.Y / ScaleY));
        }

        public int UpdateMainMenu() => ProcessTextButtons(_mainMenuButtons);
        public int UpdateGameOver() => ProcessTextButtons(_gameOverButtons);

        public bool UpdateBackButton()
        {
            Point mousePos = GetMousePos();
            if (_backButton.Bounds.Contains(mousePos))
            {
                _backButton.CurrentColor = Color.Orange;
                return IsClicked();
            }
            _backButton.CurrentColor = Color.White; return false;
        }

        public int UpdateShipSelection()
        {
            Point mousePos = GetMousePos();
            int clickedIndex = -1;
            for (int i = 0; i < _shipButtons.Count; i++)
            {
                ShipButton btn = _shipButtons[i];
                if (btn.Bounds.Contains(mousePos))
                {
                    btn.CurrentColor = Color.Orange;
                    if (IsClicked()) clickedIndex = i;
                }
                else btn.CurrentColor = Color.White;

                _shipButtons[i] = btn;
            }
            return clickedIndex;
        }

        private int ProcessTextButtons(List<MenuButton> buttons)
        {
            Point mousePos = GetMousePos();
            int clickedButtonIndex = -1;
            for (int i = 0; i < buttons.Count; i++)
            {
                MenuButton button = buttons[i];
                if (button.Bounds.Contains(mousePos))
                {
                    button.CurrentColor = Color.Orange;
                    if (IsClicked()) clickedButtonIndex = i;
                }
                else button.CurrentColor = Color.White;

                buttons[i] = button;
            }
            return clickedButtonIndex;
        }

        public void DrawMainMenu(SpriteBatch spriteBatch, int highScore, int highDistance)
        {
            spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, 800, 600), Color.White);
            DrawCenteredText(spriteBatch, "ASTEROID RUN: СЕКТОР ЗЕРО", 100, Color.Cyan);
            DrawCenteredText(spriteBatch, $"РЕКОРД: {highScore} ОЧКОВ  |  {highDistance} ПАРСЕК", 160, Color.Yellow);
            DrawButtonList(spriteBatch, _mainMenuButtons);
        }

        public void DrawGameOver(SpriteBatch spriteBatch, int score, int distance)
        {
            DrawCenteredText(spriteBatch, "КОРАБЛЬ УНИЧТОЖЕН", 180, Color.Red);
            DrawCenteredText(spriteBatch, $"СЧЕТ: {score}   ПРОГРЕСС: {distance} ПАРСЕК", 230, Color.Cyan);
            DrawButtonList(spriteBatch, _gameOverButtons);
        }

        public void DrawInstructions(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, 800, 600), Color.White);

            DrawCenteredText(spriteBatch, "ИНСТРУКЦИЯ", 50, Color.Cyan);

            int startY = 110;
            int spacing = 35;

            DrawCenteredText(spriteBatch, "--- ЦЕЛЬ ИГРЫ ---", startY, Color.Yellow);
            DrawCenteredText(spriteBatch, "Пролететь как можно дальше сквозь аномалию.", startY + spacing, Color.White);
            DrawCenteredText(spriteBatch, "Сбивайте врагов и собирайте контейнеры.", startY + spacing * 2, Color.White);

            startY += spacing * 4;

            DrawCenteredText(spriteBatch, "--- УПРАВЛЕНИЕ ---", startY, Color.Yellow);
            DrawCenteredText(spriteBatch, "[A] или [<] --- Движение влево", startY + spacing, Color.Orange);
            DrawCenteredText(spriteBatch, "[D] или [>] --- Движение вправо", startY + spacing * 2, Color.Orange);
            DrawCenteredText(spriteBatch, "[ПРОБЕЛ] --- Огонь лазером", startY + spacing * 3, Color.Orange);
            DrawCenteredText(spriteBatch, "[F11] --- Полный экран", startY + spacing * 4, Color.Orange);
            DrawCenteredText(spriteBatch, "[ESC] --- Выход из игры", startY + spacing * 5, Color.Orange);

            DrawButtonList(spriteBatch, new List<MenuButton> { _backButton });
        }

        public void DrawShipSelection(SpriteBatch spriteBatch, int currentShipIndex)
        {
            spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, 800, 600), Color.White);
            DrawCenteredText(spriteBatch, "ВЫБЕРИТЕ КОРАБЛЬ", 80, Color.Cyan);
            for (int i = 0; i < _shipButtons.Count; i++)
            {
                Color tint = (i == currentShipIndex) ? Color.LimeGreen : _shipButtons[i].CurrentColor;
                spriteBatch.Draw(_shipButtons[i].Texture, new Vector2(_shipButtons[i].Bounds.X, _shipButtons[i].Bounds.Y), tint);
            }
            DrawButtonList(spriteBatch, new List<MenuButton> { _backButton });
        }

        private void DrawButtonList(SpriteBatch spriteBatch, List<MenuButton> buttons)
        {
            foreach (var button in buttons)
            {
                Vector2 textSize = _font.MeasureString(button.Text);
                Vector2 textPos = new Vector2(button.Bounds.X + (button.Bounds.Width / 2) - (textSize.X / 2), button.Bounds.Y + (button.Bounds.Height / 2) - (textSize.Y / 2));
                spriteBatch.DrawString(_font, button.Text, textPos, button.CurrentColor);
            }
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, float y, Color color)
        {
            Vector2 size = _font.MeasureString(text);
            spriteBatch.DrawString(_font, text, new Vector2(400 - size.X / 2, y), color);
        }
    }
}