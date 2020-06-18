using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COZ.PluralsightTools
{
    /// <summary>
    /// Shift name from last to first
    /// First become last one
    /// </summary>
    public class ShiftName
    {
        private string filePath { get; set; }

        public ShiftName()
        {
            Console.WriteLine("Enter folder path");
            filePath = Console.ReadLine();

            Process();
        }

        private void Process()
        {
            var listFile = new List<string>();

            // Get files in order to creation date
            // Show files before renaming
            DirectoryInfo DirInfo = new DirectoryInfo(filePath);
            var files = DirInfo.EnumerateFiles().OrderBy(f => f.Name).ToList();
            foreach (var currentfile in files)
            {
                if (new List<string>() { ".mp4", ".aac" }.Contains(Path.GetExtension(currentfile.FullName)))
                {
                    listFile.Add(currentfile.FullName);
                    Console.WriteLine(Path.GetFileName(currentfile.FullName));
                }
            }

            // Verify file name before continue
            Console.WriteLine("Do you want to continue? (y/N)");
            if (!string.Equals(Console.ReadLine(), "y", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            string tempName = ".org";
            for (int i = listFile.Count - 1; i >= 0 ; i--)
            {
                // Change 
                string firstFileName;
                string secondFileName;

                if (i == listFile.Count - 1)
                {
                    // The last one is lost
                    firstFileName = listFile[i];
                    secondFileName = listFile[i - 1];

                    File.Move(secondFileName, secondFileName + tempName);
                    File.Move(firstFileName, secondFileName);
                }
                else if (i == 0)
                {
                    // Replace the first one with the last one
                    firstFileName = listFile[i] + tempName;
                    secondFileName = listFile[listFile.Count - 1];

                    File.Move(firstFileName, secondFileName);
                }
                else
                {
                    firstFileName = listFile[i] + tempName;
                    secondFileName = listFile[i - 1];

                    File.Move(secondFileName, secondFileName + tempName); 
                    File.Move(firstFileName, secondFileName);
                }
            }
        }
    }
}
