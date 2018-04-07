using System.Collections.Generic;

namespace BeatTheNotes.Framework.GameSystems
{
    public interface IGameSystemManager
    {
        T FindSystem<T>() where T : GameSystem;
        IList<GameSystem> GetAllGameSystems();
    }
}
