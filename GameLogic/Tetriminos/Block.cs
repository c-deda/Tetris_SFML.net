namespace Tetris
{
    public enum BlockValue
    {
        Empty,
        IShape,
        LShape,
        JShape,
        SShape,
        ZShape,
        TShape,
        OShape,
        Cleared
    }

    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Block
    {
        public BlockValue Type { get; }
        private Position _position;

        public Block(BlockValue type, Position position)
        {
            Type = type;
            _position = position;
        }

        public void Move(Position newPos)
        {
            _position = newPos;
        }

        public int GetCol()
        {
            return _position.X;
        }

        public int GetRow()
        {
            return _position.Y;
        }
    }
}
