namespace Jackhammer
{
    public class BeatmapSettingsEditor
    {
        public int BeatDivisor { get; }

        public BeatmapSettingsEditor(int beatDivisor)
        {
            BeatDivisor = beatDivisor;
        }

        public override string ToString()
        {
            return $"BeatDivisor:{BeatDivisor}";
        }
    }
}
