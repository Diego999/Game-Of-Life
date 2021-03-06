﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace Game_Of_Life
{
    /// <summary>
    /// GameEngine of a game of life
    /// </summary>
    class GameEngine
    {
        public const int NB_ROWS_GRID = 40;
        public const int NB_COLS_GRID = (int)(NB_ROWS_GRID * 1.2);

        public const int DELAY_MIN = 100;
        public const int DEFAULT_DELAY = 250;
        public const int DELAY_MAX = 5000;

        private const int PROBABILITY_DRAWING_CELL = 3; // 1/x % to create a cell

        private GameBoard gameBoard1;
        private GameBoard gameBoard2;
        private DisplayEngine displayEngine;

        private int stepGeneration;

        private int currentPopDying;
        private int currentPopDead;
        private int currentPopAlive;
        private int currentPopEmerging;

        private int delay;

        bool isInPause;

        private Action actionGUI;

        private Task[] tasks;

        private ImportExportUtility importExportUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridGameBoard"></param>
        /// <param name="gridPattern"></param>
        /// <param name="gridGameBoardCell"></param>
        /// <param name="lblValue1"></param>
        public GameEngine(Canvas gridGameBoard, Canvas gridPattern, Canvas gridGameBoardCell, TextBlock lblValue1)
        {
            displayEngine = new DisplayEngine(gridGameBoard, gridPattern, gridGameBoardCell, lblValue1);
            tasks = new Task[Environment.ProcessorCount];
            actionGUI = new Action(RunWork);

            isInPause = true;
            delay = DEFAULT_DELAY;

            gameBoard1 = new GameBoard(NB_ROWS_GRID, NB_COLS_GRID);
            gameBoard2 = new GameBoard(NB_ROWS_GRID, NB_COLS_GRID);

            importExportUtility = new ImportExportUtility();

            Init();
        }

        /// <summary>
        /// Generate games until isInPause equals false
        /// </summary>
        public void Play()
        {
            Render();
            (new Thread(ThreadTask)).Start();
        }

        /// <summary>
        /// Render then generate a step
        /// </summary>
        public void RunWork()
        {
            GenerateNextStep();
            Render();
        }

        /// <summary>
        /// Add or remove a cell of the game board
        /// </summary>
        /// <param name="x">Row</param>
        /// <param name="y">Col</param>
        /// <param name="bool">True = To add, False = To remove</param>
        public void AddRemobeCellGlobalCoordinate(double x, double y, bool add = true)
        {
            displayEngine.GetCellClickCoordinate(ref x, ref y);
            GameBoard.State oldState = gameBoard1[(int)x, (int)y];
            gameBoard1[(int)x, (int)y] = gameBoard2[(int)x, (int)y] = add ? GameBoard.State.Alive : GameBoard.State.Empty;
            if (displayEngine.DrawCellGameBoard((int)x, (int)y, GameBoard.GetRender(gameBoard1[(int)x, (int)y])))
            {
                if (add)
                    ++currentPopAlive;

                if (oldState == GameBoard.State.Emerging)
                    --currentPopEmerging;
                else if (oldState == GameBoard.State.Dying)
                    --currentPopDying;
                else if (oldState == GameBoard.State.Dead)
                    --currentPopDead;
                else if (!add && oldState == GameBoard.State.Alive)
                    --currentPopAlive;

                displayEngine.DrawStatistics(stepGeneration, currentPopAlive, currentPopEmerging, currentPopDying, currentPopDead);
            }
        }

        /// <summary>
        /// Generate a grid of cells with random values
        /// </summary>
        public void GenerateGrid()
        { 
            GameBoard.State[] states = new GameBoard.State[] { GameBoard.State.Alive, GameBoard.State.Emerging, GameBoard.State.Dying, GameBoard.State.Dead };
            Clear();

            for (int t = 0; t < tasks.Length; ++t)
            {
                int tt = t; // If we don't copy, t won't be what we think
                tasks[tt] = Task.Factory.StartNew(() =>
                    {
                        Random rand = new Random();
                        for (int i = gameBoard1.GetUpperBound(0) * tt / tasks.Length; i <= gameBoard1.GetUpperBound(0) * (tt + 1) / tasks.Length; ++i)
                            for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                                if (rand.Next(0, PROBABILITY_DRAWING_CELL) == 0)
                                    gameBoard1[i, j] = gameBoard2[i, j] = states[rand.Next(states.Length)];
                    });
            }
            Task.WaitAll(tasks);
            Init();
            Render();
        }

        /// <summary>
        /// Import a grid from a file
        /// </summary>
        public void Import()
        {
            int stepGenerationTemp = stepGeneration;
            if (importExportUtility.Import(ref stepGenerationTemp, ref gameBoard1, ref gameBoard2))
            {
                displayEngine.ClearCell();
                Init();
                stepGeneration = stepGenerationTemp;
                Render();
            }
        }

        /// <summary>
        /// Export the current grid to a file
        /// </summary>
        public void Export()
        {
            importExportUtility.Export(stepGeneration, ref gameBoard1);
        }

        /// <summary>
        /// Clear the current game board
        /// </summary>
        public void Clear()
        {
            bool oldValue = isInPause;
            isInPause = true;

            displayEngine.ClearCell();
            gameBoard1.Init();
            gameBoard2.Init();
            Init();
            displayEngine.DrawStatistics(stepGeneration, currentPopAlive, currentPopEmerging, currentPopDying, currentPopDead);

            isInPause = oldValue;
            if (!isInPause)
                Play();
        }

        /// <summary>
        /// Generate the next game
        /// </summary>
        private void GenerateNextStep()
        {
            ++stepGeneration;
            currentPopAlive = currentPopDead = currentPopDying = currentPopEmerging = 0;

            for(int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
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

            GameBoard temp = gameBoard1;
            gameBoard1 = gameBoard2;
            gameBoard2 = temp;
        }

        /// <summary>
        /// Realize the rendering
        /// </summary>
        private void Render()
        {
            displayEngine.DrawStatistics(stepGeneration, currentPopAlive, currentPopEmerging, currentPopDying, currentPopDead);
            
            for (int i = 0; i <= gameBoard1.GetUpperBound(0); ++i)
                for (int j = 0; j <= gameBoard1.GetUpperBound(1); ++j)
                   if(gameBoard1[i, j] != GameBoard.State.Empty)
                    displayEngine.DrawCellGameBoard(i, j, GameBoard.GetRender(gameBoard1[i, j]));
        }

        /// <summary>
        /// Task executed by the thread which runs the game
        /// </summary>
        private void ThreadTask()
        {
            System.Threading.Thread.Sleep(delay);
            while (!isInPause)
            {
                Application.Current.Dispatcher.Invoke(actionGUI); // It's a very bad habit to draw in the GUI with another thread than the main, so we force the main thread to execute this action
                System.Threading.Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Init the statistics
        /// </summary>
        private void Init()
        {
            stepGeneration = 0;
            currentPopDying = 0;
            currentPopDead = 0;
            currentPopAlive = 0;
            currentPopEmerging = 0;

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
        }

        #region GameEngine Get/Set

        public bool IsInPause { get { return isInPause; } }

        public void PauseChange()
        {
            isInPause = !isInPause;
        }

        public DisplayEngine DisplayEngine { get { return displayEngine; } }

        public int Delay { set { delay = (value >= DELAY_MIN && value <= DELAY_MAX) ? value : DEFAULT_DELAY; } }

        #endregion
    }
}
