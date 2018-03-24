namespace BeatTheNotes.Framework.Beatmaps
{
    public class BeatmapSettingsMetadata
    {
        public string Title { get; }
        public string Artist { get; }
        public string Creator { get; }
        /// <summary>
        /// Also named as "Difficulty"
        /// </summary>
        public string Version { get; }
        public string Tags { get; }
        public int BeatmapId { get; }
        public int BeatmapSetId { get; }

        public BeatmapSettingsMetadata(string title, string artist, string creator,
            string version, string tags, int beatmapId, int beatmapSetId)
        {
            Title = title;
            Artist = artist;
            Creator = creator;
            Version = version;

            Tags = tags;

            BeatmapId = beatmapId;
            BeatmapSetId = beatmapSetId;
        }

        /*public override string ToString()
        {
            return $"Title:{Title}\nArtist:{Artist}\nCreator:{Creator}\nVersion:{Version}\nTags:{Tags}\nBeatmapID:{BeatmapId}\nBeatmapSetID:{BeatmapSetId}";
        }*/
    }
}
