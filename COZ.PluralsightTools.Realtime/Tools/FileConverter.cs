using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace COZ.PluralsightTools.Realtime
{
    public class FileConverter
    {
        CancellationToken cancellationTokenSource;
        string workingPlace;
        string doneFolder = "\\done";
        string oldFilesFolder = "\\old";
        InfoFileReader infoFileReader;
        List<string> fileNameList;

        public FileConverter(
            CancellationToken cancellationTokenSource,
            string folderToListen,
            InfoFileReader infoFileReader)
        {
            this.cancellationTokenSource = cancellationTokenSource;
            this.workingPlace = folderToListen;
            this.infoFileReader = infoFileReader;

            doneFolder = workingPlace + doneFolder;
            Directory.CreateDirectory(doneFolder);
            oldFilesFolder = workingPlace + oldFilesFolder;
            Directory.CreateDirectory(oldFilesFolder);

            // Get file list
            fileNameList = infoFileReader.GetFileList();
        }

        public void Start()
        {
            do
            {
                Thread.Sleep(1000);

                foreach (var videoFile in Directory.EnumerateFiles(workingPlace))
                {
                    if (videoFile.EndsWith(".mp4") || videoFile.EndsWith(".ts"))
                    {
                        var videoFileWithoutExtension = Path.GetFileNameWithoutExtension(videoFile);
                        foreach (var audiofile in Directory.EnumerateFiles(workingPlace))
                        {
                            var audioFileWithoutExtension = Path.GetFileNameWithoutExtension(audiofile); ;
                            if (audiofile.EndsWith(".aac")
                                &&  string.Equals(videoFileWithoutExtension, audioFileWithoutExtension, StringComparison.InvariantCultureIgnoreCase))
                            {
                                // Get file name
                                int fileNumber = int.Parse(videoFileWithoutExtension);
                                string outputfileName = videoFileWithoutExtension + " - " + fileNameList[fileNumber];
                                string outputFullFileName = doneFolder + "\\" + outputfileName + ".mp4";
                                Console.WriteLine("Convert to: " + outputFullFileName);

                                ConvertMp4(videoFile, audiofile, outputFullFileName, outputfileName);
                            }
                        }
                    }
                }

            } while (!cancellationTokenSource.IsCancellationRequested);
        }

        private void ConvertMp4(string videoName, string audioName, string outputName, string fileName)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "C:\\ffmpeg\\bin\\ffmpeg.exe";
                startInfo.Arguments = $"-i \"{videoName}\" -i \"{audioName}\" -c:v copy -c:a aac \"{outputName}\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(50 * 1000);

                process.Close();
                process.Dispose();
            }

            Console.WriteLine("Converted, move files");
            File.Move(videoName, oldFilesFolder + "\\" + fileName + Path.GetExtension(videoName));
            File.Move(audioName, oldFilesFolder + "\\" + fileName + Path.GetExtension(audioName));
        }
    }
}
