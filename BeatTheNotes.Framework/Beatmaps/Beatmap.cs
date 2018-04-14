using System.Collections.Generic;
using BeatTheNotes.Framework.Audio;
using BeatTheNotes.Framework.Objects;
using BeatTheNotes.Framework.TimingPoints;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class Beatmap
    {
        public BeatmapSettings Settings { get; }

        public Texture2D BackgroundTexture { get; }
        public Music Music { get; }

        public TimingPointContainer TimingPoints { get; }
        public HitObjectContainer HitObjects { get; }

        public Beatmap(
            BeatmapSettings settings,
            Texture2D backgroundTexture,
            Music music,
            TimingPointContainer timingPoints,
            HitObjectContainer hitObjects)
        {
            Settings = settings;
            BackgroundTexture = backgroundTexture;
            Music = music;
            TimingPoints = timingPoints;
            HitObjects = hitObjects;
        }
    }
}
