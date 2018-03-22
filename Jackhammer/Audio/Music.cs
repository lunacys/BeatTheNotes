using System;
using Jackhammer.SoundTouch;
using NAudio.Utils;
using NAudio.Wave;

namespace Jackhammer.Audio
{
    public sealed class Music : IDisposable
    {
        //private WaveOutEvent _wave;
        //private VorbisWaveReader _oggReader;
        //private Mp3FileReader _mp3Reader;

        public TimeSpan Position
        {
            get => _wavePlayer.GetPositionTimeSpan().Multiply(PlaybackRate);
            set
            {
                _wavePlayer.Dispose();
                _wavePlayer = new WaveOutEvent();
                _wavePlayer.Init(_speedControl);
                _wavePlayer.Play();
                _reader.CurrentTime = value;
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
            set
            {
                _speedControl.PlaybackRate = value; 
            }
        }

        private WaveOutEvent _wavePlayer;
        private VarispeedSampleProvider _speedControl;
        private AudioFileReader _reader;
        private readonly string _filename;

        public Music(string filename)
        {
            _filename = filename;

            LoadFromFile(_filename);
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

            PlaybackRate = 1.0f;

            _wavePlayer = new WaveOutEvent();

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
