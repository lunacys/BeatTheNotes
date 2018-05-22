using System;
using System.IO;
using System.Threading.Tasks;

namespace BeatTheNotes.Framework.Debug.Logging
{
    public class FileLogger : Logger
    {
        public string FileDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public string FilePath => Path.Combine(FileDir,
            $"log-{DateTime.Now.Month}.{DateTime.Now.Day}.{DateTime.Now.Year}.txt");

        public override void Log(string message)
        {
            lock (LockObject)
            {
                if (!Directory.Exists(FileDir))
                    Directory.CreateDirectory(FileDir);

                using (var sw = new StreamWriter(FilePath, true))
                {
                    sw.WriteLine(message);
                    //sw.Close();
                }
            }
        }

        public override async Task LogAsync(string message)
        {
            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);

            using (var sw = new StreamWriter(FilePath, true))
            {
                await sw.WriteLineAsync(message);
                //sw.Close();
            }
        }
    }
}
