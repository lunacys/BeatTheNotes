using System;
using System.IO;
using Newtonsoft.Json;

namespace Jackhammer
{
    public static class BeatmapWriter
    {
        /// <summary>
        /// Write a beatmap to a '.jmap' file using JSON format
        /// </summary>
        /// <param name="bm">Beatmap that needs to be saved</param>
        /// <param name="mapname">Map name</param>
        public static void WriteToFile(Beatmap bm, string mapname)
        {
            // TODO: Encapsulate "Maps", maybe got to create a ContentMap class
            string file = Path.Combine("Maps", mapname, mapname + ".jmap");

            BeatmapSettings bms = new BeatmapSettings(bm.SettingsGeneral, bm.SettingsEditor,
                bm.SettingsMetadata, bm.SettingsDifficulty);

            string str = JsonConvert.SerializeObject(bms, Formatting.Indented);

            if (!Directory.Exists(Path.Combine("Maps", mapname)))
                Directory.CreateDirectory(Path.Combine("Maps", mapname));

            using (StreamWriter sw = new StreamWriter(file))
                sw.WriteLine(str);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname,
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
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", mapname,
                bms.HitObjectsFilename);
            // Write all the hit objects
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var hitObject in bm.HitObjects)
                {
                    sw.WriteLine(hitObject.EndPosition == hitObject.Position
                        ? $"{hitObject.Line} {hitObject.Position}"
                        : $"{hitObject.Line} {hitObject.Position} {hitObject.EndPosition}");
                }
            }
        }
    }
}

