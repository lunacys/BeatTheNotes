using System;
using System.Collections.Generic;
using BeatTheNotes.Framework.Settings;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Serialization
{
    public class GameSettingsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var gs = (GameSettings)value;

            writer.WriteStartObject();
            foreach (var setting in gs)
            {
                writer.WritePropertyName(setting.Key);
                writer.WriteWhitespace(" ");
                writer.WriteValue(setting.Value);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dictionary = serializer.Deserialize<Dictionary<string, object>>(reader);

            GameSettings settings = new GameSettings();

            foreach (var kv in dictionary)
            {
                settings.Add(kv.Key, kv.Value);
            }

            return settings;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GameSettings);
        }
    }
}