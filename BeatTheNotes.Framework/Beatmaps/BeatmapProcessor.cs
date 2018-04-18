using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessor
    {
        public BeatmapProcessorSettings ProcessorSettings { get; private set; }

        private List<BeatmapProcessorContainerEntry> _beatmapSettingsList;

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
            
            _beatmapSettingsList = new List<BeatmapProcessorContainerEntry>();

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

            LogHelper.Log($"BeatmapProcessor: Found {_beatmapSettingsList.Count} beatmaps");
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
                string beatmapFolder = dbDataReader.GetString(0);
                string beatmapFilename = dbDataReader.GetString(1);
                string beatmapName = dbDataReader.GetString(2);
                string beatmapVersion = dbDataReader.GetString(3);

                var bmSettingsGeneral = new BeatmapSettingsGeneral(
                    dbDataReader.GetString(4), 
                    dbDataReader.GetInt32(5),
                    dbDataReader.GetString(6));
                var bmSettingsEditor = new BeatmapSettingsEditor(dbDataReader.GetInt32(7));
                var bmSettingsMetadata = new BeatmapSettingsMetadata(dbDataReader.GetString(8),
                    dbDataReader.GetString(9), dbDataReader.GetString(10), dbDataReader.GetString(11),
                    dbDataReader.GetString(12), dbDataReader.GetInt32(13), dbDataReader.GetInt32(14));
                var bmSettingsDifficutly = new BeatmapSettingsDifficulty(dbDataReader.GetFloat(15),
                    dbDataReader.GetFloat(16), dbDataReader.GetInt32(17));
                var bmSettings = new BeatmapSettings(bmSettingsGeneral, bmSettingsEditor, bmSettingsMetadata, bmSettingsDifficutly);

                var hitObjectCount = dbDataReader.GetInt32(20);
                var bpm = dbDataReader.GetDouble(21);

                var entry = new BeatmapProcessorContainerEntry(bmSettings, beatmapFolder, beatmapFilename, beatmapName,
                    beatmapVersion, hitObjectCount, bpm);

                _beatmapSettingsList.Add(entry);
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
                    [beatmap_version]                           VARCHAR(256) DEFAULT NULL,
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

                        var version = settings.Metadata.Version;
                        var timingPoints = bmReader.ReadTimingPoints(GetBeatmapNameWithoutVersion(Path.GetFileNameWithoutExtension(file)), settings);
                        var hitObjects = bmReader.ReadHitObjects(GetBeatmapNameWithoutVersion(Path.GetFileNameWithoutExtension(file)), settings);

                        var entry = new BeatmapProcessorContainerEntry(settings, directory, Path.GetFileName(file),
                            Path.GetFileNameWithoutExtension(file), version, hitObjects.Count,
                            timingPoints[0].BeatsPerMinute);
                        _beatmapSettingsList.Add(entry);

                        // Insert a new value into the table
                        SQLiteCommand dbCmd = dbConnection.CreateCommand();
                        dbCmd.CommandText =
                                $@"INSERT INTO beatmaps (beatmap_folder, beatmap_filename, beatmap_name, beatmap_version, beatmap_settings_audio_filename, beatmap_settings_preview_time, beatmap_settings_background_filename, beatmap_settings_beat_divisor, beatmap_settings_title, beatmap_settings_artist, beatmap_settings_creator, beatmap_settings_version, beatmap_settings_tags, beatmap_settings_beatmap_id, beatmap_settings_beatmap_set_id, beatmap_settings_hp_drain_rate, beatmap_settings_overall_difficulty, beatmap_settings_key_amount, beatmap_settings_timing_points_filename, beatmap_settings_hit_objects_filename, beatmap_hit_object_count, beatmap_bpm) 
                                VALUES ( 
                                '{directory}', 
                                '{Path.GetFileName(file)}', 
                                '{Path.GetFileNameWithoutExtension(file)}',
                                '{version}',
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
                                {hitObjects.Count},
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