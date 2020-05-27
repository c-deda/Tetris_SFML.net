namespace Tetris
{
    public class IShape : Tetrimino
    {
        public IShape() : base()
        {
            for (int i = 0; i < 4; i++)
            {
                _blocks[i] = new Block(BlockValue.IShape, new Position(Constants.DespawnedPosition, Constants.DespawnedPosition));
            }
        }

        public override void Spawn()
        {
            _blocks[0].Move(new Position(3, 0));
            _blocks[1].Move(new Position(4, 0));
            _blocks[2].Move(new Position(5, 0));
            _blocks[3].Move(new Position(6, 0));
        }

        public override void Rotate(bool clockwise)
        {
            Position pivot = new Position(_blocks[1].GetCol(), _blocks[1].GetRow());

            FindNewRotationIndex(clockwise);

            switch (_rotationIndex)
            {
                case 0:
                    _blocks[0].Move(new Position(pivot.X-1, pivot.Y));
                    _blocks[2].Move(new Position(pivot.X+1, pivot.Y));
                    _blocks[3].Move(new Position(pivot.X+2, pivot.Y));
                    break;
                case 1:
                    _blocks[0].Move(new Position(pivot.X, pivot.Y-1));
                    _blocks[2].Move(new Position(pivot.X, pivot.Y+1));
                    _blocks[3].Move(new Position(pivot.X, pivot.Y+2));
                    break;
                case 2:
                    _blocks[0].Move(new Position(pivot.X+1, pivot.Y));
                    _blocks[2].Move(new Position(pivot.X-1, pivot.Y));
                    _blocks[3].Move(new Position(pivot.X-2, pivot.Y));
                    break;
                case 3:
                    _blocks[0].Move(new Position(pivot.X, pivot.Y+1));
                    _blocks[2].Move(new Position(pivot.X, pivot.Y-1));
                    _blocks[3].Move(new Position(pivot.X, pivot.Y-2));
                    break;
            }
        }
    }
}