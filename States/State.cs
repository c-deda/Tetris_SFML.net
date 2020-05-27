namespace Tetris
{
    public abstract class State
    {
        public abstract void Init(GameData data);

        public abstract void HandleInput(GameData data, SFML.Window.KeyEventArgs e);

        public abstract void Update(GameData data);

        public abstract void Draw(GameData data);
    }
}
