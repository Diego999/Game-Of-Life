using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Game_Of_Life
{
    /// <summary>
    /// Class which represents a GameBoard which has no border. It has also the different state of a cell.
    /// </summary>
    class GameBoard
    {
        private static readonly Dictionary<State, Brush> STATE_MATCH; // To match a State to a Brush
        public enum State { Alive, Emerging, Dying, Empty, Dead };

        public static readonly int MIN_ROWS = 10;
        public static readonly int MIN_COLS = 10;

        private int x; // Rows
        private int y; // Cols
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Rows</param>
        /// <param name="y">Cols</param>
        public GameBoard(int x, int y)
        {
            if (x < MIN_ROWS || y < MIN_COLS)
                throw new Exception("One or more dimensions are smaller than 10");

            this.x = x;
            this.y = y;
            gameBoard = new State[x, y];
            Init();
        }

        /// <summary>
        /// Define if a cell is "dead", means having a state amoug Empty, Dead & Dying
        /// </summary>
        /// <param name="state">Cell's state</param>
        /// <returns>bool</returns>
        public static bool IsConsideredLikeDead(State state)
        {
            return state == State.Empty || state == State.Dead || state == State.Dying;
        }

        /// <summary>
        /// Define if a cell is "alive", means having a state amoug Alive & Emerging
        /// </summary>
        /// <param name="state">Cell's state</param>
        /// <returns>bool</returns>
        public static bool IsConsideredLikeAlive(State state)
        {
            return state == State.Alive || state == State.Emerging;
        }

        /// <summary>
        /// Initialize the game board to empty state
        /// </summary>
        public void Init()
        {
            for (int i = 0; i <= gameBoard.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard.GetUpperBound(1); ++j)
                    gameBoard[i, j] = State.Empty;
        }

        /// <summary>
        /// Return the brush corresponding to the cell's state
        /// </summary>
        /// <param name="state">Cell's state</param>
        /// <returns>Brush</returns>
        public static Brush GetRender(State state)
        {
            return STATE_MATCH[state];
        }

        /// <summary>
        /// Convert the keys to avoid 
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        private void manageKeys(ref int k1, ref int k2)
        {
            if (k1 < 0)
                k1 = k1 % x + x;
            if (k2 < 0)
                k2 = k2 % y + y;

            if (k1 >= x)
                k1 %= x;
            if (k2 >= y)
                k2 %= y;
        }

        #region GameBoard Get/Set
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

        /// <summary>
        /// The GetUpperBound than a simple Array
        /// </summary>
        /// <param name="dim">The dimension desired</param>
        /// <returns>The length</returns>
        public int GetUpperBound(int dim)
        {
            return gameBoard.GetUpperBound(dim);
        }

#endregion
    }
}
