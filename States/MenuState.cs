using SFML.Graphics;
using SFML.System;
using System;

namespace Tetris
{
    public enum MenuSelection
    {
        START,
        EXIT
    };

    public class MenuState : State
    {
        public MenuSelection selection { get; set; }
        public Sprite menuBackground { get; set; }
        public RectangleShape selectionBox { get; set; }
        public Text startText { get; set; }
        public Text exitText { get; set; }

        public override void Init(ref GameData data)
        {
            // Initialize Menu Selection
            selection = MenuSelection.START;

            // Setup Assets
            menuBackground = new Sprite(data.asset.backgroundTexture, new IntRect(0, 0, Constants.WIN_WIDTH, Constants.WIN_HEIGHT));
            startText = new Text("Start", data.asset.gameFont);
            startText.Position = new Vector2f((data.window.Size.X / 2) - (startText.GetGlobalBounds().Width / 2), (data.window.Size.Y / 2));
            exitText = new Text("Exit", data.asset.gameFont);
            exitText.Position = new Vector2f((data.window.Size.X / 2) - (exitText.GetGlobalBounds().Width / 2), (data.window.Size.Y / 2) + 90);
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
            }
        }

        public override void Update(ref GameData data)
        {
            UpdateSelectionBox();
        }

        public override void Draw(ref GameData data)
        {
            data.window.Clear();

            data.window.Draw(menuBackground);
            data.window.Draw(startText);
            data.window.Draw(exitText);
            data.window.Draw(selectionBox);

            data.window.Display();
        }

        public void UpdateSelectionBox()
        {
            switch (selection)
            {
                case MenuSelection.START:
                    selectionBox = new RectangleShape(new Vector2f(startText.GetGlobalBounds().Width + 10, (startText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(startText.GetGlobalBounds().Left - 6, startText.GetGlobalBounds().Top - 6);
                    break;
                case MenuSelection.EXIT:
                    selectionBox = new RectangleShape(new Vector2f(exitText.GetGlobalBounds().Width + 10, (exitText.GetGlobalBounds().Height) * 2));
                    selectionBox.Position = new Vector2f(exitText.GetGlobalBounds().Left - 6, exitText.GetGlobalBounds().Top - 6);
                    break;
            }

            selectionBox.FillColor = new Color(255, 255, 255, 60);
        }

        public void ChangeSelection(bool up)
        {
            int selectionCount = Enum.GetNames(typeof(MenuSelection)).Length;
            int selectionInt = (int)selection;

            if (up)
            {
                selection = (MenuSelection)(((selectionInt - 1) + selectionCount) % selectionCount);
            }
            else
            {
                selection = (MenuSelection)(((selectionInt + 1) + selectionCount) % selectionCount);
            }
        }

        public void EnterSelection(ref GameData data)
        {
            switch (selection)
            {
                case MenuSelection.START:
                    data.state.addState(new GamePlayState(), false);
                    break;
                case MenuSelection.EXIT:
                    data.window.Close();
                    break;
            }
        }
    }
}
