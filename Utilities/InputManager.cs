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

        public void HandleKeyPressed(object sender, SFML.Window.KeyEventArgs e, ref GameData data)
        {
            data.state.GetActiveState().HandleInput(ref data, e);
        }
    }
}
