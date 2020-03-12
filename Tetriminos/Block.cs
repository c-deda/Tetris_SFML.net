namespace Tetris
{
    public enum BlockValue
    {
        EMPTY,
        ISHAPE,
        LSHAPE,
        JSHAPE,
        SSHAPE,
        ZSHAPE,
        TSHAPE,
        OSHAPE,
        CLEARED
    }

    public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Block
    {
        public BlockValue type { get; }
        private Position position;

        public Block(BlockValue type, Position position)
        {
            this.type = type;
            this.position = position;
        }

        public void Move(Position newPos)
        {
            position = newPos;
        }

        public int GetCol()
        {
            return position.x;
        }

        public int GetRow()
        {
            return position.y;
        }
    }
}
