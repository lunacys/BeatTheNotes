using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.Settings;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessor
    {
        public BeatmapProcessorSettings ProcessorSettings { get; private set; }

        private List<BeatmapSettings> _beatmapSettingsList;

        public int BeatmapCount => _beatmapSettingsList.Count;

        public BeatmapProcessor()
            : this(new BeatmapProcessorSettings(".btn", "Maps", "timing_points", "hit_objects", "BeatTheNotes.db"))
        { }

        public BeatmapProcessor(GameSettings gameSettings)
            : this(new BeatmapProcessorSettings(".btn", gameSettings.BeatmapFolder, "timing_points", "hit_objects", "BeatTheNotes.db"))
        { }

        public BeatmapProcessor(BeatmapProcessorSettings processorSettings)
        {
            ProcessorSettings = processorSettings;
            
            _beatmapSettingsList = new List<BeatmapSettings>();

            string folder = ProcessorSettings.BeatmapsFolder;

            bool isFolderEmpty = true;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            if (Directory.EnumerateFileSystemEntries(folder).Any())
                isFolderEmpty = false;

            if (!isFolderEmpty)
            {
                if (!File.Exists(ProcessorSettings.DatabaseName))
                    InitializeDatabase();
                else 
                    ProcessDatabase();
            }
        }

        private void ProcessDatabase()
        {
            var dbConnection = new SQLiteConnection($"Data Source={ProcessorSettings.DatabaseName};Version=3;");

            dbConnection.Open();

            var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = "SELECT * FROM beatmaps";

            var dbDataReader = dbCommand.ExecuteReader();
            
            while (dbDataReader.Read())
            {
                var text = dbDataReader.GetString(0) + " " + dbDataReader.GetString(1) + " " + dbDataReader.GetString(2) + " " + dbDataReader.GetString(3) + "\n";
                Console.WriteLine($"text is: {text}");
            }

            dbConnection.Close();
        }

        private void InitializeDatabase()
        {
            string folder = ProcessorSettings.BeatmapsFolder;
            string fileExt = ProcessorSettings.BeatmapFileExtension;

            BeatmapReader bmReader = new BeatmapReader(ProcessorSettings);

            SQLiteConnection dbConnection =
                new SQLiteConnection($"Data Source={ProcessorSettings.DatabaseName};Version=3;");
            dbConnection.Open();
            SQLiteCommand dbCreateTableCommand = dbConnection.CreateCommand();
            dbCreateTableCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS
                    [beatmaps] (                  
                    [beatmap_folder]                            VARCHAR(2048) DEFAULT NULL,               
                    [beatmap_filename]                          VARCHAR(256) DEFAULT NULL,
                    [beatmap_name]                              VARCHAR(256) DEFAULT NULL,
                    [beatmap_settings_audio_filename]           VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_preview_time]             INTEGER NOT NULL,
                    [beatmap_settings_background_filename]      VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_beat_divisor]             INTEGER NOT NULL,
                    [beatmap_settings_title]                    VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_artist]                   VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_creator]                  VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_version]                  VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_tags]                     VARCHAR(2048) DEFAULT NULL,
                    [beatmap_settings_beatmap_id]               INTEGER NOT NULL PRIMARY KEY,
                    [beatmap_settings_beatmap_set_id]           INTEGER NOT NULL,
                    [beatmap_settings_hp_drain_rate]            REAL NOT NULL,
                    [beatmap_settings_overall_difficulty]       REAL NOT NULL,
                    [beatmap_settings_key_amount]               INTEGER NOT NULL,
                    [beatmap_settings_timing_points_filename]   VARCHAR(128) DEFAULT NULL,
                    [beatmap_settings_hit_objects_filename]     VARCHAR(128) DEFAULT NULL,
                    [beatmap_hit_object_count]                  INTEGER NOT NULL,
                    [beatmap_bpm]                               REAL NOT NULL)";

            dbCreateTableCommand.ExecuteNonQuery();

            foreach (var directory in Directory.EnumerateDirectories(folder))
            {
                var files = Directory.GetFiles(directory, $"*{fileExt}");

                if (files.Length != 0)
                {
                    foreach (var file in files)
                    {
                        var settings = bmReader.ReadBeatmapSettings(file);
                        
                        var timingPoints = bmReader.ReadTimingPoints(GetBeatmapNameWithoutVersion(Path.GetFileNameWithoutExtension(file)), settings);
                        var hitOjects = bmReader.ReadHitObjects(GetBeatmapNameWithoutVersion(Path.GetFileNameWithoutExtension(file)), settings);

                        _beatmapSettingsList.Add(settings);

                        // Insert a new value into the table
                        SQLiteCommand dbCmd = dbConnection.CreateCommand();
                        dbCmd.CommandText =
                                $@"INSERT INTO beatmaps (beatmap_folder, beatmap_filename, beatmap_name, beatmap_settings_audio_filename, beatmap_settings_preview_time, beatmap_settings_background_filename, beatmap_settings_beat_divisor, beatmap_settings_title, beatmap_settings_artist, beatmap_settings_creator, beatmap_settings_version, beatmap_settings_tags, beatmap_settings_beatmap_id, beatmap_settings_beatmap_set_id, beatmap_settings_hp_drain_rate, beatmap_settings_overall_difficulty, beatmap_settings_key_amount, beatmap_settings_timing_points_filename, beatmap_settings_hit_objects_filename, beatmap_hit_object_count, beatmap_bpm) 
                                VALUES ( 
                                '{directory}', 
                                '{Path.GetFileName(file)}', 
                                '{Path.GetFileNameWithoutExtension(file)}',
                                '{settings.General.AudioFileName}',
                                {settings.General.PreviewTime},
                                '{settings.General.BackgroundFileName}',
                                {settings.Editor.BeatDivisor},
                                '{settings.Metadata.Title}',
                                '{settings.Metadata.Artist}',
                                '{settings.Metadata.Creator}',
                                '{settings.Metadata.Version}',
                                '{settings.Metadata.Tags}',
                                {settings.Metadata.BeatmapId},
                                {settings.Metadata.BeatmapSetId},
                                {settings.Difficulty.HpDrainRate:F1},
                                {settings.Difficulty.OverallDifficutly:F1},
                                {settings.Difficulty.KeyAmount},
                                '{settings.TimingPointsFilename}',
                                '{settings.HitObjectsFilename}',
                                {hitOjects.Count},
                                {timingPoints[0].BeatsPerMinute:F2});";
                        dbCmd.ExecuteNonQuery();
                    }
                }
            }

            dbConnection.Close();
        }

        private string GetBeatmapNameWithoutVersion(string beatmapName)
        {
            var index = beatmapName.LastIndexOf("[", StringComparison.Ordinal);
            return beatmapName.Substring(0, index - 1);
        }
    }
}