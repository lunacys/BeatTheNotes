namespace BeatTheNotes.Framework.Input
{
    /// <summary>
    /// Input command that does nothing
    /// </summary>
    public class InputNullCommand : IInputCommand
    {
        /// <summary>
        /// Do nothing
        /// </summary>
        public void Execute() { }
    }
}