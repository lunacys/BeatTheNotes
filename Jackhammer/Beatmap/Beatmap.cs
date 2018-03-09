using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackhammer
{
    public class Beatmap
    {
        public BeatmapSettingsGeneral SettingsGeneral { get; }
        public BeatmapSettingsEditor SettingsEditor { get; }
        public BeatmapSettingsMetadata SettingsMetadata { get; }
        public BeatmapSettingsDifficulty SettingsDifficulty { get; }

        [JsonIgnore]
        public List<TimingPoint> TimingPoints { get; }
        [JsonIgnore]
        public List<HitObject> HitObjects { get; }


        public Beatmap(
            BeatmapSettingsGeneral general,
            BeatmapSettingsEditor editor,
            BeatmapSettingsMetadata metadata,
            BeatmapSettingsDifficulty difficulty,
            List<TimingPoint> timingPoints,
            List<HitObject> hitObjects)
        {
            SettingsGeneral = general;
            SettingsEditor = editor;
            SettingsMetadata = metadata;
            SettingsDifficulty = difficulty;
            TimingPoints = timingPoints;
            HitObjects = hitObjects;
        }
    }
}
