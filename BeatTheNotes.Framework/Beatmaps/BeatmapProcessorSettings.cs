namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessorSettings
    {
        public string BeatmapFileExtension { get; }
        public string BeatmapsFolder { get; }
        public string TimingPointsFolder { get; }
        public string HitObjectsFolder { get; }
        public string DatabaseName { get; }

        public BeatmapProcessorSettings(
            string beatmapFileExtension, 
            string beatmapsFolder, 
            string timingPointsFolder,
            string hitObjectsFolder,
            string databaseName)
        {
            BeatmapFileExtension = beatmapFileExtension;
            BeatmapsFolder = beatmapsFolder;
            TimingPointsFolder = timingPointsFolder;
            HitObjectsFolder = hitObjectsFolder;
            DatabaseName = databaseName;
        }
    }
}
