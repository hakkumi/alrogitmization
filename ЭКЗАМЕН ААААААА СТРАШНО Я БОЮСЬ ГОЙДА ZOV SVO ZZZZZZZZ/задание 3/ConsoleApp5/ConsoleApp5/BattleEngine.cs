using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    using System;
    using System.Collections.Generic;

    public static class BattleEngine
    {
        private static Random rnd = new Random();

        public static Choice GetRandomChoice()
        {
            int x = rnd.Next(5);
            switch (x)
            {
                case 0: return new Rock();
                case 1: return new Paper();
                case 2: return new Scissors();
                case 3: return new Lizard();
                default: return new Spock();
            }
        }

        public static void SimulateBattles(int count)
        {
            int rockWins = 0, paperWins = 0, scissorsWins = 0, lizardWins = 0, spockWins = 0, draws = 0;

            for (int i = 0; i < count; i++)
            {
                Choice a = GetRandomChoice();
                Choice b = GetRandomChoice();

                string result = a.Battle(b);

                if (result == "Draw")
                    draws++;
                else if (result.Contains("Rock")) rockWins++;
                else if (result.Contains("Paper")) paperWins++;
                else if (result.Contains("Scissors")) scissorsWins++;
                else if (result.Contains("Lizard")) lizardWins++;
                else if (result.Contains("Spock")) spockWins++;
            }

            Console.WriteLine("RESULTS:");
            Console.WriteLine("Rock wins: " + rockWins);
            Console.WriteLine("Paper wins: " + paperWins);
            Console.WriteLine("Scissors wins: " + scissorsWins);
            Console.WriteLine("Lizard wins: " + lizardWins);
            Console.WriteLine("Spock wins: " + spockWins);
            Console.WriteLine("Draws: " + draws);
        }
    }

}
