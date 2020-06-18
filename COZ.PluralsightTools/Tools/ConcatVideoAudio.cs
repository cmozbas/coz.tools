using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COZ.PluralsightTools
{
    public class ConcatVideoAudio
    {
        public string outputVideoPath { get; set; }
        public string infoFilePath { get; set; }

        public ConcatVideoAudio()
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

            // Video Path. Create converted folder if not existe
            // C:\Users\CemÖzbas\dwhelper\Building Your First Power BI Report
            Console.WriteLine("Enter Video Path. Copy folder where videos and file are");
            var videoPath = Console.ReadLine();

            outputVideoPath = videoPath + "\\converted";
            infoFilePath = videoPath + "\\info.txt";

            if (!Directory.Exists(outputVideoPath))
            {
                Directory.CreateDirectory(outputVideoPath);
            }

            // Get all mp4 folders (.aac folders have the same name)
            foreach (var file in Directory.EnumerateFiles(videoPath))
            {
                if (file.Contains(".mp4"))
                {
                    listVideos.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

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
                else if(Regex.IsMatch(line, "\\d+m \\d+s|\\d+s|\\d+m", RegexOptions.IgnoreCase))
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

            Console.WriteLine("Number of videos: " + listVideos.Count);
            foreach (var video in listVideos)
            {
                Console.WriteLine(video);
            }

            Console.WriteLine("Number of file: " + listFileNames.Count);
            for (int i = 0; i < listFileNames.Count; i++)
            {
                Console.WriteLine("" + (i+1) + ": " + listFileNames[i]);
            }

            Console.WriteLine("Do you want to continue? (y/N)");
            var isContinue = Console.ReadLine();
            if(string.Compare(isContinue, "y", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return;
            }

            for (int i = 0; i < listVideos.Count; i++)
            {
                try
                {
                    var filePlace = int.Parse(listVideos[i]) - 1;
                    // Special caraters not welcomed by ffmpeg
                    // In the file info plural add some special caracters, we need to delete them
                    var fileName = Regex.Replace(listFileNames[filePlace], "[:#@'\"/(,.)]", "");

                    try
                    {
                        ProcessConcat(videoPath, listVideos[i], fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR _" + fileName + "_: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR count  " + i + ": " + ex.Message);
                }
            }

            // Copy info file
            File.Copy(infoFilePath, outputVideoPath + "\\info.txt");
        }

        /// <summary>
        /// Contact "video path\001 - Real video name"
        /// </summary>
        /// <param name="videoPath">video path</param>
        /// <param name="name">001</param>
        /// <param name="videoName">Real video name</param>
        private void ProcessConcat(string videoPath, string name, string videoName)
        {
            string convertVideoName = videoPath + "\\" + name + ".mp4";
            string convertaudioName = videoPath + "\\" + name + ".aac";
            string outputname = outputVideoPath + "\\" + name + " - " + videoName + ".mp4";

            // replace file
            if (File.Exists(outputname))
            {
                File.Delete(outputname);
            }

            // If mp4 exist but not the aac then the video integrated the sound
            // Just need to place it without merging
            if (File.Exists(convertVideoName) && !File.Exists(convertaudioName))
            {
                File.Copy(convertVideoName, outputname);
            }
            else
            {
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
}
