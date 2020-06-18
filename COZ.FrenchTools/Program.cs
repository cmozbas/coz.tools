using System;
using System.Collections.Generic;

namespace COZ.FrenchTools
{
    class Program
    {
        static void Main(string[] args)
        {
            int countPronoms = 0; //9
            int countTemps = 0; //4
            do
            {
                var pronoms = new List<string> { "je", "tu", "il (même que 'on') / elle(s'il y a le 'se')", "nous", "vous", "ils /elles(si il (s'il) y a le 'se')" };

                var temps = new List<string> 
                { 
                    "present simple", 
                    "present simple négatif", 
                    "passé composé", 
                    "passé composé negatif" 
                };

                Console.WriteLine("=> Ali, tu conjugues le prochain verbe avec le pronom: " + 
                    pronoms[countPronoms].ToUpper() + 
                    ", le temps: " + 
                    temps[countTemps].ToUpper());

                countPronoms++;

                if (countPronoms > 5)
                {
                    countPronoms = 0;
                    countTemps++;
                }

                if (countTemps > 3)
                    countTemps = 0;

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
