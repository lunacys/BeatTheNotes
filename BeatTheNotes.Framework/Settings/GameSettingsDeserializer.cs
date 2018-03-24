using System.IO;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Settings
{
    public static class GameSettingsDeserializer
    {
        public static GameSettings Deserialize()
        {
            string file = "settings.json";

            string str;
            using (StreamReader sr = new StreamReader(file))
            {
                str = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<GameSettings>(str);
        }
    }
}
