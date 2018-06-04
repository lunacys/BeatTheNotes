using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatTheNotes.Framework.Debugging.Logging;
using BeatTheNotes.Framework.Serialization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Settings
{
    public class GameSettingsGameComponent : DrawableGameComponent
    {
        public GameSettings GameSettings { get; private set; }

        public string SettingsFileName { get; set; }

        public object this[string name] => GameSettings.Get(name);

        public GameSettingsGameComponent(Game game)
            : base(game)
        {
            GameSettings = new GameSettings();
            SettingsFileName = "Settings.json";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (File.Exists(SettingsFileName))
            {
                string str;
                using (StreamReader sr = new StreamReader(SettingsFileName))
                {
                    str = sr.ReadToEnd();
                }

                GameSettings = JsonConvert.DeserializeObject<GameSettings>(str, new GameSettingsConverter());
            }

            Scan(Assembly.GetEntryAssembly());
        }

        protected override void UnloadContent()
        {
            string str = JsonConvert.SerializeObject(GameSettings, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(SettingsFileName))
            {
                sw.WriteLine(str);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public T Get<T>(string name)
        {
            var obj = GameSettings.Get(name);

            if (!(obj is T))
                throw new InvalidCastException();

            return (T)obj;
        }

        private void Scan(Assembly assembly)
        {
            Console.WriteLine($"Scanning assembly: {assembly.FullName}");

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var props = type.GetProperties()
                    .Where(prop => prop.IsDefined(typeof(GameSettingsEntryAttribute), false));

                //List<GameSettingsEntryAttribute> attributes = new List<GameSettingsEntryAttribute>();

                foreach (var prop in props)
                {
                    LogHelper.Target = LogTarget.Console;
                    LogHelper.Log($"{nameof(GameSettingsGameComponent)}: Found setting property: {prop.Name}");

                    var attr = (GameSettingsEntryAttribute)prop.GetCustomAttribute(typeof(GameSettingsEntryAttribute), false);

                    //attributes.Add(attr);
                    string name = !string.IsNullOrEmpty(attr.SettingName) ? attr.SettingName : prop.Name;
                    object value = attr.DefaultValue;

                    GameSettings.Add(name, value);
                }
            }
        }
    }
}