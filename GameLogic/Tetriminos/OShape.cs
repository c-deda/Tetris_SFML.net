namespace Tetris
{
    public class OShape : Tetrimino
    {
        public OShape() : base()
        {
            for (int i = 0; i < 4; i++)
            {
                _blocks[i] = new Block(BlockValue.OShape, new Position(Constants.DespawnedPosition, Constants.DespawnedPosition));
            }
        }

        public override void Spawn()
        {
            _blocks[0].Move(new Position(4, 0));
            _blocks[1].Move(new Position(5, 0));
            _blocks[2].Move(new Position(4, 1));
            _blocks[3].Move(new Position(5, 1));
        }

        public override void Rotate(bool clockwise) { }
    }
}
