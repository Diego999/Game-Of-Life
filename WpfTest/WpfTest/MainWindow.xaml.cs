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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string COLOR_UP = "#FF000000";
        private const string COLOR_DOWN = "#77000000";
        private const string BACKGROUD_COLOR_BUTTON = "#00000000";
        private const int LINE_STROCK_THICKNESS = 1;
        private const int WIDTH_CELL_PATTERN = 15;
        private const int HEIGHT_CELL_PATTERN = 15;

        public MainWindow()
        {
            InitializeComponent();

            pathBtnClean.Fill = BrushFromString(COLOR_UP);
            pathBtnNextGeneration.Fill = BrushFromString(COLOR_UP);
            pathBtnSave.Fill = BrushFromString(COLOR_UP);
            pathBtnUpload.Fill = BrushFromString(COLOR_UP);
            pathBtnPlay.Fill = BrushFromString(COLOR_UP);

            btnClean.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnNextGeneration.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnSave.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnUpload.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnPlay.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);


        }

        private static Brush BrushFromString(string color)
        {
            return (Brush)(new System.Windows.Media.BrushConverter()).ConvertFromString(color);
        }

        private void sliderDelayGeneration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //There is a bidirectional dependency between tbxDelayGeneration and the sliderDelayGeneration
            if(tbxDelayGeneration != null)
                tbxDelayGeneration.Text = e.NewValue.ToString();
        }

        private void tbxDelayBetweenGeneration_TextChanged(object sender, TextChangedEventArgs e)
        {
            int val;
            if (int.TryParse(((TextBox)sender).Text, out val))
                if (val >= sliderDelayGeneration.Minimum && val <= sliderDelayGeneration.Maximum)
                    sliderDelayGeneration.Value = val;
        }

        private void btnPlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnPlay.Fill = BrushFromString(COLOR_DOWN);
        }

        private void btnNextGeneration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnNextGeneration.Fill = BrushFromString(COLOR_DOWN);
        }

        private void btnClean_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnClean.Fill = BrushFromString(COLOR_DOWN);
        }

        private void btnUpload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnUpload.Fill = BrushFromString(COLOR_DOWN);
        }

        private void btnSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnSave.Fill = BrushFromString(COLOR_DOWN);
        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnPlay.Fill = BrushFromString(COLOR_UP);
        }

        private void btnNextGeneration_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnNextGeneration.Fill = BrushFromString(COLOR_UP);
        }

        private void btnClean_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnClean.Fill = BrushFromString(COLOR_UP);
        }

        private void btnUpload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnUpload.Fill = BrushFromString(COLOR_UP);
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnSave.Fill = BrushFromString(COLOR_UP);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double nbRows = (imgPattern.ActualHeight-LINE_STROCK_THICKNESS)/(HEIGHT_CELL_PATTERN+LINE_STROCK_THICKNESS);
            double margeTopBottom = imgPattern.ActualHeight - (int)nbRows * (HEIGHT_CELL_PATTERN + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS;
            margeTopBottom /= 2;
            double nbCols = (imgPattern.ActualWidth - LINE_STROCK_THICKNESS) / (WIDTH_CELL_PATTERN + LINE_STROCK_THICKNESS);
            double margeLeftRight = imgPattern.ActualWidth - (int)nbCols * (WIDTH_CELL_PATTERN + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS;
            margeLeftRight /= 2;

            for (int i = 0; i < nbCols; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + i * HEIGHT_CELL_PATTERN + (i+1)*LINE_STROCK_THICKNESS;
                line.X2 = line.X1;
                line.Y1 = margeTopBottom + LINE_STROCK_THICKNESS; ;
                line.Y2 = imgPattern.ActualHeight - margeTopBottom;
                imgPattern.Children.Add(line);
            }
            for (int i = 0; i < nbRows; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + LINE_STROCK_THICKNESS;
                line.X2 = imgPattern.ActualWidth - margeLeftRight;
                line.Y1 = margeTopBottom + i * WIDTH_CELL_PATTERN + (i+1)*LINE_STROCK_THICKNESS;
                line.Y2 = line.Y1;
                imgPattern.Children.Add(line);
            }
        }

    }
}
