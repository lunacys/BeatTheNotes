using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins.AssetLoaders;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins
{
    public sealed class SkinAssetManager
    {
        private Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();

        private GraphicsDevice _graphicsDevice;

        public string SkinFolder { get; }

        public SkinAssetManager(GraphicsDevice graphicsDevice, GameSettings gameSettings)
        {
            _graphicsDevice = graphicsDevice;
            SkinFolder = gameSettings.SkinsFolder;
        }

        public object this[string name] => _loadedAssets[name];

        public T Load<T>(string assetName)
        {
            // Get current assembly
            var loaderAssembly = Assembly.GetExecutingAssembly();

            Console.WriteLine($"Name: {loaderAssembly.FullName}");

            // Get first class than implements ISkinAssetLoader interface with generic type T
            var type = loaderAssembly
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && t.GetInterfaces().Any(x =>
                                         x.IsGenericType &&
                                         x.GetGenericTypeDefinition() == typeof(ISkinAssetLoader<>) &&
                                         x.GetGenericArguments()[0] == typeof(T)));

            if (type == null) throw new InvalidOperationException("Cannot find loader for this asset type");

            Console.WriteLine($"Full Name: {type.FullName}");

            foreach (var attribute in type.GetCustomAttributes(false)
                .Where(a => a is SkinAssetLoaderAttribute))
            {
                var a = attribute as SkinAssetLoaderAttribute;

            }

            // Create instance of it
            // TODO: Need to pass constructor parameters to the instance if it has ones
            var assetLoader = (ISkinAssetLoader<T>)loaderAssembly.CreateInstance(type.FullName, true);

            // Pass through the Load method 
            var asset = Load(assetName, assetLoader);

            return asset;
        }

        public T Load<T>(string assetName, ISkinAssetLoader<T> assetLoader)
        {
            var filepath = Path.Combine(SkinFolder, assetLoader.AssetSubdirectory, assetName);

            if (!File.Exists(filepath))
                throw new FileNotFoundException();

            if (_loadedAssets.ContainsKey(assetName)) return (T)_loadedAssets[assetName];

            var asset = assetLoader.LoadAsset(filepath);
            _loadedAssets.Add(assetName, asset);

            return asset;
        }
    }
}