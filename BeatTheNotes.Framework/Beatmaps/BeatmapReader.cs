using System;
using System.IO;
using BeatTheNotes.Framework.Audio;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Objects;
using BeatTheNotes.Framework.TimingPoints;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapReader
    {
        public BeatmapProcessorSettings ProcessorSettings { get; }

        public BeatmapReader(BeatmapProcessorSettings processorSettings)
        {
            ProcessorSettings = processorSettings;
        }

        public BeatmapSettings ReadBeatmapSettings(string beatmapSettingsFilename)
        {
            string file = beatmapSettingsFilename;

            if (!File.Exists(file))
                throw new FileNotFoundException("Beatmap file not found");

            string str;

            using (StreamReader sr = new StreamReader(file))
                str = sr.ReadToEnd();
            
            LogHelper.Log($"BeatmapReader: Parsing Beatmap '{beatmapSettingsFilename}'");

            JObject obj = JObject.Parse(str);

            BeatmapSettings settings =
                JsonConvert.DeserializeObject<BeatmapSettings>(obj["Settings"].ToString());

            Console.WriteLine(settings.General.BackgroundFileName);

            return settings;
        }

        public TimingPointContainer ReadTimingPoints(string beatmapName, BeatmapSettings beatmapSettings)
        {
            TimingPointContainer timingPointContainer = new TimingPointContainer();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapName,
                ProcessorSettings.TimingPointsFolder, beatmapSettings.TimingPointsFilename + "_" + beatmapSettings.Metadata.Version);

            Console.WriteLine(path);

            LogHelper.Log($"BeatmapReader: Reading Beatmap Timing Points '{beatmapName}'");

            // Reading TimingPoints file, which contains all the timing points used in the beatmap
            using (StreamReader sr = new StreamReader(path))
            {
                LogHelper.Log($"BeatmapReader: Found Beatmap Timing Points file '{beatmapSettings.TimingPointsFilename}'");
                string full = sr.ReadToEnd();
                string[] lines = full.Split('\n');
                foreach (var line in lines)
                {
                    // If the line is empty or contains only whitespaces, skip it
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line, remove all whitespaces
                    string[] tokens = Array.ConvertAll(line.Split(' '), p => p.Trim());

                    // First is TP position
                    // Second is Ms per beat
                    // Third is time signature (n/4, where n is the number)
                    // Fourth is hitsound volume
                    // Fifth determines if metronome must be reset on the start of the timing point
                    TimingPoint tp = new TimingPoint(
                        int.Parse(tokens[0]),
                        double.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        bool.Parse(tokens[4]));

                    timingPointContainer.Add(tp);
                }
            }
            LogHelper.Log($"BeatmapReader: Successfully Read Beatmap Timing Points. Total Timing Point count: {timingPointContainer.Count}");

            return timingPointContainer;
        }

        public HitObjectContainer ReadHitObjects(string beatmapName, BeatmapSettings beatmapSettings)
        {
            HitObjectContainer hitObjectContainer = new HitObjectContainer(beatmapSettings.Difficulty.KeyAmount);

            LogHelper.Log($"BeatmapReader: Reading Beatmap Hit Objects '{beatmapName}'");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapName,
                ProcessorSettings.HitObjectsFolder, beatmapSettings.HitObjectsFilename + "_" + beatmapSettings.Metadata.Version);

            if (!File.Exists(path))
                throw new FileNotFoundException("hitobjects file not found");
            
            Console.WriteLine(path);

            // Reading HitObjects file, which contains all the objects used in the beatmap
            // HitObjects file uses this format: "Column Position" for a Click, and "Column Position EndPosition" for a Hold
            using (StreamReader sr = new StreamReader(path))
            {
                LogHelper.Log($"BeatmapReader: Found Beatmap Hit Objects file '{beatmapSettings.HitObjectsFilename}'");

                string full = sr.ReadToEnd();
                string[] lines = full.Split('\n');
                foreach (var line in lines)
                {
                    // If the line is empty or contains only whitespaces, skip it
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line, remove all whitespaces
                    string[] tokens = Array.ConvertAll(line.Split(' '), p => p.Trim());

                    // If length is 3, the object is a 'Hold', else it's 'Click'
                    HitObject ho;
                    if (tokens.Length == 2)
                        ho = new NoteClick(int.Parse(tokens[0]), int.Parse(tokens[1]));
                    else if (tokens.Length == 3)
                        ho = new NoteHold(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
                    else throw new Exception("Unknown note type");

                    hitObjectContainer.Add(ho);
                }
            }
            LogHelper.Log($"BeatmapReader: Successfully Read Beatmap Hit Objects. Total Hit Objects count: {hitObjectContainer.Count}");

            return hitObjectContainer;
        }

        public Texture2D LoadBeatmapBackgroundTexture(GraphicsDevice graphicsDevice, BeatmapSettings settings, string beatmapName)
        {
            Texture2D background;

            Console.WriteLine($"Loading BG: {settings.General.BackgroundFileName}, ID: {settings.Metadata.BeatmapId}");
            using (FileStream fs =
                new FileStream(
                    Path.Combine(ProcessorSettings.BeatmapsFolder, beatmapName, settings.General.BackgroundFileName),
                    FileMode.Open))
            {
                
                background = Texture2D.FromStream(graphicsDevice, fs);
            }

            return background;
        }

        public Music LoadBeatmapMusicTrack(BeatmapSettings settings, string beatmapName)
        {
            return new Music(Path.Combine(ProcessorSettings.BeatmapsFolder, beatmapName, settings.General.AudioFileName));
        }

        /// <summary>
        /// Load Beatmap from a JSON file with file path and filename specified
        /// </summary>
        /// <returns>Beatmap</returns>
        public Beatmap ReadBeatmap(GraphicsDevice graphicsDevice, string beatmapName, string versionName)
        {
            LogHelper.Log($"BeatmapReader: Load Beatmap from file '{beatmapName}'");

            var absPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder,
                beatmapName, beatmapName + " [" + versionName + "]" + ProcessorSettings.BeatmapFileExtension);

            BeatmapSettings settings = ReadBeatmapSettings(absPath);

            if (string.IsNullOrEmpty(settings.HitObjectsFilename))
                throw new FileLoadException("HitObjectsFilename is null");
            if (string.IsNullOrEmpty(settings.TimingPointsFilename))
                throw new FileLoadException("TimingPointsFilename is null");
            //if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapSettingsFilename, settings.HitObjectsFilename)))
            //    throw new FileNotFoundException("HitObjects file not found");
            //if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapSettingsFilename, settings.TimingPointsFilename)))
            //    throw new FileNotFoundException("TimingPoints file not found");

            LogHelper.Log("BeatmapReader: Sucessfully Read Beatmap Settings");

            var timingPoints = ReadTimingPoints(beatmapName, settings);
            var hitObjects = ReadHitObjects(beatmapName, settings);
            

            LogHelper.Log($"BeatmapReader: Successfully Read Beatmap '{beatmapName}'");

            Beatmap bm = new Beatmap(settings, 
                LoadBeatmapBackgroundTexture(graphicsDevice, settings, beatmapName),
                LoadBeatmapMusicTrack(settings, beatmapName), 
                timingPoints, hitObjects);

            return bm;
        }
    }
}
