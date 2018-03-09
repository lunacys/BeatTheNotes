namespace Jackhammer
{
    public class TimingPoint
    {
        public int Position { get; }
        public double MsPerBeat { get; }

        //public double Bpm => 60000 / MsPerBeat;
        public int TimeSignature { get; }
        public int HitSoundVolume { get; }
        public bool IsResetMetronome { get; }

        public TimingPoint(int position, double msPerBeat, int timeSignature, int hitSoundVolume, bool resetMetronome)
        {
            HitSoundVolume = hitSoundVolume < 0 ? 0 : hitSoundVolume;
            HitSoundVolume = hitSoundVolume > 100 ? 100 : hitSoundVolume;
            Position = position < 0 ? 0 : position;
            MsPerBeat = msPerBeat;
            TimeSignature = timeSignature;
            IsResetMetronome = resetMetronome;
        }

        public static double BpmToMsPerBeat(double bpm)
        {
            return 60000 / bpm;
        }

        public override string ToString()
        {
            return $"{Position} {MsPerBeat:F12} {TimeSignature} {HitSoundVolume} {IsResetMetronome}";
        }
    }
}
