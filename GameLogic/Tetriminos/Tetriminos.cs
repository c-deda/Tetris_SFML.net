namespace Tetris
{
    public abstract class Tetrimino
    {
        protected Block[] _blocks;
        protected int _rotationIndex;

        public Tetrimino()
        {
            _blocks = new Block[4];
        }

        // - Up and + Down
        public void MoveUpDown(int val)
        {
            foreach (Block b in _blocks)
            {
                b.Move(new Position(b.GetCol(), b.GetRow() + val));
            }
        }

        // - Left and + Right
        public void MoveLeftRight(int val)
        {
            foreach (Block b in _blocks)
            {
                b.Move(new Position(b.GetCol() + val, b.GetRow()));
            }
        }

        protected void FindNewRotationIndex(bool clockwise)
        {
            if (clockwise)
            {
                _rotationIndex++;
            }
            else
            {
                _rotationIndex--;
            }

            _rotationIndex = (_rotationIndex + Constants.TotalRotationValues) % Constants.TotalRotationValues;
        }

        public void Despawn()
        {
            foreach (Block b in _blocks)
            {
                b.Move(new Position(Constants.DespawnedPosition, Constants.DespawnedPosition));
            }
        }

        public bool IsBlockPosition(int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_blocks[i].GetCol() == x && _blocks[i].GetRow() == y)
                {
                    return true;
                }
            }

            return false;
        }

        public Block GetBlockAt(int i)
        {
            return _blocks[i];
        }

        // Bounds Of Tower Adapts To Blocks Added To Tower
        public bool IsInBounds(Tower tower)
        {
            foreach (Block b in _blocks)
            {
                if (b.GetCol() < 0 || b.GetCol() >= Constants.TowerWidth || b.GetRow() < 0 || b.GetRow() >= Constants.TowerHeight)
                {
                    return false;
                }
                else if (tower.GetTile(b.GetRow(), b.GetCol()) != BlockValue.Empty)
                {
                    return false;
                }
            }
            
            return true;
        }

        public BlockValue GetBlockValue()
        {
            return _blocks[0].Type;
        }

        public void MakeCopy(Tetrimino copy)
        {
            copy._rotationIndex = this._rotationIndex;

            for (int i = 0; i < 4; i++)
            {
                copy._blocks[i].Move(new Position(this._blocks[i].GetCol(), this._blocks[i].GetRow()));
            }
        }

        public void CopyValuesToTowerData(Tower tower)
        {
            foreach (Block b in _blocks)
            {
                tower.AddBlock(b);
            }
        }

        public abstract void Spawn();

        public abstract void Rotate(bool clockwise);
    }
}