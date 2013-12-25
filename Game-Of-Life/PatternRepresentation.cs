using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    /// <summary>
    /// Represents a pattern
    /// </summary>
    public class PatternRepresentation
    {
        private int rows;
        private int cols;
        private String[,] pattern;

        public static readonly string ALIVE = "x";
        public static readonly string EMPTY = "-";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public PatternRepresentation(int rows, int cols)
        {
            if (rows < 1 || cols < 1)
                throw new Exception("One or more dimensions are smaller than 1");

            this.rows = rows;
            this.cols = cols;
            pattern = new String[rows, cols];
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    pattern[i, j] = EMPTY;
        }

        /// <summary>
        /// Check if the keys are correct
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <returns>bool</returns>
        private bool manageKeys(int k1, int k2)
        {
            return k1 >= 0 && k1 < rows && k2 >= 0 && k2 < cols;
        }

        #region PatternRepresentation Get / Set

        public string this[int k1, int k2]
        {
            get
            {
                manageKeys(k1, k2);
                return pattern[k1, k2];
            }
            set
            {
                manageKeys(k1, k2);
                pattern[k1, k2] = value;
            }
        }

        public int Row { get { return rows; } }

        public int Col { get { return cols; } }

        public String[,] Pattern { get {return pattern;} }

        #endregion
    }
}
