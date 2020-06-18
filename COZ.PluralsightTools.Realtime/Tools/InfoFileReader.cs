using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace COZ.PluralsightTools.Realtime
{
    public class InfoFileReader
    {
        CancellationToken cancellationTokenSource;
        string workingPlace;
        string infoFilePath;

        public InfoFileReader(string workingPlace)
        {
            this.workingPlace = workingPlace;

            infoFilePath = workingPlace + "\\info.txt";
        }

        public List<string> GetFileList()
        {
            var listFileNames = new List<string>();

            // Search clip video and take name before
            // Get the list of video names. File has to have the same order as videos
            //
            // Introduction to Integration Services
            // Clip Watched                         => After the name, follow by time
            // 1m 46s
            //
            string title = "";
            bool isModuleTitle = true;
            int count = 1;
            foreach (var line in File.ReadLines(infoFilePath))
            {
                // Un espace = on change de module
                if (string.IsNullOrWhiteSpace(line))
                {
                    isModuleTitle = true;
                }
                else if (Regex.IsMatch(line, "\\d+m \\d+s|\\d+s|\\d+m", RegexOptions.IgnoreCase))
                {
                    if (isModuleTitle)
                    {
                        // Pass the module title and the next will be chapters
                        isModuleTitle = false;
                        count = 1;
                        continue;
                    }
                    else
                    {
                        listFileNames.Add(title);
                    }

                    count = 1;
                }
                else
                {
                    // If line is not a minute then it is the title but avoid "Clip Watched"
                    // Take only the first title which is just after a minute
                    if (count == 1)
                    {
                        title = line;
                    }
                    count++;
                }
            }

            return listFileNames;
        }

        public void CopyInfoFileToWorkingPlace(string folderToListen)
        {
            File.Copy(folderToListen + "\\info.txt", workingPlace + "\\info.txt", true);
        }
    }
}
