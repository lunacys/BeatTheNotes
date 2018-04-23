using System;
using System.Collections.Generic;

namespace BeatTheNotes.Framework.Skins
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class SkinAssetLoaderAttribute : Attribute
    {
        private readonly List<string> _fileExtensions = new List<string>();

        public IEnumerable<string> FileExtensions => _fileExtensions;

        public SkinAssetLoaderAttribute(string fileExtension)
        {
            _fileExtensions.Add(fileExtension);
        }

        public SkinAssetLoaderAttribute(params string[] fileExtensions)
        {
            _fileExtensions.AddRange(fileExtensions);
        }
    }
}