using System;
using System.IO;
using BeatTheNotes.Framework.Objects;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapWriter
    {
        public BeatmapProcessorSettings ProcessorSettings { get; }

        public BeatmapWriter(BeatmapProcessorSettings processorSettings)
        {
            ProcessorSettings = processorSettings;
        }
        /// <summary>
        /// Write a beatmap to a file using JSON format
        /// </summary>
        public void WriteToFile(Beatmap bm, string beatmapName, string beatmapVersion)
        {
            // TODO: Encapsulate "Maps", maybe got to create a ContentMap class
            string file = Path.Combine(ProcessorSettings.BeatmapsFolder, beatmapName, beatmapName + " " + beatmapVersion + ProcessorSettings.BeatmapFileExtension);

            BeatmapSettings bms = bm.Settings;

            string str = JsonConvert.SerializeObject(bms, Formatting.Indented);

            if (!Directory.Exists(Path.Combine(ProcessorSettings.BeatmapsFolder, beatmapName)))
                Directory.CreateDirectory(Path.Combine(ProcessorSettings.BeatmapsFolder, beatmapName));

            using (StreamWriter sw = new StreamWriter(file))
                sw.WriteLine(str);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapName,
                bms.TimingPointsFilename);
            // Write all the timing points
            using (StreamWriter sw = new StreamWriter(Path.Combine(ProcessorSettings.TimingPointsFolder, path + beatmapVersion)))
            {
                foreach (var timingPoint in bm.TimingPoints)
                {
                    sw.WriteLine(
                        $"{timingPoint.Position} {timingPoint.MsPerBeat} {timingPoint.TimeSignature} {timingPoint.HitSoundVolume} {timingPoint.IsResetMetronome}");
                }

            }
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessorSettings.BeatmapsFolder, beatmapName,
                bms.HitObjectsFilename);
            // Write all the hit objects
            using (StreamWriter sw = new StreamWriter(Path.Combine(ProcessorSettings.HitObjectsFolder, path + beatmapVersion)))
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

