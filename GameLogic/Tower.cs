using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Tetris
{
    public enum ClearState
    {
        Check,
        Mark,
        Clear,
        Move
    }

    public class Tower
    {
        public BlockValue[,] TowerData { get; }
        public List<int> LinesToClear { get; }
        public ClearState ClearState { get; set; }

        public Tower()
        {
            TowerData = new BlockValue[Constants.TowerHeight, Constants.TowerWidth];

            for (int row = 0; row < TowerData.GetLength(0); row++)
            {
                for (int col = 0; col < TowerData.GetLength(1); col++)
                {
                    TowerData[row, col] = BlockValue.Empty;
                }
            }

            LinesToClear = new List<int>();
            ClearState = ClearState.Check;
        }

        private void CheckLinesToClear()
        {
            if (!LinesToClear.Any())
            {
                for (int row = 0; row < TowerData.GetLength(0); row++)
                {
                    bool isCleared = true;

                    for (int col = 0; col < TowerData.GetLength(1); col++)
                    {
                        if (TowerData[row, col] == BlockValue.Empty || TowerData[row, col] == BlockValue.Cleared)
                        {
                            isCleared = false;
                            continue;
                        }
                    }

                    if (isCleared)
                    {
                        LinesToClear.Add(row);
                        ClearState = ClearState.Mark;
                    }
                }

                MarkLinesForClear();
            }
        }

        private void MarkLinesForClear()
        {
            if (LinesToClear.Any())
            {
                foreach (int line in LinesToClear)
                {
                    for (int col = 0; col < TowerData.GetLength(1); col++)
                    {
                        if (TowerData[line, col] != BlockValue.Cleared)
                        {
                            TowerData[line, col] = BlockValue.Cleared;
                        }
                    }
                }

                ClearState = ClearState.Clear;
            }
        }

        private int ClearLines()
        {
            if (LinesToClear.Any())
            {
                Thread.Sleep(100);

                foreach (int line in LinesToClear)
                {
                    for (int col = 0; col < TowerData.GetLength(1); col++)
                    {
                        TowerData[line, col] = BlockValue.Empty;
                    }
                }

                ClearState = ClearState.Move;
            }

            return LinesToClear.Count();
        }

        private void MoveLinesDown()
        {
            if (LinesToClear.Any())
            {
                Thread.Sleep(100);

                int linesToMove = 0;

                for (int row = LinesToClear.Max(); row > 0; row--)
                {
                    if (LinesToClear.Contains(row))
                    {
                        linesToMove++;
                    }
                    else
                    {
                        for (int col = 0; col < TowerData.GetLength(1); col++)
                        {
                            if (TowerData[row, col] != BlockValue.Empty)
                            {
                                BlockValue tempVal = TowerData[row, col];
                                TowerData[row, col] = BlockValue.Empty;
                                TowerData[row + linesToMove, col] = tempVal;
                            }
                        }
                    }
                    
                }

                LinesToClear.Clear();
                ClearState = ClearState.Check;
            }
        }

        public void AddBlock(Block b)
        {
            int row = b.GetRow();
            int col = b.GetCol();

            if (row >= 0 && row < Constants.TowerHeight && col >= 0 && col < Constants.TowerWidth)
            {
                TowerData[row, col] = b.Type;
            }
        }

        public BlockValue GetTile(int row, int col)
        {
            if (row >= 0 && row < Constants.TowerHeight && col >= 0 && col < Constants.TowerWidth)
            {
                return TowerData[row, col];
            }
            
            return BlockValue.Empty;
        }

        public int ClearCycle()
        {
            int linesCleared = 0;

            switch (ClearState)
            {
                case ClearState.Check:
                    CheckLinesToClear();
                    break;
                case ClearState.Mark:
                    MarkLinesForClear();
                    break;
                case ClearState.Clear:
                    linesCleared = ClearLines();
                    break;
                case ClearState.Move:
                    MoveLinesDown();
                    break;
            }

            return linesCleared;
        }

        public bool CheckPieceOverlap(Tetrimino piece)
        {
            for (int row = 0; row < Constants.TowerHeight; row++)
            {
                for (int col = 0; col < Constants.TowerWidth; col++)
                {
                    if (piece.IsBlockPosition(col, row) && TowerData[row, col] != BlockValue.Empty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
