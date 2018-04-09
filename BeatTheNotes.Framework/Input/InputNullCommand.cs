namespace BeatTheNotes.Framework.Input
{
    /// <summary>
    /// Input command that does nothing
    /// </summary>
    public class InputNullCommand : IInputCommand
    {
        // Do nothing
        public void Execute() { }
    }
}