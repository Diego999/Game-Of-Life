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
   
        public MainWindow()
        {
            InitializeComponent();
            isMouseDown = false;
            gameEngine = new GameEngine(cvsGameBoard, cvsPattern, cvsGameBoardCell);

            DisplayEngine.SetFill(pathBtnClean, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnNextGeneration, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnSave, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnUpload, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnPlay, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnPause, DisplayEngine.COLOR_UP);
            DisplayEngine.SetFill(pathBtnInfo, DisplayEngine.COLOR_UP);

            DisplayEngine.SetBackground(btnClean, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(btnNextGeneration, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(btnSave, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(btnUpload, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(btnPlay, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(btnPause, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(bntInfo, DisplayEngine.BACKGROUND_GENERAL);
            DisplayEngine.SetBackground(this, DisplayEngine.BACKGROUND_GENERAL);

            drawPlayPause(gameEngine.IsInPause);

            this.Background = DisplayEngine.BrushFromString(DisplayEngine.BACKGROUND_GENERAL);
            cvsGameBoard.Background = DisplayEngine.BrushFromString(DisplayEngine.BACKGROUND_GAMEBOARD);
            lblDelayBetweenGeneration.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);
            tbxDelayGeneration.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);

            lblLegend1.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);
            lblLegend2.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);
            lblValue1.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);
            lblValue2.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);

            cbxPattern.Background = Brushes.Transparent;
            cbxPattern.Foreground = DisplayEngine.BrushFromString(DisplayEngine.COLOR_UP);
            cbxPattern.Resources.Add(SystemColors.WindowBrushKey, Brushes.Transparent);
            initCombobox();
        }

        private void drawPlayPause(bool isInPause)
        {

            btnPause.Visibility = isInPause ? Visibility.Hidden : Visibility.Visible;
            btnPlay.Visibility = isInPause ? Visibility.Visible : Visibility.Hidden;
        }

        private void initCombobox()
        {
            foreach (string key in Patterns.PATTERNS.Keys)
                cbxPattern.Items.Add(key);
        }

        #region MainWindow Miscellaneous event handler

        private void ClickCell(Point p)
        {
            if(p.Y < cvsGameBoard.ActualHeight-1 && p.Y > 1 && p.X > 1 && p.X < cvsGameBoard.ActualWidth)
                gameEngine.ClickCell(p.X, p.Y);
        }

        private void cvsGameBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            ClickCell(e.GetPosition((Canvas)sender));
        }

        private void cvsGameBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown)
                ClickCell(e.GetPosition((Canvas)sender));
        }

        private void cvsGameBoard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
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
            if (int.TryParse(((TextBox)sender).Text, out val))
                if (val >= sliderDelayGeneration.Minimum && val <= sliderDelayGeneration.Maximum)
                    sliderDelayGeneration.Value = val;
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
        #endregion

        #region MainWindow MouseLeftButtonDown
        private void btnPlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (gameEngine.IsInPause)
                DisplayEngine.SetFill(pathBtnPlay, DisplayEngine.COLOR_DOWN);
            else
                DisplayEngine.SetFill(pathBtnPause, DisplayEngine.COLOR_DOWN);
            gameEngine.PauseChange();
        }

        private void btnNextGeneration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnNextGeneration, DisplayEngine.COLOR_DOWN);
        }

        private void btnClean_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnClean, DisplayEngine.COLOR_DOWN);
        }

        private void btnUpload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnUpload, DisplayEngine.COLOR_DOWN);
        }

        private void btnSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnSave, DisplayEngine.COLOR_DOWN);
        }

        private void bntInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnInfo, DisplayEngine.COLOR_DOWN);
        }

        #endregion

        #region Mainwindow MouseLeftButtonUp
        private void bntInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnInfo, DisplayEngine.COLOR_UP);
            DisplayEngine.ShowAbout();
        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (gameEngine.IsInPause)
                DisplayEngine.SetFill(pathBtnPlay, DisplayEngine.COLOR_UP);
            else
                DisplayEngine.SetFill(pathBtnPause, DisplayEngine.COLOR_UP);
            drawPlayPause(gameEngine.IsInPause);
        }

        private void btnNextGeneration_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnNextGeneration, DisplayEngine.COLOR_UP);
        }

        private void btnClean_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnClean, DisplayEngine.COLOR_UP);
        }

        private void btnUpload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnUpload, DisplayEngine.COLOR_UP);
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayEngine.SetFill(pathBtnSave, DisplayEngine.COLOR_UP);
        }
        #endregion
    }

}
