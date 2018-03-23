using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jackhammer.Framework.Audio;
using Jackhammer.Framework.GameSystems;

namespace Jackhammer.GameSystems
{
    public class MusicSystem : GameSystem
    {
        public Music Music { get; set; }

        public long MusicPosition => (long)Music.Position.TotalMilliseconds;

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

        private string _beatmapName;
        private string _songName;

        public MusicSystem(string beatmapName, string songName)
        {
            _beatmapName = beatmapName;
            _songName = songName;
            
        }

        public override void Initialize()
        {
            base.Initialize();

            Music = new Music(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", _beatmapName, _songName));
            Music.Play();
        }

        public override void Reset()
        {
            base.Reset();

            Music.Stop();
            Music.LoadFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", _beatmapName, _songName));
            Music.Play();
        }

        public override void Dispose()
        {
            base.Dispose();
            Music.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
