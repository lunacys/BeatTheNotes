using System;

namespace BeatTheNotes.Framework.GameSystems
{
    public class GameSystemAddedEventArgs : EventArgs
    {
        public IGameSystemManager GameSystemManager { get; }
        public GameSystem GameSystem { get; }

        public GameSystemAddedEventArgs(IGameSystemManager gameSystemManager, GameSystem gameSystem)
        {
            GameSystemManager = gameSystemManager;
            GameSystem = gameSystem;
        }
    }
}
