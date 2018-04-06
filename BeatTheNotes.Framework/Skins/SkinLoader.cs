using System;
using System.IO;
using BeatTheNotes.Framework.Logging;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Skins
{
    public static class SkinLoader
    {
        private static Texture2D LoadTextureFromFile(GraphicsDevice device, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"File '{filename}' Not Found");

            Texture2D t;
            
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                t = Texture2D.FromStream(device, fs);
            }
            
            LogHelper.Log($"SkinLoader: Sucessfully loaded asset '{filename}'");

            return t;
        }

        private static SoundEffect LoadSoundEffectFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"File '{filename}' Not Found");

            SoundEffect se;

            using (var fs = new FileStream(filename, FileMode.Open))
                se = SoundEffect.FromStream(fs);

            LogHelper.Log($"SkinLoader: Sucessfully loaded asset '{filename}'");

            return se;
        }

        public static Skin LoadDefault(ContentManager content)
        {
            Skin skin = new Skin();
            try
            {
                skin.Settings = new SkinSettings
                {
                    NoteType = "Brick",
                    AuthorName = "loonacuse",
                    SkinName = "Default",
                    HitPosition = 0,
                    PlayfieldPositionX = 200
                };

                skin.DefaultBackground = content.Load<Texture2D>(Path.Combine("Images", "DefaultBackground"));
                skin.NoteClickTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteClick"));
                skin.NoteHoldTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteHold"));
                skin.PlayfieldLineTexture = content.Load<Texture2D>(Path.Combine("Images", "PlayfieldLine"));
                skin.ButtonTexture = content.Load<Texture2D>(Path.Combine("Images", "Button"));
                skin.Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"));

                skin.ScoreMarvelousTexture = content.Load<Texture2D>(Path.Combine("Images", "HitMarvelous"));
                skin.ScorePerfectTexture = content.Load<Texture2D>(Path.Combine("Images", "HitPerfect"));
                skin.ScoreGreatTexture = content.Load<Texture2D>(Path.Combine("Images", "HitGreat"));
                skin.ScoreGoodTexture = content.Load<Texture2D>(Path.Combine("Images", "HitGood"));
                skin.ScoreBadTexture = content.Load<Texture2D>(Path.Combine("Images", "HitBad"));
                skin.ScoreMissTexture = content.Load<Texture2D>(Path.Combine("Images", "HitMiss"));

                skin.HealthBarBg = content.Load<Texture2D>(Path.Combine("Images", "HealthBarBg"));
                skin.HealthBar = content.Load<Texture2D>(Path.Combine("Images", "HealthBar"));

                skin.HitNormal = content.Load<SoundEffect>(Path.Combine("Sfx", "HitNormal"));
                skin.ComboBreak = content.Load<SoundEffect>(Path.Combine("Sfx", "ComboBreak"));
            }
            catch (Exception e)
            {
                LogHelper.Log($"SkinLoader: Unexpected error while creating Default skin. Maybe some files are missed? {e}", LogLevel.Critical);
                throw;
            }

            return skin;
        }

        public static Skin Load(ContentManager content, GraphicsDevice device, string skinName)
        {
            Skin skin;

            if (skinName.ToLower() != "default")
            {
                LogHelper.Log($"SkinLoader: The skin name is not Default. Loading skin from custom directory '{skinName}'");

                string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Skins", skinName);
                string skinSettings = Path.Combine(dirPath, "Skin.json");

                if (!Directory.Exists(dirPath))
                    throw new DirectoryNotFoundException("Skin Not Found");
                if (!File.Exists(skinSettings))
                    throw new FileNotFoundException("Skin settings file not found");

                string str;

                using (StreamReader sr = new StreamReader(skinSettings))
                {
                    str = sr.ReadToEnd();
                }

                SkinSettings ss = JsonConvert.DeserializeObject<SkinSettings>(str);

                skin = new Skin();

                try
                {
                    skin.Settings = ss;

                    if (skin.Settings.NoteType.ToLower() == "brick")
                    {
                        skin.NoteClickTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteClick.png"));
                        skin.NoteHoldTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteHold.png"));
                    }
                    else if (skin.Settings.NoteType.ToLower() == "arrow")
                    {
                        skin.NoteClickTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteClick1.png"));
                        skin.NoteClickTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteClick2.png"));
                        skin.NoteClickTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteClick3.png"));
                        skin.NoteClickTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteClick4.png"));

                        skin.NoteHoldTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "NoteHold.png"));
                    }
                    else
                    {
                        throw new InvalidDataException("Unknown note type");
                    }
                    skin.DefaultBackground = LoadTextureFromFile(device, Path.Combine(dirPath, "DefaultBackground.png"));
                    skin.PlayfieldLineTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "PlayfieldLine.png"));
                    skin.ButtonTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "Button.png"));

                    skin.ScoreMarvelousTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitMarvelous.png"));
                    skin.ScorePerfectTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitPerfect.png"));
                    skin.ScoreGreatTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitGreat.png"));
                    skin.ScoreGoodTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitGood.png"));
                    skin.ScoreBadTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitBad.png"));
                    skin.ScoreMissTexture = LoadTextureFromFile(device, Path.Combine(dirPath, "HitMiss.png"));

                    skin.HealthBarBg = LoadTextureFromFile(device, Path.Combine(dirPath, "HealthBarBg.png"));
                    skin.HealthBar = LoadTextureFromFile(device, Path.Combine(dirPath, "HealthBar.png"));

                    skin.Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"));

                    skin.HitNormal = LoadSoundEffectFromFile(Path.Combine(dirPath, "HitNormal.wav"));
                    skin.ComboBreak = LoadSoundEffectFromFile(Path.Combine(dirPath, "ComboBreak.wav"));
                }
                catch (Exception e)
                {
                    LogHelper.Log(
                        $"SkinLoader: Unexpected error while opening custom skin. Taking default skin instead: {e}", LogLevel.Error);
                    skin = LoadDefault(content);
                }
            }
            else
            {
                LogHelper.Log($"SkinLoader: The skin name is Default. Taking default skin");

                skin = LoadDefault(content);
            }

            return skin;
        }
    }
}
