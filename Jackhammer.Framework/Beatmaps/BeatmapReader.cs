using System;
using System.Collections.Generic;
using System.IO;
using Jackhammer.Framework.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jackhammer.Framework.Beatmaps
{
    public static class BeatmapReader
    {
        [Obsolete("Use JSON format instead")]
        public static Beatmap LoadFromFileOld(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("Could not find the map file");

            using (StreamReader stream = new StreamReader(filename))
            {
                BeatmapSettingsGeneral general = new BeatmapSettingsGeneral("null", 0, "null");
                BeatmapSettingsEditor editor = new BeatmapSettingsEditor(4);
                BeatmapSettingsMetadata metadata = new BeatmapSettingsMetadata("null", "null", "null", "null", "null", -1, -1);
                BeatmapSettingsDifficulty difficulty = new BeatmapSettingsDifficulty(1, 1, 4);

                List<TimingPoint> timingPoints = new List<TimingPoint>();
                List<HitObject> hitObjects = new List<HitObject>();

                string GetVal(string key)
                {
                    string s = stream.ReadLine();
                    if (s != null && s.LastIndexOf(key, StringComparison.Ordinal) == 0)
                    {
                        return s.Substring(key.Length);
                    }
                    throw new FileLoadException($"Could not find an option '{key}'");
                }

                while (!stream.EndOfStream)
                {
                    string s = stream.ReadLine();
                    if (s == "[General]")
                    {
                        general = new BeatmapSettingsGeneral(GetVal("AudioFilename:"),
                            int.Parse(GetVal("PreviewTime:")),
                            GetVal("BackgroundFilename:"));
                    }
                    s = stream.ReadLine();
                    if (s == "[Editor]")
                    {
                        editor = new BeatmapSettingsEditor(int.Parse(GetVal("BeatDivisor:")));
                    }
                    s = stream.ReadLine();
                    if (s == "[Metadata]")
                    {
                        metadata = new BeatmapSettingsMetadata(
                            GetVal("Title:"),
                            GetVal("Artist:"),
                            GetVal("Creator:"),
                            GetVal("Version:"),
                            GetVal("Tags:"),
                            int.Parse(GetVal("BeatmapID:")),
                            int.Parse(GetVal("BeatmapSetID:"))
                            );
                    }
                    s = stream.ReadLine();
                    if (s == "[Difficulty]")
                    {
                        difficulty = new BeatmapSettingsDifficulty(
                            float.Parse(GetVal("HPDrainRate:")),
                            float.Parse(GetVal("OverallDifficulty:")),
                            int.Parse(GetVal("KeyAmount:"))
                            );
                    }
                    s = stream.ReadLine();
                    if (s == "[TimingPoints]")
                    {
                        Console.WriteLine("in timing points");
                        s = stream.ReadLine();

                        while (s != "[HitObjects]")
                        {
                            if (s != null)
                            {
                                Console.WriteLine("in timing points");
                                string[] tokens = s.Split(' ');
                                TimingPoint tp = new TimingPoint(
                                    int.Parse(tokens[0]),
                                    double.Parse(tokens[1]),
                                    int.Parse(tokens[2]),
                                    int.Parse(tokens[3]),
                                    bool.Parse(tokens[4]));
                                timingPoints.Add(tp);
                            }

                            s = stream.ReadLine();
                        }
                    }

                    if (s == "[HitObjects]")
                    {
                        while (!stream.EndOfStream)
                        {
                            s = stream.ReadLine();

                            if (s != null)
                            {
                                string[] tokens = s.Split(' ');

                                HitObject ho;

                                foreach (var token in tokens)
                                {
                                    Console.WriteLine(token);
                                }

                                if (tokens.Length == 3)
                                {
                                    ho = new HitObject(
                                        int.Parse(tokens[0]),
                                        int.Parse(tokens[1]),
                                        int.Parse(tokens[2])
                                    );
                                }
                                else
                                {
                                    ho = new HitObject(
                                        int.Parse(tokens[0]),
                                        int.Parse(tokens[1]));
                                }

                                hitObjects.Add(ho);
                            }
                        }
                    }
                }

                Beatmap bm = new Beatmap(
                    new BeatmapSettings(general, editor, metadata, difficulty), 
                    timingPoints,
                    hitObjects
                );

                return bm;
            }
        }

        /// <summary>
        /// Load Beatmap from a JSON '.jmap' file with file path and filename specified
        /// </summary>
        /// <returns>Beatmap</returns>
        public static Beatmap LoadFromFile(string mapname)
        {
            LogHelper.Log($"BeatmapReader: Load Beatmap from file '{mapname}'");

            // TODO: Encapsulate "Maps"
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname, mapname + ".jmap");

            if (!File.Exists(file))
                throw new FileNotFoundException("Beatmap file not found");

            string str;
            using (StreamReader sr = new StreamReader(file))
                str = sr.ReadToEnd();


            LogHelper.Log($"BeatmapReader: Parsing Beatmap '{mapname}'");
            
            JObject obj = JObject.Parse(str);

            BeatmapSettings settings =
                JsonConvert.DeserializeObject<BeatmapSettings>(obj["Settings"].ToString());

            // TODO: Replace it when added game mode support
            if (settings.GameModeName != "Standard")
                throw new FileLoadException("Non-supported game mode");

            if (string.IsNullOrEmpty(settings.HitObjectsFilename))
                throw new FileLoadException("HitObjectsFilename is null");
            if (string.IsNullOrEmpty(settings.TimingPointsFilename))
                throw new FileLoadException("TimingPointsFilename is null");
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname, settings.HitObjectsFilename)))
                throw new FileNotFoundException("HitObjects file not found");
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname, settings.TimingPointsFilename)))
                throw new FileNotFoundException("TimingPoints file not found");

            List<TimingPoint> timingPoints = new List<TimingPoint>();
            List<HitObject> hitObjects = new List<HitObject>();

            LogHelper.Log($"BeatmapReader: Reading Beatmap Timing Points '{mapname}'");

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname,
                settings.TimingPointsFilename);
            // Reading TimingPoints file, which contains all the timing points used in the beatmap
            using (StreamReader sr = new StreamReader(path))
            {
                LogHelper.Log($"BeatmapReader: Found Beatmap Timing Points file '{settings.TimingPointsFilename}'");
                string full = sr.ReadToEnd();
                string[] lines = full.Split('\n');
                foreach (var line in lines)
                {
                    // If the line is empty or contans only whitespaces, skip it
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line, remove all whitespaces
                    string[] tokens = Array.ConvertAll(line.Split(' '), p => p.Trim());

                    // First is TP position
                    // Second is Ms per beat
                    // Third is time signarure (n/4, where n is the number)
                    // Fourth is hitsound volume
                    // Fifth determines if metronome must be reset on the start of the timing point
                    TimingPoint tp = new TimingPoint(
                        int.Parse(tokens[0]),
                        double.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        bool.Parse(tokens[4]));

                    timingPoints.Add(tp);
                }
            }
            LogHelper.Log($"BeatmapReader: Sucessfully Read Beatmap Timing Points. Total Timing Point count: {timingPoints.Count}");

            LogHelper.Log($"BeatmapReader: Reading Beatmap Hit Objects '{mapname}'");
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname,
                settings.HitObjectsFilename);
            // Reading HitObjects file, which contans all the objects used in the beatmap
            // HitObjects file uses this format: "Line Position" for a Click, and "Line Position EndPosition" for a Hold
            using (StreamReader sr = new StreamReader(path))
            {
                LogHelper.Log($"BeatmapReader: Found Beatmap Hit Objects file '{settings.HitObjectsFilename}'");

                string full = sr.ReadToEnd();
                string[] lines = full.Split('\n');
                foreach (var line in lines)
                {
                    // If the line is empty or contans only whitespaces, skip it
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line, remove all whitespaces
                    string[] tokens = Array.ConvertAll(line.Split(' '), p => p.Trim());

                    // If length is 3, the object is a 'Hold', else it's 'Click'
                    var ho = ((tokens.Length == 3)
                        ? (new HitObject(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2])))
                        : (new HitObject(int.Parse(tokens[0]), int.Parse(tokens[1])))
                    );

                    hitObjects.Add(ho);
                }
            }
            LogHelper.Log($"BeatmapReader: Sucessfully Read Beatmap Hit Objects. Total Hit Objects count: {hitObjects.Count}");

            Beatmap bm = new Beatmap(settings, timingPoints, hitObjects);

            LogHelper.Log($"BeatmapReader: Sucessfully Read Beatmap '{mapname}'");

            return bm;
        }
    }
}
