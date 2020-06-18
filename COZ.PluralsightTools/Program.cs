

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace COZ.PluralsightTools
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("1 => ConcatVideoAudio (default)");
                Console.WriteLine("2 => RenameFilesWithNumbers");
                Console.WriteLine("3 => ShiftName");
                var choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice) || string.Equals(choice, "1", StringComparison.InvariantCultureIgnoreCase))
                {
                    new ConcatVideoAudio();
                }
                else if (string.Equals(choice, "2", StringComparison.InvariantCultureIgnoreCase))
                {
                    new RenameFilesWithNumbers();
                }
                else if (string.Equals(choice, "3", StringComparison.InvariantCultureIgnoreCase))
                {
                    new ShiftName();
                }

                Console.WriteLine("HOME: restart? (y/N)");
            }
            while (string.Equals("y", Console.ReadLine(), StringComparison.InvariantCultureIgnoreCase));
        }

    }
}

