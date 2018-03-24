using System;

namespace BeatTheNotes.Framework.GameSystems
{
    public class SystemAddedEventArgs : EventArgs
    {
        public IGameSystemManager GameSystemManager { get; }

        public SystemAddedEventArgs(IGameSystemManager gameSystemManager)
        {
            GameSystemManager = gameSystemManager;
        }
    }
}
