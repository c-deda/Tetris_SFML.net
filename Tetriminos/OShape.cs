namespace Tetris
{
    public class OShape : Tetrimino
    {
        public OShape() : base()
        {
            for (int i = 0; i < 4; i++)
            {
                blocks[i] = new Block(BlockValue.OSHAPE, new Position(Constants.DESPAWNED_POS, Constants.DESPAWNED_POS));
            }

            rotationIndex = 0;
        }

        public override void Spawn()
        {
            blocks[0].Move(new Position(4, 0));
            blocks[1].Move(new Position(5, 0));
            blocks[2].Move(new Position(4, 1));
            blocks[3].Move(new Position(5, 1));
        }

        public override void Rotate(bool clockwise)
        {

        }
    }
}
