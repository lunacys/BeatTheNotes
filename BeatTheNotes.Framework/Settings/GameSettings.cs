using System;
using System.Collections;
using System.Collections.Generic;

namespace BeatTheNotes.Framework.Settings
{
    public class GameSettings : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _gameSettings;

        public object this[string name] => Get(name);

        public GameSettings()
        {
            _gameSettings = new Dictionary<string, object>();
        }

        public void Add(string name, object value)
        {
            if (_gameSettings.Count != 0 && _gameSettings.ContainsKey(name))
                return;

            _gameSettings[name] = value;
        }

        public object Get(string name)
        {
            if (!_gameSettings.ContainsKey(name))
                throw new ArgumentException("Cannot find setting with this name", name);

            return _gameSettings[name];
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _gameSettings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}