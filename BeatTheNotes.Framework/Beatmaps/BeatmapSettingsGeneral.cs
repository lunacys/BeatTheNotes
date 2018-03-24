namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapSettingsGeneral
    {
        public string AudioFileName { get; }
        public int PreviewTime { get; }
        public string BackgroundFileName { get; }

        public BeatmapSettingsGeneral(string audioFileName, int previewTime, string backgroundFileName)
        {
            AudioFileName = audioFileName;
            PreviewTime = previewTime;
            BackgroundFileName = backgroundFileName;
        }

        public override string ToString()
        {
            return $"AudioFilename:{AudioFileName}\nPreviewTime:{PreviewTime}\nBackgroundFilename:{BackgroundFileName}";
        }
    }
}
