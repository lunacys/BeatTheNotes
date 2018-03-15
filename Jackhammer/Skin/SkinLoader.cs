using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.Skin
{
    public static class SkinLoader
    {
        private static Texture2D LoadFromFile(GraphicsDevice device, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"File '{filename}' Not Found");

            Texture2D t = null;
            
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                t = Texture2D.FromStream(device, fs);
            }
            
            LogHelper.Log($"SkinLoader: Sucessfully loaded asset '{filename}'");

            return t;
        }

        private static Skin LoadDefault(ContentManager content)
        {
            Skin skin = new Skin();
            try
            {
                skin.DefaultBackground = content.Load<Texture2D>(Path.Combine("Images", "DefaultBackground"));
                skin.NoteClickTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteClick"));
                skin.NoteHoldTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteHold"));
                skin.PlayfieldLineTexture = content.Load<Texture2D>(Path.Combine("Images", "PlayfieldLine"));
                skin.ButtonTexture = content.Load<Texture2D>(Path.Combine("Images", "Button"));
                skin.Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"));
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
                if (!Directory.Exists(dirPath))
                    throw new DirectoryNotFoundException("Skin Not Found");

                skin = new Skin();

                try
                {
                    skin.DefaultBackground = LoadFromFile(device, Path.Combine(dirPath, "DefaultBackground"));
                    skin.NoteClickTexture = LoadFromFile(device, Path.Combine(dirPath, "NoteClick.png"));
                    skin.NoteHoldTexture = LoadFromFile(device, Path.Combine(dirPath, "NoteHold.png"));
                    skin.PlayfieldLineTexture = LoadFromFile(device, Path.Combine(dirPath, "PlayfieldLine.png"));
                    skin.ButtonTexture = LoadFromFile(device, Path.Combine(dirPath, "Button.png"));
                    skin.Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"));
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
