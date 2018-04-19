using BeatTheNotes.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapProcessorContainerEntry
    {
        public BeatmapSettings BeatmapSettings { get; }
        public Texture2D BackgroundTexture { get; }
        public Music Music { get; }
        public string BeatmapFolder { get; }
        public string BeatmapFilename { get; }
        public string BeatmapName { get; }
        public string BeatmapVersion { get; }
        public int HitObjectCount { get; }
        public double BeatsPerMinute { get; }

        public BeatmapProcessorContainerEntry(
            BeatmapSettings beatmapSettings, 
            Texture2D backgroundTexture,
            Music music,
            string beatmapFolder,
            string beatmapFilename, 
            string beatmapName, 
            string beatmapVersion, 
            int hitObjectCount,
            double beatsPerMinute)
        {
            BeatmapSettings = beatmapSettings;
            BackgroundTexture = backgroundTexture;
            Music = music;
            BeatmapFilename = beatmapFilename;
            BeatmapFolder = beatmapFolder;
            BeatmapName = beatmapName;
            BeatmapVersion = beatmapVersion;
            HitObjectCount = hitObjectCount;
            BeatsPerMinute = beatsPerMinute;
        }

        public override string ToString()
        {
            return
                $"BM Folder: {BeatmapFolder}\n" +
                $"BM Filename: {BeatmapFilename}\n" +
                $"BM Name: {BeatmapName}\n" +
                $"BM Version: {BeatmapVersion}\n" +
                $"Hit Object Count: {HitObjectCount}\n" +
                $"BPM: {BeatsPerMinute}\n" +
                $"Settings:\n{BeatmapSettings}";
        }
    }
}