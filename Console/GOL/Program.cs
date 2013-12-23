using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    
    class Program
    {
        private const int X = 30;
        private const int Y = 30;

        static void Main(string[] args)
        {
            GameBoard gameBoard1 = new GameBoard(X, Y);
            GameBoard gameBoard2 = new GameBoard(X, Y);

            while (true)
            {
                gameBoard1.draw();
                for (int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
                    for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                    {
                        int neighbors = 0;
                        for (int ii = -1; ii <= 1; ++ii)
                            for (int jj = -1; jj <= 1; ++jj)
                                if(gameBoard1[i+ii, j+jj] == GameBoard.State.Alive)
                                    ++neighbors;
                        if (gameBoard1[i, j] == GameBoard.State.Alive)
                            --neighbors;

                        if (gameBoard1[i, j] == GameBoard.State.Alive && (neighbors < 2 || neighbors > 3))
                            gameBoard2[i, j] = GameBoard.State.Dead;
                        else if (gameBoard1[i, j] == GameBoard.State.Dead && neighbors == 3 || gameBoard1[i, j] == GameBoard.State.Alive && (neighbors == 2 || neighbors == 3))
                            gameBoard2[i, j] = GameBoard.State.Alive;
                        else
                            gameBoard2[i, j] = GameBoard.State.Dead;
                    }
                GameBoard temp = gameBoard1;
                gameBoard1 = gameBoard2;
                gameBoard2 = temp;
                Console.ReadKey();
                Console.Clear();
            }



        }
    }
}
