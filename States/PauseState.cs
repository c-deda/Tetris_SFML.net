using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System;

namespace Tetris
{
    public enum PauseSelection
    {
        Continue,
        Restart,
        Quit
    };

    public class PauseState : State
    {
        private Text _pausedText;
        private Text _continueText;
        private Text _restartText;
        private Text _quitText;
        private Sound _moveSound;
        private Sound _selectSound;

        public PauseSelection pauseSelection;
        public RectangleShape selectionBox;

        public override void Init(GameData data)
        {
            // Initialize Selection
            pauseSelection = PauseSelection.Continue;

            // Initialize Text
            _pausedText = new Text("PAUSED", data.Asset.GameFont, 50);
            _pausedText.Position = new Vector2f((Constants.WindowWidth / 2) - (_pausedText.GetGlobalBounds().Width / 2), Constants.WindowHeight / 10);
            _continueText = new Text("Continue", data.Asset.GameFont);
            _continueText.Position = new Vector2f((Constants.WindowWidth / 2) - (_continueText.GetGlobalBounds().Width / 2), (Constants.WindowHeight / 2) - 50);
            _restartText = new Text("Restart", data.Asset.GameFont);
            _restartText.Position = new Vector2f((Constants.WindowWidth / 2) - (_restartText.GetGlobalBounds().Width / 2), (Constants.WindowHeight / 2) + 50);
            _quitText = new Text("Quit", data.Asset.GameFont);
            _quitText.Position = new Vector2f((Constants.WindowWidth / 2) - (_quitText.GetGlobalBounds().Width / 2), (Constants.WindowHeight / 2) + 150);
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
                case SFML.Window.Keyboard.Key.Escape:
                    pauseSelection = PauseSelection.Continue;
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

            data.Window.Draw(_pausedText);
            data.Window.Draw(_continueText);
            data.Window.Draw(_restartText);
            data.Window.Draw(_quitText);
            data.Window.Draw(selectionBox);

            data.Window.Display();
        }

        public void ChangeSelection(bool up)
        {
            int selectionCount = Enum.GetNames(typeof(PauseSelection)).Length;
            int selectionInt = (int)pauseSelection;

            if (up)
            {
                pauseSelection = (PauseSelection)(((selectionInt - 1) + selectionCount) % selectionCount);
            }
            else
            {
                pauseSelection = (PauseSelection)(((selectionInt + 1) + selectionCount) % selectionCount);
            }

            _moveSound.Play();
        }

        public void UpdateSelectionBox()
        {
            switch (pauseSelection)
            {
                case PauseSelection.Continue:
                    selectionBox = new RectangleShape(new Vector2f(_continueText.GetGlobalBounds().Width + 10, (_continueText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(_continueText.GetGlobalBounds().Left - 6, _continueText.GetGlobalBounds().Top - 6);
                    break;
                case PauseSelection.Restart:
                    selectionBox = new RectangleShape(new Vector2f(_restartText.GetGlobalBounds().Width + 10, (_restartText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(_restartText.GetGlobalBounds().Left - 6, _restartText.GetGlobalBounds().Top - 6);
                    break;
                case PauseSelection.Quit:
                    selectionBox = new RectangleShape(new Vector2f(_quitText.GetGlobalBounds().Width + 10, (_quitText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(_quitText.GetGlobalBounds().Left - 6, _quitText.GetGlobalBounds().Top - 6);
                    break;
            }

            selectionBox.FillColor = new Color(255, 255, 255, 60);
        }

        public void EnterSelection(GameData data)
        {
            switch (pauseSelection)
            {
                case PauseSelection.Continue:
                    data.State.RemoveState();
                    break;
                case PauseSelection.Restart:
                    data.State.AddState(new GamePlayState(), true);
                    break;
                case PauseSelection.Quit:
                    data.State.RemoveState();
                    data.State.AddState(new MenuState(), true);
                    break;
            }

            _selectSound.Play();
        }
    }
}
