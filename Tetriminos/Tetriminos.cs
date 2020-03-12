namespace Tetris
{
    public abstract class Tetrimino
    {
        protected Block[] blocks;
        protected int rotationIndex;

        public Tetrimino()
        {
            blocks = new Block[4];
        }

        // - Up and + Down
        public void MoveUpDown(int val)
        {
            foreach (Block b in blocks)
            {
                b.Move(new Position(b.GetCol(), b.GetRow() + val));
            }
        }

        // - Left and + Right
        public void MoveLeftRight(int val)
        {
            foreach (Block b in blocks)
            {
                b.Move(new Position(b.GetCol() + val, b.GetRow()));
            }
        }

        protected void FindNewRotationIndex(bool clockwise)
        {
            if (clockwise)
            {
                rotationIndex++;
            }
            else
            {
                rotationIndex--;
            }

            rotationIndex = (rotationIndex + Constants.ROTATION_VALUES) % Constants.ROTATION_VALUES;
        }

        public void Despawn()
        {
            foreach (Block b in blocks)
            {
                b.Move(new Position(Constants.DESPAWNED_POS, Constants.DESPAWNED_POS));
            }
        }

        public bool IsBlockPosition(int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                if (blocks[i].GetCol() == x && blocks[i].GetRow() == y)
                {
                    return true;
                }
            }

            return false;
        }

        public Block GetBlockAt(int i)
        {
            return blocks[i];
        }

        // Bounds Of Tower Adapts To Blocks Added To Tower
        public bool IsInBounds(Tower tower)
        {
            foreach (Block b in blocks)
            {
                if (b.GetCol() < 0 || b.GetCol() >= Constants.TOWER_WIDTH || b.GetRow() < 0 || b.GetRow() >= Constants.TOWER_HEIGHT)
                {
                    return false;
                }
                else if (tower.GetTile(b.GetRow(), b.GetCol()) != BlockValue.EMPTY)
                {
                    return false;
                }
            }
            
            return true;
        }

        public BlockValue GetBlockValue()
        {
            return blocks[0].type;
        }

        public void MakeCopy(Tetrimino copy)
        {
            copy.rotationIndex = this.rotationIndex;
            for (int i = 0; i < 4; i++)
            {
                copy.blocks[i].Move(new Position(this.blocks[i].GetCol(), this.blocks[i].GetRow()));
            }
        }

        public void CopyValuesToTower(Tower tower)
        {
            foreach (Block b in blocks)
            {
                tower.AddBlock(b);
            }
        }

        public abstract void Spawn();

        public abstract void Rotate(bool clockwise);
    }
}