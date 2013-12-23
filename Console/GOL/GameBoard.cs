using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    class GameBoard
    {
        public enum State {Alive, Emerging, Dying, Dead};
        private static readonly Dictionary<State, string> STATE_MATCH;

        private int x;
        private int y;
        private State[,] gameBoard;

        static GameBoard()
        {
            STATE_MATCH = new Dictionary<State, string>();
            STATE_MATCH.Add(State.Alive, "X");
            STATE_MATCH.Add(State.Dead, " ");
        }

        public GameBoard(int x, int y)
        {
            this.x = x;
            this.y = y;
            gameBoard = new State[x, y];
            for (int i = 0; i <= gameBoard.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard.GetUpperBound(1); ++j)
                    gameBoard[i, j] = State.Dead;
            gameBoard[12, 15] = gameBoard[12, 14] = gameBoard[12, 16] = gameBoard[11, 16] = gameBoard[10, 15] = State.Alive;
        }

        public State this[int k1, int k2]
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
        }

        public void draw()
        {
            for (int j = 0; j <= gameBoard.GetUpperBound(1)+2; ++j)
                Console.Write("-");
            Console.WriteLine();
            for (int i = 0; i <= gameBoard.GetUpperBound(0); ++i)
            {
                Console.Write("|");
                for (int j = 0; j <= gameBoard.GetUpperBound(1); ++j)
                {
                    Console.Write(STATE_MATCH[gameBoard[i, j]]);
                }
                Console.Write("|");
                Console.WriteLine();
            }
            for (int j = 0; j <= gameBoard.GetUpperBound(1)+2; ++j)
                Console.Write("-");
            Console.WriteLine();
        }
    }
}
