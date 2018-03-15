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
                throw new FileNotFoundException("File Not Found");

            Texture2D t;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                t = Texture2D.FromStream(device, fs);
            }

            return t;
        }

        public static Skin Load(ContentManager content, GraphicsDevice device, string skinName)
        {
            Skin skin;

            if (skinName.ToLower() != "default")
            {
                string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Skins", skinName);
                if (!Directory.Exists(dirPath))
                    throw new DirectoryNotFoundException("Skin Not Found");

                skin = new Skin
                {
                    NoteClickTexture = LoadFromFile(device, Path.Combine(dirPath, "NoteClick.png")),
                    NoteHoldTexture = LoadFromFile(device, Path.Combine(dirPath, "NoteHold.png")),
                    PlayfieldLineTexture = LoadFromFile(device, Path.Combine(dirPath, "PlayfieldLine.png")),
                    ButtonTexture = LoadFromFile(device, Path.Combine(dirPath, "Button.png")),
                    Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"))
                };
            }
            else
            {
                skin = new Skin
                {
                    NoteClickTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteClick")),
                    NoteHoldTexture = content.Load<Texture2D>(Path.Combine("Images", "NoteHold")),
                    PlayfieldLineTexture = content.Load<Texture2D>(Path.Combine("Images", "PlayfieldLine")),
                    ButtonTexture = content.Load<Texture2D>(Path.Combine("Images", "Button")),
                    Font = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"))
                };
            }

            return skin;
        }
    }
}
