using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    class DisplayEngine
    {
        public static readonly string COLOR_UP = "#FF555555";
        public static readonly string COLOR_DOWN = "#FF222222";
        public static readonly string BACKGROUD_COLOR_BUTTON = "#00000000";
        public static readonly string BACKGROUND_GENERAL = "#FF111111";
        public static readonly string BACKGROUND_GAMEBOARD = "#FF111111";

        private static readonly int LINE_STROCK_THICKNESS = 1;
        private static readonly int[,] FACTORS;

        private Dictionary<int, Dictionary<int, List<UIElement>>> shapeHistory;

        private Canvas gridGameBoard;
        private Canvas gridPattern;
        private Canvas gridGameBoardCell;
        private Label lblValue1;
        private Label lblValue2;

        private double cellWidthGameBoard;
        private double cellHeightGameBoard;
        private double cellMargeTopBottom;
        private double cellMargeLeftRight;

        static DisplayEngine()
        {
            FACTORS = new int[4, 4] { {0, 0, 0, 2}, {1, 0, 0, 2}, {0, 1, 0, 0}, {0, 1, 2, 0} }; //w = 1, h = 2, for the draw of a cell in gridGameBoard
        }

        public DisplayEngine(Canvas gridGameBoard, Canvas gridPattern, Canvas gridGameBoardCell, Label lblValue1, Label lblValue2)
        {
            this.gridGameBoardCell = gridGameBoardCell;
            this.gridGameBoard = gridGameBoard;
            this.gridPattern = gridPattern;
            this.lblValue1 = lblValue1;
            this.lblValue2 = lblValue2;

            cellWidthGameBoard = 0;
            cellHeightGameBoard = 0;
            cellMargeTopBottom = 0;
            cellMargeLeftRight = 0;

            shapeHistory = new Dictionary<int, Dictionary<int, List<UIElement>>>();
            for (int i = 0; i < GameEngine.NB_ROWS_GRID; ++i)
            {
                shapeHistory[i] = new Dictionary<int, List<UIElement>>();
                for (int j = 0; j < GameEngine.NB_COLS_GRID; ++j)
                    shapeHistory[i][j] = new List<UIElement>();
            }
        }

        public void ComputeSizeCellGameBoard()
        {
            cellWidthGameBoard = (gridGameBoard.ActualWidth - GameEngine.NB_COLS_GRID * LINE_STROCK_THICKNESS) / GameEngine.NB_COLS_GRID - 0.025;
            cellHeightGameBoard = (gridGameBoard.ActualHeight - GameEngine.NB_ROWS_GRID * LINE_STROCK_THICKNESS) / GameEngine.NB_ROWS_GRID - 0.025;
            cellMargeTopBottom = (gridGameBoard.ActualHeight - (int)GameEngine.NB_ROWS_GRID * (cellHeightGameBoard + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
            cellMargeLeftRight = (gridGameBoard.ActualWidth - (int)GameEngine.NB_COLS_GRID * (cellWidthGameBoard + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
        }

        public static void SetBackground(Control control, string color)
        {
            control.Background = BrushFromString(color);
        }

        public static void SetBackground(Panel panel, string color)
        {
            panel.Background = BrushFromString(color);
        }

        public static void SetFill(Shape shape, string color)
        {
            shape.Fill = BrushFromString(color);
        }

        public static Brush BrushFromString(string color)
        {
            return (Brush)(new System.Windows.Media.BrushConverter()).ConvertFromString(color);
        }

        public void GetCellClickCoordinate(ref double x, ref double y)
        {
            x -= cellMargeLeftRight;
            y -= cellMargeTopBottom;
            x /= (cellWidthGameBoard + LINE_STROCK_THICKNESS);
            y /= (cellHeightGameBoard + LINE_STROCK_THICKNESS);
        }

        public void DrawStatistics(double generation, double currentPopAlive, double currentPopEmerging, double currentPopDying, double currentPopDead, double totPopEmerged, double totPopDead, double totPopDying)
        {
            lblValue1.Content = currentPopAlive.ToString() + Environment.NewLine + currentPopEmerging.ToString() + Environment.NewLine + currentPopDying.ToString() + Environment.NewLine + currentPopDead.ToString();
            lblValue2.Content = generation.ToString() + Environment.NewLine + totPopEmerged.ToString() + Environment.NewLine + totPopDead.ToString() + Environment.NewLine + totPopDying.ToString();
        }

        public void DrawGameBoard()
        {
            DrawGrid(gridGameBoard, GameEngine.NB_COLS_GRID, GameEngine.NB_ROWS_GRID, cellMargeTopBottom, cellMargeLeftRight, cellWidthGameBoard, cellHeightGameBoard);
        }

        public void DrawPattern(PatternRepresentation pattern)
        {
            double nbCols = pattern.Col;
            double nbRows = pattern.Row;
            double width = gridPattern.ActualWidth / nbCols - 2;
            double height = gridPattern.ActualHeight / nbRows - 2;

            double margeTopBottom = (gridPattern.ActualHeight - (int)nbRows * (height + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
            double margeLeftRight = (gridPattern.ActualWidth - (int)nbCols * (width + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;

            DrawGrid(gridPattern, nbCols, nbRows, margeTopBottom, margeLeftRight, width, height);

            for (int i = 0; i < pattern.Row; ++i)
                for (int j = 0; j < pattern.Col; ++j)
                    if (pattern[i, j] == PatternRepresentation.ALIVE)
                        DrawCell(gridPattern, i, j, margeTopBottom, margeLeftRight, width, height, COLOR_DOWN);
        }

        private static void DrawGrid(Canvas canvas, double nbCols, double nbRows, double margeTopBottom, double margeLeftRight, double width, double height)
        {
            canvas.Children.Clear();
            for (int i = 0; i <= (int)nbCols; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + i * width + (i + 1) * LINE_STROCK_THICKNESS;
                line.X2 = line.X1;
                line.Y1 = margeTopBottom + LINE_STROCK_THICKNESS; ;
                line.Y2 = canvas.ActualHeight - margeTopBottom;
                canvas.Children.Add(line);
            }
            for (int i = 0; i <= (int)nbRows; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + LINE_STROCK_THICKNESS;
                line.X2 = canvas.ActualWidth - margeLeftRight;
                line.Y1 = margeTopBottom + i * height + (i + 1) * LINE_STROCK_THICKNESS;
                line.Y2 = line.Y1;
                canvas.Children.Add(line);
            }
        }

        public void ClearGameBoardCells()
        {
            gridGameBoardCell.Children.Clear();
        }

        public void DrawCellGameBoard(int i, int j, Cell cell)
        {
            DrawCell(gridGameBoard, i, j, cellMargeTopBottom, cellMargeLeftRight, cellWidthGameBoard, cellHeightGameBoard, cell.GetRender(), shapeHistory);
        }

        private static void DrawCell(Canvas canvas, int i, int j, double margeTopBottom, double margeLeftRight, double width, double height, string color, Dictionary<int, Dictionary<int, List<UIElement>>> shapeHistory = null)
        {
            if(shapeHistory != null)
                foreach (UIElement ui in shapeHistory[i][j])
                    canvas.Children.Remove(ui);

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = BrushFromString(color);
            rectangle.StrokeThickness = LINE_STROCK_THICKNESS;
            rectangle.Width = width;
            rectangle.Height = height;
            double top = margeTopBottom + LINE_STROCK_THICKNESS * (i + 2) + i * height;
            double left = margeLeftRight + LINE_STROCK_THICKNESS * (j + 2) + j * width;
            Canvas.SetTop(rectangle, top);
            Canvas.SetLeft(rectangle, left);
            canvas.Children.Add(rectangle);
            if (shapeHistory != null)
                shapeHistory[i][j].Add(rectangle);

            for(int ii = 0; ii <= FACTORS.GetUpperBound(1); ++ii)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS/2.0;
                line.X1 = left + GetFactor(ii, 0, width, height);
                line.X2 = line.X1 + GetFactor(ii, 1, width, height);
                line.Y1 = top + GetFactor(ii, 2, width, height);
                line.Y2 = line.Y1 + GetFactor(ii, 3, width, height);
                canvas.Children.Add(line);
                if (shapeHistory != null)
                    shapeHistory[i][j].Add(line);
            }
        }

        private static double GetFactor(int i, int j, double width, double height)
        {
            return (FACTORS[i, j] == 1 ? width : FACTORS[i, j] == 2 ? height : 0);
        }

        public static void ShowAbout()
        {
            MessageBox.Show(@"The game of life is a game created by John Horton Conway in 1970. The game consists in a grid of cells where a cell can have one state among several and then, they live depending 3 rules (Wikipedia) :

1) Any live cell with fewer than two live neighbours dies, as if caused by under-population.
2) Any live cell with two or three live neighbours lives on to the next generation.
3) Any live cell with more than three live neighbours dies, as if by overcrowding.
4) Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

The game generate each step of living and you can see new cells emerging, dying, dead or alive.", "About Game Of Life", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
