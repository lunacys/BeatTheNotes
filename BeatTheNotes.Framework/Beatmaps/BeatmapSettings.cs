using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Beatmaps
{
    // TODO: Make GameMode support
    public class BeatmapSettings
    {
        public string GameModeName { get; }
        public BeatmapSettingsGeneral General { get; }
        public BeatmapSettingsEditor Editor { get; }
        public BeatmapSettingsMetadata Metadata { get; }
        public BeatmapSettingsDifficulty Difficulty { get; }
        [JsonProperty(PropertyName = "TimingPointsFilename")]
        public string TimingPointsFilename { get; }
        [JsonProperty(PropertyName = "HitObjectsFilename")]
        public string HitObjectsFilename { get; }

        public BeatmapSettings(BeatmapSettingsGeneral general, BeatmapSettingsEditor editor,
            BeatmapSettingsMetadata metadata, BeatmapSettingsDifficulty difficulty)
        {
            GameModeName = "Standard";

            General = general;
            Editor = editor;
            Metadata = metadata;
            Difficulty = difficulty;

            TimingPointsFilename = "timing_points";
            HitObjectsFilename = "hit_objects";
        }

        public override string ToString()
        {
            return
                $" > General:\n{General}\n" +
                $" > Editor:\n{Editor}\n" +
                $" > Metadata:\n{Metadata}\n" +
                $" > Difficulty:\n{Difficulty}\n" +
                $" > Timing Points Filename: {TimingPointsFilename}\n" +
                $" > Hit Objects Filename: {HitObjectsFilename}";
        }
    }
}
