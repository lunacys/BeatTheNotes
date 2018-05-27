using System;

namespace BeatTheNotes.Framework.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GameSettingsEntryAttribute : Attribute
    {
        public string SettingName { get; set; }

        public object DefaultValue { get; set; }
        public GameSettingsEntryAttribute(string settingName, object defaultValue)
        {
            SettingName = settingName;
            DefaultValue = defaultValue;
        }
    }
}