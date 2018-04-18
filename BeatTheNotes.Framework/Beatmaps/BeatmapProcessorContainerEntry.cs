namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessorContainerEntry
    {
        public BeatmapSettings BeatmapSettings { get; }
        public string BeatmapFolder { get; }
        public string BeatmapFilename { get; }
        public string BeatmapName { get; }
        public string BeatmapVersion { get; }
        public int HitObjectCount { get; }
        public double BeatsPerMinute { get; }

        public BeatmapProcessorContainerEntry(
            BeatmapSettings beatmapSettings, 
            string beatmapFolder,
            string beatmapFilename, 
            string beatmapName, 
            string beatmapVersion, 
            int hitObjectCount,
            double beatsPerMinute)
        {
            BeatmapSettings = beatmapSettings;
            BeatmapFilename = beatmapFilename;
            BeatmapFolder = beatmapFolder;
            BeatmapName = beatmapName;
            BeatmapVersion = beatmapVersion;
            HitObjectCount = hitObjectCount;
            BeatsPerMinute = beatsPerMinute;
        }
    }
}