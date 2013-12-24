using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    public class PatternRepresentation
    {
        private int rows;
        private int cols;
        private String[,] pattern;

        public static readonly string ALIVE = "x";
        public static readonly string EMPTY = "-";

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

        private bool manageKeys(int k1, int k2)
        {
            return k1 >= 0 && k1 < rows && k2 >= 0 && k2 < cols;
        }

        public int Row { get { return rows; } }

        public int Col { get { return cols; } }

        public String[,] Pattern { get {return pattern;}}
    }
}
