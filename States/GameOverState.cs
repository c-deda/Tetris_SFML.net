using SFML.Graphics;
using SFML.System;

namespace Tetris
{
    public class GameOverState : State
    {
        private Text _gameOverText;
        private Text _restartText;

        public override void Init(GameData data)
        {
            _gameOverText = new Text("GAME OVER!", data.Asset.GameFont, 50);
            _gameOverText.Position = new Vector2f((Constants.WindowWidth / 2) - (_gameOverText.GetGlobalBounds().Width / 2), (Constants.WindowWidth / 2) - 50);
            _gameOverText.OutlineThickness = 10;
            _gameOverText.OutlineColor = Color.Black;
            _restartText = new Text("\'esc\' to restart", data.Asset.GameFont);
            _restartText.Position = new Vector2f((Constants.WindowWidth / 2) - ((_restartText).GetGlobalBounds().Width / 2), (Constants.WindowWidth / 2) + 60);
            _restartText.OutlineThickness = 5;
            _restartText.OutlineColor = Color.Black;
        }

        public override void HandleInput(GameData data, SFML.Window.KeyEventArgs e)
        {
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                data.State.AddState(new GamePlayState(), true);
            }
        }

        public override void Update(GameData data) { }

        public override void Draw(GameData data)
        {
            data.Window.Draw(_gameOverText);
            data.Window.Draw(_restartText);
            data.Window.Display();
        }
    }
}
