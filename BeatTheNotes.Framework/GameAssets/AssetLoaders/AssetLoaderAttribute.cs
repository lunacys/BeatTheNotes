using System;
using System.Collections.Generic;

namespace BeatTheNotes.Framework.GameAssets.AssetLoaders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AssetLoaderAttribute : Attribute
    {
        private readonly List<string> _assetFileExtensions = new List<string>();

        /// <summary>
        /// Friendly asset loader name
        /// </summary>
        public string AssetLoaderName { get; set; }

        public string AssetSubdirectory { get; set; }

        public string AssetFilenameMask { get; set; }

        public IEnumerable<string> AssetFileExtensions => _assetFileExtensions;

        public AssetLoaderAttribute(string assetLoaderName, string assetSubdirectory, string assetFilenameMask, string assetFileExtension)
        {
            AssetLoaderName = assetLoaderName;
            AssetSubdirectory = assetSubdirectory;
            AssetFilenameMask = assetFilenameMask;
            _assetFileExtensions.Add(assetFileExtension);
        }

        public AssetLoaderAttribute(string assetLoaderName, string assetSubdirectory, string assetFilenameMask, params string[] assetFileExtensions)
        {
            AssetLoaderName = assetLoaderName;
            AssetSubdirectory = assetSubdirectory;
            AssetFilenameMask = assetFilenameMask;
            _assetFileExtensions.AddRange(assetFileExtensions);
        }
    }
}