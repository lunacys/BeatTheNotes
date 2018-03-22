using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jackhammer.SoundTouch;
using NAudio.Utils;
using NAudio.Vorbis;
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
            get => _wavePlayer.GetPositionTimeSpan();
            set
            {
                _reader.CurrentTime = value;
                Dispose();
                LoadFromFile(_filename);
                Play();
            }
        }

        public float PlaybackRate
        {
            // TODO: Reset song
            get => _speedControl.PlaybackRate;
            set
            {
                _speedControl.PlaybackRate = value; 
            }
/*0.5f + value * 0.1f;*/
        }

        private WaveOutEvent _wavePlayer;
        private VarispeedSampleProvider _speedControl;
        private AudioFileReader _reader;
        private readonly string _filename;

        public Music(string filename)
        {
            _filename = filename;
            _wavePlayer = new WaveOutEvent();
            LoadFromFile(_filename);
        }

        public void LoadFromFile(string filename)
        {
            /*string ext = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(ext) || (ext.ToLower() != ".mp3" && ext.ToLower() != ".ogg"))
                throw new InvalidDataException("Not supported file format");

            switch (ext.ToLower())
            {
                case ".mp3":
                    _mp3Reader = new Mp3FileReader(filename);
                    _wave.Init(_mp3Reader);
                    break;
                case ".ogg":
                    _oggReader = new VorbisWaveReader(filename);
                    _wave.Init(_oggReader);
                    break;
            }*/

            _reader?.Dispose();
            _speedControl?.Dispose();
            _reader = null;
            _speedControl = null;
            
            if (filename == null) return;
            _reader = new AudioFileReader(filename);
            
            _speedControl = new VarispeedSampleProvider(_reader, 100, new SoundTouchProfile(true, false));
        }

        public void Play()
        {
            if (_wavePlayer == null)
            {
                _wavePlayer = new WaveOutEvent();
                _wavePlayer.PlaybackStopped += (sender, args) => 
                {
                    if (args.Exception != null)
                        LogHelper.Log($"Playback Stopped Unexpectedly: {args.Exception.Message}", LogLevel.Error);
                };
            }
            if (_speedControl == null)
            {
                LoadFromFile(_filename);
                if (_speedControl == null) return;
            }

            _wavePlayer.Init(_speedControl);

            _wavePlayer.Play();
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
