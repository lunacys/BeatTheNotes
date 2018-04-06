using System;
using SoundTouch;
using NAudio.Wave;

namespace BeatTheNotes.Framework.Audio
{
    public sealed class Music : IDisposable
    {
        public TimeSpan Position
        {
            get => _reader.CurrentTime;//_wavePlayer.GetPositionTimeSpan().Multiply(PlaybackRate);
            set
            {
                _reader.CurrentTime = value;
                _speedControl.Reposition();
            }
        }

        public float Volume
        {
            get => _wavePlayer.Volume;
            set => _wavePlayer.Volume = value;
        }

        public float PlaybackRate
        {
            // TODO: Reset song
            get => _speedControl.PlaybackRate;
            set => _speedControl.PlaybackRate = value;
        }

        private WaveOutEvent _wavePlayer;
        private VarispeedSampleProvider _speedControl;
        private AudioFileReader _reader;

        public Music(string filename)
        {
            LoadFromFile(filename);
        }

        public void LoadFromFile(string filename)
        {
            Dispose();

            _reader = null;
            _speedControl = null;
            _wavePlayer = null;
            
            if (filename == null) return;
            _reader = new AudioFileReader(filename);
            _speedControl = new VarispeedSampleProvider(_reader, 100, new SoundTouchProfile(true, true));

            _wavePlayer = new WaveOutEvent();

            PlaybackRate = 1.0f;

            _wavePlayer.Volume = 1.0f;
            _wavePlayer.Init(_speedControl);
        }

        public void Play()
        {
            _wavePlayer?.Play();
        }

        public void Stop()
        {
            _wavePlayer?.Stop();
        }

        public void Reset()
        {
            _reader.CurrentTime = TimeSpan.Zero;
            _speedControl.Reposition();
        }

        public void Pause()
        {
            _wavePlayer?.Pause();
        }

        public void Dispose()
        {
            _wavePlayer?.Dispose();
            _reader?.Dispose();
            _speedControl?.Dispose();
        }
    }
}
