using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BeatTheNotes.Framework.Audio;
using BeatTheNotes.Framework.GameSystems;

namespace BeatTheNotes.GameSystems
{
    public class MusicSystem : GameSystem
    {
        public Music Music { get; set; }

        public TimeSpan MusicPosition
        {
            get => Music.Position;
            set => Music.Position = value;
        }

        public float PlaybackRate
        {
            get => Music.PlaybackRate;
            set => Music.PlaybackRate = value;
        }

        public float Volume
        {
            get => Music.Volume;
            set => Music.Volume = value;
        }
        

        public MusicSystem()
        { }

        public override void Initialize()
        {
            base.Initialize();
            
            Music?.Play();
        }

        public override void Reset()
        {
            base.Reset();

            Music?.Reset();
        }

        public override void Dispose()
        {
            base.Dispose();
            Music?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
