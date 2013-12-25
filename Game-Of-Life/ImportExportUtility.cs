using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Game_Of_Life
{
    class ImportExportUtility
    {
        private static readonly Dictionary<GameBoard.State, string> STATE_MATCH;
        private static readonly Dictionary<string, GameBoard.State> STATE_MATCH_INVERSE;

        private static readonly char SEPARTOR = ';';

        private static readonly string DEFAULT_FILENAME = "GameOfLife";
        private static readonly string FILENAME = "Game Of Life";
        private static readonly string EXTENSION = "gol";

        private static readonly int GENERATION_POS = 0;
        private static readonly int ROWS_POS = 1;
        private static readonly int COLS_POS = 2;
        private static readonly int GAMEBOARD_POS = 3;

        private static readonly string ALIVE = "C";
        private static readonly string EMERGING = "I";
        private static readonly string EMPTY = "H";
        private static readonly string DYING = "B";
        private static readonly string DEAD = "T";

        private OpenFileDialog ofd;
        private SaveFileDialog sfd;

        static ImportExportUtility()
        {
            STATE_MATCH = new Dictionary<GameBoard.State, string>();
            STATE_MATCH.Add(GameBoard.State.Alive, ALIVE);
            STATE_MATCH.Add(GameBoard.State.Emerging, EMERGING);
            STATE_MATCH.Add(GameBoard.State.Empty, EMPTY);
            STATE_MATCH.Add(GameBoard.State.Dying, DYING);
            STATE_MATCH.Add(GameBoard.State.Dead, DEAD);

            STATE_MATCH_INVERSE = new Dictionary<string, GameBoard.State>();
            foreach (KeyValuePair<GameBoard.State, string> entry in STATE_MATCH)
                STATE_MATCH_INVERSE.Add(entry.Value, entry.Key);
        }

        /// <summary>
        /// Constructor, we want to modify the reference of the object when we import
        /// </summary>
        /// <param name="gameboard1"></param>
        /// <param name="gameboard2"></param>
        public ImportExportUtility()
        {
            ofd = new OpenFileDialog();
            sfd = new SaveFileDialog();

            ofd.FileName = sfd.FileName = DEFAULT_FILENAME + "." + EXTENSION;
            ofd.DefaultExt = sfd.DefaultExt = "." + EXTENSION;
            ofd.Filter = sfd.Filter = FILENAME + " Files (*." + EXTENSION + ")|*." + EXTENSION;    
        }

        public void Export(int stepGeneration, ref GameBoard gameboard1)
        {
            if(sfd.ShowDialog() == true)
            {
                StringBuilder output = new StringBuilder(stepGeneration.ToString() + SEPARTOR + (1 + gameboard1.GetUpperBound(0)).ToString() + SEPARTOR + (1 + gameboard1.GetUpperBound(1)).ToString() + SEPARTOR);
                for (int i = 0; i <= gameboard1.GetUpperBound(0); ++i)
                    for (int j = 0; j <= gameboard1.GetUpperBound(1); ++j)
                        output.Append(STATE_MATCH[gameboard1[i, j]]);

                byte[] bytes = GetBytes(output.ToString());
                FileStream file = File.Create(sfd.FileName);
                for (int i = 0; i < bytes.Length; ++i)
                    file.WriteByte(bytes[i]);
            }
        }

        public bool Import(ref int nbGeneration, ref GameBoard gameboard1, ref GameBoard gameboard2)
        {
            if(ofd.ShowDialog() == true)
            {
                String[] res = GetString(File.ReadAllBytes(ofd.FileName)).Split(SEPARTOR);
                if(res.Length != GAMEBOARD_POS + 1)
                    ShowCorruptFileDialog();

                int nbGenerationTemp = 0;
                int rows = 0;
                int cols = 0;
                String input = "";

                for (int i = 0; i < res.Length; ++i)
                    if(i == GENERATION_POS)
                    {
                        if (!int.TryParse(res[i], out nbGenerationTemp))
                        {
                            ShowCorruptFileDialog();
                            return false;
                        }
                    }
                    else if(i == ROWS_POS)
                    {
                        if (!int.TryParse(res[i], out rows))
                        { 
                            ShowCorruptFileDialog();
                            return false;
                        }
                        if (rows < GameBoard.MIN_ROWS)
                        { 
                            ShowCorruptFileDialog();
                            return false;
                        }
                    }
                    else if(i == COLS_POS)
                    {
                        if (!int.TryParse(res[i], out cols))
                        { 
                            ShowCorruptFileDialog();
                            return false;
                        }
                        if (cols < GameBoard.MIN_COLS)
                        { 
                            ShowCorruptFileDialog();
                            return false;
                        }
                    }
                    else if(i == GAMEBOARD_POS)
                        input = res[i];
                if (input.Length != cols*rows)
                {
                    ShowCorruptFileDialog();
                    return false;
                }
                nbGeneration = nbGenerationTemp;
                GameBoard gameboard1Temp = new GameBoard(rows, cols);
                GameBoard gameboard2Temp = new GameBoard(rows, cols);
                for (int i = 0; i < rows; ++i)
                    for (int j = 0; j < cols; ++j)
                        if(!STATE_MATCH_INVERSE.ContainsKey(input[i * (1 + j)].ToString()))
                        { 
                            ShowCorruptFileDialog();
                            return false;
                        }
                        else
                            gameboard1Temp[i, j] = gameboard2Temp[i, j] = STATE_MATCH_INVERSE[input[i * (1 + j)].ToString()];
                gameboard1 = gameboard1Temp;
                gameboard2 = gameboard2Temp;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Show an error dialog with the error "File corrupted !"
        /// </summary>
        private static void ShowCorruptFileDialog()
        {
            MessageBox.Show("File corrupted !", "File corrupted !", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Convert a string to byte. Source : http://stackoverflow.com/questions/472906/net-string-to-byte-array-c-sharp?answertab=active#tab-top
        /// </summary>
        /// <param name="str">string to convert</param>
        /// <returns>byte array</returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Convert byte array to string. Source : http://stackoverflow.com/questions/472906/net-string-to-byte-array-c-sharp?answertab=active#tab-top
        /// </summary>
        /// <param name="bytes">byte array to convert</param>
        /// <returns>string</returns>
        /// ///
        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
