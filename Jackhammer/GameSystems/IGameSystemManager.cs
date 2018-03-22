namespace Jackhammer.GameSystems
{
    public interface IGameSystemManager
    {
        T FindSystem<T>() where T : GameSystem;
    }
}
