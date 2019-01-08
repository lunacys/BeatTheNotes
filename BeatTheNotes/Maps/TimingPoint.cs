using Newtonsoft.Json;

namespace BeatTheNotes.Maps
{
    public class TimingPoint
    {
        /// <summary>
        /// Gets position in milliseconds
        /// </summary>
        public int Position { get; }
        /// <summary>
        /// Gets milliseconds per beat
        /// </summary>
        public double MsPerBeat { get; }

        /// <summary>
        /// Gets beats per minute (BPM) from current MsPerBeat
        /// </summary>
        [JsonIgnore]
        public double BeatsPerMinute => 60000 / MsPerBeat;

        /// <summary>
        /// Gets time signature in format n/4 where n is the value
        /// </summary>
        public int TimeSignature { get; }

        /// <summary>
        /// Gets hit sound volume, the minimum is 0, the maximum is 100
        /// </summary>
        public int HitSoundVolume { get; }

        /// <summary>
        /// Gets whether metronome must be reset when reaching current timing point
        /// </summary>
        public bool MustResetMetronome { get; }

        public TimingPoint(int position, double msPerBeat, int timeSignature, int hitSoundVolume, bool resetMetronome)
        {
            HitSoundVolume = hitSoundVolume < 0 ? 0 : hitSoundVolume;
            HitSoundVolume = hitSoundVolume > 100 ? 100 : hitSoundVolume;
            Position = position < 0 ? 0 : position;
            MsPerBeat = msPerBeat;
            TimeSignature = timeSignature;
            MustResetMetronome = resetMetronome;
        }

        /// <summary>
        /// Converts beats per minute to milliseconds per beat
        /// </summary>
        /// <param name="bpm">Beats per minute</param>
        /// <returns>Milliseconds per beat</returns>
        public static double BpmToMsPerBeat(double bpm)
        {
            return 60000 / bpm;
        }

        public override string ToString()
        {
            return $"{Position} {MsPerBeat:F12} {TimeSignature} {HitSoundVolume} {MustResetMetronome}";
        }
    }
}