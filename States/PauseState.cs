using SFML.Graphics;
using SFML.System;
using System;

namespace Tetris
{
    public enum PauseSelection
    {
        CONTINUE,
        RESTART,
        QUIT
    };

    public class PauseState : State
    {
        public Text pausedText { get; set; }
        public Text continueText { get; set; }
        public Text restartText { get; set; }
        public Text quitText { get; set; }

        public PauseSelection pauseSelection;
        public RectangleShape selectionBox;

        public override void Init(ref GameData data)
        {
            // Initialize Selection
            pauseSelection = PauseSelection.CONTINUE;

            // Initialize Text
            pausedText = new Text("PAUSED", data.asset.gameFont, 50);
            pausedText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - (pausedText.GetGlobalBounds().Width / 2), Constants.WIN_HEIGHT / 10);
            continueText = new Text("Continue", data.asset.gameFont);
            continueText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - (continueText.GetGlobalBounds().Width / 2), (Constants.WIN_HEIGHT / 2) - 50);
            restartText = new Text("Restart", data.asset.gameFont);
            restartText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - (restartText.GetGlobalBounds().Width / 2), (Constants.WIN_HEIGHT / 2) + 50);
            quitText = new Text("Quit", data.asset.gameFont);
            quitText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - (quitText.GetGlobalBounds().Width / 2), (Constants.WIN_HEIGHT / 2) + 150);
        }

        public override void HandleInput(ref GameData data, SFML.Window.KeyEventArgs e)
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
                    EnterSelection(ref data);
                    break;
                case SFML.Window.Keyboard.Key.Escape:
                    pauseSelection = PauseSelection.CONTINUE;
                    EnterSelection(ref data);
                    break;
            }
        }

        public override void Update(ref GameData data)
        {
            UpdateSelectionBox();
        }

        public override void Draw(ref GameData data)
        {
            data.window.Clear();

            data.window.Draw(pausedText);
            data.window.Draw(continueText);
            data.window.Draw(restartText);
            data.window.Draw(quitText);
            data.window.Draw(selectionBox);

            data.window.Display();
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
        }

        public void UpdateSelectionBox()
        {
            switch (pauseSelection)
            {
                case PauseSelection.CONTINUE:
                    selectionBox = new RectangleShape(new Vector2f(continueText.GetGlobalBounds().Width + 10, (continueText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(continueText.GetGlobalBounds().Left - 6, continueText.GetGlobalBounds().Top - 6);
                    break;
                case PauseSelection.RESTART:
                    selectionBox = new RectangleShape(new Vector2f(restartText.GetGlobalBounds().Width + 10, (restartText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(restartText.GetGlobalBounds().Left - 6, restartText.GetGlobalBounds().Top - 6);
                    break;
                case PauseSelection.QUIT:
                    selectionBox = new RectangleShape(new Vector2f(quitText.GetGlobalBounds().Width + 10, (quitText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(quitText.GetGlobalBounds().Left - 6, quitText.GetGlobalBounds().Top - 6);
                    break;
            }

            selectionBox.FillColor = new Color(255, 255, 255, 60);
        }

        public void EnterSelection(ref GameData data)
        {
            switch (pauseSelection)
            {
                case PauseSelection.CONTINUE:
                    data.state.removeState(new GamePlayState(), false);
                    break;
                case PauseSelection.RESTART:
                    data.state.removeState(new GamePlayState(), true);
                    break;
                case PauseSelection.QUIT:
                    data.state.removeState(new MenuState(), false);
                    break;
            }
        }
    }
}
