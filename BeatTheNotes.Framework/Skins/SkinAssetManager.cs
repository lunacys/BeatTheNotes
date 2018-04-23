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
            var loaderAssembly = Assembly.GetCallingAssembly();

            // TODO: Maybe should check the type if its interface type is T
            // Get first class than realizes ISkinAssetLoader interface
            var type = loaderAssembly
                .GetTypes().FirstOrDefault(t => t.IsClass && t.GetInterface(nameof(ISkinAssetLoader<T>)) != null);

            if (type == null) throw new InvalidOperationException("Cannot find loader for this asset type");

            foreach (var attribute in type.GetCustomAttributes(false)
                .Where(a => a is SkinAssetLoaderAttribute))
            {
                var a = attribute as SkinAssetLoaderAttribute;

            }

            // Create instance of it
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