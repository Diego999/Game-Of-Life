using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    class GameBoard
    {
        private int x;
        private int y;
        private Cell[,] gameBoard;

        public GameBoard(int x, int y)
        {
            if (x < 10 || y < 10)
                throw new Exception("One or more dimensions are smaller than 10");

            this.x = x;
            this.y = y;
            gameBoard = new Cell[x, y];
            for (int i = 0; i <= gameBoard.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard.GetUpperBound(1); ++j)
                    gameBoard[i, j] = new Cell(0, 0);
        }

        public Cell this[int k1, int k2]
        {
            get
            {
                manageKeys(ref k1, ref k2);
                return gameBoard[k1, k2];
            }
            set
            {
                manageKeys(ref k1, ref k2);
                gameBoard[k1, k2] = value;
            }
        }

        public int GetUpperBound(int dim)
        {
            return gameBoard.GetUpperBound(dim);
        }

        private void manageKeys(ref int k1, ref int k2)
        {
            if (k1 < 0)
                k1 += x;
            if (k2 < 0)
                k2 += y;
            if (k1 >= x)
                k1 -= x;
            if (k2 >= y)
                k2 -= y;

            if (k1 >= x || k2 >= y)
                throw new IndexOutOfRangeException();
        }
    }
}
