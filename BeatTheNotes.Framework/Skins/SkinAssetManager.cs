using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins.AssetLoaders;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Skins
{
    public class SkinAssetManager
    {
        private readonly Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();

        private readonly GraphicsDevice _graphicsDevice;
        private readonly GameSettings _gameSettings;

        public string SkinFolder { get; }
        public string CurrentSkinName { get; set; }

        public string SkinSettingsFilename => "Settings.json";

        public SkinAssetManager(GraphicsDevice graphicsDevice, GameSettings gameSettings)
        {
            _graphicsDevice = graphicsDevice;
            _gameSettings = gameSettings;

            SkinFolder = gameSettings.SkinsFolder;

            CurrentSkinName = gameSettings.Skin;
        }

        public object this[string name] => _loadedAssets[name];

        /// <summary>
        /// Load an asset from the <see cref="SkinFolder"/> using the <see cref="CurrentSkinName"/>
        /// </summary>
        /// <typeparam name="T">Type of asset</typeparam>
        /// <param name="assetName">Asset name</param>
        /// <returns>The asset</returns>
        public virtual T Load<T>(string assetName)
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

            /*foreach (var attribute in type.GetCustomAttributes(false)
                .Where(a => a is SkinAssetLoaderAttribute))
            {
                var a = attribute as SkinAssetLoaderAttribute;
            }*/

            ISkinAssetLoader<T> assetLoader;

            // If it is a graphic asset, we should set its GraphicsDevice property
            if (type.GetInterfaces().Any(x => x == typeof(ISkinAssetGraphic)))
            {
                var graphAsset = (ISkinAssetGraphic)Activator.CreateInstance(type);
                graphAsset.GraphicsDevice = _graphicsDevice;

                assetLoader = graphAsset as ISkinAssetLoader<T>;
            }
            else
            {
                assetLoader = (ISkinAssetLoader<T>)Activator.CreateInstance(type);
            }

            // Pass through the Load method 
            var asset = Load(assetName, assetLoader);

            return asset;
        }

        private bool CanRead(Type type)
        {
            return true;
        }

        private T Load<T>(string assetName, ISkinAssetLoader<T> assetLoader)
        {
            var filepath = Path.Combine(SkinFolder, CurrentSkinName, assetLoader.AssetSubdirectory, assetName);

            if (!File.Exists(filepath))
                throw new FileNotFoundException();

            if (_loadedAssets.ContainsKey(assetName)) return (T)_loadedAssets[assetName];

            var asset = assetLoader.LoadAsset(filepath);
            _loadedAssets.Add(assetName, asset);

            return asset;
        }
    }
}