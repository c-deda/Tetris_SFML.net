using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tetris
{
    public class GameData
    {
        public RenderWindow Window { get; }
        public StateManager State { get; }
        public AssetManager Asset { get; }
        public InputManager Input { get; }

        public GameData()
        {
            // Create Main Window
            Window = new RenderWindow(new VideoMode(Constants.WindowWidth, Constants.WindowHeight), Constants.WindowTitle, Styles.Close | Styles.Titlebar);
            Window.SetFramerateLimit(Constants.Framerate);
            Window.SetView(new View(new Vector2f(Window.Size.X / 2, Window.Size.Y / 2), new Vector2f(Window.Size.X, Window.Size.Y)));

            // Create State Manager
            State = new StateManager();

            // Create Asset Manager
            Asset = new AssetManager();

            // Create Input Manager
            Input = new InputManager();
        }
    }

    public class TetrisGame
    {
        private GameData _data;
        private Clock _clock;

        public TetrisGame()
        {
            _data = new GameData();

            // Initial State
            _data.State.AddState(new MenuState(), false);

            // Setup Event Handlers
            _data.Window.Closed += (sender, e) => _data.Input.OnClosed(sender, e);
            _data.Window.KeyPressed += (sender, e) => _data.Input.HandleKeyPressed(sender, e, _data);
        }

        public void Run()
        {
            _clock = new Clock();

            while (_data.Window.IsOpen)
            {
                // Process State Changes
                _data.State.ProcessStateChanges(_data);
                // Handle Events
                _data.Window.DispatchEvents();
                // Update
                _data.State.GetActiveState().Update(_data);
                // Draw
                _data.State.GetActiveState().Draw(_data);
            }
        }
    }
}
