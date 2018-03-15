using System.IO;
using Newtonsoft.Json;

namespace Jackhammer
{
    public static class GameSettingsSerializer
    {
        public static void Serialize(GameSettings settings)
        {
            string file = "settings.json";

            string str = JsonConvert.SerializeObject(settings, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(str);
            }
        }
    }
}
