namespace BeatTheNotes.Framework.GameSystems
{
    public interface IGameSystemManager
    {
        T FindSystem<T>() where T : GameSystem;
    }
}
