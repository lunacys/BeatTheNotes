namespace BeatTheNotes.Shared.GameSystems
{
    public class OnScoreGetEventHandler
    {
        public string HitValueName { get; }
        public int HitValue { get; }

        public OnScoreGetEventHandler(string hitValueName, int hitValue)
        {
            HitValueName = hitValueName;
            HitValue = hitValue;
        }
    }
}
