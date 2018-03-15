using System.Collections.Generic;

namespace Jackhammer
{
    public class Beatmap
    {
        public BeatmapSettings Settings { get; }

        public List<TimingPoint> TimingPoints { get; }

        public List<HitObject> HitObjects { get; }

        public Beatmap(
            BeatmapSettings settings,
            List<TimingPoint> timingPoints,
            List<HitObject> hitObjects)
        {
            Settings = settings;
            TimingPoints = timingPoints;
            HitObjects = hitObjects;
        }
    }
}
