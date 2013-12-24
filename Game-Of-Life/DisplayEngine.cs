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
        public const string COLOR_UP = "#FF555555";
        public const string COLOR_DOWN = "#FF222222";
        public const string BACKGROUD_COLOR_BUTTON = "#00000000";
        public const string BACKGROUND_GENERAL = "#FF111111";
        public const string BACKGROUND_GAMEBOARD = "#FF111111";

        private const int LINE_STROCK_THICKNESS = 1;

        private Canvas gridGameBoard;
        private Canvas gridPattern;

        public DisplayEngine(Canvas gridGameBoard, Canvas gridPattern)
        {
            this.gridGameBoard = gridGameBoard;
            this.gridPattern = gridPattern;
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

        public void drawGameBoard()
        {
            double nbRows = GameEngine.NB_ROWS_GRID;
            double nbCols = GameEngine.NB_COLS_GRID;
            double width = (gridGameBoard.ActualWidth - nbCols * LINE_STROCK_THICKNESS) / nbCols - 0.025;
            double height = (gridGameBoard.ActualHeight - nbRows * LINE_STROCK_THICKNESS) / nbRows - 0.025;

            double margeTopBottom = (gridGameBoard.ActualHeight - (int)nbRows * (height + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
            double margeLeftRight = (gridGameBoard.ActualWidth - (int)nbCols * (width + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;

            drawGrid(gridGameBoard, nbCols, nbRows, margeTopBottom, margeLeftRight, width, height);
        }

        public void drawPattern(PatternRepresentation pattern)
        {
            double nbCols = pattern.Col;
            double nbRows = pattern.Row;
            double width = gridPattern.ActualWidth / nbCols - 2;
            double height = gridPattern.ActualHeight / nbRows - 2;

            double margeTopBottom = (gridPattern.ActualHeight - (int)nbRows * (height + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
            double margeLeftRight = (gridPattern.ActualWidth - (int)nbCols * (width + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;

            drawGrid(gridPattern, nbCols, nbRows, margeTopBottom, margeLeftRight, width, height);

            for (int i = 0; i < pattern.Row; ++i)
                for (int j = 0; j < pattern.Col; ++j)
                    if (pattern[i, j] == PatternRepresentation.ALIVE)
                        drawCell(gridPattern, i, j, margeTopBottom, margeLeftRight, width, height);
        }

        private static void drawGrid(Canvas canvas, double nbCols, double nbRows, double margeTopBottom, double margeLeftRight, double width, double height)
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

        private static void drawCell(Canvas canvas, int i, int j, double margeTopBottom, double margeLeftRight, double width, double height)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = BrushFromString(COLOR_DOWN);
            rectangle.StrokeThickness = LINE_STROCK_THICKNESS;
            rectangle.Width = width;
            rectangle.Height = height;
            Canvas.SetTop(rectangle, (int)(margeTopBottom + LINE_STROCK_THICKNESS * (i + 2) + i * height));
            Canvas.SetLeft(rectangle, (int)(margeLeftRight + LINE_STROCK_THICKNESS * (j + 2) + j * width));
            canvas.Children.Add(rectangle);
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
