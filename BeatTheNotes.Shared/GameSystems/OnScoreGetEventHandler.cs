namespace BeatTheNotes.Shared.GameSystems
{
    public class OnScoreGetEventHandler
    {
        public string HitValueName { get; }
        public int HitValue { get; }
        public float Offset { get; }

        public OnScoreGetEventHandler(string hitValueName, int hitValue, float offset)
        {
            HitValueName = hitValueName;
            HitValue = hitValue;
            Offset = offset;
        }
    }
}
