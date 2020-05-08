using SFML.Graphics;
using SFML.System;

namespace Tetris
{
    public class GameOverState : State
    {
        public Text gameOverText { get; set; }
        public Text restartText { get; set; }

        public override void Init(ref GameData data)
        {
            gameOverText = new Text("GAME OVER!", data.asset.gameFont, 50);
            gameOverText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - (gameOverText.GetGlobalBounds().Width / 2), (Constants.WIN_WIDTH / 2) - 50);
            gameOverText.OutlineThickness = 10;
            gameOverText.OutlineColor = Color.Black;
            restartText = new Text("\'esc\' to restart", data.asset.gameFont);
            restartText.Position = new Vector2f((Constants.WIN_WIDTH / 2) - ((restartText).GetGlobalBounds().Width / 2), (Constants.WIN_WIDTH / 2) + 60);
            restartText.OutlineThickness = 5;
            restartText.OutlineColor = Color.Black;
        }

        public override void HandleInput(ref GameData data, SFML.Window.KeyEventArgs e)
        {
            switch (e.Code)
            {
                case SFML.Window.Keyboard.Key.Escape:
                    data.state.AddState(new GamePlayState(), true);
                    break;
            }
        }

        public override void Update(ref GameData data)
        {
            
        }

        public override void Draw(ref GameData data)
        {
            data.window.Draw(gameOverText);
            data.window.Draw(restartText);
            data.window.Display();
        }
    }
}
