using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    class GameBoard
    {
        public enum State {Alive, Emerging, Dying, Empty, Dead};
        public static readonly Dictionary<State, string> STATE_MATCH;

        private int x;
        private int y;
        private State[,] gameBoard;

        static GameBoard()
        {
            STATE_MATCH = new Dictionary<State, string>();
            STATE_MATCH.Add(State.Alive, "X");
            STATE_MATCH.Add(State.Emerging, "x");
            STATE_MATCH.Add(State.Empty, " ");
            STATE_MATCH.Add(State.Dying, "o");
            STATE_MATCH.Add(State.Dead, "O");
        }

        public GameBoard(int x, int y)
        {
            if (x < 10 || y < 10)
                throw new Exception("One or more dimensions are smaller than 10");

            this.x = x;
            this.y = y;
            gameBoard = new State[x, y];
            for (int i = 0; i <= gameBoard.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard.GetUpperBound(1); ++j)
                    gameBoard[i, j] = State.Empty;
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

            if (k1 >= x || k2 >= y)
                throw new IndexOutOfRangeException();
        }
    }
}
