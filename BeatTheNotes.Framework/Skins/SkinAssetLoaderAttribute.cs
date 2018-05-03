using System;
using System.Collections.Generic;

namespace BeatTheNotes.Framework.Skins
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class SkinAssetLoaderAttribute : Attribute
    {
        private readonly List<string> _fileExtensions = new List<string>();

        public string Name { get; set; }

        public IEnumerable<string> FileExtensions => _fileExtensions;

        public SkinAssetLoaderAttribute(string name, string fileExtension)
        {
            Name = name;
            _fileExtensions.Add(fileExtension);
        }

        public SkinAssetLoaderAttribute(string name, params string[] fileExtensions)
        {
            Name = name;
            _fileExtensions.AddRange(fileExtensions);
        }
    }
}