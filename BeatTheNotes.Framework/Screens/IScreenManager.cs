namespace BeatTheNotes.Framework.Screens
{
    public interface IScreenManager
    {
        T FindScreen<T>() where T : Screen;
    }
}