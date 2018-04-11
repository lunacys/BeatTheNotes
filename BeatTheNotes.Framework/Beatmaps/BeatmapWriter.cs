using System;
using System.IO;
using BeatTheNotes.Framework.Objects;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Beatmaps
{
    public static class BeatmapWriter
    {
        /// <summary>
        /// Write a beatmap to a file using JSON format
        /// </summary>
        /// <param name="bm">Beatmap that needs to be saved</param>
        /// <param name="mapname">Map name</param>
        public static void WriteToFile(Beatmap bm, string beatmapFolder, string mapname)
        {
            // TODO: Encapsulate "Maps", maybe got to create a ContentMap class
            string file = Path.Combine(beatmapFolder, mapname, mapname + BeatmapProcessorSettings.BeatmapFileExtension);

            BeatmapSettings bms = bm.Settings;

            string str = JsonConvert.SerializeObject(bms, Formatting.Indented);

            if (!Directory.Exists(Path.Combine(beatmapFolder, mapname)))
                Directory.CreateDirectory(Path.Combine(beatmapFolder, mapname));

            using (StreamWriter sw = new StreamWriter(file))
                sw.WriteLine(str);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, beatmapFolder, mapname,
                bms.TimingPointsFilename);
            // Write all the timing points
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var timingPoint in bm.TimingPoints)
                {
                    sw.WriteLine(
                        $"{timingPoint.Position} {timingPoint.MsPerBeat} {timingPoint.TimeSignature} {timingPoint.HitSoundVolume} {timingPoint.IsResetMetronome}");
                }

            }
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, beatmapFolder, mapname,
                bms.HitObjectsFilename);
            // Write all the hit objects
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var hitObject in bm.HitObjects)
                {
                    if (hitObject is NoteClick)
                    {
                        var noteClick = hitObject as NoteClick;
                        sw.WriteLine($"{noteClick.Column} {noteClick.Position}");
                    }
                    else if (hitObject is NoteHold)
                    {
                        var noteHold = hitObject as NoteHold;
                        sw.WriteLine($"{noteHold.Column} {noteHold.Position} {noteHold.EndPosition}");
                    }
                    else throw new Exception("Unknown note type");
                }
            }
        }
    }
}

