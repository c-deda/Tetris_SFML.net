using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System;

namespace Tetris
{
    public enum MenuSelection
    {
        Start,
        Exit
    };

    public class MenuState : State
    {
        private MenuSelection _selection;
        private Sprite _menuBackground;
        private RectangleShape _selectionBox;
        private Text _startText;
        private Text _exitText;
        private Sound _moveSound;
        private Sound _selectSound;

        public override void Init(GameData data)
        {
            // Initialize Menu Selection
            _selection = MenuSelection.Start;

            // Setup Assets
            _menuBackground = new Sprite(data.Asset.BackgroundTexture, new IntRect(0, 0, Constants.WindowWidth, Constants.WindowHeight));
            _startText = new Text("Start", data.Asset.GameFont);
            _startText.Position = new Vector2f((data.Window.Size.X / 2) - (_startText.GetGlobalBounds().Width / 2), (data.Window.Size.Y / 2));
            _exitText = new Text("Exit", data.Asset.GameFont);
            _exitText.Position = new Vector2f((data.Window.Size.X / 2) - (_exitText.GetGlobalBounds().Width / 2), (data.Window.Size.Y / 2) + 90);
            _moveSound = new Sound(data.Asset.MenuMoveSound);
            _selectSound = new Sound(data.Asset.MenuSelectSound);
        }

        public override void HandleInput(GameData data, SFML.Window.KeyEventArgs e)
        {
            switch (e.Code)
            {
                case SFML.Window.Keyboard.Key.Up:
                    ChangeSelection(true);
                    break;
                case SFML.Window.Keyboard.Key.Down:
                    ChangeSelection(false);
                    break;
                case SFML.Window.Keyboard.Key.Return:
                    EnterSelection(data);
                    break;
            }
        }

        public override void Update(GameData data)
        {
            UpdateSelectionBox();
        }

        public override void Draw(GameData data)
        {
            data.Window.Clear();

            data.Window.Draw(_menuBackground);
            data.Window.Draw(_startText);
            data.Window.Draw(_exitText);
            data.Window.Draw(_selectionBox);

            data.Window.Display();
        }

        public void UpdateSelectionBox()
        {
            switch (_selection)
            {
                case MenuSelection.Start:
                    _selectionBox = new RectangleShape(new Vector2f(_startText.GetGlobalBounds().Width + 10, (_startText.GetGlobalBounds().Height) * 2));
                    _selectionBox.Position = new Vector2f(_startText.GetGlobalBounds().Left - 6, _startText.GetGlobalBounds().Top - 6);
                    break;
                case MenuSelection.Exit:
                    _selectionBox = new RectangleShape(new Vector2f(_exitText.GetGlobalBounds().Width + 10, (_exitText.GetGlobalBounds().Height) * 2));
                    _selectionBox.Position = new Vector2f(_exitText.GetGlobalBounds().Left - 6, _exitText.GetGlobalBounds().Top - 6);
                    break;
            }

            _selectionBox.FillColor = new Color(255, 255, 255, 60);
        }

        public void ChangeSelection(bool up)
        {
            int selectionCount = Enum.GetNames(typeof(MenuSelection)).Length;
            int selectionInt = (int)_selection;

            if (up)
            {
                _selection = (MenuSelection)(((selectionInt - 1) + selectionCount) % selectionCount);
            }
            else
            {
                _selection = (MenuSelection)(((selectionInt + 1) + selectionCount) % selectionCount);
            }

            _moveSound.Play();
        }

        public void EnterSelection(GameData data)
        {
            switch (_selection)
            {
                case MenuSelection.Start:
                    data.State.AddState(new GamePlayState(), true);
                    break;
                case MenuSelection.Exit:
                    data.Window.Close();
                    break;
            }

            _selectSound.Play();
        }
    }
}
