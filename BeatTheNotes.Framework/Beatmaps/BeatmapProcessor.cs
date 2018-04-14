using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.Settings;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessor
    {
        private BeatmapProcessorSettings _processorSettings;

        private List<BeatmapSettings> _beatmapSettingsList;

        public Beatmap CurrentBeatmap { get; private set; }
        public int BeatmapCount => _beatmapSettingsList.Count;

        public BeatmapProcessor()
            : this(new BeatmapProcessorSettings(".btn", "Maps", "timing_points", "hit_objects"))
        { }

        public BeatmapProcessor(GameSettings gameSettings)
            : this(new BeatmapProcessorSettings(".btn", gameSettings.BeatmapFolder, "timing_points", "hit_objects"))
        { }

        public BeatmapProcessor(BeatmapProcessorSettings processorSettings)
        {
            _processorSettings = processorSettings;
            
            _beatmapSettingsList = new List<BeatmapSettings>();

            string folder = _processorSettings.BeatmapsFolder;
            string fileExt = _processorSettings.BeatmapFileExtension;
            bool isFolderEmpty = true;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            if (Directory.EnumerateFileSystemEntries(folder).Any())
                isFolderEmpty = false;

            if (!isFolderEmpty)
            {
                // TODO: Process database
                /*SQLiteConnection dbConnection = new SQLiteConnection("Data Source=BeatTheNotes.db;Version=3;");
                dbConnection.Open();
                SQLiteCommand dbCmd = dbConnection.CreateCommand();*/

                BeatmapReader bmReader = new BeatmapReader(_processorSettings);

                foreach (var directory in Directory.EnumerateDirectories(folder))
                {
                    var files = Directory.GetFiles(Path.Combine(folder, directory), $"*.{fileExt}");

                    if (files.Length != 0)
                    {
                        foreach (var file in files)
                        {
                            
                        }
                    }
                }
            }
        }
    }
}