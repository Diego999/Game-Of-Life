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

        public GameEngine(Canvas gridGameBoard, Canvas gridPattern, Canvas gridGameBoardCell)
        {
            displayEngine = new DisplayEngine(gridGameBoard, gridPattern, gridGameBoardCell);

            stepGeneration = 0;
            isInPause = true;
            currentPopDying = 0;
            currentPopDead = 0;
            currentPopAlive = 0;
            currentPopEmerging = 0;

            totPopDying = 0;
            totPopDead = 0;
            totPopEmerged = 0;

            init(NB_COLS_GRID, NB_ROWS_GRID);
        }

        public void generateNextStep()
        {
            ++stepGeneration;
            currentPopAlive = currentPopDead = currentPopDying = currentPopEmerging = 0;

            for (int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                {
                    int neighbors = 0;
                    for (int ii = -1; ii <= 1; ++ii)
                        for (int jj = -1; jj <= 1; ++jj)
                            if (gameBoard1[i + ii, j + jj].IsConsideredLikeAlive())
                                ++neighbors;
                    if (gameBoard1[i, j].IsConsideredLikeAlive())
                        --neighbors;

                    if (gameBoard1[i, j].IsConsideredLikeAlive() && (neighbors < 2 || neighbors > 3))
                    {
                        gameBoard2[i, j].SetDying();
                        ++currentPopDying;
                    }
                    else if (gameBoard1[i, j].IsConsideredLikeDead() && neighbors == 3)
                    {
                        gameBoard2[i, j].SetEmerging();
                        ++currentPopEmerging;
                    }
                    else if (gameBoard1[i, j].IsDying())
                    {
                        gameBoard2[i, j].SetDead();
                        ++currentPopDead;
                    }
                    else if (gameBoard1[i, j].IsEmerging())
                        gameBoard2[i, j].SetAlive();
                    else
                        gameBoard2[i, j] = gameBoard1[i, j];

                    if (gameBoard2[i, j].IsAlive())
                        ++currentPopAlive;
                }

            totPopEmerged += currentPopEmerging;
            totPopDying += currentPopDying;
            totPopDead += currentPopDead;
            GameBoard temp = gameBoard1;
            gameBoard1 = gameBoard2;
            gameBoard2 = temp;
        }

        public void ClickCell(double x, double y)
        {
            displayEngine.GetCellClickCoordinate(ref x, ref y);
            //displayEngine.DrawGameBoard();
            displayEngine.DrawCellGameBoard((int)y, (int)x);
            
        }

        public void render()
        {
            /*Console.WriteLine("\t\tGeneration " + stepGeneration);
            Console.WriteLine();
            Console.WriteLine("Current pop alive : " + (currentPopAlive + currentPopEmerging + currentPopDying));
            Console.WriteLine("Current pop emerging : " + currentPopEmerging);
            Console.WriteLine("Current pop dying : " + currentPopDying);
            Console.WriteLine("Current pop dead : " + currentPopDead);
            Console.WriteLine();
            Console.WriteLine("Total pop emerging : " + totPopEmerged);
            Console.WriteLine("Total pop dead : " + totPopDead);
            Console.WriteLine("Total pop dying : " + totPopDying);
            Console.WriteLine();

            for (int j = 0; j <= gameBoard1.GetUpperBound(1) + 2; ++j)
                Console.Write("-");
            Console.WriteLine();
            for (int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
            {
                Console.Write("|");
                for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                {
                    Console.Write(GameBoard.STATE_MATCH[gameBoard1[i, j]]);
                }
                Console.Write("|");
                Console.WriteLine();
            }
            for (int j = 0; j <= gameBoard1.GetUpperBound(1) + 2; ++j)
                Console.Write("-");
            Console.WriteLine();*/
        }

        private void init(int x, int y)
        {
            gameBoard1 = new GameBoard(x, y);
            gameBoard2 = new GameBoard(x, y);

            gameBoard1[2, 0].SetAlive();
            gameBoard1[2, 1].SetAlive();
            gameBoard1[2, 2].SetAlive();
            gameBoard1[1, 2].SetAlive();
            gameBoard1[0, 1].SetAlive();

            for(int i = 0; i < gameBoard1.GetUpperBound(0); ++i)
                for(int j = 0; j < gameBoard1.GetUpperBound(1); ++j)
                    if(gameBoard1[i, j].IsAlive())
                        ++currentPopAlive;
                    else if(gameBoard1[i, j].isDead())
                        ++currentPopDead;
                    else if(gameBoard1[i, j].IsDying())
                        ++currentPopDying;
                    else if(gameBoard1[i, j].IsEmerging())
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
