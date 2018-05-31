using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatTheNotes.Framework.GameAssets.AssetLoaders;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.GameAssets
{
    /// <summary>
    /// Represents an asset loader that can load raw (not converted to .xnb format) files.
    /// </summary>
    public class AssetManager
    {
        private readonly Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
        private readonly Dictionary<string, object> _assetLoaders = new Dictionary<string, object>();

        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Gets asset directory which is in use
        /// </summary>
        public string AssetDirectory { get; set; }

        public AssetManager(GraphicsDevice graphicsDevice, string assetDirectory = "Content")
        {
            _graphicsDevice = graphicsDevice;

            AssetDirectory = assetDirectory;
        }

        public object this[string name] => _loadedAssets[name];

        /// <summary>
        /// Load an asset from the <see cref="AssetDirectory"/>
        /// </summary>
        /// <typeparam name="T">Type of asset</typeparam>
        /// <param name="assetName">Asset name</param>
        /// <returns>The asset</returns>
        public virtual T Load<T>(string assetName)
        {
            if (_loadedAssets.ContainsKey(assetName))
                return (T)_loadedAssets[assetName];

            // Get current assembly
            var loaderAssembly = Assembly.GetExecutingAssembly();

            Console.WriteLine($"Name: {loaderAssembly.FullName}");

            // Get first class than implements IAssetLoader interface with generic type T
            var type = loaderAssembly
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && t.GetInterfaces().Any(x =>
                                         x.IsGenericType &&
                                         x.GetGenericTypeDefinition() == typeof(IAssetLoader<>) &&
                                         x.GetGenericArguments()[0] == typeof(T)));

            if (type == null) throw new InvalidOperationException("Cannot find loader for this asset type");

            //Console.WriteLine($"Full Name: {type.FullName}");

            string assetLoaderName = String.Empty;
            string assetSubdirectory = null;
            string assetFilenameMask = string.Empty;
            IEnumerable<string> assetFileExtensions = new List<string>();

            var attribute = type.GetCustomAttributes(false).FirstOrDefault(a => a is AssetLoaderAttribute);
            if (attribute is AssetLoaderAttribute a1)
            {
                assetLoaderName = a1.AssetLoaderName;
                assetSubdirectory = a1.AssetSubdirectory;
                assetFilenameMask = a1.AssetFilenameMask;
                assetFileExtensions = a1.AssetFileExtensions;
            }

            IAssetLoader<T> assetLoader;

            // Optimization: if we've already used this asset loader we do not need to create instance of it again
            if (_assetLoaders.ContainsKey(assetLoaderName))
            {
                assetLoader = (IAssetLoader<T>)_assetLoaders[assetLoaderName];
            }
            else
            {
                // If it is a graphic asset, we should set its GraphicsDevice property
                if (type.GetInterfaces().Any(x => x == typeof(IGraphicalAsset)))
                {
                    var graphAsset = (IGraphicalAsset)Activator.CreateInstance(type);
                    graphAsset.GraphicsDevice = _graphicsDevice;

                    assetLoader = graphAsset as IAssetLoader<T>;
                }
                else
                {
                    assetLoader = (IAssetLoader<T>)Activator.CreateInstance(type);
                }

                _assetLoaders[assetLoaderName] = assetLoader;
            }

            // Pass through the Load method 
            var asset = Load(assetName, assetLoader, assetSubdirectory, assetFileExtensions);

            return asset;
        }

        private T Load<T>(string assetName,
            IAssetLoader<T> assetLoader,
            string assetSubdirectory,
            IEnumerable<string> assetFileExtensions)
        {
            var filepath =
                (assetSubdirectory != null)
                    ? (Path.Combine(AssetDirectory, assetSubdirectory, assetName))
                    : (Path.Combine(AssetDirectory, assetName));

            if (!File.Exists(filepath))
                throw new FileNotFoundException();

            if (!IsInCollection(assetFileExtensions, filepath))
                throw new Exception("Cannot load an asset with this extension");

            //if (_loadedAssets.ContainsKey(assetName)) return (T)_loadedAssets[assetName];

            var asset = assetLoader.LoadAsset(filepath);
            _loadedAssets.Add(assetName, asset);

            return asset;
        }

        private bool IsInCollection(IEnumerable<string> fileExtensions, string filepath)
        {
            bool result = false;

            foreach (var extension in fileExtensions)
            {
                var ext = Path.GetExtension(filepath);

                if (ext == extension)
                    result = true;
            }

            return result;
        }


    }
}