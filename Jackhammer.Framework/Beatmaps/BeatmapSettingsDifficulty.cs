namespace Jackhammer.Framework.Beatmaps
{
    public class BeatmapSettingsDifficulty
    {
        public float HpDrainRate { get; }
        public float OverallDifficutly { get; }
        public int KeyAmount { get; }

        public BeatmapSettingsDifficulty(float hpDrainRate, float overallDifficutly, int keyAmount)
        {
            HpDrainRate = hpDrainRate;
            OverallDifficutly = overallDifficutly;
            KeyAmount = keyAmount;
        }

        public override string ToString()
        {
            return $"HpDrainRate:{HpDrainRate}\nOverallDifficulty:{OverallDifficutly}\nKeyAmount:{KeyAmount}";
        }
    }
}
