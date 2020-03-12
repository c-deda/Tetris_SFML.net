namespace Tetris
{
    public abstract class State
    {
        public abstract void Init(ref GameData data);

        public abstract void HandleInput(ref GameData data, SFML.Window.KeyEventArgs e);

        public abstract void Update(ref GameData data);

        public abstract void Draw(ref GameData data);
    }
}
