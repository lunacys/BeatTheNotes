using System;

namespace BeatTheNotes.Framework.GameSystems
{
    public class SystemAddedEventArgs : EventArgs
    {
        public IGameSystemManager GameSystemManager { get; }
        public GameSystem GameSystem { get; }

        public SystemAddedEventArgs(IGameSystemManager gameSystemManager, GameSystem gameSystem)
        {
            GameSystemManager = gameSystemManager;
            GameSystem = gameSystem;
        }
    }
}
