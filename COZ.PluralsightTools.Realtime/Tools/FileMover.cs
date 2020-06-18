using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace COZ.PluralsightTools.Realtime
{
    public class FileMover
    {
        CancellationToken cancellationTokenSource;
        string folderToListen;
        string workingPlace;
        int fileNumber;

        public FileMover(
            CancellationToken cancellationTokenSource, 
            string folderToListen,
            string workingPlace,
            int fileNumber)
        {
            this.cancellationTokenSource = cancellationTokenSource;
            this.folderToListen = folderToListen;
            this.workingPlace = workingPlace;
            this.fileNumber = fileNumber;
        }

        /// <summary>
        /// First put the aac file
        /// Then put the video ts or mp4 file
        /// </summary>
        public void Start()
        {
            bool isWaitingVideoFile = true;
            string mp4Extension = ".mp4";
            string aacExtension = ".aac";
            string tsExtension = ".ts";

            do
            {
                Thread.Sleep(1000);

                foreach (var file in Directory.EnumerateFiles(folderToListen))
                {
                    string fileName = string.Empty;

                    if (fileNumber.ToString().Length == 1)
                    {
                        fileName = "00" + fileNumber;
                    }
                    else if (fileNumber.ToString().Length == 2)
                    {
                        fileName = "0" + fileNumber;
                    }
                    else if (fileNumber.ToString().Length == 3)
                    {
                        fileName = "" + fileNumber;
                    }
 
                    fileName = workingPlace + "\\" + fileName;

                    if (file.EndsWith(aacExtension))
                    {
                        Console.WriteLine("Move file: " + fileName + aacExtension);
                        File.Move(file, fileName + aacExtension);
                        isWaitingVideoFile = true;
                    }
                    if (file.EndsWith(tsExtension))
                    {
                        Console.WriteLine("Move file: " + fileName + tsExtension);
                        File.Move(file, fileName + tsExtension);
                        isWaitingVideoFile = false;
                    }
                    if (file.EndsWith(mp4Extension))
                    {
                        Console.WriteLine("Move file: " + fileName + mp4Extension);
                        File.Move(file, fileName + mp4Extension);
                        isWaitingVideoFile = false;
                    }
                }

                if (!isWaitingVideoFile)
                {
                    fileNumber++;
                }

            } while (!cancellationTokenSource.IsCancellationRequested);
        }
    }
}
