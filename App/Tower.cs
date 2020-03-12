using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Tetris
{
    public enum ClearState
    {
        CHECK,
        MARK,
        CLEAR,
        MOVE
    }

    public class Tower
    {
        public BlockValue[,] tower { get; }
        public List<int> linesToClear { get; }
        public ClearState clearState { get; set; }

        public Tower()
        {
            tower = new BlockValue[Constants.TOWER_HEIGHT, Constants.TOWER_WIDTH];

            for (int row = 0; row < tower.GetLength(0); row++)
            {
                for (int col = 0; col < tower.GetLength(1); col++)
                {
                    tower[row, col] = BlockValue.EMPTY;
                }
            }

            linesToClear = new List<int>();
            clearState = ClearState.CHECK;
        }

        private void CheckLinesToClear()
        {
            if (!linesToClear.Any())
            {
                for (int row = 0; row < tower.GetLength(0); row++)
                {
                    bool isCleared = true;

                    for (int col = 0; col < tower.GetLength(1); col++)
                    {
                        if (tower[row, col] == BlockValue.EMPTY || tower[row, col] == BlockValue.CLEARED)
                        {
                            isCleared = false;
                            continue;
                        }
                    }

                    if (isCleared)
                    {
                        linesToClear.Add(row);
                        clearState = ClearState.MARK;
                    }
                }

                MarkLinesForClear();
            }
        }

        private void MarkLinesForClear()
        {
            if (linesToClear.Any())
            {
                foreach (int line in linesToClear)
                {
                    for (int col = 0; col < tower.GetLength(1); col++)
                    {
                        if (tower[line, col] != BlockValue.CLEARED)
                        {
                            tower[line, col] = BlockValue.CLEARED;
                        }
                    }
                }

                clearState = ClearState.CLEAR;
            }
        }

        private int ClearLines()
        {
            if (linesToClear.Any())
            {
                Thread.Sleep(100);

                foreach (int line in linesToClear)
                {
                    for (int col = 0; col < tower.GetLength(1); col++)
                    {
                        tower[line, col] = BlockValue.EMPTY;
                    }
                }

                clearState = ClearState.MOVE;
            }

            return linesToClear.Count();
        }

        private void MoveLinesDown()
        {
            if (linesToClear.Any())
            {
                Thread.Sleep(100);

                int linesToMove = 0;

                for (int row = linesToClear.Max(); row > 0; row--)
                {
                    if (linesToClear.Contains(row))
                    {
                        linesToMove++;
                    }
                    else
                    {
                        for (int col = 0; col < tower.GetLength(1); col++)
                        {
                            if (tower[row, col] != BlockValue.EMPTY)
                            {
                                BlockValue tempVal = tower[row, col];
                                tower[row, col] = BlockValue.EMPTY;
                                tower[row + linesToMove, col] = tempVal;
                            }
                        }
                    }
                    
                }

                linesToClear.Clear();
                clearState = ClearState.CHECK;
            }
        }

        public void AddBlock(Block b)
        {
            int row = b.GetRow();
            int col = b.GetCol();

            if (row >= 0 && row < Constants.TOWER_HEIGHT && col >= 0 && col < Constants.TOWER_WIDTH)
            {
                tower[row, col] = b.type;
            }
        }

        public BlockValue GetTile(int row, int col)
        {
            if (row >= 0 && row < Constants.TOWER_HEIGHT && col >= 0 && col < Constants.TOWER_WIDTH)
            {
                return tower[row, col];
            }
            
            return BlockValue.EMPTY;
        }

        public int ClearCycle()
        {
            int linesCleared = 0;

            switch (clearState)
            {
                case ClearState.CHECK:
                    CheckLinesToClear();
                    break;
                case ClearState.MARK:
                    MarkLinesForClear();
                    break;
                case ClearState.CLEAR:
                    linesCleared = ClearLines();
                    break;
                case ClearState.MOVE:
                    MoveLinesDown();
                    break;
            }

            return linesCleared;
        }

        public bool CheckPieceOverlap(Tetrimino piece)
        {
            for (int row = 0; row < Constants.TOWER_HEIGHT; row++)
            {
                for (int col = 0; col < Constants.TOWER_WIDTH; col++)
                {
                    if (piece.IsBlockPosition(col, row) && tower[row, col] != BlockValue.EMPTY)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
