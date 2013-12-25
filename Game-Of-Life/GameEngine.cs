using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace Game_Of_Life
{
    class GameEngine
    {
        public const int NB_ROWS_GRID = 80;
        public const int NB_COLS_GRID = (int)(NB_ROWS_GRID * 1.33);

        private GameBoard gameBoard1;
        private GameBoard gameBoard2;
        private DisplayEngine displayEngine;

        private int stepGeneration;

        private int currentPopDying;
        private int currentPopDead;
        private int currentPopAlive;
        private int currentPopEmerging;

        private int totPopDying;
        private int totPopDead;
        private int totPopEmerged;

        bool isInPause;

        public GameEngine(Canvas gridGameBoard, Canvas gridPattern, Canvas gridGameBoardCell, Label lblValue1, Label lblValue2)
        {
            displayEngine = new DisplayEngine(gridGameBoard, gridPattern, gridGameBoardCell, lblValue1, lblValue2);

            stepGeneration = 0;
            isInPause = true;
            currentPopDying = 0;
            currentPopDead = 0;
            currentPopAlive = 0;
            currentPopEmerging = 0;

            totPopDying = 0;
            totPopDead = 0;
            totPopEmerged = 0;

            Init(NB_ROWS_GRID, NB_COLS_GRID);
        }

        public void run()
        {
            while(!isInPause)
            {
                Render();
                GenerateNextStep();
            }
        }

        public void GenerateNextStep()
        {
            ++stepGeneration;
            currentPopAlive = currentPopDead = currentPopDying = currentPopEmerging = 0;

            Parallel.For(0, gameBoard1.GetUpperBound(0) + 1, i =>
                {
                    for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                    {
                        int neighbors = 0;
                        for (int ii = -1; ii <= 1; ++ii)
                            for (int jj = -1; jj <= 1; ++jj)
                                if (GameBoard.IsConsideredLikeAlive(gameBoard1[i + ii, j + jj]))
                                    ++neighbors;
                        if (GameBoard.IsConsideredLikeAlive(gameBoard1[i, j]))
                            --neighbors;

                        if (GameBoard.IsConsideredLikeAlive(gameBoard1[i, j]) && (neighbors < 2 || neighbors > 3))
                        {
                            gameBoard2[i, j] = GameBoard.State.Dying;
                            ++currentPopDying;
                        }
                        else if (GameBoard.IsConsideredLikeDead(gameBoard1[i, j]) && neighbors == 3)
                        {
                            gameBoard2[i, j] = GameBoard.State.Emerging;
                            ++currentPopEmerging;
                        }
                        else if (gameBoard1[i, j] == GameBoard.State.Dying)
                        {
                            gameBoard2[i, j] = GameBoard.State.Dead;
                            ++currentPopDead;
                        }
                        else if (gameBoard1[i, j] == GameBoard.State.Emerging)
                            gameBoard2[i, j] = GameBoard.State.Alive;
                        else
                            gameBoard2[i, j] = gameBoard1[i, j];

                        if (gameBoard2[i, j] == GameBoard.State.Alive)
                            ++currentPopAlive;
                    }
                });

            totPopEmerged += currentPopEmerging;
            totPopDying += currentPopDying;
            totPopDead += currentPopDead;
            GameBoard temp = gameBoard1;
            gameBoard1 = gameBoard2;
            gameBoard2 = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Row</param>
        /// <param name="y">Col</param>
        public void ClickCell(double x, double y)
        {
            displayEngine.GetCellClickCoordinate(ref x, ref y);
            gameBoard1[(int)x, (int)y] = GameBoard.State.Alive;
            displayEngine.DrawCellGameBoard((int)x, (int)y, GameBoard.GetRender(gameBoard1[(int)x, (int)y]));
        }

        public void Render()
        {
            displayEngine.DrawStatistics(stepGeneration, currentPopAlive + currentPopEmerging + currentPopDying, currentPopEmerging, currentPopDying, currentPopDead, totPopEmerged, totPopDead, totPopDying);

            for (int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                    if(gameBoard1[i, j] != GameBoard.State.Empty)
                        displayEngine.DrawCellGameBoard(i, j, GameBoard.GetRender(gameBoard1[i, j]));
        }

        private void Init(int x, int y)
        {
            gameBoard1 = new GameBoard(x, y);
            gameBoard2 = new GameBoard(x, y);

            for(int i = 0; i < gameBoard1.GetUpperBound(0); ++i)
                for(int j = 0; j < gameBoard1.GetUpperBound(1); ++j)
                    if(gameBoard1[i, j] == GameBoard.State.Alive)
                        ++currentPopAlive;
                    else if (gameBoard1[i, j] == GameBoard.State.Dead)
                        ++currentPopDead;
                    else if (gameBoard1[i, j] == GameBoard.State.Dying)
                        ++currentPopDying;
                    else if (gameBoard1[i, j] == GameBoard.State.Emerging)
                        ++currentPopEmerging;

            totPopDead += currentPopDead;
            totPopDying += currentPopDying;
            totPopEmerged += currentPopEmerging;
        }

        #region GameEngine Get/Set

        public bool IsInPause { get { return isInPause; } }

        public void PauseChange()
        {
            isInPause = !isInPause;
        }

        public DisplayEngine DisplayEngine { get { return displayEngine; } }

        #endregion
    }
}
