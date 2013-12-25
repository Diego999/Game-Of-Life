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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameEngine gameEngine;
        private bool isMouseDown;
        private int lastClickX; //Rows
        private int lastClickY; //Cols
        public MainWindow()
        {
            InitializeComponent();
            
            isMouseDown = false;
            lastClickX = -10;
            lastClickY = -10;

            gameEngine = new GameEngine(cvsGameBoard, cvsPattern, cvsGameBoardCell, tbxValue1);

            pathBtnClear.Fill = DisplayEngine.COLOR_UP;
            pathBtnNextGeneration.Fill = DisplayEngine.COLOR_UP;
            pathBtnSave.Fill = DisplayEngine.COLOR_UP;
            pathBtnLoad.Fill = DisplayEngine.COLOR_UP;
            pathBtnPlay.Fill = DisplayEngine.COLOR_UP;
            pathBtnPause.Fill = DisplayEngine.COLOR_UP;
            pathBtnInfo.Fill = DisplayEngine.COLOR_UP;
            pathBtnGenerate.Fill = DisplayEngine.COLOR_UP;

            btnClear.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnNextGeneration.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnSave.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnLoad.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnPlay.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnPause.Background = DisplayEngine.BACKGROUND_GENERAL;
            bntInfo.Background = DisplayEngine.BACKGROUND_GENERAL;
            btnGenerate.Background = DisplayEngine.BACKGROUND_GENERAL;
            this.Background = DisplayEngine.BACKGROUND_GENERAL;

            drawPlayPause(gameEngine.IsInPause);

            sliderDelayGeneration.Minimum = GameEngine.DELAY_MIN;
            sliderDelayGeneration.Maximum = GameEngine.DELAY_MAX;
            tbxDelayGeneration.Text = GameEngine.DEFAULT_DELAY.ToString();

            lblDelayBetweenGeneration.Foreground = DisplayEngine.COLOR_UP;
            tbxDelayGeneration.Foreground = DisplayEngine.COLOR_UP;

            tbxLegend1.Foreground = DisplayEngine.COLOR_UP;
            tbxValue1.Foreground = DisplayEngine.COLOR_UP;

            cbxPattern.Background = Brushes.Transparent;
            cbxPattern.Foreground = DisplayEngine.COLOR_UP;
            cbxPattern.Resources.Add(SystemColors.WindowBrushKey, Brushes.Transparent);
            initCombobox();
        }

        private void drawPlayPause(bool isInPause)
        {
            btnPause.Visibility = isInPause ? Visibility.Hidden : Visibility.Visible;
            btnPlay.Visibility = isInPause ? Visibility.Visible : Visibility.Hidden;
            btnNextGeneration.Visibility = gameEngine.IsInPause ? Visibility.Visible : Visibility.Hidden;
            btnSave.Visibility = gameEngine.IsInPause ? Visibility.Visible : Visibility.Hidden;
            btnLoad.Visibility = gameEngine.IsInPause ? Visibility.Visible : Visibility.Hidden;
            btnGenerate.Visibility = gameEngine.IsInPause ? Visibility.Visible : Visibility.Hidden;
        }

        private void initCombobox()
        {
            foreach (string key in Patterns.PATTERNS.Keys)
                cbxPattern.Items.Add(key);
        }

        #region MainWindow Miscellaneous event handler

        private void ClickCell(Point p)
        {
            if (p.Y < cvsGameBoard.ActualHeight - 1 && p.Y > 1 && p.X > 1 && p.X < cvsGameBoard.ActualWidth && lastClickX != (int)p.Y && lastClickY != (int)p.X)
            {
                gameEngine.ClickCell(p.Y, p.X);
                lastClickY = (int)p.X;
                lastClickY = (int)p.Y;
            }
        }

        private void cvsGameBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isMouseDown = true;
                ClickCell(e.GetPosition((Canvas)sender));
            }
        }

        private void cvsGameBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && e.LeftButton == MouseButtonState.Pressed)
                ClickCell(e.GetPosition((Canvas)sender));
        }

        private void cvsGameBoard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                isMouseDown = false;
        }

        private void sliderDelayGeneration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //There is a bidirectional dependency between tbxDelayGeneration and the sliderDelayGeneration
            if (tbxDelayGeneration != null)
                tbxDelayGeneration.Text = e.NewValue.ToString();
        }

        private void tbxDelayBetweenGeneration_TextChanged(object sender, TextChangedEventArgs e)
        {
            int val;
            bool hasBeenChanged = false;
            if (int.TryParse(((TextBox)sender).Text, out val))
                if (val >= sliderDelayGeneration.Minimum && val <= sliderDelayGeneration.Maximum)
                {
                    sliderDelayGeneration.Value = val;
                    gameEngine.Delay = val;
                    hasBeenChanged = true;
                }
            tbxDelayGeneration.Background = hasBeenChanged ? DisplayEngine.BACKGROUND_GENERAL : DisplayEngine.WARNING;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbxPattern.SelectedItem = Patterns.PATTERNS.Keys.First();
            cbxPattern_SelectionChanged(cbxPattern, null);
            gameEngine.DisplayEngine.ComputeSizeCellGameBoard();
            gameEngine.DisplayEngine.DrawGameBoard();
        }

        private void cbxPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cvsPattern.Children.Clear();
            string t = ((ComboBox)sender).SelectedItem.ToString();
            gameEngine.DisplayEngine.DrawPattern(Patterns.PATTERNS[((ComboBox)sender).SelectedItem.ToString()]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!gameEngine.IsInPause)
                gameEngine.PauseChange();
        }

        #endregion

        #region MainWindow MouseLeftButtonDown
        private void btnPlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (gameEngine.IsInPause)
                pathBtnPlay.Fill = DisplayEngine.COLOR_DOWN;
            else
                pathBtnPause.Fill = DisplayEngine.COLOR_DOWN;
            gameEngine.PauseChange();
        }

        private void btnNextGeneration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnNextGeneration.Fill = DisplayEngine.COLOR_DOWN;
        }

        private void btnClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnClear.Fill = DisplayEngine.COLOR_DOWN;
        }

        private void btnLoad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnLoad.Fill =  DisplayEngine.COLOR_DOWN;
        }

        private void btnSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnSave.Fill = DisplayEngine.COLOR_DOWN;
        }

        private void bntInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnInfo.Fill = DisplayEngine.COLOR_DOWN;
        }

        private void btnGenerate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathBtnGenerate.Fill = DisplayEngine.COLOR_DOWN;
        }

        #endregion

        #region Mainwindow MouseLeftButtonUp
        private void bntInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnInfo.Fill = DisplayEngine.COLOR_UP;
            DisplayEngine.ShowAbout();
        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (gameEngine.IsInPause)
                pathBtnPlay.Fill =  DisplayEngine.COLOR_UP;
            else
                pathBtnPause.Fill =  DisplayEngine.COLOR_UP;
            drawPlayPause(gameEngine.IsInPause);
            gameEngine.Play();
        }

        private void btnNextGeneration_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           pathBtnNextGeneration.Fill =  DisplayEngine.COLOR_UP;
           gameEngine.RunWork();
        }

        private void btnClear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnClear.Fill = DisplayEngine.COLOR_UP;
            gameEngine.Clear();
        }

        private void btnLoad_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnLoad.Fill =  DisplayEngine.COLOR_UP;
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnSave.Fill =  DisplayEngine.COLOR_UP;
        }

        private void btnGenerate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathBtnGenerate.Fill = DisplayEngine.COLOR_UP;
            gameEngine.GenerateGrid();
        }
        #endregion
    }

}
