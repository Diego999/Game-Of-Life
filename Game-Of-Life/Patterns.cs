﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Game_Of_Life
{
    /// <summary>
    /// Static class which holds all the patterns find in the folder "patterns"
    /// </summary>
    class Patterns
    {
        public static readonly Dictionary<string, PatternRepresentation> PATTERNS; // Dictionary where the key is the name of the pattern and the value the pattern
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
                String[] text = System.IO.File.ReadAllText(filePath).Split(nl, StringSplitOptions.None);
                if (text.Length > 0)
                {
                    string key = Path.GetFileNameWithoutExtension(filePath);
                    PatternRepresentation value = new PatternRepresentation(text.Length, text[0].Length);

                    bool isPatternValid = true;
                    for (int i = 0; i < text.Length && isPatternValid; ++i)
                    {
                        isPatternValid = text[i].Length == text[0].Length;
                        for (int j = 0; j < text[i].Length && isPatternValid; ++j)
                            isPatternValid = text[i][j].ToString() == PatternRepresentation.ALIVE || text[i][j].ToString() == PatternRepresentation.EMPTY;
                    }

                    if(isPatternValid)
                    {
                        for (int i = 0; i < text.Length; ++i)
                            for (int j = 0; j < text[i].Length; ++j)
                                if (text[i][j].ToString() == PatternRepresentation.ALIVE)
                                    value[i, j] = PatternRepresentation.ALIVE;
                        PATTERNS.Add(key, value);
                    }
                }
            }
        }
    }
}
