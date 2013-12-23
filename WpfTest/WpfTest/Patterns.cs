using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfTest
{
    class Patterns
    {
        public static readonly Dictionary<string, PatternRepresentation> PATTERNS;

        static Patterns()
        {
            PATTERNS = new Dictionary<string, PatternRepresentation>();

            string path = System.Environment.CurrentDirectory;
            path = path.Substring(0, path.LastIndexOf("bin")) + "patterns";
            string[] filePaths = Directory.GetFiles(path, "*.txt");

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
