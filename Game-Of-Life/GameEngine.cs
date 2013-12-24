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

        private static bool IsCellDead(GameBoard.State s)
        {
            return s == GameBoard.State.Empty || s == GameBoard.State.Dead || s == GameBoard.State.Dying;
        }

        private static bool IsCellAlive(GameBoard.State s)
        {
            return s == GameBoard.State.Alive || s == GameBoard.State.Emerging;
        }

        public GameEngine(Canvas gridGameBoard, Canvas gridPattern)
        {
            displayEngine = new DisplayEngine(gridGameBoard, gridPattern);

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
                            if (IsCellAlive(gameBoard1[i + ii, j + jj]))
                                ++neighbors;
                    if (IsCellAlive(gameBoard1[i, j]))
                        --neighbors;

                    if (IsCellAlive(gameBoard1[i, j]) && (neighbors < 2 || neighbors > 3))
                    {
                        gameBoard2[i, j] = GameBoard.State.Dying;
                        ++currentPopDying;
                    }
                    else if (IsCellDead(gameBoard1[i, j]) && neighbors == 3)
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

            totPopEmerged += currentPopEmerging;
            totPopDying += currentPopDying;
            totPopDead += currentPopDead;
            GameBoard temp = gameBoard1;
            gameBoard1 = gameBoard2;
            gameBoard2 = temp;
        }

        public void render()
        {
            Console.WriteLine("\t\tGeneration " + stepGeneration);
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
            Console.WriteLine();
        }

        private void init(int x, int y)
        {
            gameBoard1 = new GameBoard(x, y);
            gameBoard2 = new GameBoard(x, y);

            gameBoard1[2, 0] = gameBoard1[2, 1] = gameBoard1[2, 2] = gameBoard1[1, 2] = gameBoard1[0, 1] = GameBoard.State.Alive;

            for(int i = 0; i < gameBoard1.GetUpperBound(0); ++i)
                for(int j = 0; j < gameBoard1.GetUpperBound(1); ++j)
                    switch (gameBoard1[i, j])
                    {
                        case GameBoard.State.Alive:
                            ++currentPopAlive;
                            break;

                        case GameBoard.State.Dead:
                            ++currentPopDead;
                            break;

                        case GameBoard.State.Dying:
                            ++currentPopDying;
                            break;

                        case GameBoard.State.Emerging:
                            ++currentPopEmerging;
                            break;

                        default:
                            break;
                    }

            totPopDead += currentPopDead;
            totPopDying += currentPopDying;
            totPopEmerged += currentPopEmerging;
        }

        public bool IsInPause { get { return isInPause; } }

        public void PauseChange()
        {
            isInPause = !isInPause;
        }

        public DisplayEngine DisplayEngine { get { return displayEngine; } }
    }
}
