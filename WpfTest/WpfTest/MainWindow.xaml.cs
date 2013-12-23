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
        private const string COLOR_UP = "#FF555555";
        private const string COLOR_DOWN = "#77000000";
        private const string BACKGROUD_COLOR_BUTTON = "#00000000";
        private const string BACKGROUND_GENERAL = "#FF222222";
        private const string BACKGROUND_GAMEBOARD = "#FF111111";
        private const int LINE_STROCK_THICKNESS = 1;

        private bool isInPause;

        public MainWindow()
        {
            InitializeComponent();
            isInPause = true;    

            pathBtnClean.Fill = BrushFromString(COLOR_UP);
            pathBtnNextGeneration.Fill = BrushFromString(COLOR_UP);
            pathBtnSave.Fill = BrushFromString(COLOR_UP);
            pathBtnUpload.Fill = BrushFromString(COLOR_UP);
            pathBtnPlay.Fill = BrushFromString(COLOR_UP);
            pathBtnPause.Fill = BrushFromString(COLOR_UP);

            btnClean.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnNextGeneration.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnSave.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnUpload.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnPlay.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);
            btnPause.Background = BrushFromString(BACKGROUD_COLOR_BUTTON);

            btnPause.Visibility = Visibility.Hidden;

            this.Background = BrushFromString(BACKGROUND_GENERAL);
            cvsGameBoard.Background = BrushFromString(BACKGROUND_GAMEBOARD);
            lblDelayBetweenGeneration.Foreground = BrushFromString(COLOR_UP);
            tbxDelayGeneration.Foreground = BrushFromString(COLOR_UP);
            initCombobox();
        }

        private void drawPlay()
        {
            btnPause.Visibility = Visibility.Hidden;
            btnPlay.Visibility = Visibility.Visible;
        }

        private void drawPause()
        {
            btnPause.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Hidden;
        }

        private void initCombobox()
        {
            foreach (string key in Patterns.PATTERNS.Keys)
                cbxPattern.Items.Add(key);
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
            if (isInPause)
                pathBtnPlay.Fill = BrushFromString(COLOR_DOWN);
            else
                pathBtnPause.Fill = BrushFromString(COLOR_DOWN);
            isInPause = !isInPause;
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
            if (isInPause)
            {
                pathBtnPlay.Fill = BrushFromString(COLOR_UP);
                drawPlay();
            }
            else
            {
                pathBtnPause.Fill = BrushFromString(COLOR_UP);
                drawPause();
            }
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
            cbxPattern.SelectedItem = Patterns.PATTERNS.Keys.First();
            cbxPattern_SelectionChanged(cbxPattern, null);
        }

        private void cbxPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imgPattern.Children.Clear();
            string t = ((ComboBox)sender).SelectedItem.ToString();
            drawPattern(Patterns.PATTERNS[((ComboBox)sender).SelectedItem.ToString()]);
        }

        private void drawPattern(PatternRepresentation pattern)
        {
            double nbCols = pattern.Col;
            double nbRows = pattern.Row;
            double width = imgPattern.ActualWidth / nbCols-2;
            double height = imgPattern.ActualHeight / nbRows-2;

            double margeTopBottom = (imgPattern.ActualHeight - (int)nbRows * (height + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;
            double margeLeftRight = (imgPattern.ActualWidth - (int)nbCols * (width + LINE_STROCK_THICKNESS) - LINE_STROCK_THICKNESS) / 2.0;

            for (int i = 0; i <= (int)nbCols; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + i * width + (i + 1) * LINE_STROCK_THICKNESS;
                line.X2 = line.X1;
                line.Y1 = margeTopBottom + LINE_STROCK_THICKNESS; ;
                line.Y2 = imgPattern.ActualHeight - margeTopBottom;
                imgPattern.Children.Add(line);
            }
            for (int i = 0; i <= (int)nbRows; ++i)
            {
                Line line = new Line();
                line.Stroke = BrushFromString(COLOR_UP);
                line.StrokeThickness = LINE_STROCK_THICKNESS;
                line.X1 = margeLeftRight + LINE_STROCK_THICKNESS;
                line.X2 = imgPattern.ActualWidth - margeLeftRight;
                line.Y1 = margeTopBottom + i * height + (i + 1) * LINE_STROCK_THICKNESS;
                line.Y2 = line.Y1;
                imgPattern.Children.Add(line);
            }

            for(int i = 0; i < pattern.Row; ++i)
                for(int j = 0; j < pattern.Col; ++j)
                {
                    if (pattern[i, j] == PatternRepresentation.ALIVE)
                    {
                        Rectangle rectangle = new Rectangle();
                        rectangle.Fill = BrushFromString(COLOR_DOWN);
                        rectangle.StrokeThickness = LINE_STROCK_THICKNESS;
                        rectangle.Width = width;
                        rectangle.Height = height;
                        Canvas.SetTop(rectangle, (int)(margeTopBottom + LINE_STROCK_THICKNESS * (i + 2) + i * height));
                        Canvas.SetLeft(rectangle, (int)(margeLeftRight + LINE_STROCK_THICKNESS * (j + 2) + j * width));
                        imgPattern.Children.Add(rectangle);
                    }
                }
        }
    }
}
