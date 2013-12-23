using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    
    class Program
    {
        private const int X = 15;
        private const int Y = 15;

        static void Main(string[] args)
        {
            GameEngine gameEngine = new GameEngine(X, Y);
            
            while (true)
            {
                gameEngine.render();
                Console.ReadKey();
                Console.Clear();
                gameEngine.generateNextStep();
            }
        }
    }
}
