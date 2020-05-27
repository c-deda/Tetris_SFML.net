using SFML.Graphics;
using System;

namespace Tetris
{
    public class InputManager
    {
        public void OnClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public void HandleKeyPressed(object sender, SFML.Window.KeyEventArgs e, GameData data)
        {
            data.State.GetActiveState().HandleInput(data, e);
        }
    }
}
