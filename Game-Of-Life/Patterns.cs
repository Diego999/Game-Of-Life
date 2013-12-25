using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game_Of_Life
{
    /// <summary>
    /// Static class which holds all the patterns
    /// </summary>
    class Patterns
    {
        /// <summary>
        /// Dictionary where the key is the name of the pattern and the value the pattern
        /// </summary>
        public static readonly Dictionary<string, PatternRepresentation> PATTERNS;
        private static readonly string DIRECTORY_PATTERNS = "patterns";
        private static readonly string PATTERN_EXT = "txt";
        static Patterns()
        {
            PATTERNS = new Dictionary<string, PatternRepresentation>();

            string path = System.Environment.CurrentDirectory;
            path = path.Substring(0, path.LastIndexOf("bin")) + DIRECTORY_PATTERNS;
            string[] filePaths = Directory.GetFiles(path, "*." + PATTERN_EXT);

            string[] nl = new string[] { Environment.NewLine};
            foreach(string filePath in filePaths)
            {
                string key = Path.GetFileNameWithoutExtension(filePath);
                String[] text = System.IO.File.ReadAllText(filePath).Split(nl, StringSplitOptions.None);
                PatternRepresentation value = new PatternRepresentation(text.Length, text[0].Length);

                for (int i = 0; i < text.Length; ++i)
                    for (int j = 0; j < text[i].Length; ++j)
                        if (text[i][j].ToString() == PatternRepresentation.ALIVE)
                            value[i, j] = PatternRepresentation.ALIVE;
                PATTERNS.Add(key, value);
            }
        }
    }
}
