using System;

namespace Jackhammer.Framework.GameSystems
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
