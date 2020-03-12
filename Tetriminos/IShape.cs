namespace Tetris
{
    public class IShape : Tetrimino
    {
        public IShape() : base()
        {
            for(int i = 0; i < 4; i++)
            {
                blocks[i] = new Block(BlockValue.ISHAPE, new Position(Constants.DESPAWNED_POS, Constants.DESPAWNED_POS));
            }

            rotationIndex = 0;
        }

        public override void Spawn()
        {
            blocks[0].Move(new Position(3, 0));
            blocks[1].Move(new Position(4, 0));
            blocks[2].Move(new Position(5, 0));
            blocks[3].Move(new Position(6, 0));
        }

        public override void Rotate(bool clockwise)
        {
            Position pivot = new Position(blocks[1].GetCol(), blocks[1].GetRow());

            FindNewRotationIndex(clockwise);

            switch (rotationIndex)
            {
                case 0:
                    blocks[0].Move(new Position(pivot.x - 1, pivot.y));
                    blocks[2].Move(new Position(pivot.x + 1, pivot.y));
                    blocks[3].Move(new Position(pivot.x + 2, pivot.y));
                    break;
                case 1:
                    blocks[0].Move(new Position(pivot.x, pivot.y - 1));
                    blocks[2].Move(new Position(pivot.x, pivot.y + 1));
                    blocks[3].Move(new Position(pivot.x, pivot.y + 2));
                    break;
                case 2:
                    blocks[0].Move(new Position(pivot.x + 1, pivot.y));
                    blocks[2].Move(new Position(pivot.x - 1, pivot.y));
                    blocks[3].Move(new Position(pivot.x - 2, pivot.y));
                    break;
                case 3:
                    blocks[0].Move(new Position(pivot.x, pivot.y + 1));
                    blocks[2].Move(new Position(pivot.x, pivot.y - 1));
                    blocks[3].Move(new Position(pivot.x, pivot.y - 2));
                    break;
            }
        }
    }
}