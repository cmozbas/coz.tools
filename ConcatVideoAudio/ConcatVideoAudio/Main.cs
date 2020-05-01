using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConcatVideoAudio
{
    public class Main
    {
        public Main()
        {
            string key = "y";
            do
            {
                Process();

                Console.WriteLine("Do you want to do it again? y/N");
                key = Console.ReadLine();
            } while (key == "y");
        }

        private void Process()
        {
            var listVideos = new List<string>();
            var listFileNames = new List<string>();

            Console.WriteLine("Enter Video Path. Copy folder where videos and file are");
            // C:\Users\CemÖzbas\dwhelper\Building Your First Power BI Report
            var videoPath = Console.ReadLine();
            var outputVideoPath = videoPath + "\\converted";
            if (!Directory.Exists(outputVideoPath))
            {
                Directory.CreateDirectory(outputVideoPath);
            }


            foreach (var file in Directory.EnumerateFiles(videoPath))
            {
                if (file.Contains(".mp4"))
                {
                    listVideos.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            // Search clip video and take name before
            string videoName = "";
            string videoAfterName = "Clip Watched";
            foreach (var line in File.ReadLines(videoPath + "\\info.txt"))
            {
                if (string.Compare(videoAfterName, line, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    listFileNames.Add(videoName);
                }
                else
                {
                    // keep the old one
                    videoName = line;
                }
            }

            for (int i = 0; i < listVideos.Count; i++)
            {
                var filePlace = int.Parse(listVideos[i]) - 1;
                // special caraters not welcomed by ffmpeg
                var fileName = Regex.Replace(listFileNames[filePlace], "[:#@.,'\"]", "");
                ProcessConcat(videoPath, outputVideoPath, listVideos[i], fileName);
            }
        }

        /// <summary>
        /// Contact "video path\001 - Real video name"
        /// </summary>
        /// <param name="videoPath">video path</param>
        /// <param name="name">001</param>
        /// <param name="videoName">Real video name</param>
        private void ProcessConcat(string videoPath, string outputVideoPath, string name, string videoName)
        {
            string convertVideoName = videoPath + "\\" + name + ".mp4";
            string convertaudioName = videoPath + "\\" + name + ".aac";
            string outputname = outputVideoPath + "\\" + name + " - " + videoName + ".mp4";

            // replace file
            if (File.Exists(outputname))
            {
                File.Delete(outputname);
            }

            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "C:\\ffmpeg\\bin\\ffmpeg.exe";
                startInfo.Arguments = $"-i \"{convertVideoName}\" -i \"{convertaudioName}\" -c:v copy -c:a aac \"{outputname}\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(50 * 1000);

                process.Close();
                process.Dispose();
            }
        }
    }
}
