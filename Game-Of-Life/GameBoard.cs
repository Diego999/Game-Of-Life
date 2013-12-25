using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_Of_Life
{
    class GameBoard
    {
        private static readonly Dictionary<State, Brush> STATE_MATCH;
        public enum State { Alive, Emerging, Dying, Empty, Dead };

        private int x;
        private int y;
        private State[,] gameBoard;

        static GameBoard()
        {
            STATE_MATCH = new Dictionary<State, Brush>();
            STATE_MATCH.Add(State.Alive, DisplayEngine.BrushFromString("#7700AA00"));
            STATE_MATCH.Add(State.Emerging, DisplayEngine.BrushFromString("#7700CCCC"));
            STATE_MATCH.Add(State.Empty, DisplayEngine.BrushFromString("#FF111111"));
            STATE_MATCH.Add(State.Dying, DisplayEngine.BrushFromString("#77FF0000"));
            STATE_MATCH.Add(State.Dead, DisplayEngine.BrushFromString("#55FF0000"));
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

        public static bool IsConsideredLikeDead(State state)
        {
            return state == State.Empty || state == State.Dead || state == State.Dying;
        }

        public static bool IsConsideredLikeAlive(State state)
        {
            return state == State.Alive || state == State.Emerging;
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

        public static Brush GetRender(State state)
        {
            return STATE_MATCH[state];
        }
    }
}
