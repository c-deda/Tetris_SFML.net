using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tetris
{
    public class GameData
    {
        public RenderWindow window { get; }
        public StateManager state { get; }
        public AssetManager asset { get; }
        public InputManager input { get; }

        public GameData()
        {
            // Create Main Window
            window = new RenderWindow(new VideoMode(Constants.WIN_WIDTH, Constants.WIN_HEIGHT), Constants.WIN_TITLE, Styles.Close | Styles.Titlebar);
            window.SetFramerateLimit(Constants.FRAMERATE);
            window.SetView(new View(new SFML.System.Vector2f(window.Size.X / 2, window.Size.Y / 2), new SFML.System.Vector2f(window.Size.X, window.Size.Y)));

            // Create State Manager
            state = new StateManager();

            // Create Asset Manager
            asset = new AssetManager();

            // Create Input Manager
            input = new InputManager();
        }
    }

    public class TetrisGame
    {
        private GameData data;
        Clock clock;

        public TetrisGame()
        {
            data = new GameData();

            // Initial State
            data.state.addState(new MenuState(), false);

            // Setup Event Handlers
            data.window.Closed += (sender, e) => data.input.OnClosed(sender, e);
            data.window.KeyPressed += (sender, e) => data.input.HandleKeyPressed(sender, e, ref data);
        }

        public void Run()
        {
            clock = new Clock();

            while (data.window.IsOpen)
            {
                // Process State Changes
                data.state.ProcessStateChanges(ref data);
                // Handle Events
                data.window.DispatchEvents();
                // Update
                data.state.GetActiveState().Update(ref data);
                // Draw
                data.state.GetActiveState().Draw(ref data);
            }
        }
    }
}
