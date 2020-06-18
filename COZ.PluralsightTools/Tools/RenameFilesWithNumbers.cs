using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace COZ.PluralsightTools
{
    public class RenameFilesWithNumbers
    {
        private string renameFolderPath { get; set; }
        private string filePath { get; set; }

        public RenameFilesWithNumbers()
        {
            Console.WriteLine("Enter Path");
            filePath = Console.ReadLine();

            Console.WriteLine("Enter start number between 1 - 999");
            var startNumber = Console.ReadLine();

            Process(startNumber, filePath);
        }

        public void Process(string startNumber, string filePath)
        {
            int number = int.Parse(startNumber);
            string newFileName = startNumber;
            var listFile = new List<string>();

            // Get files in order to creation date
            // Show files before renaming
            DirectoryInfo DirInfo = new DirectoryInfo(filePath);
            var files = DirInfo.EnumerateFiles().ToList();
            files.Sort((x, y) => ExtractNumber(x.Name).CompareTo(ExtractNumber(y.Name)));

            foreach (var currentfile in files)
            {
                if (new List<string>() { ".mp4", ".aac" }.Contains(Path.GetExtension(currentfile.FullName)))
                {
                    listFile.Add(Path.GetFileNameWithoutExtension(currentfile.FullName));
                    Console.WriteLine(Path.GetFileNameWithoutExtension(currentfile.FullName));
                }
            }

            // Verify file name before continue
            Console.WriteLine("Do you want to continue? (y/N)");
            if(!string.Equals(Console.ReadLine(), "y", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            // Create rename folder
            if(!Directory.Exists(filePath + "\\renamed"))
            {
                Directory.CreateDirectory(filePath + "\\renamed");
            }

            renameFolderPath = filePath + "\\renamed";

            // Copy all filed with new name
            for (int i = 0; i < listFile.Count; i++)
            {
                var currentfile = listFile[i];

                if (number.ToString().Length == 1)
                {
                    newFileName = "00" + number;
                }
                else if (number.ToString().Length == 2)
                {
                    newFileName = "0" + number;
                }
                else if (number.ToString().Length == 3)
                {
                    newFileName = "" + number;
                }

                // Get mp4 and aac file names. Both doesn't have to exist 
                var oldFileNameMp4 = filePath + "\\" + currentfile + ".mp4";
                var oldFileNameAac = filePath + "\\" + currentfile + ".aac";

                // In case this is the new name
                var newFileNameMp4 = renameFolderPath + "\\" + newFileName + ".mp4";
                var newFileNameAac = renameFolderPath + "\\" + newFileName + ".aac";

                if (File.Exists(oldFileNameMp4))
                {
                    File.Copy(oldFileNameMp4, newFileNameMp4, true);
                }

                // Pass to the next file if also aac present
                if (File.Exists(oldFileNameAac))
                {
                    File.Copy(oldFileNameAac, newFileNameAac, true);
                    i++;
                }

                number++;
            }

            // Copy info file
            File.Copy(filePath + "\\info.txt", filePath + "\\renamed\\info.txt");
        }

        public static double ExtractNumber(string text)
        {
            Match match = Regex.Match(text, @"[0-9]+\.[0-9]|[0-9]+");
            if (match == null)
            {
                return 0.0;
            }

            double value;
            if (!double.TryParse(match.Value, out value))
            {
                return 0.0;
            }

            return value;
        }

    }
}
