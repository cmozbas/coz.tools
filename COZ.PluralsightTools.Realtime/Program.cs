using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace COZ.PluralsightTools.Realtime
{
    /// <summary>
    /// For MP2T
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Where we copy the videos for the first time
            // And also the convert operations
            string workingPlace = "\\video";

            // Take the folder path
            string folderToListen;
            Console.WriteLine("Listen folder: ");
            folderToListen = Console.ReadLine();
            workingPlace = folderToListen + workingPlace;

            // Create working folder
            Directory.CreateDirectory(workingPlace);

            int fileNumber = 1;
            Console.WriteLine("Enter File Number: [1-999]");
            fileNumber = int.Parse(Console.ReadLine());

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // Info file reader
            var infoFileReader = new InfoFileReader(workingPlace);
            infoFileReader.CopyInfoFileToWorkingPlace(folderToListen);

            // Start file listener
            var fileMover = new FileMover(cancellationTokenSource.Token, folderToListen, workingPlace, fileNumber);
            Task newFileLIstener = new Task(() => fileMover.Start(), TaskCreationOptions.LongRunning);
            newFileLIstener.Start();

            // Start file converter
            var fileConverter = new FileConverter(cancellationTokenSource.Token, workingPlace, infoFileReader);
            Task newFileConverter = new Task(() => fileConverter.Start(), TaskCreationOptions.LongRunning);
            newFileConverter.Start();

            Console.WriteLine("Press any key to stop ...");
            Console.ReadKey();
        }
    }
}
